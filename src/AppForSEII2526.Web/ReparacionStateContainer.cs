using AppForSEII2526.Web.API;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppForSEII2526.Web
{
    public class ReparacionStateContainer
   
        {
          
            public CrearReparacionDTO Reparacion { get; private set; } = new CrearReparacionDTO()
            {
                ReparacionesItems = new List<ReparacionesItemDTO>()
            };

            public float PrecioTotal
            {
                get
                {
                   
                    return Reparacion.PrecioTotal;
                }
            }

            public event Action? OnChange;

            private void NotifyStateChanged() => OnChange?.Invoke();

            public void AgregarHerramientaAReparacion(HerramientasParaReparaciónDTO herramienta)
            {
                Reparacion.ReparacionesItems.Add(new ReparacionesItemDTO()
                {
                    HerramientaNombre = herramienta.Nombre,           
                    HerramientaPrecio = herramienta.Precio,
                    HerramientaDescripcion = $"Reparar {herramienta.Nombre}",
                    HerramientaCantidad = 1
                });

                NotifyStateChanged();
            }

            public void QuitarItemDeReparacion(ReparacionesItemDTO item)
            {
                Reparacion.ReparacionesItems.Remove(item);
                NotifyStateChanged();
            }

            public void VaciarReparacion()
            {
                Reparacion.ReparacionesItems.Clear();
                NotifyStateChanged();
            }

            public void ReparacionProcesada()
            {
                Reparacion = new CrearReparacionDTO()
                {
                    ReparacionesItems = new List<ReparacionesItemDTO>()
                };
                NotifyStateChanged();
            }

         
            
          

        
        }
    


}
