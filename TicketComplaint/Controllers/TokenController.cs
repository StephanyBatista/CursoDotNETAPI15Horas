using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using TicketComplaint.Domain;
using TicketComplaint.Dtos;
using TicketComplaint.Infra.Db;

namespace TicketComplaint.Controllers
{
    [ApiController]
    [Route("v1/token")]
    public class TokenController : ControllerBase
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;

        public TokenController(ApplicationDbContext applicationDbContext, UserManager<IdentityUser> userManager,
            IConfiguration configuration)
        {
            this.applicationDbContext = applicationDbContext;
            this.userManager = userManager;
            this.configuration = configuration;
        }
        
        [HttpPost]
        public async Task<IActionResult> Post(LoginDto dto)
        {
            var user = await userManager.FindByEmailAsync(dto.Email);
            if(user == null)
                throw new InvalidOperationException("Login invalid");

            var result = userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

            if(result == PasswordVerificationResult.Failed)
                throw new InvalidOperationException("Login invalid");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration.GetValue<string>("JwtBearerTokenSettings:SecretKey"));
            var expires = DateTime.UtcNow.AddSeconds(10);
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            });
            identity.AddClaims(await userManager.GetClaimsAsync(user));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = expires,
                SigningCredentials = 
                            new SigningCredentials(
                                new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = configuration.GetValue<string>("JwtBearerTokenSettings:Audience"),
                Issuer = configuration.GetValue<string>("JwtBearerTokenSettings:Issuer")
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new {
                token = tokenHandler.WriteToken(token),
                expires
            });
        }
    }

    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
