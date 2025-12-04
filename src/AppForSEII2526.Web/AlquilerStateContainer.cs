using System;
using System.Collections.Generic;
using System.Linq;
using AppForSEII2526.Web.API;

namespace AppForSEII2526.Web
{
    public class AlquilerStateContainer
    {
        //Creamos una instancia del alquiler cuando se crea un statecontainer
        public CrearAlquilerDTO Alquilar { get; private set; } = new CrearAlquilerDTO()
        {
            Items = new List<CrearAlquilerItemDTO>()
        };

        // Calculamos el precio total de las herramientas que hemos seleccionado para alquilar
        public decimal PrecioTotal => Convert.ToDecimal(Alquilar.Items.Sum(ri => ri.HerramientaCantidad * ri.HerramientaPrecio));

        public event Action? OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();

        public void AddHerramientaToAlquiler(HerramientasParaAlquilarDTO herramienta)
        {
            // Antes de añadirla comprobamos si ya esta
            if (!Alquilar.Items.Any(ri => ri.HerramientaId == herramienta.Id))
            {
                // Si no esta en la lista la añadimos
                Alquilar.Items.Add(new CrearAlquilerItemDTO()
                {
                    // Añadir más atributos
                    HerramientaId = herramienta.Id,
                    HerramientaCantidad = 1,
                    HerramientaPrecio = herramienta.Precio
                });
                NotifyStateChanged();
            }
        }

        // Para borrar herramientas seleccionadas de la lista
        public void BorraItemParaAlquilar(CrearAlquilerItemDTO item)
        {
            Alquilar.Items.Remove(item);
            NotifyStateChanged();
        }

        // Para eliminar todas las herramientas de la lista
        public void LimpiarCarritoAlquiler()
        {
            Alquilar.Items.Clear();
            NotifyStateChanged();
        }

        // Ya hicimos el proceso de alquiler, por lo tento creamos un nuevo alquiler

        public void AlquilerProcesado()
        {
            // Terminamos el proceeso de alquiler asi que creaemos un nuevo objeto sin datos
            Alquilar = new CrearAlquilerDTO()
            {
                Items = new List<CrearAlquilerItemDTO>()
            };
            NotifyStateChanged();
        }
    }
}
