using Microsoft.AspNetCore.Components.Authorization;

namespace PtgExpressHub.Web.Runtime;

public class AuthService
{
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    public AuthService(AuthenticationStateProvider authenticationStateProvider)
    {
        _authenticationStateProvider = authenticationStateProvider;
    }

    public bool AuthenticateUser(string username, string password)
    {
        if (username == "admin" && password == "1234")
        {
            ((CustomAuthenticationStateProvider)_authenticationStateProvider!).AuthenticateUser(username);
            return true;
        }

        return false;
    }

    public void LogoutUser()
    {

    }

    public async Task<bool> IsAuthorizedAsync()
    {
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        return authState != null &&
            authState.User.Identity != null &&
            authState.User.Identity.IsAuthenticated;
    }
}
