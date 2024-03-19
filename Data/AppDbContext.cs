using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Baldly.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppUser>(options)
{
    public DbSet<Url>? Urls { get; set; }
}