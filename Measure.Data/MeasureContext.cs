using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace Measure.Data
{
    public class MeasureContext: DbContext
    {
        public MeasureContext()

        {
        }
        public MeasureContext(DbContextOptions<MeasureContext> options)
          : base(options)
        {

        }
        public DbSet<Measure> Measures { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseSqlServer(ConfigurationManager.AppSettings["MeasureDB"]);
            }
        }
    }
}
