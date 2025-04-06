using Microsoft.AspNetCore.Components.Authorization;
using PtgExpressHub.Web.Components;
using PtgExpressHub.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Blazored.LocalStorage;
using PtgExpressHub.Web.Runtime;

namespace PtgExpressHub.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddDbContext<PtgExpressDataContext>(options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("CloudConnection"), 
                sqlOptions => sqlOptions.EnableRetryOnFailure()));
        
        builder.Services.AddScoped<AuthService>();
        builder.Services.AddScoped<StorageDownloaderService>();
        builder.Services.AddScoped<IApplicationRepository, ApplicationBuildRepository>();
        builder.Services.AddBlazoredLocalStorage();
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.Name = "auth_token";
                options.LoginPath = "/auth/login";
                options.Cookie.MaxAge = TimeSpan.FromDays(3);
            });

        builder.Services.AddAuthorization();
        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddHttpContextAccessor();

        var app = builder.Build();
        
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }
        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}
