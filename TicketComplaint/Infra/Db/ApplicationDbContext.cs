using Microsoft.EntityFrameworkCore;
using TicketComplaint.Domain;

namespace TicketComplaint.Infra.Db
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //FluentAPI
            modelBuilder.Entity<Client>()
                .Property(c => c.Name).HasMaxLength(120).IsRequired();
            modelBuilder.Entity<Client>()
                .Property(c => c.Email).HasMaxLength(80).IsRequired();

            modelBuilder.Entity<Ticket>()
                .HasMany(t => t.Complaints)
                .WithOne(c => c.Ticket)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}