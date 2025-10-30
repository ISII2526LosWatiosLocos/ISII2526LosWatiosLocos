using System;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII2526.API.Models
{
    public class Alquiler
    {
        public Alquiler() { }

        public Alquiler(string direccionEnvio, DateOnly fechaAlquiler, DateOnly fechaInicio, DateOnly fechaFin, float precioTotal, List<AlquilarItem> alquilarItems, MetodosPago metodoPago, ApplicationUser usuario)
        {
            DireccionEnvio = direccionEnvio;
            FechaAlquiler = fechaAlquiler;
            FechaInicio = fechaInicio;
            FechaFin = fechaFin;
            PrecioTotal = precioTotal;
            AlquilarItems = alquilarItems;
            MetodoPago = metodoPago;
            Usuario = usuario;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "No puede contener más de 100 caracteres")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Text), Display(Name = "Dirección de Envío")]
        public string DireccionEnvio { get; set; } = string.Empty;

        [Required]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Date), Display(Name = "Fecha de Alquiler")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateOnly FechaAlquiler { get; set; }

        [Required]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Date), Display(Name = "Fecha de Inicio")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateOnly FechaInicio { get; set; }

        [Required]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Date), Display(Name = "Fecha de Fin")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateOnly FechaFin { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Text), Display(Name = "Período")]
        public int Periodo => FechaFin.DayNumber - FechaInicio.DayNumber;

        [Required]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency), Display(Name = "Precio Total")]
        public float PrecioTotal { get; set; }

        // Relaciones
        public List<AlquilarItem> AlquilarItems { get; set; }
        public MetodosPago MetodoPago { get; set; }
        public ApplicationUser Usuario { get; set; }

    }
}
