using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SecondApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StockController : ControllerBase
    {
        private readonly ILogger<StockController> _logger;

        public StockController(ILogger<StockController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("{id}")]
        public int Get(int id)
        {
            return id switch 
            {
                1 => 100,
                2 => 30,
                3 => 40,
                _ => 10
            };
        }

         [HttpGet]
        [Route("{id}/special")]
        public int GetSpecialStock(int id)
        {
            return id switch 
            {
                1 => 4,
                2 => 5,
                _ => 10
            };
        }
    }
}
