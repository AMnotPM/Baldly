namespace Baldly.Controllers;

public class UrlController : Controller
{
    private IUrlsService _urlsService;
    private readonly IMapper _mapper;

    public UrlController(IUrlsService urlsService, IMapper mapper)
    {
        _urlsService = urlsService;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var isAdmin = User.IsInRole(Role.Admin);
        var allUrls = await _urlsService.GetUrlsAsync(loggedInUserId, isAdmin);
        var mappedAllUrls = _mapper.Map<List<Url>, List<GetUrlVm>>(allUrls);

        return View(mappedAllUrls);
    }

    public Task<IActionResult> Create()
    {
        return Task.FromResult<IActionResult>(RedirectToAction("Index"));
    }

    public async Task<IActionResult> Remove(int id)
    {
        await _urlsService.DeleteAsync(id);
        return RedirectToAction("Index");
    }
}