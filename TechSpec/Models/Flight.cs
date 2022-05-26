using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using TechSpec.Enums;

namespace TechSpec.Models;

public class Flight
{
    public Guid Id { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    
    public DateTimeOffset Departure { get; set; }
    public DateTimeOffset Arrival { get; set; }
    public FlightStatuses Status { get; set; }

}