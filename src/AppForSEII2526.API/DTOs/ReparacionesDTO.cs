using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.DTOs
{
    public class ReparacionesDTO
    {


        public DateTime FechaEntrega { get; set; }
        public DateTime FechaRecogida { get; set; }
        public float PrecioTotal { get; set; }
        public MetodosPago MétodoPago { get; set; }
        public List<ReparaciónItem> ReparaciónItems { get; set; }
      


        private string nombre { get; set; } 
        private string apellidos { get; set; }

        public string NombreHerramienta { get; set; }
        public string DescripcionHerramienta { get; set; }
        public int CantidadHerramienta { get; set; }
        public float PrecioHerramienta { get; set; }

        public ReparacionesDTO(string nombre, string apellidos, DateTime FechaEntrega, DateTime FechaRecogida, float PrecioTotal, String NombreHerramienta,  float PrecioHerramienta, string DescripcionHerramienta, int CantidadHerramienta)
        {
            this.nombre = nombre;
            this.apellidos = apellidos;
            this.FechaEntrega = FechaEntrega;
            this.FechaRecogida = FechaRecogida;
            this.PrecioTotal = PrecioTotal;
            this.NombreHerramienta= NombreHerramienta;
            this.PrecioHerramienta = PrecioHerramienta;
            this.DescripcionHerramienta = DescripcionHerramienta;
            this.CantidadHerramienta = CantidadHerramienta; 
                 }

        public override bool Equals(object? obj)
        {
            return obj is ReparacionesDTO dTO &&
                   FechaEntrega == dTO.FechaEntrega &&
                   FechaRecogida == dTO.FechaRecogida &&
                   PrecioTotal == dTO.PrecioTotal &&
                   EqualityComparer<MetodosPago>.Default.Equals(MétodoPago, dTO.MétodoPago) &&
                   EqualityComparer<List<ReparaciónItem>>.Default.Equals(ReparaciónItems, dTO.ReparaciónItems) &&
                   nombre == dTO.nombre &&
                   apellidos == dTO.apellidos &&
                   DescripcionHerramienta == dTO.DescripcionHerramienta &&
                   CantidadHerramienta == dTO.CantidadHerramienta &&
                   PrecioHerramienta == dTO.PrecioHerramienta;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(FechaEntrega);
            hash.Add(FechaRecogida);
            hash.Add(PrecioTotal);
            hash.Add(MétodoPago);
            hash.Add(ReparaciónItems);
            hash.Add(nombre);
            hash.Add(apellidos);
            hash.Add(DescripcionHerramienta);
            hash.Add(CantidadHerramienta);
            hash.Add(PrecioHerramienta);
            return hash.ToHashCode();
        }
    }
}



