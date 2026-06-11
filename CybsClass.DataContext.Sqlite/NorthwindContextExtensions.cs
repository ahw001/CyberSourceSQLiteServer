using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CybsClass.EntityModels;

public static class CybsClassContextExtensions
{
    public static IServiceCollection AddB2cNorthwindContext(
      this IServiceCollection services,
      string? connectionString = null)
    {
        connectionString ??= "Data Source=" + Path.Combine(AppContext.BaseDirectory, "Data", "B2CNorthwind.sqlite");

        services.AddDbContext<B2cNorthwindContext>(options =>
        {
            options.UseSqlite(connectionString);
        },
        contextLifetime: ServiceLifetime.Transient,
        optionsLifetime: ServiceLifetime.Transient);

        return services;
    }
}
