namespace AppForSEII2526.API.DTOs
{
    public class ComprasDTO
    {
        private string Nombre { get; set; }
        private string Apellidos { get; set; }
        public string DirecciónEnvío { get; set; }
        public float PrecioTotal { get; set; }
        public DateOnly FechaCompra { get; set; }
        public string NombreHerramienta { get; set; }
        public string MaterialHerramienta { get; set; }
        public float PrecioHerramienta { get; set; }
        public string DescripcionHerramienta { get; set; }
        public int CantidadHerramienta { get; set; }


        public ComprasDTO(string Nombre, string Apellidos, string DirecciónEnvío, float PrecioTotal, DateOnly FechaCompra, string NombreHerramienta, string MaterialHerramienta, float PrecioHerramienta, string DescripcionHerramienta, int CantidadHerramienta)
        {
            this.Nombre = Nombre;
            this.Apellidos = Apellidos;
            this.DirecciónEnvío = DirecciónEnvío;
            this.PrecioTotal = PrecioTotal;
            this.FechaCompra = FechaCompra;
            this.NombreHerramienta = NombreHerramienta;
            this.MaterialHerramienta = MaterialHerramienta;
            this.PrecioHerramienta = PrecioHerramienta;
            this.DescripcionHerramienta = DescripcionHerramienta;
            this.CantidadHerramienta = CantidadHerramienta;
            
        }

    }
}
