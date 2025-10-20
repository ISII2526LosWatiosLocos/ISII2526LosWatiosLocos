namespace AppForSEII2526.API.DTOs
{
    public class OfertasDTO
    {
        public DateTime FechaFinal { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaOferta { get; set; }
        public string TipoDirigida { get; set; }
        public string MetodoPago { get; set; }

        public string nombreHerramienta { get; set; }

        public string materialHerramienta { get; set; }

        public string fabricanteHerramienta { get; set; }

        public float precioHerramienta { get; set; }

        public float precioOferta { get; set; }

        public OfertasDTO(DateTime fechaFinal, DateTime fechaInicio, DateTime fechaOferta, string tipoDirigida, string metodoPago, string nombreHerramienta, string materialHerramienta, string fabricanteHerramienta, float precioHerramienta, float precioOferta)
        {
            FechaFinal = fechaFinal;
            FechaInicio = fechaInicio;
            FechaOferta = fechaOferta;
            TipoDirigida = tipoDirigida;
            MetodoPago = metodoPago;
            this.nombreHerramienta = nombreHerramienta;
            this.materialHerramienta = materialHerramienta;
            this.fabricanteHerramienta = fabricanteHerramienta;
            this.precioHerramienta = precioHerramienta;
            this.precioOferta = precioOferta;
        }

        public override bool Equals(object? obj)
        {
            return obj is OfertasDTO dTO &&
                   FechaFinal == dTO.FechaFinal &&
                   FechaInicio == dTO.FechaInicio &&
                   FechaOferta == dTO.FechaOferta &&
                   TipoDirigida == dTO.TipoDirigida &&
                   MetodoPago == dTO.MetodoPago &&
                   nombreHerramienta == dTO.nombreHerramienta &&
                   materialHerramienta == dTO.materialHerramienta &&
                   fabricanteHerramienta == dTO.fabricanteHerramienta &&
                   precioHerramienta == dTO.precioHerramienta &&
                   precioOferta == dTO.precioOferta;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(FechaFinal);
            hash.Add(FechaInicio);
            hash.Add(FechaOferta);
            hash.Add(TipoDirigida);
            hash.Add(MetodoPago);
            hash.Add(nombreHerramienta);
            hash.Add(materialHerramienta);
            hash.Add(fabricanteHerramienta);
            hash.Add(precioHerramienta);
            hash.Add(precioOferta);
            return hash.ToHashCode();
        }
    }
}

