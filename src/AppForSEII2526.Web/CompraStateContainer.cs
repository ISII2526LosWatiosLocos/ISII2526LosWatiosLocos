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

        public void AñadirHerramientaAlCarroDeCompra(HerramientasParaComprarDTO herramienta)
        {
            // Buscar si la herramienta ya está en el carrito
            var itemExistente = Compra.Items
                .FirstOrDefault(i => i.NombreHerramienta == herramienta.Nombre);

            if (itemExistente != null)
            {
                // Si existe, aumentamos la cantidad
                itemExistente.CantidadHerramienta++;
            }
            else
            {
                // Si no existe, la agregamos con cantidad 1
                Compra.Items.Add(new CompraItemsDTO()
                {
                    NombreHerramienta = herramienta.Nombre,
                    MaterialHerramienta = herramienta.Material,
                    PrecioHerramienta = herramienta.Precio,
                    CantidadHerramienta = 1
                });
            }

            NotifyStateChanged();
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