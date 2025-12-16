// Path: test/AppForSEII2526.UIT/CU_Reparacion/SeleccionarHerramientasParaReparacion_PO.cs
using AppForSEII2526.UIT.Shared;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.CU_Reparacion
{
    public class SeleccionarHerramientasParaReparacion_PO : PageObject
    {
        // Selectores confirmados del Razor (SeleccionherramientaParaReparaciones.razor)
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

            WaitForBeingVisible(_inputTiempoReparacion);
            _driver.FindElement(_inputTiempoReparacion).Clear();
            _driver.FindElement(_inputTiempoReparacion).SendKeys(tiempoReparacion);

            _driver.FindElement(_buscarButton).Click();

            WaitForBeingVisible(_tableReparacion);
        }

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