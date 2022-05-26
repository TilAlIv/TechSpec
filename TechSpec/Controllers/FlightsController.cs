using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        if (!_db.Users.Any())
        {
            _db.Users.Add(new User { Username = "Tom", Password = "12345Aa" });
            _db.Users.Add(new User { Username = "Alice", Password = "12345Aa" });
            _db.SaveChanges();
        }


    }


    

}