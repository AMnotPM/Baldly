namespace Baldly.Services;

public interface IUrlsService
{
    Task<List<Url>> GetUrlsAsync(string? userId, bool isAdmin);
    Task<Url> AddAsync(Url url);
    Task<Url?> GetByIdAsync(int id);
    Task<Url?> UpdateAsync(int id, Url url);
    Task DeleteAsync(int id);
}