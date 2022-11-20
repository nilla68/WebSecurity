using Microsoft.EntityFrameworkCore;

namespace TheBlog.DataAccess
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext(DbContextOptions options) : base(options) { }

        public DbSet<BlogPostEntity> BlogPosts => Set<BlogPostEntity>();
    }
}