using Microsoft.AspNetCore.Mvc;
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


}