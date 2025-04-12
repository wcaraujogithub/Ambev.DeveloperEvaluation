using Ambev.DeveloperEvaluation.ORM;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.DeveloperEvaluation.Integration.Fixtures
{
    public class CustomWebApplicationFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");

            builder.ConfigureServices(services =>
            {
                // Substitua o DbContext real pelo banco em memória
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<DefaultContext>));

                if (descriptor != null)
                    services.Remove(descriptor);

                // Adiciona o DbContext com um banco de dados em memória
                services.AddDbContext<DefaultContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryTestDb");
                });

                // Rebuild provider
                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<DefaultContext>();
                db.Database.EnsureCreated();
            });
        }
    }

}
