namespace AppForSEII2526.API.DTOs
{
    public class ReparacionesDTO
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
        public List<HerramientasDTO> Items { get; set; }

        // mirar al final que se hace con stos dos 
        public List<ReparaciónItem> ReparaciónItems { get; set; } 
        public ApplicationUser Usuario { get; set; }


        public ReparacionesDTO(int Id, DateTime FechaEntrega, DateTime FechaRecogida, float PrecioTotal, MetodosPago MétodoPago, List<HerramientasDTO> Items, ApplicationUser Usuario) {
            this.Id = Id;
            this.FechaEntrega=FechaEntrega;
            this.FechaRecogida = FechaRecogida;
            this.PrecioTotal=PrecioTotal;
            this.MétodoPago = MétodoPago;
            this.Usuario = Usuario;
            this.Items = Items;
        }

        public override bool Equals(object? obj)
        {
            return obj is ReparacionesDTO dTO &&
                   Id == dTO.Id &&
                   FechaEntrega == dTO.FechaEntrega &&
                   FechaRecogida == dTO.FechaRecogida &&
                   PrecioTotal == dTO.PrecioTotal &&
                   EqualityComparer<MetodosPago>.Default.Equals(MétodoPago, dTO.MétodoPago) &&
                   EqualityComparer<List<HerramientasDTO>>.Default.Equals(Items, dTO.Items) &&
                   EqualityComparer<List<ReparaciónItem>>.Default.Equals(ReparaciónItems, dTO.ReparaciónItems) &&
                   EqualityComparer<ApplicationUser>.Default.Equals(Usuario, dTO.Usuario);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, FechaEntrega, FechaRecogida, PrecioTotal, MétodoPago, Items, ReparaciónItems, Usuario);
        }
    }
}
