using System.ComponentModel.DataAnnotations;

namespace TechSpec.Models;

public class Role
{
    public Guid Id { get; set; }
    public string Code { get; set; }
}