var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    "specific",
    "{controller=Home}/{action=Index}/{userId}/{linkId}");
app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");

//Seed the database
DbInitializer.SeedDefaultUsersAndRolesAsync(app).Wait();

// Recover the original url and redirect the user to it
app.MapFallback(async (AppDbContext db, HttpContext ctx) =>
{
    var path = ctx.Request.Path.ToUriComponent().Trim('/');
    if (db.Urls != null)
    {
        var urlMatch = await db.Urls.FirstOrDefaultAsync(x =>
            x.ShortLink != null && x.ShortLink.Trim() == path.Trim());

        if (urlMatch == null)
            return Results.BadRequest("Invalid request");

        if (urlMatch.OriginalLink != null) return Results.Redirect(urlMatch.OriginalLink);
    }

    throw new InvalidOperationException();
});

app.Run();