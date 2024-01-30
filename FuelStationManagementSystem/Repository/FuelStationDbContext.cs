using FuelStationManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FuelStationManagementSystem.Repository
{
    public class FuelStationDbContext : DbContext
    {
        public FuelStationDbContext(DbContextOptions<FuelStationDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<FuelTransaction> FuelTransactions { get; set; }
        public DbSet<Balance> Balances { get; set; }
    }
}
