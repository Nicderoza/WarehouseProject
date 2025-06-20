using Microsoft.EntityFrameworkCore;
using Warehouse.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace Warehouse.Data
{
  public class WarehouseContext : DbContext
  {
    public WarehouseContext(DbContextOptions<WarehouseContext> options) : base(options) { }

    public DbSet<Orders> Orders { get; set; }
    public DbSet<OrderItems> OrderItems { get; set; }
    public DbSet<Products> Products { get; set; }
    public DbSet<Users> Users { get; set; }
    public DbSet<Categories> Categories { get; set; }
    public DbSet<Suppliers> Suppliers { get; set; }
    public DbSet<UsersSuppliers> UsersSuppliers { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItems> CartItems { get; set; }
    public DbSet<OrderStatus> OrderStatuses { get; set; }
    public DbSet<Roles> Roles { get; set; }
    public DbSet<RolePermissions> RolePermissions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<Orders>(entity =>
      {
        entity.HasKey(e => e.OrderID);
        entity.Property(e => e.OrderDate).IsRequired();
        entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)").IsRequired();

        entity.HasOne(o => o.User)
              .WithMany(u => u.Orders)
              .HasForeignKey(o => o.UserID)
              .OnDelete(DeleteBehavior.Restrict);

        entity.HasMany(o => o.OrderItems)
              .WithOne(oi => oi.Order)
              .HasForeignKey(oi => oi.OrderID)
              .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(o => o.OrderStatus)
              .WithMany(os => os.Orders)
              .HasForeignKey(o => o.OrderStatusFkID)
              .OnDelete(DeleteBehavior.NoAction);
      });

      modelBuilder.Entity<OrderItems>(entity =>
      {
        entity.HasKey(e => e.OrderItemID);
        entity.Property(e => e.Quantity).IsRequired();
        entity.Property(e => e.Price).HasColumnType("decimal(18, 2)").IsRequired();
        entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()").IsRequired();
        entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETUTCDATE()").IsRequired();

        entity.HasOne(oi => oi.Product)
              .WithMany(p => p.OrderItems)
              .HasForeignKey(oi => oi.ProductID)
              .OnDelete(DeleteBehavior.NoAction);
      });

      modelBuilder.Entity<Products>(entity =>
      {
        entity.HasKey(e => e.ProductID);
        entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
        entity.Property(e => e.Quantity).IsRequired();
        entity.Property(e => e.Price).HasColumnType("decimal(18, 2)").IsRequired();

        entity.HasOne(p => p.Category)
              .WithMany(c => c.Products)
              .HasForeignKey(p => p.CategoryID)
              .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(p => p.Supplier)
              .WithMany(s => s.Products)
              .HasForeignKey(p => p.SupplierID)
              .OnDelete(DeleteBehavior.Restrict);

        entity.HasMany(p => p.CartItems)
              .WithOne(ci => ci.Product)
              .HasForeignKey(ci => ci.ProductID)
              .OnDelete(DeleteBehavior.NoAction);
      });

      modelBuilder.Entity<Users>(entity =>
      {
        entity.HasKey(e => e.UserID);
        entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
        entity.Property(e => e.Email).IsRequired().HasMaxLength(255);

        entity.HasMany(u => u.UsersSuppliers)
              .WithOne(us => us.User)
              .HasForeignKey(us => us.UserID)
              .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(u => u.Role)
              .WithMany(r => r.Users)
              .HasForeignKey(u => u.RoleID)
              .OnDelete(DeleteBehavior.NoAction);

        entity.HasOne(u => u.Cart)
              .WithOne(c => c.User)
              .HasForeignKey<Cart>(c => c.UserID)
              .OnDelete(DeleteBehavior.Cascade);
      });

      modelBuilder.Entity<Categories>(entity =>
      {
        entity.HasKey(e => e.CategoryID);
        entity.Property(e => e.CategoryName).IsRequired().HasMaxLength(100);
      });

      modelBuilder.Entity<Suppliers>(entity =>
      {
        entity.HasKey(e => e.SupplierID);
        entity.Property(e => e.CompanyName).IsRequired().HasMaxLength(255);

        entity.HasMany(s => s.UsersSuppliers)
              .WithOne(us => us.Supplier)
              .HasForeignKey(us => us.SupplierID)
              .OnDelete(DeleteBehavior.Cascade);
      });

      modelBuilder.Entity<OrderStatus>(entity =>
      {
        entity.HasKey(e => e.OrderStatusID);
        entity.Property(e => e.StatusName).IsRequired().HasMaxLength(50);
      });

      modelBuilder.Entity<UsersSuppliers>().HasKey(us => new { us.UserID, us.SupplierID });

      modelBuilder.Entity<UsersSuppliers>()
          .HasOne(us => us.User)
          .WithMany(u => u.UsersSuppliers)
          .HasForeignKey(us => us.UserID);

      modelBuilder.Entity<UsersSuppliers>()
          .HasOne(us => us.Supplier)
          .WithMany(s => s.UsersSuppliers)
          .HasForeignKey(us => us.SupplierID);

      modelBuilder.Entity<Cart>(entity =>
      {
        entity.ToTable("Carts");
        entity.HasKey(e => e.CartID);
        entity.Property(e => e.CartID).ValueGeneratedOnAdd();
        entity.Property(e => e.UserID).IsRequired();
        entity.Property(e => e.CreatedAt).IsRequired();
        entity.Property(e => e.UpdatedAt).IsRequired();

        entity.HasOne(c => c.User)
              .WithOne(u => u.Cart)
              .HasForeignKey<Cart>(c => c.UserID)
              .OnDelete(DeleteBehavior.Cascade);

        entity.HasMany(c => c.CartItems)
              .WithOne(ci => ci.Cart)
              .HasForeignKey(ci => ci.CartID)
              .OnDelete(DeleteBehavior.Cascade);
      });

      modelBuilder.Entity<CartItems>(entity =>
      {
        entity.ToTable("CartItems");
        entity.HasKey(e => e.ID);
        entity.Property(e => e.ID).ValueGeneratedOnAdd();
        entity.Property(e => e.CartID).IsRequired();
        entity.Property(e => e.ProductID).IsRequired();
        entity.Property(e => e.Quantity).IsRequired();
        entity.Property(e => e.CreatedAt).IsRequired();
        entity.Property(e => e.UpdatedAt).IsRequired();

        entity.HasOne(ci => ci.Cart)
              .WithMany(c => c.CartItems)
              .HasForeignKey(ci => ci.CartID)
              .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(ci => ci.Product)
              .WithMany(p => p.CartItems)
              .HasForeignKey(ci => ci.ProductID)
              .OnDelete(DeleteBehavior.NoAction);
      });

      modelBuilder.Entity<Roles>()
    .HasMany(r => r.Permissions)
    .WithMany(p => p.Roles)
    .UsingEntity<RolePermissions>(
        j => j
            .HasOne(rp => rp.Permission)
            .WithMany(p => p.RolePermissions)
            .HasForeignKey(rp => rp.PermissionID),
        j => j
            .HasOne(rp => rp.Role)
            .WithMany(r => r.RolePermissions)
            .HasForeignKey(rp => rp.RoleID),
        j =>
        {
          j.HasKey(rp => new { rp.RoleID, rp.PermissionID });
          j.ToTable("RolePermissions");
        });

      modelBuilder.Entity<RolePermissions>()
    .HasKey(rp => new { rp.RoleID, rp.PermissionID });

      modelBuilder.Entity<RolePermissions>()
          .HasOne(rp => rp.Role)
          .WithMany(r => r.RolePermissions)
          .HasForeignKey(rp => rp.RoleID);

      modelBuilder.Entity<RolePermissions>()
          .HasOne(rp => rp.Permission)
          .WithMany(p => p.RolePermissions)
          .HasForeignKey(rp => rp.PermissionID);
    }
  }
}
