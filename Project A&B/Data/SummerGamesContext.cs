using Microsoft.EntityFrameworkCore;
using Project_A_B.Models;

namespace Project_A_B.Data
{
    public class SummerGamesContext : DbContext
    {
        //To give access to IHttpContextAccessor for Audit Data with IAuditable
        private readonly IHttpContextAccessor _httpContextAccessor;

        //Property to hold the UserName value
        public string UserName
        {
            get; private set;
        }

        public SummerGamesContext(DbContextOptions<SummerGamesContext> options,
            IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            if (_httpContextAccessor.HttpContext != null)
            {
                //We have a HttpContext, but there might not be anyone Authenticated
                UserName = _httpContextAccessor.HttpContext?.User.Identity.Name;
                UserName ??= "Unknown";
            }
            else
            {
                //No HttpContext so seeding data
                UserName = "Seed Data";
            }
        }

        public SummerGamesContext(DbContextOptions<SummerGamesContext> options)
            : base(options)
        {
        }

        public DbSet<Sport> Sports { get; set; }
        public DbSet<Contingent> Contingents { get; set; }
        public DbSet<Athlete> Athletes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Auditing for Athlete and Sport
            modelBuilder.Entity<Athlete>().HasKey(a => a.ID);
            modelBuilder.Entity<Sport>().HasKey(s => s.ID);

            // Restricting Cascade Delete
            modelBuilder.Entity<Athlete>()
                .HasOne(a => a.Sport)
                .WithMany(s => s.Athletes)
                .HasForeignKey(a => a.SportID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Athlete>()
                .HasOne(a => a.Contingent)
                .WithMany(c => c.Athletes)
                .HasForeignKey(a => a.ContingentID)
                .OnDelete(DeleteBehavior.Restrict);

            // Unique constraints
            modelBuilder.Entity<Athlete>().HasIndex(a => a.AthleteCode).IsUnique();
            modelBuilder.Entity<Contingent>().HasIndex(c => c.Code).IsUnique();
            modelBuilder.Entity<Sport>().HasIndex(s => s.Code).IsUnique();

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                if (entry.Entity is IAuditable trackable)
                {
                    var now = DateTime.UtcNow;
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            trackable.UpdatedOn = now;
                            trackable.UpdatedBy = UserName;
                            break;

                        case EntityState.Added:
                            trackable.CreatedOn = now;
                            trackable.CreatedBy = UserName;
                            trackable.UpdatedOn = now;
                            trackable.UpdatedBy = UserName;
                            break;
                    }
                }
            }
        }

    }
}
