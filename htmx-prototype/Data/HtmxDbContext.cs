using htmx_prototype.Models;
using Microsoft.EntityFrameworkCore;

namespace htmx_prototype.Data
{
    public class HtmxDbContext : DbContext
    {
        public HtmxDbContext(DbContextOptions<HtmxDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}