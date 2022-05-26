using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TechSpec.Models;
using TechSpec.Services;

namespace TechSpec.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : Controller
{
    private readonly Context _db;
    public AccountController(Context context)
    {
        _db = context;
        if (!_db.Roles.Any())
        {
            _db.Roles.Add(new Role { Code = "user" });
            _db.Roles.Add(new Role { Code = "moderator" });
            _db.SaveChanges();
        }
        if (!_db.Users.Any())
        {
            _db.Users.Add(new User { Username = "Tom", Password = "12345Aa", RoleId = _db.Roles.ToList()[0].Id, Role = _db.Roles.ToList()[0] });
            _db.Users.Add(new User { Username = "Alice", Password = "12345Aa", RoleId = _db.Roles.ToList()[1].Id, Role = _db.Roles.ToList()[1] });
            _db.SaveChanges();
        }

    }
    
    [HttpGet("/AllRoles")]
    public async Task<ActionResult<IEnumerable<Role>>> GetRole()
    {
        try
        {
            return await _db.Roles.ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("Произошла ошибка, попробуйте позже");
        }
        
    }
    
    [HttpGet("/AllUsers")]
    public async Task<ActionResult<IEnumerable<User>>> Get()
    {
        try
        {
            return await _db.Users.ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("Произошла ошибка, попробуйте позже");
        }
        
    }

    [HttpPost("/Auth")]
    public IActionResult Token(string username, string password)
    {
        var identity = GetIdentity(username, password);
        if (identity == null)
        {
            return BadRequest(new { errorText = "Invalid username or password." });
        }

        var now = DateTime.UtcNow;
        
        var jwt = new JwtSecurityToken(
                issuer: AuthOptions.Issuer,
                audience: AuthOptions.Audience,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.Lifetime)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        var response = new
        {
            access_token = encodedJwt,
            username = identity.Name
        };

        return Json(response);
    }

    private ClaimsIdentity GetIdentity(string username, string password)
    {
        User user = _db.Users.FirstOrDefault(x => x.Username == username && x.Password == password);
        if (user != null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Username),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.Code)
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }
        
        return null;
    }

}