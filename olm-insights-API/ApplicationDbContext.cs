using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;

namespace olm_insights_API
{
    public class ApplicationDbContext : DbContext
{
        public DbSet<Blog> Blogs { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }

    public class Blog
    {
        public int BlogId { get; set; }
        public string Title { get; set; }
    }
}
