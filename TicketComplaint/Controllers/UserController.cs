using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TicketComplaint.Domain;
using TicketComplaint.Dtos;
using TicketComplaint.Infra.Db;

namespace TicketComplaint.Controllers
{
    [ApiController]
    [Route("v1/user")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly UserManager<IdentityUser> userManager;

        public UserController(ApplicationDbContext applicationDbContext, UserManager<IdentityUser> userManager)
        {
            this.applicationDbContext = applicationDbContext;
            this.userManager = userManager;
        }
        
        
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new {
                Name = User.Identity.Name,
                Role = User.Claims.First(c => c.Type == ClaimTypes.Role).Value
            });
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserDto dto)
        {
            var user = new IdentityUser { UserName = dto.UserName, Email = dto.Email};
            var result = await userManager.CreateAsync(user, dto.Password);

            if(!result.Succeeded)
                throw new InvalidOperationException(result.Errors.First().Description);

            var nameClaim = new Claim(ClaimTypes.Name, dto.Name);
            var cpfClaim = new Claim("CPF", dto.CPF);

            await userManager.AddClaimAsync(user, nameClaim);
            await userManager.AddClaimAsync(user, cpfClaim);

            return Ok();
        }
    }

    public class UserDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string CPF { get; set; }
    }
}
