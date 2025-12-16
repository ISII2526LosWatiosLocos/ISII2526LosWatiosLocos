// Path: test/AppForSEII2526.UIT/CU_Reparacion/DetalleReparacion_PO.cs
using AppForSEII2526.UIT.Shared;
using OpenQA.Selenium;
using System.Collections.Generic;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.CU_Reparacion
{
    public class DetalleReparacion_PO : PageObject
    {
        // Selectores asumidos basados en Flujo Básico 7
        private readonly By _tableReparacionItems = By.Id("TableReparacionItems"); // ID asumido
        private readonly By _nombreCliente = By.Id("Detail_NombreCliente"); // ID asumido
        private readonly By _fechaEntrega = By.Id("Detail_FechaEntrega");   // ID asumido
        private readonly By _precioTotal = By.Id("Detail_PrecioTotal");     // ID asumido


        public DetalleReparacion_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        // Comprueba la tabla de ítems de reparación
        public bool CheckReparacionItemTable(List<string[]> expectedItems)
        {
            // Flujo Básico 7: herramientas a reparar (nombre, precio, descripción del problema y cantidad).
            return CheckBodyTable(expectedItems, _tableReparacionItems);
        }

        // Comprueba los datos del encabezado/resumen (Flujo Básico 7)
        public bool CheckReparacionSummary(string expectedNombreApellidos, string expectedFechaEntrega, string expectedPrecioTotal)
        {
            WaitForBeingVisible(_nombreCliente);
            bool nombreApellidosOK = _driver.FindElement(_nombreCliente).Text.Contains(expectedNombreApellidos);
            bool fechaEntregaOK = _driver.FindElement(_fechaEntrega).Text.Contains(expectedFechaEntrega);
            bool precioTotalOK = _driver.FindElement(_precioTotal).Text.Contains(expectedPrecioTotal);

            return nombreApellidosOK && fechaEntregaOK && precioTotalOK;
        }
    }
}