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
            Items = new List<AlquilarItemsDTO>()
        };

        // Calculamos el precio total de las herramientas que hemos seleccionado para alquilar
        public decimal PrecioTotal => Convert.ToDecimal(Alquilar.Items.Sum(ri => ri.CantidadItem * ri.PrecioItem));

        public event Action? OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();

        public void AddHerramientaToAlquiler(HerramientasParaAlquilarDTO herramienta)
        {
            // Antes de añadirla comprobamos si ya esta
            if (!Alquilar.Items.Any(ri => ri.NombreItem == herramienta.Nombre))
            {
                // Si no esta en la lista la añadimos
                Alquilar.Items.Add(new AlquilarItemsDTO()
                {
                    // Añadir más atributos
                    IdItem = herramienta.Id,
                    NombreItem = herramienta.Nombre,
                    MaterialItem = herramienta.Material,
                    PrecioItem = herramienta.Precio,
                    CantidadItem = 1
                });
                NotifyStateChanged();
            }
        }

        // Para borrar herramientas seleccionadas de la lista
        public void BorraItemParaAlquilar(AlquilarItemsDTO item)
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
                Items = new List<AlquilarItemsDTO>()
            };
            NotifyStateChanged();
        }
    }
}
