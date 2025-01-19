using Microsoft.AspNetCore.Components;
using PtgExpressHub.Web.Runtime;

namespace PtgExpressHub.Web.Components.Pages;

public partial class Dashboard
{
    private bool isAuthenticated;

    protected override async Task OnInitializedAsync()
    {
        isAuthenticated = await AuthService.IsAuthorizedAsync();

        if (!isAuthenticated)
        {
            NavigationManager.NavigateTo("/auth/login");
        }
    }
}
