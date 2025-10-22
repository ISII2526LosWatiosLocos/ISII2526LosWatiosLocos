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

        public override bool Equals(object? obj)
        {
            return obj is ComprasDTO dTO &&
                   Nombre == dTO.Nombre &&
                   Apellidos == dTO.Apellidos &&
                   DirecciónEnvío == dTO.DirecciónEnvío &&
                   PrecioTotal == dTO.PrecioTotal &&
                   FechaCompra.Equals(dTO.FechaCompra) &&
                   NombreHerramienta == dTO.NombreHerramienta &&
                   MaterialHerramienta == dTO.MaterialHerramienta &&
                   PrecioHerramienta == dTO.PrecioHerramienta &&
                   DescripcionHerramienta == dTO.DescripcionHerramienta &&
                   CantidadHerramienta == dTO.CantidadHerramienta;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Nombre);
            hash.Add(Apellidos);
            hash.Add(DirecciónEnvío);
            hash.Add(PrecioTotal);
            hash.Add(FechaCompra);
            hash.Add(NombreHerramienta);
            hash.Add(MaterialHerramienta);
            hash.Add(PrecioHerramienta);
            hash.Add(DescripcionHerramienta);
            hash.Add(CantidadHerramienta);
            return hash.ToHashCode();
        }
    }
}
