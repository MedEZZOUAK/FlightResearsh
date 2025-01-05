using System.Text.Json.Serialization;

public class SegmentsDTO
{
    public string DepartureSegment { get; set; }
    public string DepartureDate { get; set; }
    public string ArrivalSegment { get; set; }
    public string ArrivalDate { get; set; }

    public SegmentsDTO(
        string departureDate, 
        string arrivalDate, 
        string departure, 
        string arrival
        )
    {
        DepartureDate = departureDate;
        ArrivalDate = arrivalDate;
        DepartureSegment = departure;
        ArrivalSegment = arrival;
    }
}