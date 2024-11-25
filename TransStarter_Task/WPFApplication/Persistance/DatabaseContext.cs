using Microsoft.EntityFrameworkCore;
using TransStarter_Task.WPFApplication.Common.Entity;

namespace TransStarter_Task.WPFApplication.Persistance;
internal class DatabaseContext : DbContext
{
    internal DatabaseContext () { }

    protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=./Database.db3");
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating (ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>()
            .HasOne(x => x.VehicleInfo)
            .WithMany(x => x.Cars);

        modelBuilder.Entity<Order>()
            .HasOne(x => x.Customer)
            .WithMany(x => x.Orders);
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<VehicleInfo> VehicleInfos { get; set; }
    public DbSet<Car> Cars { get; set; }
}