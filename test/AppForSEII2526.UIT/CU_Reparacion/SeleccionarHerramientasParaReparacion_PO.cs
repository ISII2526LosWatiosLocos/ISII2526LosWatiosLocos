// Path: test/AppForSEII2526.UIT/CU_Reparacion/SeleccionarHerramientasParaReparacion_PO.cs
using AppForSEII2526.UIT.Shared;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.CU_Reparacion
{
    public class SeleccionarHerramientasParaReparacion_PO : PageObject
    {
        // Selectores basados en SeleccionherramientaParaReparaciones.razor
        private readonly By _inputNombre = By.Id("inputTitle");
        private readonly By _inputTiempoReparacion = By.Id("inputGenre"); // ID del Razor: inputGenre
        private readonly By _buscarButton = By.Id("buscarHerramientas");
        private readonly By _tableReparacion = By.Id("TableReparacion");
        private readonly By _procesarReparacionButton = By.Id("procesarReparacionButton");

        // Selector para el div de errores que se hace visible (el div pasa a tener hidden="False" cuando hay errores)
        private readonly By _errorDivVisible = By.XPath("//div[@hidden='False']/p[text()='hAquí voy a mostrar los errores']");


        public SeleccionarHerramientasParaReparacion_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void BuscarHerramientas(string nombre, string tiempoReparacion, string fechaDesde = "", string fechaHasta = "")
        {
            // Solo implementamos los filtros disponibles en el Razor (Nombre y TiempoReparacion)
            WaitForBeingVisible(_inputNombre);
            _driver.FindElement(_inputNombre).Clear();
            _driver.FindElement(_inputNombre).SendKeys(nombre);

            WaitForBeingVisible(_inputTiempoReparacion);
            _driver.FindElement(_inputTiempoReparacion).Clear();
            _driver.FindElement(_inputTiempoReparacion).SendKeys(tiempoReparacion);

            _driver.FindElement(_buscarButton).Click();

            WaitForBeingVisible(_tableReparacion);
        }

        public void AñadirHerramientaAReparacionCart(string nombreHerramienta)
        {
            // El ID del botón de añadir en el Razor es "OfertaData_..."
            By addToolButton = By.Id($"OfertaData_{nombreHerramienta}");
            WaitForBeingClickable(addToolButton);
            _driver.FindElement(addToolButton).Click();
        }

        public void RemoveHerramientaFromReparacionCart(string nombreHerramienta)
        {
            By removeButton = By.Id($"removeHerramienta_{nombreHerramienta}");
            WaitForBeingClickable(removeButton);
            _driver.FindElement(removeButton).Click();
        }

        public void ProcesarReparacion()
        {
            WaitForBeingClickable(_procesarReparacionButton);
            _driver.FindElement(_procesarReparacionButton).Click();
        }

        public bool CheckListOfHerramientas(List<string[]> expectedRows)
        {
            return CheckBodyTable(expectedRows, _tableReparacion);
        }

        public bool ReparacionNotAvailable()
        {
            // El carrito se esconde si está vacío (EsconderReparacionCrrito)
            // Comprobamos si el botón 'Procesar Reparación' NO es visible.
            try
            {
                WaitForBeingVisible(_procesarReparacionButton);
                return false; // Si es visible, el carrito está disponible.
            }
            catch (WebDriverTimeoutException)
            {
                return true; // Si salta excepción, no es visible (es decir, el carrito no está disponible).
            }
        }

        public bool CheckMessageError(string expectedError)
        {
            try
            {
                // El div de error se muestra sin el atributo hidden
                WaitForBeingVisible(_errorDivVisible);
                string actualText = _driver.FindElement(_errorDivVisible).Text;
                return actualText.Contains(expectedError);
            }
            catch (WebDriverTimeoutException)
            {
                _output.WriteLine("Error: El mensaje de error esperado no se hizo visible.");
                return false;
            }
        }
    }
}