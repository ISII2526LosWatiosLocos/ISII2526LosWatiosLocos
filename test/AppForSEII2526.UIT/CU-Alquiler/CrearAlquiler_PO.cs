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
        private By _modificarHerramientas = By.Id("ModifyHerramientas");

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

            WaitForBeingVisible(_nombreUsuario);
            _driver.FindElement(_nombreUsuario).Clear();
            _driver.FindElement(_nombreUsuario).SendKeys(usuario);

            WaitForBeingVisible(_apellidosUsuario);
            _driver.FindElement(_apellidosUsuario).Clear();
            _driver.FindElement(_apellidosUsuario).SendKeys(apellidos);

            WaitForBeingVisible(_direccion);
            _driver.FindElement(_direccion).Clear();
            _driver.FindElement(_direccion).SendKeys(direccion);

            WaitForBeingVisible(_telefono);
            _driver.FindElement(_telefono).Clear();
            _driver.FindElement(_telefono).SendKeys(telefono);

            WaitForBeingVisible(_correo);
            _driver.FindElement(_correo).Clear();
            _driver.FindElement(_correo).SendKeys(correo);
            
            // Fechas (usando método heredado de PageObject)
            WaitForBeingVisible(_fechaInicio);
            InputDateInDatePicker(_fechaInicio, inicio);

            WaitForBeingVisible(_fechaFin);
            InputDateInDatePicker(_fechaFin, fin);

            var select = new SelectElement(_driver.FindElement(_metodoPago));
            // select.SelectByValue("1"); // Por value (PayPal)
            select.SelectByText(metodoPago);

        }

        // acepta int para evitar ambigüedades y envía el número correcto al input
        public void EstablecerCantidad(int herramientaId, int cantidad)
        {
            By inputCantidad = By.Id($"cantidad_{herramientaId}");

            WaitForBeingVisible(inputCantidad);
            var element = _driver.FindElement(inputCantidad);

            // Borra y escribe el número correcto
            element.Clear();
            // En algunos navegadores/input tipo number puede ser necesario enviar como string
            element.SendKeys(cantidad.ToString());
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
        public void PulsarModificarCarrito()
        {
            WaitForBeingClickable(_modificarHerramientas);
            _driver.FindElement(_modificarHerramientas).Click();
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
