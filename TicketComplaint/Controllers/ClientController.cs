using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TicketComplaint.Domain;
using TicketComplaint.Dtos;
using TicketComplaint.Infra.Db;

namespace TicketComplaint.Controllers
{
    
    [ApiController]
    [Route("v1/client")]
    public class ClientController : ControllerBase
    {
        private readonly ApplicationDbContext applicationDbContext;

        public ClientController(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        
        [HttpPost]
        public IActionResult Post([FromBody] ClientDto dto)
        {
            Client client = new() { Name = dto.Name, Email = dto.Email};
            applicationDbContext.Clients.Add(client);
            applicationDbContext.SaveChanges();
            return Created("v1/client/" + client.Id, client.Id);
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var client = applicationDbContext.Clients.First(c => c.Id == id);
            return Ok(client);
        }

        [HttpGet]
        public IActionResult List([FromQuery] int page)
        {
            if(page < 1)
                throw new InvalidOperationException("Page invalid");
            
            var row = 5;
            var clients = applicationDbContext.Clients.Skip(row * (page -1)).Take(row);
            
            return Ok(clients);
        }
    }
}
