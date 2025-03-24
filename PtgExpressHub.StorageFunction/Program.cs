using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PtgExpressHub.Domain;

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables()
    .Build();

var connectionString = config.GetConnectionString("CloudConnection");

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddDbContext<PtgExpressDataContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IApplicationRepository, ApplicationBuildRepository>();
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
    })
    .Build();

host.Run();
