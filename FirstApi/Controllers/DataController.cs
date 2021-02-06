using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FirstApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<DataController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _secondApiUrl;
        public DataController(ILogger<DataController> logger,
            IConfiguration configuration,
            IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _configuration = configuration;
            _secondApiUrl = _configuration["Settings:ApiUrl"];

            _clientFactory = clientFactory;
        }

        [HttpGet]
        public async Task<IEnumerable<FirstApiResponse>> Get()
        {
            _logger.LogInformation($"DataController GET called.");
            _logger.LogInformation($"Calling Api Url: {_secondApiUrl}/data");
            var client = _clientFactory.CreateClient("SecondApi");
            
            var productTask1 = client.GetAsync($"{_secondApiUrl}/stock/1");
            var productTask2 = client.GetAsync($"{_secondApiUrl}/stock/2");
            var productTask3 = client.GetAsync($"{_secondApiUrl}/stock/3");

            Task.WaitAll(productTask1, productTask2, productTask3);

            int stock1 = 0, stock2 = 0, stock3 = 0;

            if (productTask1.Result.IsSuccessStatusCode)
            {
                stock1 = await productTask1.Result.Content.ReadAsAsync<int>();
            }
            if (productTask2.Result.IsSuccessStatusCode)
            {
                stock2 = await productTask2.Result.Content.ReadAsAsync<int>();
            }
            if (productTask3.Result.IsSuccessStatusCode)
            {
                stock3 = await productTask3.Result.Content.ReadAsAsync<int>();
            }

            return new List<FirstApiResponse>{
                new FirstApiResponse { ProductName = "Product1", Stock=stock1 },
                new FirstApiResponse { ProductName = "Product2", Stock=stock2 },
                new FirstApiResponse { ProductName = "Product3", Stock=stock3 },
            };
        }

        [HttpGet]
        [Route("special")]
        public async Task<IEnumerable<FirstApiResponse>> GetSpecialProducts()
        {
            var secondApiUrl = _configuration["Settings:ApiUrl"];
            var client = _clientFactory.CreateClient("SecondApi");

            var productTask1 = client.GetAsync($"{_secondApiUrl}/stock/1");
            var productTask2 = client.GetAsync($"{_secondApiUrl}/stock/2");
             int stock1 = 0, stock2 = 0;

            if (productTask1.Result.IsSuccessStatusCode)
            {
                stock1 = await productTask1.Result.Content.ReadAsAsync<int>();
            }
            if (productTask2.Result.IsSuccessStatusCode)
            {
                stock2 = await productTask2.Result.Content.ReadAsAsync<int>();
            }
            return new List<FirstApiResponse>{
                new FirstApiResponse { ProductName = "Product1", Stock=stock1 },
                new FirstApiResponse { ProductName = "Product2", Stock=stock2 }
            };
        }
    }
}
