using htmx.Models;
using Microsoft.EntityFrameworkCore;

namespace htmx_prototype.Data
{
    public class HtmxDbContext : DbContext
    {
        public HtmxDbContext(DbContextOptions<HtmxDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Test Product 1",
                    Price = 19.99,
                    Description = "This is a test product",
                    PreviewImage = "https://upload.wikimedia.org/wikipedia/commons/thumb/8/89/Tomato_je.jpg/1200px-Tomato_je.jpg"
                },
                new Product
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Test Product 2",
                    Price = 29.99,
                    Description = "This is another test product",
                    PreviewImage = "https://upload.wikimedia.org/wikipedia/commons/thumb/8/89/Tomato_je.jpg/1200px-Tomato_je.jpg"
                }
            );
        }
    }
}