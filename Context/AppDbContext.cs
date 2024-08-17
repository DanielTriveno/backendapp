using Microsoft.EntityFrameworkCore;
using WebApiUser.Models;

namespace WebApiUser.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }

    }
}
