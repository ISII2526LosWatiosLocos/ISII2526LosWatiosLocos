using System.ComponentModel.DataAnnotations;
namespace AppForSEII2526.API.Models
{
    public class Reparación
    {
        [Key]
        public int Id { get; set; }


            [Required]
            [DataType(System.ComponentModel.DataAnnotations.DataType.Text), Display(Name = "Apellido Cliente")]
            public string ApellidoCliente { get; set; }

            [Required]
            [DataType(System.ComponentModel.DataAnnotations.DataType.Date), Display(Name = "Fecha Entrega")]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
            public DateTime FechaEntrega { get; set; }

            [Required]
            [DataType(System.ComponentModel.DataAnnotations.DataType.Date), Display(Name = "Fecha Recogida")]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
            public DateTime FechaRecogida { get; set; }

            [Required]
            [DataType(System.ComponentModel.DataAnnotations.DataType.Text), Display(Name = "Nombre Cliente")]
            public string NombreCliente { get; set; }

            [Required]
            [DataType(System.ComponentModel.DataAnnotations.DataType.PhoneNumber), Display(Name = "Número Teléfono")]
            public string NumTelefono { get; set; }

            [Required]
            [DataType(System.ComponentModel.DataAnnotations.DataType.Currency), Display(Name = "Precio Total")]
            public float PrecioTotal { get; set; }

        //Realciones
        public List<ReparaciónItem> ReparaciónItems { get; set; }

    }
}
