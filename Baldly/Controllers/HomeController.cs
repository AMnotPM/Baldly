namespace Baldly.Controllers;

public class HomeController : Controller
{
    private AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var newUrl = new PostUrlVm();
        return View(newUrl);
    }

    public IActionResult ShortenUrl(PostUrlVm postUrlVm)
    {
        //Validate the Model
        if (!ModelState.IsValid) return View("Index", postUrlVm);

        var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var newUrl = new Url()
        {
            OriginalLink = postUrlVm.Url,
            ShortLink = GenerateShortUrl(6),
            NrOfClicks = 0,
            UserId = loggedInUserId,
            DateCreated = DateTime.UtcNow
        };


        // Construct the url
        var resultUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/{newUrl.ShortLink}";

        _context.Urls?.Add(newUrl);
        _context.SaveChanges();

        TempData["Message"] = $"Your url was shorted successfully to {resultUrl}";

        return RedirectToAction("Index");
    }

    private string GenerateShortUrl(int length)
    {
        var random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        return new string(
            Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)])
                .ToArray()
        );
    }
}