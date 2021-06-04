using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace exe2.Controllers
{
    [ApiController]
    [Route("api/[controller]")] //api/products
    public class ProductsController : ControllerBase
    {
        private static Dictionary<int, string> products = new Dictionary<int, string>();
        private IConfiguration configuration;


        public ProductsController(IConfiguration configuration)
        {
            if(!products.Any()) {
                products.Add(1, "Caneta");
                products.Add(2, "Caderno");
            }

            this.configuration = configuration;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] string name) 
        {
            if(!string.IsNullOrEmpty(name))
            {
                var result = products.Where(p => p.Value.Contains(name));
                return Ok(result);
            }
            
            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id) 
        {
            var result = products.First(p => p.Key == id);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id) 
        {
            if(products.Any(p => p.Key == id))
            {
                products.Remove(id);
                return Ok(); 
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] ProductsDto dto)
        {
            products.Add(dto.Id, dto.Name);

            return Created("api/products/", dto.Id);
        }
    }

    public class ProductsDto {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}