
using AppForSEII2526.Web.API;
namespace AppForSEII2526.Web
{
    public class ReparacionStateContainer
    {
        // Adaptado: ReparacionForCreateDTO -> CrearReparacionDTO
        public CrearReparacionDTO Reparacion { get; private set; } = new CrearReparacionDTO()
        {
            // Adaptado: ReparacionItem -> Items
            // Nota: Tu DTO usa List<T>, así que inicializamos como List.
            ReparacionesItems = new List<ReparacionesItemDTO>()
        };

        public event Action? OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();

        // He adaptado la entrada para recibir la herramienta seleccionada en la vista
        public void AddReparacionItem(HerramientasParaReparaciónDTO item)
        {
            // Adaptado: Comprobamos por Nombre porque no tenemos ID fiables en el DTO de selección
            if (!Reparacion.ReparacionesItems.Any(ri => ri.HerramientaNombre == item.Nombre))
            {
                Reparacion.ReparacionesItems.Add(new ReparacionesItemDTO()
                {
                    // Adaptado: Mapeo de propiedades a tus nombres de DTO
                    // IdHerramienta = ??? (Lo omitimos o ponemos 0 si no lo tienes)
                    HerramientaNombre = item.Nombre,
                    HerramientaDescripcion = "", // Campo obligatorio en tu DTO, inicializado vacío
                    HerramientaPrecio = item.Precio,
                    HerramientaCantidad = 1 // Inicializamos a 1

                    // Nota: Tu ReparacionesItemDTO no tiene 'NombreFabricante' ni 'TiempoReparacion'
                    // ni 'PrecioTotal' guardado, así que no los asignamos aquí.
                });

                NotifyStateChanged();
            }
        }

        public void RemoveReparacionItem(ReparacionesItemDTO item)
        {
            // Adaptado: Buscamos por Nombre
            var itemToRemove = Reparacion.ReparacionesItems.FirstOrDefault(ri => ri.HerramientaNombre == item.HerramientaNombre);
            if (itemToRemove != null)
            {
                Reparacion.ReparacionesItems.Remove(itemToRemove);
                NotifyStateChanged();
            }
        }

        public void ClearReparacionItems()
        {
            Reparacion.ReparacionesItems.Clear();
            NotifyStateChanged();
        }

        public void ReparacionProcessed()
        {
            Reparacion = new CrearReparacionDTO()
            {
                ReparacionesItems = new List<ReparacionesItemDTO>(),
                // Mantenemos la lógica de fecha que tenías antes por si acaso
                FechaEntrega = DateTime.Now.AddDays(1)
            };
            NotifyStateChanged();
        }

        // Propiedad extra útil para mostrar el total en la UI (basada en tu código anterior)
        public decimal PrecioTotal
        {
            get
            {
                return Math.Round(
                    (decimal)Reparacion.ReparacionesItems.Sum(i => i.HerramientaPrecio * i.HerramientaCantidad),
                    2,
                    MidpointRounding.AwayFromZero
                );
            }
        }
    }
}