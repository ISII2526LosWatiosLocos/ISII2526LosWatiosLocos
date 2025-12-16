using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using AppForSEII2526.UIT.Shared;
using Xunit.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System;
// Añadido para métodos base temporales. Asegúrate de tener la referencia:
using SeleniumExtras.WaitHelpers;

namespace AppForSEII2526.UIT.CU_Reparacion
{
    // Asume que PageObject hereda de una clase base que tiene IWebDriver _driver y ITestOutputHelper _output
    public class DetalleReparacion_PO : PageObject
    {
        // IDs mapeados a DetalleReparacion.razor
        By labelFechaEntrega = By.Id("FechaEntrega"); // td con la fecha de entrega
        By tablaHerramientasReparadas = By.Id("HerramientasReparacion"); // Tabla de items
        By finalTotalPrice = By.Id("TotalPrice"); // td en el footer con el precio total

        // XPath para obtener el valor de la celda "Nombre y Apellidos"
        By rowNombreCompleto = By.XPath("//table[@class='table table-borderless table-sm']/tbody/tr[1]/td");

        public DetalleReparacion_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output) { }

        // ======================================================================
        // MÉTODOS BASE TEMPORALES PARA RESOLVER EL ERROR DE COMPILACIÓN
        // SI ESTOS MÉTODOS EXISTEN EN TU CLASE BASE (PageObject), BÓRRALOS DE AQUÍ.
        // ======================================================================
        protected void WaitForBeingVisible(By locator, int timeoutSeconds = 30)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutSeconds));
            wait.Until(ExpectedConditions.ElementIsVisible(locator));
        }
        // ======================================================================


        // Verifica el nombre y apellidos (usa la primera celda de la tabla de cabecera)
        public bool VerificarNombreCompleto(string nombreEsperado)
        {
            WaitForBeingVisible(rowNombreCompleto);
            return _driver.FindElement(rowNombreCompleto).Text.Contains(nombreEsperado);
        }

        // Verifica la fecha de entrega
        public bool VerificarFechaEntrega(string fechaEsperada)
        {
            WaitForBeingVisible(labelFechaEntrega);
            // El formato es "dd/MM/yyyy HH:mm:ss", la prueba solo verifica "dd/MM/yyyy"
            return _driver.FindElement(labelFechaEntrega).Text.Contains(fechaEsperada);
        }

        // Verifica el precio total (usa el footer <tfoot>)
        public bool VerificarPrecioTotal(string precioEsperado)
        {
            WaitForBeingVisible(finalTotalPrice);
            // Busca el precio total formateado ("57.40€" en el caso de la prueba)
            return _driver.FindElement(finalTotalPrice).Text.Contains(precioEsperado);
        }

        // Verifica la tabla de herramientas reparadas (Paso 7 del Flujo Básico)
        public bool CheckHerramientasReparadas(List<string[]> expectedHerramientas)
        {
            // Asumiendo que CheckBodyTable está en tu PageObject base y compara el body de la tabla
            return CheckBodyTable(expectedHerramientas, tablaHerramientasReparadas);
        }
    }
}