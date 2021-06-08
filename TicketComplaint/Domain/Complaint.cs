namespace TicketComplaint.Domain
{
    public class Complaint
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public Ticket Ticket { get; set; }
    }
}