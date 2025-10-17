using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace AppForSEII2526.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options) {

    public DbSet<Reparación> Reparaciones { get; set; }
    public DbSet<ReparaciónItem> ReparaciónItems { get; set; } 
    public DbSet<Compra> Compras { get; set; }
    public DbSet<CompraItem> CompraItems { get; set; }
    public DbSet<Oferta> Ofertas { get; set; }
    public DbSet<OfertaItem> OfertaItems { get; set; }
    public DbSet<Alquiler> Alquileres { get; set; }
    public DbSet<AlquilarItem> AlquilarItems { get; set; }
    public DbSet<Herramienta> Herramientas { get; set; }
    public DbSet<Fabricante> Fabricantes { get; set; }

    public DbSet<MetodosPago> MetodosPagos { get; set; }
    public DbSet<Efectivo> Efectivos { get; set; }
    public DbSet<TarjetaCredito> TarjetasCredito { get; set; }
    public DbSet<Paypal> Paypals { get; set; }



    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<MetodosPago>()
        .HasDiscriminator<string>("TipoDePago") // 1. Usa una columna llamada "TipoDePago" y que sea de tipo string.
        .HasValue<TarjetaCredito>("TarjetaCredito") // 2. Si el valor es "TarjetaCredito", es un objeto TarjetaCredito.
        .HasValue<Paypal>("PayPal")                 // 3. Si el valor es "PayPal", es un objeto PayPal.
        .HasValue<Efectivo>("Efectivo");           // 4. Si el valor es "Efectivo", es un objeto Efectivo.
    }

}
