using API_DOC.Models;
using Microsoft.EntityFrameworkCore;

namespace API_DOC.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Method> Methods { get; set; } = null!;
        public DbSet<UserModel> Users { get; set; } = null!;
    }
}