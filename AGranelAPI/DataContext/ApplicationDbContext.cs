using AGranelAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AGranelAPI.DataContext
{
    public class ApplicationDbContext :  DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {

        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<UserRol> UsuarioRol { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetallesVenta { get; set; }
        public DbSet<Producto> Productos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        //    modelBuilder.Entity<UserRol>()
        //    .HasKey(ur => new { ur.UsuarioID, ur.RolID });

        //    modelBuilder.Entity<UserRol>()
        //        .HasOne(ur => ur.User)
        //        .WithMany(u => u.Roles)
        //        .HasForeignKey(ur => ur.UsuarioID);

        //    modelBuilder.Entity<UserRol>()
        //        .HasOne(ur => ur.Rol)
        //        .WithMany(r => r.Usuarios)
        //        .HasForeignKey(ur => ur.RolID);


        //    modelBuilder.Entity<DetalleVenta>()
        //        .HasKey(dv => dv.DetalleID);

        //    modelBuilder.Entity<DetalleVenta>()
        //        .HasOne(dv => dv.Venta)
        //        .WithMany(v => v.DetallesVenta)
        //        .HasForeignKey(dv => dv.VentaID);

        //    modelBuilder.Entity<DetalleVenta>()
        //        .HasOne(dv => dv.Producto)
        //        .WithMany(p => p.DetallesVenta)
        //        .HasForeignKey(dv => dv.ProductoID);
        }
    }
}
