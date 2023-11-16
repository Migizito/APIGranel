using AGranelAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AGranelAPI.DataContext
{
    public class ApplicationDbContext :  DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {

        }
        public DbSet<User> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<UserRol> UsuariosRoles { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetallesVenta { get; set; }
        public DbSet<Producto> Productos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<UserRol>()
            .HasKey(ur => new { ur.UserID, ur.RoleID });

            modelBuilder.Entity<UserRol>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.Roles)
                .HasForeignKey(ur => ur.UserID);

            modelBuilder.Entity<UserRol>()
                .HasOne(ur => ur.Rol)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(ur => ur.RoleID);

            modelBuilder.Entity<Venta>()
                .HasOne(v => v.Vendedor)
                .WithMany(u => u.Ventas)
                .HasForeignKey(v => v.VendedorID);

            modelBuilder.Entity<DetalleVenta>()
                .HasKey(dv => dv.DetailID);

            modelBuilder.Entity<DetalleVenta>()
                .HasOne(dv => dv.Venta)
                .WithMany(v => v.DetallesVenta)
                .HasForeignKey(dv => dv.SaleID);

            modelBuilder.Entity<DetalleVenta>()
                .HasOne(dv => dv.Producto)
                .WithMany(p => p.DetallesVenta)
                .HasForeignKey(dv => dv.ProductID);
        }
    }
}
