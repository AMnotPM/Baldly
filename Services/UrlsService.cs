namespace Baldly.Services;

public class UrlsService : IUrlsService
{
    private AppDbContext _context;

    public UrlsService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Url?> GetByIdAsync(int id)
    {
        if (_context.Urls != null)
        {
            var url = await _context.Urls.FirstOrDefaultAsync(n => n.Id == id);
            return url;
        }

        throw new InvalidOperationException();
    }

    public async Task<List<Url>> GetUrlsAsync(string? userId, bool isAdmin)
    {
        if (_context.Urls != null)
        {
            var allUrlsQuery = _context.Urls.Include(n => n.User);

            if (isAdmin)
                return await allUrlsQuery.ToListAsync();
            else
                return await allUrlsQuery.Where(n => n.UserId == userId).ToListAsync();
        }

        throw new InvalidOperationException();
    }

    public async Task<Url> AddAsync(Url url)
    {
        if (_context.Urls != null) await _context.Urls.AddAsync(url);
        await _context.SaveChangesAsync();

        return url;
    }

    public async Task<Url?> UpdateAsync(int id, Url url)
    {
        if (_context.Urls != null)
        {
            var urlDb = await _context.Urls.FirstOrDefaultAsync(n => n.Id == id);
            if (urlDb != null)
            {
                urlDb.OriginalLink = url.OriginalLink;
                urlDb.ShortLink = url.ShortLink;
                urlDb.DateUpdated = DateTime.UtcNow;

                await _context.SaveChangesAsync();
            }

            return urlDb;
        }

        throw new InvalidOperationException();
    }

    public async Task DeleteAsync(int id)
    {
        if (_context.Urls != null)
        {
            var urlDb = await _context.Urls.FirstOrDefaultAsync(n => n.Id == id);

            if (urlDb != null)
            {
                _context.Remove(urlDb);
                await _context.SaveChangesAsync();
            }
        }
    }
}