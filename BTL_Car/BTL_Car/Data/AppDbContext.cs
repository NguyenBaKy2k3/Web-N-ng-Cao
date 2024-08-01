using BTL_Car.Models;
using Microsoft.EntityFrameworkCore;

namespace BTL_Car.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Users> Users { get; set; }
        public DbSet<Cars> Cars { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Comments> Comments { get; set; }

    }
}
