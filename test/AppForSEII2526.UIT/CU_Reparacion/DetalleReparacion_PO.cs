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
        private readonly By _tableReparacionItems = By.Id("HerramientasReparacion"); // CORREGIDO: id="HerramientasReparacion"
        // Se usa XPath para el nombre/apellidos ya que no tiene un ID único en el Razor proporcionado
        private readonly By _nombreCliente = By.XPath("//th[text()='Nombre y Apellidos']/following-sibling::td"); 
        private readonly By _fechaEntrega = By.Id("FechaEntrega");   // CORREGIDO: id="FechaEntrega"
        private readonly By _precioTotal = By.Id("PrecioTotal");     // CORREGIDO: id="PrecioTotal"

        
        public DetalleReparacion_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public bool CheckReparacionItemTable(List<string[]> expectedItems)
        {
            // Columnas esperadas en el Razor: ID, Nombre, Precio, Cantidad, Descripción
            return CheckBodyTable(expectedItems, _tableReparacionItems);
        }
        
        public bool CheckReparacionSummary(string expectedNombreApellidos, string expectedFechaEntrega, string expectedPrecioTotal)
        {
            WaitForBeingVisible(_fechaEntrega); // Se usa FechaEntrega para la espera
            
            // Nombre y Apellidos (usando el XPath)
            bool nombreApellidosOK = _driver.FindElement(_nombreCliente).Text.Contains(expectedNombreApellidos);
            
            bool fechaEntregaOK = _driver.FindElement(_fechaEntrega).Text.Contains(expectedFechaEntrega);
            bool precioTotalOK = _driver.FindElement(_precioTotal).Text.Contains(expectedPrecioTotal);
            
            return nombreApellidosOK && fechaEntregaOK && precioTotalOK;
        }
    }
}