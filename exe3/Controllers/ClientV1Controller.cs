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
    [Route("v1/client")]
    public class ClientV1Controller : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody] ClientV1Dto dto)
        {
            return Created("url", dto.Id);
        }
    }

    public class ClientV1Dto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ClientV1DtoValidation : AbstractValidator<ClientV1Dto>
    {
        public ClientV1DtoValidation()
        {
            RuleFor(d => d.Id).Must(id => id > 0).WithMessage("Id invalid");
            RuleFor(d => d.Name).NotEmpty();
        }
    }
}
