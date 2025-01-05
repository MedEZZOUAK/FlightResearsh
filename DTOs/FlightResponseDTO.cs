using System.Text.Json.Serialization;
using FlightSearchAPI.DTOs;

public class FlightResponseDto
{
    public string Source { get; set; }
    public string Price { get; set; }
    public string DepartureDateTime { get; set; }
    public string ArrivalDateTime { get; set; }
    public int NumberOfStops { get; set; }
    public string CarrierCode { get; set; }
    public bool HasWifi { get; set; }
    public bool HasPowerOutlet { get; set; }
    public bool HasMealService { get; set; }
    public List<SegmentsDTO> Segments {get ; set;}

    public FlightResponseDto(
        string source, 
        string price, 
        string departureDateTime, 
        string arrivalDateTime, 
        int numberOfStops,
        string carrierCode,
        bool hasWifi,
        bool hasPowerOutlet,
        bool hasMealService,
        List<SegmentsDTO> segments)
    {
        Source = source;
        Price = price;
        DepartureDateTime = departureDateTime;
        ArrivalDateTime = arrivalDateTime;
        NumberOfStops = numberOfStops;
        CarrierCode = carrierCode;
        HasWifi = hasWifi;
        HasPowerOutlet = hasPowerOutlet;
        HasMealService = hasMealService;
        Segments=segments;
    }
}