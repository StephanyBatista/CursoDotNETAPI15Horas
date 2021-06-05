using System.Collections.Generic;
using TicketComplaint.Domain;

namespace TicketComplaint
{
    public static class Repository
    {
        public static List<Client> Clients { get; set; } = new List<Client>();
        public static List<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}