using Microsoft.EntityFrameworkCore;
using UserManagementApp.Data.Entities;

namespace UserManagementApp.Data.Contexts
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Login)
                      .IsRequired();
                entity.HasIndex(u => u.Login)
                      .IsUnique();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
