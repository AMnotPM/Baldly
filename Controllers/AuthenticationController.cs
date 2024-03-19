namespace Baldly.Controllers;

public class AuthenticationController : Controller
{
    private readonly IUsersService _usersService;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;

    public AuthenticationController(IUsersService usersService,
        SignInManager<AppUser> signInManager,
        UserManager<AppUser> userManager)
    {
        _usersService = usersService;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task<IActionResult> Users()
    {
        var users = await _usersService.GetUsersAsync();
        return View(users);
    }

    public Task<IActionResult> Login()
    {
        return Task.FromResult<IActionResult>(View(new LoginVm()));
    }

    public async Task<IActionResult> LoginSubmitted(LoginVm loginVm)
    {
        if (!ModelState.IsValid) return View("Login", loginVm);

        if (loginVm.EmailAddress != null)
        {
            var user = await _userManager.FindByEmailAsync(loginVm.EmailAddress);
            if (user != null)
            {
                var userPasswordCheck = loginVm.Password != null &&
                                        await _userManager.CheckPasswordAsync(user, loginVm.Password);
                if (userPasswordCheck)
                {
                    if (loginVm.Password != null)
                    {
                        var userLoggedIn =
                            await _signInManager.PasswordSignInAsync(user, loginVm.Password, false, false);

                        if (userLoggedIn.Succeeded)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError("",
                                "Invalid login attempt. Please, check your username and password");
                            return View("Login", loginVm);
                        }
                    }
                }
                else
                {
                    await _userManager.AccessFailedAsync(user);

                    if (await _userManager.IsLockedOutAsync(user))
                    {
                        ModelState.AddModelError("", "Your account is locked, please try again in 10 mins");
                        return View("Login", loginVm);
                    }

                    ModelState.AddModelError("", "Invalid login attempt. Please, check your username and password");
                    return View("Login", loginVm);
                }
            }
        }


        return RedirectToAction("Index", "Home");
    }

    public Task<IActionResult> Register()
    {
        return Task.FromResult<IActionResult>(View(new RegisterVm()));
    }

    public async Task<IActionResult> RegisterUser(RegisterVm registerVm)
    {
        if (!ModelState.IsValid) return View("Register", registerVm);

        //Check if the user exists
        if (registerVm.EmailAddress != null)
        {
            var user = await _userManager.FindByEmailAsync(registerVm.EmailAddress);
            if (user != null)
            {
                ModelState.AddModelError("", "Email address is already in use.");
                return View("Register", registerVm);
            }
        }

        var newUser = new AppUser()
        {
            Email = registerVm.EmailAddress,
            UserName = registerVm.EmailAddress,
            FullName = registerVm.FullName,
            LockoutEnabled = true
        };

        if (registerVm.Password != null)
        {
            var userCreated = await _userManager.CreateAsync(newUser, registerVm.Password);
            if (userCreated.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, Role.User);

                //Login the user
                await _signInManager.PasswordSignInAsync(newUser, registerVm.Password, false, false);
            }
            else
            {
                foreach (var error in userCreated.Errors) ModelState.AddModelError("", error.Description);

                return View("Register", registerVm);
            }
        }

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}