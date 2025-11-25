using AppForSEII2526.Web.API;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppForSEII2526.Web
{
    public class CompraStateContainer
    {

        // Creamos una instancia de Compra cuando CompraStateContainer es instanciado
        public CrearCompraDTO Compra { get; private set; } = new CrearCompraDTO()
        {
            Items = new List<CompraItemsDTO>()
        };

        // Calculamos el precio total de la compra, lo hago en decimal y no float porque así está en el ejemplo
        // dos decimales para moneda (2)
        // opción recomendada para cálculos financieros (MidpointRounding.AwayFromZero)
        public decimal PrecioTotal
        {
            get
            {
                return Math.Round(
                    Compra.Items.Sum(i => (decimal)i.PrecioHerramienta * i.CantidadHerramienta),
                    2,
                    MidpointRounding.AwayFromZero
                );
            }
        }

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();


        // PREGUNTARLE A NOELIA SI HAY QUE HACER ALGUNA COMPROBACIÓN,
        // ADEMÁS DE DONDE TENGO QUE SACAR EL RESTO DE ATRIBUTOS QUE ESPERA UN CompraItemsDTO QUE HerramientasParaComprarDTO NO DA
        public void AñadirHerramientaAlCarroDeCompra(HerramientasParaComprarDTO herramienta)
        {
            // Aquí se pueden hacer comprobaciones adicionales antes de meter los items al carro, pero por ahora no hago ninguna
            Compra.Items.Add(new CompraItemsDTO()
                {
                // IdHerramienta = idHerramienta;
                NombreHerramienta = herramienta.Nombre,
                MaterialHerramienta = herramienta.Material,
                PrecioHerramienta = herramienta.Precio
                // DescripcionHerramienta = descripcionHerramienta;
                // CantidadHerramienta = cantidadHerramienta;
            }
            );

        }

        // Elimina solo las herramientas seleccionadas de la compra (el item, más bien)
        public void QuitarItemDelCarroDeCompra(CompraItemsDTO item)
        {
            Compra.Items.Remove(item);

        }
        
        // Elimina todos las herramientas de la compra
        public void VaciarCarroDeCompra()
        {
            Compra.Items.Clear();

        }

        // En cuanto terminamos de procesarla, creamos otra instancia de Compra para atender la siguiente
        public void CompraProcesada()
        {
            // Al acabar el proceso creamos un objeto vacío
            Compra = new CrearCompraDTO()
            {
                Items = new List<CompraItemsDTO>()
            };
        }
    }
}