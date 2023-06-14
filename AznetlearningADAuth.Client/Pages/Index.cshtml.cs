using AznetlearningADAuth.API;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
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

            //SecretClient secretClient = new SecretClient(new Uri("https://az-learning-key-vault.vault.azure.net/"),
            //   new DefaultAzureCredential());

            //KeyVaultSecret keyVaultSecret = secretClient.GetSecret("cosmos-db-connectionstring");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(response.StatusCode.ToString());
            }

            return Page();
        }
    }
}