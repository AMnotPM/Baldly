namespace Baldly.Models;

public class AppUser : IdentityUser
{
    public string? FullName { get; set; }
    public List<Url> Urls { get; set; } = new();
}