using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace FlightSearchAPI.Services
{
    public class AmadeusAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private string _accessToken;
        private DateTime _tokenExpiry;

        private readonly string _authUrl = "https://test.api.amadeus.com/v1/security/oauth2/token";

        public AmadeusAuthService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        /// <summary>
        /// Récupère le token d'accès depuis l'API Amadeus.
        /// </summary>
        public async Task<string> GetAccessTokenAsync()
        {
            // Retourner le token existant s'il est encore valide
            if (!string.IsNullOrEmpty(_accessToken) && _tokenExpiry > DateTime.UtcNow)
            {
                return _accessToken;
            }

            // Préparer le contenu de la requête
            var clientId = _configuration["Amadeus:ApiKey"];
            var clientSecret = _configuration["Amadeus:ApiSecret"];

            if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
            {
                throw new System.Exception("Les identifiants Amadeus (ClientId et ClientSecret) ne sont pas configurés.");
            }

            var content = new StringContent(
                $"grant_type=client_credentials&client_id={clientId}&client_secret={clientSecret}",
                Encoding.UTF8,
                "application/x-www-form-urlencoded"
            );

            // Effectuer la requête POST pour obtenir le token
            var response = await _httpClient.PostAsync(_authUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new System.Exception($"Erreur d'authentification API : {response.StatusCode} - {errorContent}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Amadeus Auth API Response: {responseContent}");
            var authResponse = JsonSerializer.Deserialize<AuthResponse>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (authResponse == null || string.IsNullOrEmpty(authResponse.access_token))
            {
                throw new System.Exception("Réponse d'authentification invalide depuis Amadeus.");
            }

            _accessToken = authResponse.access_token;
            _tokenExpiry = DateTime.UtcNow.AddSeconds(authResponse.expires_in - 60); // Moins 60 secondes de marge

            return _accessToken;
        }

        /// <summary>
        /// Modèle pour désérialiser la réponse d'authentification.
        /// </summary>
        private class AuthResponse
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public int expires_in { get; set; }
        }
    }
}
