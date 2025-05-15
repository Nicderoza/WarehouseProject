using Microsoft.EntityFrameworkCore;
using Warehouse.Data.Models;

public class WarehouseContext : DbContext
{
  public WarehouseContext(DbContextOptions<WarehouseContext> options) : base(options) { }

  public DbSet<Categories> Categories { get; set; }
  public DbSet<Cities> Cities { get; set; }
  public DbSet<Orders> Orders { get; set; }
  public DbSet<Products> Products { get; set; }
  public DbSet<Suppliers> Suppliers { get; set; }
  public DbSet<Users> Users { get; set; }
  public DbSet<Owners> Owners { get; set; }
  public DbSet<Roles> Roles { get; set; }
  public DbSet<OrderItems> OrderItems { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    // Configurazione della tabella Roles
    modelBuilder.Entity<Roles>(entity =>
    {
      entity.HasKey(r => r.RoleID);
      entity.Property(r => r.RoleName).IsRequired().HasMaxLength(50);
      entity.ToTable("Roles"); // Corretto il nome della tabella
    });

    // Configurazione della tabella Users
    modelBuilder.Entity<Users>(entity =>
    {
      entity.HasKey(u => u.UserID);
      entity.HasOne(u => u.Role)
          .WithMany(r => r.Users)
          .HasForeignKey(u => u.RoleID)
          .OnDelete(DeleteBehavior.Restrict);
      entity.Property(u => u.CreatedAt).HasColumnType("datetime2"); // Aggiungi questa riga
    });

    modelBuilder.Entity<Categories>(entity =>
    {
      entity.HasKey(c => c.CategoryID);
    });

    modelBuilder.Entity<Cities>(entity =>
    {
      entity.HasKey(c => c.CityID);
      entity.HasMany(c => c.Suppliers)
          .WithOne(s => s.City)
          .HasForeignKey(s => s.CityID)
          .OnDelete(DeleteBehavior.Restrict);
    });

    modelBuilder.Entity<Orders>(entity =>
    {
      entity.HasKey(o => o.OrderID);
      entity.HasOne(o => o.User)
          .WithMany(u => u.Orders)
          .HasForeignKey(o => o.UserID);
      entity.HasOne(o => o.Product)
          .WithMany(p => p.Orders)
          .HasForeignKey(o => o.ProductID);
    });

    modelBuilder.Entity<Suppliers>(entity =>
    {
      entity.HasKey(s => s.SupplierID);
      entity.HasOne(s => s.City)
          .WithMany(c => c.Suppliers)
          .HasForeignKey(s => s.CityID);
    });

    modelBuilder.Entity<Products>(entity =>
    {
      entity.HasKey(p => p.ProductID);
      entity.HasOne(p => p.Category)
          .WithMany(c => c.Products)
          .HasForeignKey(p => p.CategoryID)
          .OnDelete(DeleteBehavior.Restrict);
      entity.HasOne(p => p.Supplier)
          .WithMany(s => s.Products)
          .HasForeignKey(p => p.SupplierID)
          .OnDelete(DeleteBehavior.Restrict);
      entity.HasMany(p => p.Orders)
          .WithOne(o => o.Product)
          .HasForeignKey(o => o.ProductID)
          .OnDelete(DeleteBehavior.Restrict);
    });

    modelBuilder.Entity<OrderItems>(entity =>
    {
      entity.HasOne(oi => oi.Product)
          .WithMany(p => p.OrderItems)
          .HasForeignKey(oi => oi.ProductID);
    });

    modelBuilder.Entity<Owners>(entity =>
    {
      entity.HasKey(o => o.OwnerID);
    });
  }
}
