using AznetlearningADAuth.API;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Net.Http.Headers;

namespace AznetlearningADAuth.Client.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public List<WeatherForecast> lstWeather { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<IActionResult> OnGet()
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var httpClient = _httpClientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7197/WeatherForecast");
            request.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);

            var response = await httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            lstWeather = JsonConvert.DeserializeObject<List<WeatherForecast>>(content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(response.StatusCode.ToString());
            }

            return Page();
        }
    }
}