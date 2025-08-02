using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ComuneOnline.Data
{
    public class ComuneDbContextFactory : IDesignTimeDbContextFactory<ComuneDbContext>
    {
        public ComuneDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<ComuneDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new ComuneDbContext(optionsBuilder.Options);
        }
    }
}
