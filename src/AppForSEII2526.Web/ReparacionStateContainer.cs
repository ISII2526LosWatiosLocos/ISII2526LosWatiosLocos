using AppForSEII2526.Web.API;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppForSEII2526.Web
{
    public class ReparacionStateContainer
   
        {
            // EXACTAMENTE como tu compañero
            public CrearReparacionDTO Reparacion { get; private set; } = new CrearReparacionDTO()
            {
                ReparacionesItems = new List<ReparacionesItemDTO>()
            };

            public float PrecioTotal
            {
                get
                {
                    // Aquí deberías calcular basado en precios reales
                    // Por ahora devuelve 0 o el valor que ya tenga el DTO
                    return Reparacion.PrecioTotal;
                }
            }

            public event Action? OnChange;

            private void NotifyStateChanged() => OnChange?.Invoke();

            public void AgregarHerramientaAReparacion(HerramientasParaReparaciónDTO herramienta)
            {
                Reparacion.ReparacionesItems.Add(new ReparacionesItemDTO()
                {
                    
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

            // Métodos adicionales que necesitas (pero tu compañero no tiene):
            
          

        
        }
    


}
