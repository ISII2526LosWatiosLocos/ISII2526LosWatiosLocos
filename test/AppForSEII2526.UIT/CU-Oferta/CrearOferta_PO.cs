using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using AppForSEII2526.UIT.Shared;
using Xunit.Abstractions;
using System;

namespace AppForSEII2526.UIT.CU_Oferta
{
    public class CrearOferta_PO : PageObject
    {
        // IDs extraídos de CrearOferta.razor
        private By _fechaInicio = By.Id("FechaInicio");
        private By _fechaFin = By.Id("FechaFin");
        private By _metodoPago = By.Id("MetodoPago");
        private By _nombreUsuario = By.Id("NombreUsuario");
        private By _dirigidaA = By.Id("DirigidaA");
        private By _submitButton = By.Id("Submit");
        private By _errorsShown = By.Id("ErrorsShown");
        private By _modificarButton = By.Id("ModifyHerramientas");

        public CrearOferta_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void RellenarDatosGenerales(DateTime inicio, DateTime fin, string pagoValue, string usuario, string dirigidaA)
        {
            WaitForBeingVisible(_fechaInicio);
            // Fechas (usando método heredado de PageObject)
            InputDateInDatePicker(_fechaInicio, inicio);
            InputDateInDatePicker(_fechaFin, fin);

            // Selectores
            new SelectElement(_driver.FindElement(_metodoPago)).SelectByValue(pagoValue);

            // Texto
            _driver.FindElement(_nombreUsuario).Clear();
            _driver.FindElement(_nombreUsuario).SendKeys(usuario);

            // Opcional
            if (!string.IsNullOrEmpty(dirigidaA))
            {
                new SelectElement(_driver.FindElement(_dirigidaA)).SelectByValue(dirigidaA);
            }
        }

        public void EstablecerPorcentaje(int herramientaId, int porcentaje)
        {
            By inputPorcentaje = By.Id($"Porcentaje_{herramientaId}");

            WaitForBeingVisible(inputPorcentaje);
            var element = _driver.FindElement(inputPorcentaje);

            element.SendKeys(Keys.Control + "a");
            element.SendKeys(Keys.Delete);
            element.SendKeys(porcentaje.ToString());
        }

        public void PulsarCrearOferta()
        {
            WaitForBeingClickable(_submitButton);
            _driver.FindElement(_submitButton).Click();
        }

        public void PulsarModificarCarrito()
        {
            WaitForBeingClickable(_modificarButton);
            _driver.FindElement(_modificarButton).Click();
        }

        public void ConfirmarModal()
        {
            PressOkModalDialog();
        }

        public bool CheckErrorMessage(string message)
        {
            try
            {
                WaitForBeingVisible(_errorsShown);
                string actualError = _driver.FindElement(_errorsShown).Text;
                return actualError.Contains(message);
            }
            catch (WebDriverTimeoutException)
            {
                return false; // No salió el mensaje de error
            }
        }
    }
}