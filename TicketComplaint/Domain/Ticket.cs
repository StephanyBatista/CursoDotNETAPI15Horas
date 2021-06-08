using System;
using System.Collections.Generic;

namespace TicketComplaint.Domain
{
    public class Ticket
    {
        public int Id { get; set; }
        public Client Client { get; set; }
        public DateTime CreateOn { get; set; }
        public List<Complaint> Complaints { get; set; } = new List<Complaint>();
        public bool Resolved { get; set; }
    }
}