using Dating.Controllers;
using Dating.Models;
using Microsoft.EntityFrameworkCore;

namespace Dating.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
        public DbSet<UsersModels> Users { get; set; }
        public DbSet<LikeModels> Likes { get; set; }
        public DbSet<UserProfile> Profiles { get; set; }
        public DbSet<AdminModel> Admins { get; set; }
        public DbSet<MatchModels> Matches { get; set; }
    }
}
