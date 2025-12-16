// Path: test/AppForSEII2526.UIT/CU_Reparacion/DetalleReparacion_PO.cs
using AppForSEII2526.UIT.Shared;
using OpenQA.Selenium;
using System.Collections.Generic;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.CU_Reparacion
{
    public class DetalleReparacion_PO : PageObject
    {
        // Selectores CORREGIDOS basados en DetalleReparacion.razor
        private readonly By _tableReparacionItems = By.Id("HerramientasReparacion"); // ID de la tabla de ítems

        // XPath para el Nombre y Apellidos (el dato está en el td que sigue al th)
        private readonly By _nombreCliente = By.XPath("//th[text()='Nombre y Apellidos']/following-sibling::td");

        private readonly By _fechaEntrega = By.Id("FechaEntrega");   // ID en el td de la fecha de entrega
        private readonly By _precioTotal = By.Id("PrecioTotal");     // ID en el td del período de reparación/precio total

        public DetalleReparacion_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public bool CheckReparacionItemTable(List<string[]> expectedItems)
        {
            // Las columnas en el Razor son: ID, Nombre, Precio, Cantidad, Descripción
            return CheckBodyTable(expectedItems, _tableReparacionItems);
        }

        public bool CheckReparacionSummary(string expectedNombreApellidos, string expectedFechaEntrega, string expectedPrecioTotal)
        {
            WaitForBeingVisible(_fechaEntrega);

            bool nombreApellidosOK = _driver.FindElement(_nombreCliente).Text.Contains(expectedNombreApellidos);
            bool fechaEntregaOK = _driver.FindElement(_fechaEntrega).Text.Contains(expectedFechaEntrega);

            bool precioTotalOK = _driver.FindElement(_precioTotal).Text.Contains(expectedPrecioTotal);

            return nombreApellidosOK && fechaEntregaOK && precioTotalOK;
        }
    }
}