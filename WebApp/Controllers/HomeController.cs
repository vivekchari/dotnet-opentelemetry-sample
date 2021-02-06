using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _configuration = configuration;
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
            // var activity = new Activity("CallToBackend").Start();

            var apiUrl = _configuration["Settings:ApiUrl"];
            _logger.LogInformation($"Calling Api Url: {apiUrl}/data");
            var client = _clientFactory.CreateClient("FirstApiClient");
            HttpResponseMessage response = await client.GetAsync($"{apiUrl}/data");
            _logger.LogInformation($"Api response: {response.StatusCode}");


            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsAsync<IEnumerable<ApiResponse>>();
                // activity.Stop();
                return View(data);
            }
            // activity.Stop();
            return View(Enumerable.Empty<ApiResponse>());
        }

        public async Task<IActionResult> SpecialProducts()
        {
            var activity = Activity.Current;
            activity.DisplayName = "GetSpecialProducts";
            activity.SetTag("request.userid", "TestUser");
            activity.AddEvent(new ActivityEvent("SpecialProductsRequested"));
            var apiUrl = _configuration["Settings:ApiUrl"];
            var client = _clientFactory.CreateClient("FirstApiClient");
            HttpResponseMessage response = await client.GetAsync($"{apiUrl}/data/special");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsAsync<IEnumerable<ApiResponse>>();
                return View(data);
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
