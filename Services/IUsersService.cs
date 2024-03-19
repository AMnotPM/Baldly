namespace Baldly.Services;

public interface IUsersService
{
    Task<List<AppUser>> GetUsersAsync();
}