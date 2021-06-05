using FluentValidation;

namespace TicketComplaint.Dtos
{
    public class ClientDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class ClientDtoValidator : AbstractValidator<ClientDto>
    {
        public ClientDtoValidator()
        {
            RuleFor(c => c.Name).NotEmpty().MaximumLength(4);
            RuleFor(c => c.Email).NotEmpty().EmailAddress().MaximumLength(80);
        }
    }
}