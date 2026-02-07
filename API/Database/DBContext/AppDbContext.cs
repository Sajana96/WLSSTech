using API.Auth;
using API.Database.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace API.Database.DBContext
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext (DbContextOptions<AppDbContext> dbContextOptions) : base(dbContextOptions) { }

        public DbSet<Incident> Incidents => Set<Incident>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Incident>(e =>
            {
                e.ToTable("Incidents");

                e.HasKey(x => x.Id);

                e.Property(x => x.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                e.Property(x => x.Description)
                    .HasMaxLength(2000);

                e.Property(x => x.Severity)
                    .HasConversion<string>()    // enum to string
                    .IsRequired()
                    .HasMaxLength(20);

                e.Property(x => x.Status)
                    .HasConversion<string>()    
                    .IsRequired()
                    .HasMaxLength(20);

                e.Property(x => x.Location)
                    .HasMaxLength(200);

                e.Property(x => x.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                e.Property(x => x.CreatedAt)
                    .IsRequired();


                //Indexing for filtering
                e.HasIndex(x => x.Status); 
                e.HasIndex(x => x.Severity);
                e.HasIndex(x => x.CreatedAt);
                e.HasIndex(x => x.CreatedBy);

            });
        }
    }
}
