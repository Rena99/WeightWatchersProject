using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace Tracking.Data
{
    public class TrackingContext : DbContext
    {
        public TrackingContext()

        {
        }
        public TrackingContext(DbContextOptions<TrackingContext> options)
          : base(options)
        {

        }
        public DbSet<Tracking> Trackings { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseSqlServer(ConfigurationManager.AppSettings["TrackingDB"]);
            }
        }
       
    }
}

