using System.Text.Json.Serialization;

public class FlightResponseDto
{
    public string Source { get; set; }
    public string Price { get; set; }
    public string DepartureDateTime { get; set; }
    public string ArrivalDateTime { get; set; }
    public int NumberOfStops { get; set; }

    public FlightResponseDto(string source, string price, string departureDateTime, string arrivalDateTime, int numberOfStops)
    {
        Source = source;
        Price = price;
        DepartureDateTime = departureDateTime;
        ArrivalDateTime = arrivalDateTime;
        NumberOfStops = numberOfStops;
    }
}