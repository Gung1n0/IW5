using IW5_project.Api.DAL.EF;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IW5_project.Api.App.EndToEndTests;

// Starts the real API in memory (TestServer) on top of a private, throw-away SQLite database
// Every factory instance gets its own empty database, so tests never influence each other
public class IW5ApiApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string connectionString = $"Data Source=iw5-e2e-{Guid.NewGuid():N};Mode=Memory;Cache=Shared";
    private readonly SqliteConnection keepAliveConnection;

    public IW5ApiApplicationFactory()
    {
        keepAliveConnection = new SqliteConnection(connectionString);
        keepAliveConnection.Open();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Production");

        builder.ConfigureTestServices(services =>
        {
            var originalRegistrations = services
                .Where(d => d.ServiceType.IsGenericType
                            && d.ServiceType.GenericTypeArguments is [var contextType]
                            && contextType == typeof(DatabaseContext)
                            && d.ServiceType.Name.Contains("DbContextOptions"))
                .ToList();

            foreach (var registration in originalRegistrations)
            {
                services.Remove(registration);
            }

            services.AddDbContext<DatabaseContext>(options => options.UseSqlite(connectionString));

        });
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);

        using var scope = host.Services.CreateScope();
        scope.ServiceProvider.GetRequiredService<DatabaseContext>().Database.EnsureCreated();

        return host;
    }

    public void Seed(Action<DatabaseContext> seed)
    {
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        seed(db);
        db.SaveChanges();
    }

    public T Query<T>(Func<DatabaseContext, T> query)
    {
        using var scope = Services.CreateScope();
        return query(scope.ServiceProvider.GetRequiredService<DatabaseContext>());
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
        {
            keepAliveConnection.Dispose();
        }
    }
}
