using Microsoft.EntityFrameworkCore;
using StackOverFlowReplica.StackOverFlowReplica.Models;

namespace StackOverFlowReplica.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Question> Questions { get; set; }
    }
}
