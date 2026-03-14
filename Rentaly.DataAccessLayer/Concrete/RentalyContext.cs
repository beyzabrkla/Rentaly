using Microsoft.EntityFrameworkCore;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.DataAccessLayer.Concrete
{
    public class RentalyContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=BEYZA\\BEYZA_DEV;Database=RentalyDb;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Decimal hassasiyetleri
            modelBuilder.Entity<Car>().Property(c => c.DailyPrice).HasPrecision(18, 2);
            modelBuilder.Entity<Car>().Property(c => c.DepositAmount).HasPrecision(18, 2);
            modelBuilder.Entity<Rental>().Property(r => r.TotalPrice).HasPrecision(18, 2);

            // TÜM ilişkileri bul ve Cascade'i İPTAL ET (En sağlam yöntem)
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            // Alternatif olarak, her ilişkiyi tek tek NoAction yapalım
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.NoAction;
            }

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Branch> Branches { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarModel> CarModels { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<CarImage> CarImages { get; set; }
    }
}