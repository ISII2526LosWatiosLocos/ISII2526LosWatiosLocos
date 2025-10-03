using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options) {
    public DbSet<ReparaciónItem> ReparaciónItems { get; set; } 

    public DbSet<Compra> Compras { get; set; }
    public DbSet<CompraItem> CompraItems { get; set; }
    public DbSet<Oferta> Ofertas { get; set; }
    public DbSet<OfertaItem> OfertaItems { get; set; }
    public DbSet<Herramienta> Herramientas { get; set; }
    public DbSet<Fabricante> Fabricantes { get; set; }

    public DbSet<MetodosPago> MetodosPagos { get; set; }
    public DbSet<Efectivo> Efectivos { get; set; }
    public DbSet<TarjetaCredito> TarjetasCredito { get; set; }
    public DbSet<Paypal> Paypals { get; set; }

    public DbSet<DirigidaOferta> DirigidasOfertas { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Socio> Socios { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
