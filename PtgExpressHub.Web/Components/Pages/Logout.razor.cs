﻿using Microsoft.AspNetCore.Components;

namespace PtgExpressHub.Web.Components.Pages;

public partial class Logout
{
    [CascadingParameter]
    public HttpContext HttpContext { get; set; } = default!;

    protected async override Task OnInitializedAsync()
    {
        await _authService.LogOutUserAsync(HttpContext);
        _navigation.NavigateTo("/auth/login");
    }
}
