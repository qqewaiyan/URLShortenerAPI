using Microsoft.EntityFrameworkCore;
using URLShortenerAPI.Entity;

namespace URLShortenerAPI
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<UserAccount> Users => Set<UserAccount>();
        public DbSet<Url> Urls => Set<Url>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Url>()
                .HasIndex(x => x.ShortCode)
                .IsUnique();

            modelBuilder.Entity<Url>()
                .HasOne(x => x.User)
                .WithMany(u => u.Urls)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserAccount>()
                .HasIndex(x => x.Email)
                .IsUnique();
        }
    }
}
