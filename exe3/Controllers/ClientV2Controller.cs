using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace exe3.Controllers
{
    [ApiController]
    [Route("v2/client")]
    public class ClientV2Controller : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody] ClientV2Dto dto)
        {
            return Created("url", dto.Id);
        }
    }

    public class ClientV2Dto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class ClientV2DtoValidation : AbstractValidator<ClientV2Dto>
    {
        public ClientV2DtoValidation()
        {
            RuleFor(d => d.Id).Must(id => id > 0).WithMessage("Id invalid");
            RuleFor(d => d.Name).NotEmpty();
            RuleFor(d => d.Email).NotEmpty().EmailAddress();
        }
    }
}
