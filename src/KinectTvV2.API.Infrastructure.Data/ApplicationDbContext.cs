using KinectTvV2.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KinectTvV2.API.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }
        
        /// <summary>
        /// Только один элемент в таблице
        /// </summary>
        public DbSet<SettingEntity> SettingEntities { get; protected set; }
    }
}