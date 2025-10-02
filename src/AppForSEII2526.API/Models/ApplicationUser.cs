using Microsoft.AspNetCore.Identity;

namespace AppForSEII2526.API.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser {
    [Required, StringLength(50, ErrorMessage = "Numero de caracteres excedido. No se puede más de 50 caracteres")]
    public string Nombre { get; set; }

    [Required, StringLength(100, ErrorMessage = "Numero de caracteres excedido. No se puede más de 100 caracteres")]
    public string Apellidos { get; set; }

    [StringLength(20, ErrorMessage = "Numero de caracteres excedido. No se puede más de 20 caracteres")]
    public string Correo { get; set; }
}