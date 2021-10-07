using KinectTvV2.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KinectTvV2.API.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<FileInfoEntity> FileInfoEntities { get; protected set; }
        public ApplicationDbContext() : base()
        { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("User ID=postgres;Password=root;Server=localhost;Port=5432;Database=ittv-backend;Integrated Security=true;");
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FileInfoEntity>().HasKey(x => x.Id);
            modelBuilder.Entity<FileInfoEntity>().HasIndex(x => x.BaseName)
                .IsUnique()
                .HasDatabaseName("IX_BASE64NAME_FILE");
            modelBuilder.Entity<FileInfoEntity>().HasIndex(x => x.Inactive);
            
            base.OnModelCreating(modelBuilder);
        }
    }
}