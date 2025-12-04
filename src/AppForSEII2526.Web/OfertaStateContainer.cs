using AppForSEII2526.Web.API;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppForSEII2526.Web
{

    public class OfertaItemViewModel
    {
        public HerramientasParaOfertarDTO InfoVisual { get; set; }
        public CrearOfertaItemDTO DatosEnvio { get; set; }

        // Propiedad calculada para mostrar el precio final en tiempo real en la tabla
        public float PrecioFinal => InfoVisual.Precio * (1 - (DatosEnvio.PorcentajeDescuento / 100f));
    }
    public class OfertaStateContainer
    {
        // Instancia principal del DTO que se enviará al final
        public CrearOfertaDTO Oferta { get; private set; } = new CrearOfertaDTO { Items = new List<CrearOfertaItemDTO>()};

        // Lista enriquecida para usar en la Interfaz (Blazor). 
        // Usamos esta lista en el foreach de la tabla HTML.
        public List<OfertaItemViewModel> ItemsVisuales { get; private set; } = new List<OfertaItemViewModel>();

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();

        // PASO 3: Añadir herramienta al carrito de ofertas
        public void AddHerramientaToOferta(HerramientasParaOfertarDTO herramienta)
        {
            // Comprobamos si ya existe usando el ID de la herramienta
            if (!ItemsVisuales.Any(i => i.InfoVisual.Id == herramienta.Id))
            {
                // Creamos el ViewModel combinando visual + datos
                var nuevoItem = new OfertaItemViewModel
                {
                    InfoVisual = herramienta,
                    DatosEnvio = new CrearOfertaItemDTO
                    {
                        HerramientaId = herramienta.Id,
                        PorcentajeDescuento = 0 // Inicializamos a 0
                    }
                };

                // Añadimos a la lista visual
                ItemsVisuales.Add(nuevoItem);

                // Mantenemos sincronizada la lista del DTO principal automáticamente
                Oferta.Items.Add(nuevoItem.DatosEnvio);

                NotifyStateChanged();
            }
        }

        // FLUJO ALTERNATIVO 2: Borrar herramienta de la oferta
        public void RemoveOfertaItem(OfertaItemViewModel item)
        {
            ItemsVisuales.Remove(item);
            Oferta.Items.Remove(item.DatosEnvio);
            NotifyStateChanged();
        }

        // Limpiar todo el carrito
        public void ClearOfertaCart()
        {
            ItemsVisuales.Clear();
            Oferta.Items.Clear();
            NotifyStateChanged();
        }

        // PASO 6/7: Al terminar el proceso, reseteamos todo
        public void OfertaProcesada()
        {
            // Reiniciamos el DTO y la lista visual
            Oferta = new CrearOfertaDTO();
            ItemsVisuales = new List<OfertaItemViewModel>();
            NotifyStateChanged();
        }
    }
}