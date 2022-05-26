using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechSpec.Enums;
using TechSpec.Models;

namespace TechSpec.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FlightsController : ControllerBase
{
    private readonly Context _db;
    public FlightsController(Context context)
    {
        _db = context;
    }

    [Authorize]
    [HttpGet("/allFlights")]
    public async Task<ActionResult<IEnumerable<Flight>>> Get()
    {
        try
        {
            return await _db.Flights.ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("Произошла ошибка, попробуйте позже");
        }
        
    }
    
    [Authorize(Roles = "moderator")]
    [HttpPost("/addNewFlight")]
    public async Task<ActionResult<Flight>> Get(string origin, string destination, DateTimeOffset departure, DateTimeOffset arrival)
    {
        try
        {
            if (!String.IsNullOrEmpty(origin) & !String.IsNullOrEmpty(destination) & departure != new DateTimeOffset() & arrival != new DateTimeOffset())
            {
                Flight flight = new Flight()
                {
                    Origin = origin,
                    Destination = destination,
                    Departure = departure,
                    Arrival = arrival,
                    Status = FlightStatuses.InTime
                };

                 _db.Flights.Add(flight);
                 await _db.SaveChangesAsync();
                
                return new ObjectResult(flight);
            }
            return Content("Не указаны нужные параметры");
        }
        
        catch (DbUpdateException e)
        {
            Console.WriteLine(e.Message);
            return BadRequest("Неверный формат даты и времени");
        }
        
        catch (Exception e)
        {
             Console.WriteLine(e.Message);
             return BadRequest("Произошла ошибка, попробуйте позже");
        }
        
    }
    
    [Authorize(Roles = "moderator")]
    [HttpPut("/editFlight")]
    public async Task<ActionResult<Flight>> Put(Guid? id, FlightStatuses? status)
    {
        try
        {
            if (id != null & status != null)
            {
                var flight = _db.Flights.FirstOrDefault(f => f.Id == id);
                if (flight != null)
                {
                    flight.Status = (FlightStatuses)status;
                    
                    _db.Update(flight);
                    await _db.SaveChangesAsync();
                    return Ok(flight);
                }
            }

            return Content("рейс не найден");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("Произошла ошибка, попробуйте позже");
        }
        
    }

    
    [Authorize(Roles = "moderator")]
    [HttpDelete("/deleteFlight")]
    public async Task<ActionResult<Flight>> Delete(Guid id)
    {
        try
        {
            Flight flight = _db.Flights.FirstOrDefault(x => x.Id == id);
            if (flight == null)
            {
                return NotFound();
            }
            _db.Flights.Remove(flight);
            await _db.SaveChangesAsync();
            
            return Ok(flight);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("Произошла ошибка, попробуйте позже");
        }
        
    }

    

}