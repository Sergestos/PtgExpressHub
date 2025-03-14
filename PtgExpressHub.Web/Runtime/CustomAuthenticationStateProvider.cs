﻿using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var identity = new ClaimsIdentity(
        [
           new Claim(ClaimTypes.Name, "mrfibuli"),
        ], "CustomAuthentication");

        var user = new ClaimsPrincipal(identity);

        return Task.FromResult(new AuthenticationState(user));
    }

    public void AuthenticateUser(string username)
    {
        var identity = new ClaimsIdentity(
        [
           new Claim(ClaimTypes.Name, username),
        ], "CustomAuthentication");

        var user = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(user)));
    }

    public Task MarkUserAsLoggedOut()
    {
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(null)));
        return Task.CompletedTask;
    }
}