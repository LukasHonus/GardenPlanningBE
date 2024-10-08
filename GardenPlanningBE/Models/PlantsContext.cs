using Microsoft.EntityFrameworkCore;

namespace GardenPlanningBE.Models
{
    public class PlantsContext : DbContext
    {
        public PlantsContext(DbContextOptions<PlantsContext> options) : base(options)
        {
            
        }
        public DbSet<Plants> Plants { get; set; }
    }
}
