using System.Diagnostics;
using FlightResearsh.Models;
using FlightSearchAPI.DTOs;
using FlightSearchAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightResearsh.Controllers
{
  public class HomeController : Controller
  {
    private readonly AmadeusFlightService _flightService;

    public HomeController(AmadeusFlightService flightService)
    {
      _flightService = flightService;
    }

    public IActionResult Index()
    {
      return View();
    }

    [HttpPost]
    public async Task<IActionResult> SearchFlights([FromBody] FlightRequestDTO request)
    {
      var isRoundTrip = request.FlightType == "round-trip";
      var flights = await _flightService.SearchFlightsAsync(
          request.DepartureCity,
          request.ArrivalCity,
          request.DepartureDate,
          !isRoundTrip,
          request.ReturnDate,
          request.Passengers
      );
      Console.WriteLine(Json(flights));
      return Json(flights);
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
