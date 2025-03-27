using Microsoft.AspNetCore.Components.Authorization;
using PtgExpressHub.Web.Components;
using PtgExpressHub.Web.Runtime;
using PtgExpressHub.Domain;
using Microsoft.EntityFrameworkCore;

namespace PtgExpressHub.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddDbContext<PtgExpressDataContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("CloudConnection")));

        builder.Services.AddScoped<AuthService>();
        builder.Services.AddScoped<IApplicationRepository, ApplicationBuildRepository>();
        builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

        builder.Services.AddAuthentication("CookieAuth")
            .AddCookie("CookieAuth", options =>
            {
                options.LoginPath = "/auth/login";
                options.LogoutPath = "/auth/logout";
            });

        builder.Services.AddAuthorization();

        var app = builder.Build();
        
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}
