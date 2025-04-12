using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace PtgExpressHub.Web.Components.Pages;

public partial class Login
{
    [SupplyParameterFromForm]
    public UserCredentials Credentials { get; set; } = new();

    [CascadingParameter]
    public HttpContext HttpContext { get; set; } = default!;

    private string ErrorMessage { get; set; } = string.Empty;

    protected override void OnInitialized()
    {
        if (HttpContext != null && HttpContext.User.Identity.IsAuthenticated)
        {
            _navigationManager.NavigateTo("/");
        }        
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await _jsRuntime.InvokeVoidAsync("checkAndHideExitBtn");        
    }

    private async Task HandleLoginAsync()
    {
        var isAuthentificated = _authenticationService.LoginAsync(Credentials.Username!, Credentials.Password!, HttpContext).Result;
        if (isAuthentificated)
        {
            await Task.Run(() => _navigationManager.NavigateTo("/", true));
        }            
        else
        {
            ErrorMessage = "Логин или пароль введены неверно.";
        }
    }

    public class UserCredentials
    {
        public string? Username { get; set; }

        public string? Password { get; set; }
    }
}
