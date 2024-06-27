using BootcampSecurity.MS_Auth.Domain;
using BootcampSecurity.MS_Auth.Helpers;
using BootcampSecurity.MS_Auth.Infraestructure;
using BootcampSecurity.MS_Auth.Infrastructure;
using BootcampSecurity.MS_Auth.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Data;
using System.Data.SqlClient;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices((context, services) =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        var configuration = context.Configuration;
        var connectionString = configuration.GetConnectionString("dbLibrary");

        // Cambiar el ámbito de IDbConnection a Scoped
        services.AddTransient<IDbConnection>(sp => new SqlConnection(connectionString));

        services.AddSingleton<JwtHelper>();
        services.AddScoped<AuthService>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<ITokenRepository, TokenRepository>();
        services.AddTransient<IDataAccessRepository, GenericRepository>();
    })
    .Build();

host.Run();

