using Microsoft.EntityFrameworkCore;
using ProductImporter.Model;

namespace ProductImporter.Target.EntityFramework
{
    // defining database connection
    public class TargetContext : DbContext
    {
        public TargetContext(DbContextOptions<TargetContext> options) : base(options) 
        { }
        
        public DbSet<Product> Products { get; set; } // table in the db

        protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.Entity<Product>().OwnsOne(x => x.Price);
    }
}
