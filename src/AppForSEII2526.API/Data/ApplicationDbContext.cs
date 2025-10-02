using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options) {
    public DbSet<Reparación> reparaciones { get; set; }
    public DbSet<ReparaciónItem> ReparaciónItems { get; set; }
    public DbSet<Herramienta> Herramientas { get; set; }
    public DbSet<Fabricante> Fabricantes { get; set; }

}
