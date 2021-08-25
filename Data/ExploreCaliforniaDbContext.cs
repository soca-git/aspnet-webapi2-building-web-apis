using ExploreCalifornia.Data.Entities;
using System.Data.Entity;

namespace ExploreCalifornia.Data
{
    public partial class ExploreCaliforniaDbContext : DbContext
    {
        public ExploreCaliforniaDbContext() : base("name=ExploreCalifornia")
        {
            // Disabled due to navigation properties on Tour & Reservation entities.
            this.Configuration.LazyLoadingEnabled = false;
        }

        public virtual DbSet<AuthorizedApp> AuthorizedApps { get; set; }
        public virtual DbSet<Tour> Tours { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // AuthorizedClient/App
            modelBuilder.Entity<AuthorizedApp>()
                .Property(e => e.AppToken)
                .IsUnicode(false);
            modelBuilder.Entity<AuthorizedApp>()
                .Property(e => e.AppSecret)
                .IsUnicode(false);

            // Tour
            modelBuilder.Entity<Tour>()
                .Property(e => e.Price)
                .HasPrecision(19, 4);
        }
    }
}
