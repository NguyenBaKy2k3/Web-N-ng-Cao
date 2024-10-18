using Dating.Controllers;
using Dating.Models;
using Microsoft.EntityFrameworkCore;

namespace Dating.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
        public DbSet<UsersModels> Users { get; set; }
        //public DbSet<MainInterfaceController> MainInterface { get; set; }
        //public DbSet<UserProfileViewModel> UserProfiles { get; set; }
    }
}
