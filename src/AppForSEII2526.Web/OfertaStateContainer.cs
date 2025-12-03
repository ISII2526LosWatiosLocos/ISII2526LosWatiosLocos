using AppForSEII2526.Web.API;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppForSEII2526.Web
{

    public class OfertaStateContainer
    {
        // Instancia principal del DTO que se enviará al final
        public CrearOfertaDTO Oferta { get; private set; } = new CrearOfertaDTO { Items = new List<OfertaItemsDTO>() };

        public float PrecioFinal
        {
            get
            {
                return Oferta.Items.Sum(i => i.PrecioFinalOferta);
            }
        }


        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();

        // PASO 3: Añadir herramienta al carrito de ofertas
        public void AddHerramientaToOferta(HerramientasParaOfertarDTO herramienta)
        {
            if (!Oferta.Items.Any(item => item.NombreHerramienta == herramienta.Nombre))
            {
                Oferta.Items.Add(new OfertaItemsDTO
                {
                    NombreHerramienta = herramienta.Nombre,
                    MaterialHerramienta = herramienta.Material,
                    PrecioHerramienta = herramienta.Precio,
                    // Se inicializa el PrecioFinalOferta con el PrecioHerramienta.
                    PrecioFinalOferta = herramienta.Precio
                });
            }

            NotifyStateChanged();
        }

        // FLUJO ALTERNATIVO 2: Borrar herramienta de la oferta
        public void RemoveOfertaItem(OfertaItemsDTO item)
        {
            Oferta.Items.Remove(item);
            NotifyStateChanged();
        }

        // Limpiar todo el carrito
        public void ClearOfertaCart()
        {
            Oferta.Items.Clear();
            NotifyStateChanged();
        }

        // PASO 6/7: Al terminar el proceso, reseteamos todo
        public void OfertaProcesada()
        {

            Oferta = new CrearOfertaDTO { Items = new List<OfertaItemsDTO>() };
            NotifyStateChanged();
        }
    }
}