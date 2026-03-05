using Microsoft.EntityFrameworkCore;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.DataAccessLayer.Concrete
{
    public class RentalyContext :DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=BEYZA\\BEYZA_DEV;Database=RentalyDb;Trusted_Connection=True;");
        }

        public DbSet<Branch> Branches { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarModel> CarModels { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Rental> Rentals { get; set; }
    }
}
