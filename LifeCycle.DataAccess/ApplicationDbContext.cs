using LifeCycle.Models;
using Microsoft.EntityFrameworkCore;

namespace LifeCycle.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Component> Components { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Line> Lines { get; set; }
        public DbSet<Machine> Machines { get; set; }
        public DbSet<Article> Articles { get; set; }

    }
}
