using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TicketComplaint.Infra.Db;

namespace TicketComplaint
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddProblemDetails(
                setup => {
                    setup.IncludeExceptionDetails = (_, _) => false;
                    setup.Map<InvalidOperationException>(ex => new ProblemDetails{
                        Title = "Um erro ocorreu",
                        Detail = ex.Message,
                        Status = StatusCodes.Status400BadRequest
                    });
                }
            );
            
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer("Server=localhost;Database=TicketComplaint;User Id=sa;Password=@Sql2019;MultipleActiveResultSets=true;Encrypt=YES;TrustServerCertificate=YES")
            );

            services.AddIdentity<IdentityUser, IdentityRole>(options => {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 3;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddControllers()
                .AddFluentValidation(s => s.RegisterValidatorsFromAssemblyContaining<Startup>());

            // Autenticação antiga
            // services.AddAuthentication("BasicAuthentication")
            //     .AddScheme<AuthenticationSchemeOptions, BasicAuthHandler>("BasicAuthentication", null);

            var key = Encoding.ASCII.GetBytes(Configuration.GetValue<string>("JwtBearerTokenSettings:SecretKey"));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = Configuration.GetValue<string>("JwtBearerTokenSettings:Issuer"),
                    ValidateAudience = true,
                    ValidAudience = Configuration.GetValue<string>("JwtBearerTokenSettings:Audience"),
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                };  
            });

            services.AddAuthorization(options => {
                options.AddPolicy("CPFPolicy", policy => policy.RequireClaim("CPF", "XX", "YYY", "ZZZ"));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TicketComplaint", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TicketComplaint v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseProblemDetails().UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
