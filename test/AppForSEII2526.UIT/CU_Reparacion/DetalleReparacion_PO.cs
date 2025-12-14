using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.CU_Reparacion
{
    public class DetalleReparacion_PO : PageObject
    {
        // Tablas de la página de detalle (PASO 7)
        By tablaDetallesReparacion = By.Id("tablaDetallesReparacion");
        By tablaHerramientasReparadas = By.Id("tablaHerramientasReparadas");

        // Elementos individuales para verificación directa
        By labelNombreCompleto = By.Id("NombreCompleto");
        By labelFechaEntrega = By.Id("FechaEntrega");
        By labelFechaRecogida = By.Id("FechaRecogida");
        By labelPrecioTotal = By.Id("PrecioTotal");
        By labelMetodoPago = By.Id("MetodoPago");

        public DetalleReparacion_PO(IWebDriver driver, ITestOutputHelper output)
            : base(driver, output)
        {
        }

        // Verificar detalles generales usando tabla 
        public bool CheckDetallesReparacion(List<string[]> expectedDetalles)
        {
            return CheckBodyTable(expectedDetalles, tablaDetallesReparacion);
        }

        // Verificar herramientas reparadas
        public bool CheckHerramientasReparadas(List<string[]> expectedHerramientas)
        {
            return CheckBodyTable(expectedHerramientas, tablaHerramientasReparadas);
        }

        // Métodos de verificación directa (alternativa)
        public bool VerificarNombreCompleto(string nombreEsperado)
        {
            WaitForBeingVisible(labelNombreCompleto);
            return _driver.FindElement(labelNombreCompleto).Text.Contains(nombreEsperado);
        }

        public bool VerificarFechaEntrega(string fechaEsperada)
        {
            WaitForBeingVisible(labelFechaEntrega);
            return _driver.FindElement(labelFechaEntrega).Text.Contains(fechaEsperada);
        }

        public bool VerificarPrecioTotal(string precioEsperado)
        {
            WaitForBeingVisible(labelPrecioTotal);
            var texto = _driver.FindElement(labelPrecioTotal).Text;
            return texto.Contains(precioEsperado);
        }

        public bool VerificarMetodoPago(string metodoPagoEsperado)
        {
            WaitForBeingVisible(labelMetodoPago);
            return _driver.FindElement(labelMetodoPago).Text.Contains(metodoPagoEsperado);
        }
    }
}