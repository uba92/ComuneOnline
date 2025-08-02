using ComuneOnline.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ComuneOnline.Data
{
    public class ComuneDbContext : DbContext


    {
        public ComuneDbContext(DbContextOptions<ComuneDbContext> options) : base(options)
        {
            
        }

        public DbSet<Cittadino> Cittadini { get; set; }
    }
}
