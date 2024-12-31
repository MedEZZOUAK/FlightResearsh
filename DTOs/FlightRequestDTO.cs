using System.Text.Json.Serialization;

namespace FlightSearchAPI.DTOs
{
    public class FlightRequestDTO
    {
        [JsonPropertyName("departureCity")]
        public string DepartureCity { get; set; }

        [JsonPropertyName("arrivalCity")]
        public string ArrivalCity { get; set; }

        [JsonPropertyName("departureDate")]
        public string DepartureDate { get; set; }

        [JsonPropertyName("returnDate")]
        public string ReturnDate { get; set; }

        [JsonPropertyName("passengers")]
        public int Passengers { get; set; }

        [JsonPropertyName("class")]
        public string Class { get; set; }

        [JsonPropertyName("flightType")]
        public string FlightType { get; set; } // Aller-Retour / Aller Simple
    }
}