namespace Baldly.Services;

public class UsersService(AppDbContext context) : IUsersService
{
    public async Task<List<AppUser>> GetUsersAsync()
    {
        var users = await context.Users.Include(n => n.Urls).ToListAsync();
        return users;
    }
}