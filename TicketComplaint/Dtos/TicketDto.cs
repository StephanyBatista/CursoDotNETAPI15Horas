using System.Collections.Generic;
using FluentValidation;

namespace TicketComplaint.Dtos
{
    public class TicketDto
    {
        public int ClientId { get; set; }
        public List<string> Complaints { get; set; }
    }

    public class TicketDtoValidator : AbstractValidator<TicketDto>
    {
        public TicketDtoValidator()
        {
            RuleFor(c => c.ClientId).Must(id => id > 0);
            RuleFor(c => c.Complaints).NotEmpty();
        }
    }
}