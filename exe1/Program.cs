using System;
using FluentColorConsole;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace exe1
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = 
                new WebHostBuilder()
                .UseKestrel()
                .Configure(app => 
                    app.Run(context => context.Response.WriteAsync("teste 3")))
                .Build();

            host.Run();
        }
    }
}
