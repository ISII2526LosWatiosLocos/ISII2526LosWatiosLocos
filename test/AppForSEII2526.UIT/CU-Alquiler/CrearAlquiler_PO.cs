using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using AppForSEII2526.UIT.Shared;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.CU_Alquiler
{
    public class CrearAlquiler_PO: PageObject
    {
        // IDs extraídos de CrearAlquiler.razor
        private By _nombreUsuario = By.Id("Name");
        private By _apellidosUsuario = By.Id("Surname");
        private By _direccion = By.Id("DeliveryAddress");
        private By _fechaInicio = By.Id("InitialDate");
        private By _fechaFin = By.Id("FinalDate");
        private By _telefono = By.Id("Phone");
        private By _correo = By.Id("Email");
        private By _metodoPago = By.Id("PaymentMethod");
        private By _submitButton = By.Id("Submit");
        private By _errorsShown = By.Id("ErrorsShown");

        public CrearAlquiler_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }
        public void RellenarDatosGenerales( DateTime inicio,
                                            DateTime fin,
                                            string usuario,
                                            string apellidos,
                                            string direccion,
                                            string telefono,
                                            string correo,
                                            string metodoPago)
        {
            WaitForBeingVisible(_fechaInicio);
            // Fechas (usando método heredado de PageObject)
            InputDateInDatePicker(_fechaInicio, inicio);
            InputDateInDatePicker(_fechaFin, fin);

            // Selectores
            new SelectElement(_driver.FindElement(_metodoPago)).SelectByValue(metodoPago);

            // Texto
            _driver.FindElement(_nombreUsuario).Clear();
            _driver.FindElement(_nombreUsuario).SendKeys(usuario);
            _driver.FindElement(_apellidosUsuario).Clear();
            _driver.FindElement(_apellidosUsuario).SendKeys(apellidos);
            _driver.FindElement(_direccion).Clear();
            _driver.FindElement(_direccion).SendKeys(direccion);
            _driver.FindElement(_telefono).Clear();
            _driver.FindElement(_telefono).SendKeys(telefono);
            _driver.FindElement(_correo).Clear();
            _driver.FindElement(_correo).SendKeys(correo);

        }

        public void EstablecerCantidad(int herramientaId, int cantidad)
        {
            By inputCantidad = By.Id($"cantidad_{herramientaId}");

            WaitForBeingVisible(inputCantidad);
            var element = _driver.FindElement(inputCantidad);

            element.SendKeys(Keys.Control + "a");
            element.SendKeys(Keys.Delete);
            element.SendKeys(inputCantidad.ToString());
        }
        public void PulsarCrearAlquiler()
        {
            WaitForBeingClickable(_submitButton);
            _driver.FindElement(_submitButton).Click();
        }
        public void ConfirmarModal()
        {
            // Usa el método ya existente en tu PageObject base
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
