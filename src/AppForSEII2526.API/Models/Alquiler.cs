using System;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII2526.API.Models
{
    public class Alquiler
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "No puede contener más de 100 caracteres")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Text), Display(Name = "Nombre Cliente")]
        public string NombreCliente { get; set; } = string.Empty;

        [Required]
        [StringLength(100, ErrorMessage = "No puede contener más de 100 caracteres")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Text), Display(Name = "Apellido Cliente")]
        public string ApellidoCliente { get; set; } = string.Empty;

        [Required]
        [StringLength(100, ErrorMessage = "No puede contener más de 100 caracteres")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Text), Display(Name = "Dirección de Envío")]
        public string DireccionEnvio { get; set; } = string.Empty;

        [Required]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Date), Display(Name = "Fecha de Alquiler")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaAlquiler { get; set; }

        [Required]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Date), Display(Name = "Fecha de Inicio")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaInicio { get; set; }

        [Required]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Date), Display(Name = "Fecha de Fin")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaFin { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Text), Display(Name = "Período")]
        public int Periodo => (FechaFin - FechaInicio).Days;


        [Required]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency), Display(Name = "Precio Total")]
        public float PrecioTotal { get; set; }

        // Campos opcionales

        [DataType(System.ComponentModel.DataAnnotations.DataType.PhoneNumber), Display(Name = "Número de Teléfono")]
        public string ?NumeroTelefono { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.EmailAddress), Display(Name = "Correo Electrónico")]
        public string ?Correo { get; set; }

        // Relaciones
        public List<AlquilarItem> AlquilarItems { get; set; }
        public MetodosPago metodosPago { get; set; }

    }
}
