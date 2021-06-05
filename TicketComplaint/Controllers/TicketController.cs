using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TicketComplaint.Domain;
using TicketComplaint.Dtos;
using TicketComplaint.Infra.Db;

namespace TicketComplaint.Controllers
{
    [ApiController]
    [Route("v1/ticket")]
    public class TicketController : ControllerBase
    {
        private readonly ApplicationDbContext applicationDbContext;

        public TicketController(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        
        [HttpPost]
        public IActionResult Post([FromBody] TicketDto dto)
        {
            var client = applicationDbContext.Clients.FirstOrDefault(c => c.Id == dto.ClientId);
            if(client == null)
                throw new InvalidOperationException("Client not found");

            var ticket = new Ticket() { Client = client, CreateOn = DateTime.Now };
            foreach (var item in dto.Complaints)
            {
                ticket.Complaints.Add(new Complaint{ Description = item });
            }
            applicationDbContext.Tickets.Add(ticket);
            applicationDbContext.SaveChanges();

            return Created("v1/ticket/" + ticket.Id, ticket.Id);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var ticket = applicationDbContext.Tickets
                .Include(t => t.Client)
                .Include(t => t.Complaints)
                .First(c => c.Id == id);
            return Ok(ticket);
        }
    }
}
