using AccountProvider.Data.Contexts;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AccountProvider.Functions;
using AccountProvider.Interfaces;
using AccountProvider.Repositories;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddDbContext<Context>(x => x.UseSqlServer(Environment.GetEnvironmentVariable("SqlServerAccount")));
    })
    .Build();

host.Run();
