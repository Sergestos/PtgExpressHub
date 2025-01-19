namespace PtgExpressHub.Web.Components.Pages;

public partial class Logout
{
    protected override async Task OnInitializedAsync()
    {
        var customAuthProvider = (CustomAuthenticationStateProvider)AuthenticationStateProvider;
        await customAuthProvider.MarkUserAsLoggedOut();

        Navigation.NavigateTo("/");
    }
}
