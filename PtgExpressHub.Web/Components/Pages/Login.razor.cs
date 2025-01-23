namespace PtgExpressHub.Web.Components.Pages;

public partial class Login
{
    private UserCredentials credentials = new();

    private string ErrorMessage { get; set; } = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        if (await _authService.IsAuthorizedAsync())
        {
            _navigation.NavigateTo("/");
        }
    }

    private void HandleLogin()
    {        
        if (_authService.AuthenticateUser(credentials.Username!, credentials.Password!))
        {
            _navigation.NavigateTo("/");
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
