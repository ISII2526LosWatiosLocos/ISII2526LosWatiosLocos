using System.ComponentModel.DataAnnotations;

namespace AppForSEII2526.API.Models
{
    public class Oferta
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Date), Display(Name = "Fecha final")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateOnly FechaFinal { get; set; }

        [Required]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Date), Display(Name = "Fecha inicio")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateOnly FechaInicio { get; set; }

        [Required]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Date), Display(Name = "Fecha oferta")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateOnly FechaOferta { get; set; }

        public tipoDirigidaOferta? TipoDirigida { get; set; }

        public List<OfertaItem> Items { get; set; }

        public MetodosPago MetodosPago { get; set; }

        public Oferta(DateOnly fechaFinal, DateOnly fechaInicio, DateOnly fechaOferta, tipoDirigidaOferta? tipoDirigida, List<OfertaItem> items, MetodosPago metodosPago, ApplicationUser usuario)
        {
            FechaFinal = fechaFinal;
            FechaInicio = fechaInicio;
            FechaOferta = fechaOferta;
            TipoDirigida = tipoDirigida;
            Items = items;
            MetodosPago = metodosPago;
            Usuario = usuario;
        }

        public Oferta()
        {
        }

        public ApplicationUser Usuario { get; set; }
    }
}
