// Path: test/AppForSEII2526.UIT/CU_Reparacion/SeleccionarHerramientasParaReparacion_PO.cs
using AppForSEII2526.UIT.Shared;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.CU_Reparacion
{
    public class SeleccionarHerramientasParaReparacion_PO : PageObject
    {
        private readonly By _inputNombre = By.Id("inputTitle");
        private readonly By _inputTiempoReparacion = By.Id("inputGenre");
        private readonly By _buscarButton = By.Id("buscarHerramientas");
        private readonly By _tableReparacion = By.Id("TableReparacion");
        private readonly By _procesarReparacionButton = By.Id("procesarReparacionButton");
        private readonly By _errorDivVisible = By.XPath("//div[@hidden='False']/p[text()='hAquí voy a mostrar los errores']");

        public SeleccionarHerramientasParaReparacion_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void BuscarHerramientas(string nombre, string tiempoReparacion)
        {
            WaitForBeingVisible(_inputNombre);
            _driver.FindElement(_inputNombre).Clear();
            _driver.FindElement(_inputNombre).SendKeys(nombre);

            // CORRECCIÓN PARA EL VALOR INICIAL '0' EN inputGenre
            WaitForBeingVisible(_inputTiempoReparacion);
            IWebElement tiempoInput = _driver.FindElement(_inputTiempoReparacion);

            // Simular Ctrl+A o Cmd+A para seleccionar todo el contenido y luego borrarlo.
            tiempoInput.SendKeys(Keys.Control + "a");
            tiempoInput.SendKeys(Keys.Delete);

            if (!string.IsNullOrEmpty(tiempoReparacion))
            {
                tiempoInput.SendKeys(tiempoReparacion);
            }

            _driver.FindElement(_buscarButton).Click();

            WaitForBeingVisible(_tableReparacion);
        }

        // Resto de los métodos del PO sin cambios...
        public void AñadirHerramientaAReparacionCart(string nombreHerramienta)
        {
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
            try
            {
                var wait = new WebDriverWait(_driver, new TimeSpan(0, 0, 1));
                wait.Until(ExpectedConditions.ElementIsVisible(_procesarReparacionButton));
                return false;
            }
            catch (WebDriverTimeoutException)
            {
                return true;
            }
        }

        public bool CheckMessageError(string expectedError)
        {
            try
            {
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