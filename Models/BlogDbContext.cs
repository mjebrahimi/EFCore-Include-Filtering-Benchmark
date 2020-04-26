using Microsoft.EntityFrameworkCore;

namespace EFCore_Include_Filtering_Benchmark
{
    public class BlogDbContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseInMemoryDatabase("memorydb");
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=TempTestDb2;Integrated Security=true");

            base.OnConfiguring(optionsBuilder);
        }
    }
}
