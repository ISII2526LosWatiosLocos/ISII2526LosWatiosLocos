using System.ComponentModel.DataAnnotations;
namespace AppForSEII2526.API.Models
{
    public class Reparación
    {
        [Key]
        public int Id { get; set; }



            [Required]
            [DataType(System.ComponentModel.DataAnnotations.DataType.Date), Display(Name = "Fecha Entrega")]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
            public DateTime FechaEntrega { get; set; }

            [Required]
            [DataType(System.ComponentModel.DataAnnotations.DataType.Date), Display(Name = "Fecha Recogida")]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
            public DateTime FechaRecogida { get; set; }




            [Required]
            [DataType(System.ComponentModel.DataAnnotations.DataType.Currency), Display(Name = "Precio Total")]
            public float PrecioTotal { get; set; }

            [Required]
            public MetodosPago MétodoPago { get; set; }
        //Relaciones 
        public List<ReparaciónItem> ReparaciónItems { get; set; }


    }
}
