using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace edificio_digital.Entity.Data;

/// <summary>
/// Permite que las herramientas <c>dotnet ef</c> creen el contexto sin ejecutar la aplicación web,
/// leyendo la cadena desde el <c>appsettings.json</c> del host <c>edificio_digital</c>.
/// </summary>
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var hostDir = FindHostProjectDirectory();
        var configuration = new ConfigurationBuilder()
            .SetBasePath(hostDir)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("PostgreSql")
            ?? throw new InvalidOperationException(
                "Defina ConnectionStrings:PostgreSql en appsettings.json del proyecto edificio_digital.");

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new AppDbContext(optionsBuilder.Options);
    }

    private static string FindHostProjectDirectory()
    {
        var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (dir is not null)
        {
            var hostFolder = Path.Combine(dir.FullName, "edificio_digital");
            if (File.Exists(Path.Combine(hostFolder, "appsettings.json")))
                return hostFolder;

            if (dir.Name == "edificio_digital" && File.Exists(Path.Combine(dir.FullName, "appsettings.json")))
                return dir.FullName;

            dir = dir.Parent;
        }

        throw new InvalidOperationException(
            "Ejecuta dotnet ef desde la carpeta de la solución (donde está edificio_digital/appsettings.json).");
    }
}
