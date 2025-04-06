using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

public class AuthService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(IConfiguration config, IHttpContextAccessor httpContextAccessor, AuthenticationStateProvider authenticationStateProvider)
    {
        _configuration = config;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> LoginAsync(string username, string password, HttpContext httpContext)
    {
        if (_configuration["SingleUser:Login"] != username || _configuration["SingleUser:Password"] != password)
            return false;

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await httpContext.SignInAsync(principal);

        return true;
    }

    public async Task LogOutUserAsync(HttpContext httpContext)
    {
        await httpContext.SignOutAsync();
    }

    public bool IsUserAuthenticated()
    {
        if (_httpContextAccessor == null)
            return false;

        return _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
    }
}