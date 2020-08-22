using Microsoft.EntityFrameworkCore;
using WebApplication.Models;

namespace WebApplication.Data
{
    public class ReservationContext: DbContext
    {
        public ReservationContext(DbContextOptions<ReservationContext> options)
            : base(options)
        {
        }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Client>(entity => {
                entity.HasIndex(e => e.Name).IsUnique();
            });
            builder.Entity<Reservation>(entity =>
            {
                entity.HasCheckConstraint("constraint_ranking", "\"Ranking\" >= 0 and \"Ranking\" <= 5");
            });
            base.OnModelCreating(builder);
        }
    }
    

}