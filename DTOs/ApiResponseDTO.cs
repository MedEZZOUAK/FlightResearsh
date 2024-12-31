public class ApiResponse
{
    public Meta Meta { get; set; }
    public List<FlightOffer> Data { get; set; }
}

public class Meta
{
    public int Count { get; set; }
    public Links Links { get; set; }
}

public class Links
{
    public string Self { get; set; }
}

public class FlightOffer
{
    public string Source { get; set; }
    public Price Price { get; set; }
    public List<Itinerary> Itineraries { get; set; }
}

public class Price
{
    public string Currency { get; set; }
    public string GrandTotal { get; set; }
}

public class Itinerary
{
    public List<Segment> Segments { get; set; }
}

public class Segment
{
    public Departure Arrival { get; set; }
    public Departure Departure { get; set; }
    public int NumberOfStops { get; set; }
}

public class Departure
{
    public string IataCode { get; set; }
    public string At { get; set; }
    public string Terminal { get; set; }
}