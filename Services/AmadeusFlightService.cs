using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using FlightSearchAPI.DTOs;

namespace FlightSearchAPI.Services
{
    public class AmadeusFlightService
    {
        private readonly HttpClient _httpClient;
        private readonly AmadeusAuthService _authService;
        private readonly string _baseUrl = "https://test.api.amadeus.com/v2";

        public AmadeusFlightService(HttpClient httpClient, AmadeusAuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
        }

        /// <summary>
        /// Recherche les vols en fonction des paramètres fournis.
        /// </summary>
        public async Task<List<FlightResponseDto>> SearchFlightsAsync(string origin, string destination, string departureDate, Boolean oneWay, string gobackDate, int adults)
        {
            // Obtenir le token d'authentification
            var token = await _authService.GetAccessTokenAsync();

            // Configurer l'en-tête d'autorisation
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = $"{_baseUrl}/shopping/flight-offers?" +
                        $"originLocationCode={origin}&" +
                        $"destinationLocationCode={destination}&" +
                        $"departureDate={departureDate}&" +
                        $"adults={adults}";
            if (!oneWay)
            {
                url = url + $"&returnDate={gobackDate}";
            }

            // Effectuer la requête GET
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new System.Exception($"Erreur API externe : {response.StatusCode} - {errorContent}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            //check if there is a flight offer
            if (result.Data.Count == 0)
            {
                Console.WriteLine("No flight offer found");
            }
            var flightOffers = new List<FlightResponseDto>();
            foreach (var offer in result.Data)
            {
                var SegmentsList= new List<SegmentsDTO>();
                foreach (var itinerary in offer.Itineraries)
                {
                    // Extracting the first and last segments (departure and arrival)
                    var departure = itinerary.Segments.FirstOrDefault()?.Departure;
                    var arrival = itinerary.Segments.LastOrDefault()?.Arrival;
                    var carrierCode = itinerary.Segments.FirstOrDefault()?.CarrierCode ?? "Unknown";
                    var hasWifi = offer.Services?.HasWifi ?? false;
                    var hasPowerOutlet = offer.Services?.HasPowerOutlet ?? false;
                    var hasMealService = offer.Services?.HasMealService ?? false;
                    foreach(var segment in itinerary.Segments){
                        var DepartureSegment = segment.Departure.IataCode;
                        var DepartureDate = segment.Departure.At;
                        var ArrivalSegment = segment.Arrival.IataCode;
                        var ArrivalDate = segment.Arrival.At;
                        var dto= new SegmentsDTO(
                            DepartureDate,
                            ArrivalDate,
                            DepartureSegment,
                            ArrivalSegment
                        );
                        SegmentsList.Add(dto);
                    }
                    // If the itinerary is valid and has departure and arrival information
                    if (departure != null && arrival != null)
                    {
                        var price = offer.Price.GrandTotal;
                        var source = offer.Source;
                        var numberOfStops = itinerary.Segments.Count();
                        numberOfStops=numberOfStops-1;
                        var dto = new FlightResponseDto(
                            source,
                            price,
                            departure.At,
                            arrival.At,
                            numberOfStops,
                            carrierCode,
                            hasWifi,
                            hasPowerOutlet,
                            hasMealService,
                            SegmentsList
                        );
                        
                        flightOffers.Add(dto);
                    }
                }
            }
            return flightOffers;
        }
    }
}
