using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Services

{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Airline> Airlines { get; set; }
    }
}
