using Microsoft.EntityFrameworkCore;
using Subscriber.Data.Entities;
using System.Configuration;

namespace Subscriber.Models
{
    public class WeightWatchers : DbContext
    {
        public WeightWatchers()
        {
        }
        public WeightWatchers(DbContextOptions<WeightWatchers> options)
          : base(options)
        {

        }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Data.Entities.Subscriber> Subscribers { get; set; }       
    }
}

