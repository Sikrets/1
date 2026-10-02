using Microsoft.EntityFrameworkCore;

namespace GameStudioApp
{
    public class GameContext : DbContext
    {
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Studio> Studios { get; set; }
        public DbSet<Game> Games { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=GameStudioDB;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>(e =>
            {
                e.ToTable("Countries");
                e.HasKey(c => c.Id);
                e.Property(c => c.Name).IsRequired().HasMaxLength(100);
                e.HasIndex(c => c.Name).IsUnique();
            });

            modelBuilder.Entity<City>(e =>
            {
                e.ToTable("Cities");
                e.HasKey(c => c.Id);
                e.Property(c => c.Name).IsRequired().HasMaxLength(100);

                e.HasOne(c => c.Country)
                 .WithMany(c => c.Cities)
                 .HasForeignKey(c => c.CountryId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Studio>(e =>
            {
                e.ToTable("Studios");
                e.HasKey(s => s.Id);
                e.Property(s => s.Name).IsRequired().HasMaxLength(100);
                e.HasIndex(s => s.Name).IsUnique();

                e.HasOne(s => s.City)
                 .WithMany(c => c.Studios)
                 .HasForeignKey(s => s.CityId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Game>(e =>
            {
                e.ToTable("Games");
                e.HasKey(g => g.Id);
                e.Property(g => g.Title).IsRequired().HasMaxLength(200);
                e.Property(g => g.Genre).HasMaxLength(50);
                e.Property(g => g.Price).HasColumnType("decimal(18,2)");

                e.HasOne(g => g.Studio)
                 .WithMany(s => s.Games)
                 .HasForeignKey(g => g.StudioId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Country>().HasData(
                new Country { Id = 1, Name = "USA" },
                new Country { Id = 2, Name = "Japan" },
                new Country { Id = 3, Name = "Poland" }
            );

            modelBuilder.Entity<City>().HasData(
                new City { Id = 1, Name = "Los Angeles", CountryId = 1 },
                new City { Id = 2, Name = "Tokyo", CountryId = 2 },
                new City { Id = 3, Name = "Warsaw", CountryId = 3 }
            );

            modelBuilder.Entity<Studio>().HasData(
                new Studio { Id = 1, Name = "Rockstar Games", CityId = 1, FoundationYear = 1998 },
                new Studio { Id = 2, Name = "Nintendo", CityId = 2, FoundationYear = 1889 },
                new Studio { Id = 3, Name = "CD Projekt", CityId = 3, FoundationYear = 1994 }
            );

            modelBuilder.Entity<Game>().HasData(
                new Game { Id = 1, Title = "GTA V", Genre = "Action", ReleaseYear = 2013, Price = 59.99m, StudioId = 1 },
                new Game { Id = 2, Title = "The Witcher 3", Genre = "RPG", ReleaseYear = 2015, Price = 39.99m, StudioId = 3 },
                new Game { Id = 3, Title = "Super Mario Odyssey", Genre = "Platformer", ReleaseYear = 2017, Price = 49.99m, StudioId = 2 }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}