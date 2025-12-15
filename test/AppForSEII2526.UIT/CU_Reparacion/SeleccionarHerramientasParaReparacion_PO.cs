using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using AppForSEII2526.UIT.Shared;
using Xunit.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System;
using SeleniumExtras.WaitHelpers; // Requiere que tengas la librería Selenium.Support instalada

namespace AppForSEII2526.UIT.CU_Reparacion
{
    // AÑADIDO: Asumo que PageObject hereda de una clase base que tiene IWebDriver _driver y ITestOutputHelper _output
    public class SeleccionarHerramientasParaReparacion_PO : PageObject
    {
        // ... (resto de selectores)
        By inputTitle = By.Id("inputTitle");
        By inputGenre = By.Id("inputGenre");
        By BotonbuscarHerramientas = By.Id("buscarHerramientas");
        By tableReparacion = By.Id("TableReparacion");
        By BotonRepararHerramientas = By.Id("procesarReparacionButton");
        By errorShownBy = By.XPath("//div[starts-with(@class, 'row') and not(@hidden)]/p");
        By inputFrom = By.Id("fromDate");
        By inputTo = By.Id("toDate");
        // ... (fin de selectores)

        public SeleccionarHerramientasParaReparacion_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output) { }

        // MÉTODOS BASE TEMPORALMENTE AÑADIDOS PARA RESOLVER ERROR DE COMPILACIÓN
        // Si estos métodos ya están en tu PageObject base, BÓRRAMELOS de aquí.
        protected void WaitForBeingVisible(By locator, int timeoutSeconds = 30)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutSeconds));
            wait.Until(ExpectedConditions.ElementIsVisible(locator));
        }

        protected void WaitForBeingClickable(By locator, int timeoutSeconds = 30)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutSeconds));
            wait.Until(ExpectedConditions.ElementToBeClickable(locator));
        }
        // FIN DE MÉTODOS BASE TEMPORALMENTE AÑADIDOS


        // SOLUCIÓN SRE: Recarga los elementos justo antes de usarlos
        public void BuscarHerramientas(string nombre, string tiempoReparacion, string from, string to)
        {
            WaitForBeingClickable(inputTitle);

            // 1. Nombre (inputTitle)
            var inputNombreElement = _driver.FindElement(By.Id("inputTitle"));
            inputNombreElement.Clear();
            if (!string.IsNullOrEmpty(nombre))
                inputNombreElement.SendKeys(nombre);

            // 2. Tiempo (inputGenre)
            if (string.IsNullOrEmpty(tiempoReparacion))
                tiempoReparacion = "0";

            var inputTiempoElement = _driver.FindElement(By.Id("inputGenre"));
            inputTiempoElement.Clear();
            inputTiempoElement.SendKeys(tiempoReparacion);

            // 3. Fechas (Se asume que inputFrom/inputTo existen si se usan en el test)
            if (!string.IsNullOrEmpty(from))
                _driver.FindElement(inputFrom).SendKeys(from);
            if (!string.IsNullOrEmpty(to))
                _driver.FindElement(inputTo).SendKeys(to);

            _driver.FindElement(BotonbuscarHerramientas).Click();
        }

        // SOLUCIÓN FALLO 0 FILAS: Espera explícita después de la búsqueda
        public void WaitForResults()
        {
            WaitForBeingVisible(tableReparacion);
        }

        public bool CheckListOfHerramientas(List<string[]> expectedHerramientas)
        {
            return CheckBodyTable(expectedHerramientas, tableReparacion);
        }

        public void AddHerramientaToReparacionCart(string herramientaNombre)
        {
            By addButtonId = By.Id("OfertaData_" + herramientaNombre);
            WaitForBeingClickable(addButtonId);
            _driver.FindElement(addButtonId).Click();
        }

        public void RemoveHerramientaFromReparacionCart(string herramientaNombre)
        {
            By removeButtonId = By.Id("removeHerramienta_" + herramientaNombre);
            WaitForBeingClickable(removeButtonId);
            _driver.FindElement(removeButtonId).Click();
        }

        public void PressRepararHerramientas()
        {
            WaitForBeingClickable(BotonRepararHerramientas);
            _driver.FindElement(BotonRepararHerramientas).Click();
        }

        public bool IsRepararButtonEnabled()
        {
            try
            {
                IWebElement button = _driver.FindElement(BotonRepararHerramientas);
                return button.Enabled && button.Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public bool CheckMessageError(string errorMessage)
        {
            try
            {
                WaitForBeingVisible(errorShownBy, 5);
                return _driver.FindElement(errorShownBy).Text.Contains(errorMessage);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ReparacionNotAvailable()
        {
            return _driver.FindElements(BotonRepararHerramientas).Count == 0 || !_driver.FindElement(BotonRepararHerramientas).Displayed;
        }
    }
}