using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using AppForSEII2526.UIT.Shared;
using Xunit.Abstractions;
using System;

namespace AppForSEII2526.UIT.CU_Compra
{
    internal class CrearCompra_PO : PageObject
    {
        // IDs extraídos de CrearCompra.razor (solo las que utilizo, no todas)
        private By _ErrorsShown = By.Id("ErrorsShown");
        private By _Nombre = By.Id("Nombre");
        private By _Apellidos = By.Id("Apellidos");
        private By _DireccionEnvio = By.Id("DireccionEnvio");
        private By _MetodoPago = By.Id("MetodoPago");
        private By _Submit = By.Id("Submit");
        private By _modificarButton = By.Id("ModifyHerramientas");

        public CrearCompra_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void RellenarDatosGenerales(string nombre, string apellidos, string direccionEnvio, string pagoValue)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(3)); // Espera a renderizar
            // Texto
            WaitForBeingVisible(_Nombre);
            _driver.FindElement(_Nombre).Clear();
            _driver.FindElement(_Nombre).SendKeys(nombre);

            WaitForBeingVisible(_Apellidos);
            _driver.FindElement(_Apellidos).Clear();
            _driver.FindElement(_Apellidos).SendKeys(apellidos);

            WaitForBeingVisible(_DireccionEnvio);
            _driver.FindElement(_DireccionEnvio).Clear();
            _driver.FindElement(_DireccionEnvio).SendKeys(direccionEnvio);

            // Selectores
            new SelectElement(_driver.FindElement(_MetodoPago)).SelectByValue(pagoValue);

            // Opcional
            // TODO: Implementar atributos opcionales en la UI
        }

        public void RellenarDescripcion(int IdHerramienta, string descripcion)
        {
            By inputDescripcion = By.Id($"Descripcion_{IdHerramienta}");

            WaitForBeingVisible(inputDescripcion);
            var element = _driver.FindElement(inputDescripcion);

            element.SendKeys(Keys.Control + "a");
            element.SendKeys(Keys.Delete);
            if (descripcion == null)
            {
                element.Clear();
            }
            else
            {
                element.SendKeys(descripcion);
            }

        }

        public void PulsarModificarCarrito()
        {
            WaitForBeingClickable(_modificarButton);
            _driver.FindElement(_modificarButton).Click();
        }

        public void PulsarCrearCompra()
        {
            WaitForBeingClickable(_Submit);
            _driver.FindElement(_Submit).Click();
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
                WaitForBeingVisible(_ErrorsShown);
                string actualError = _driver.FindElement(_ErrorsShown).Text;
                return actualError.Contains(message);
            }
            catch (WebDriverTimeoutException)
            {
                return false; // No salió el mensaje de error
            }
        }

        //Comprobar si el botón de crear está activo
        public bool IsCrearButtonEnabled()
        {
            WaitForBeingVisible(_Submit);
            return _driver.FindElement(_Submit).Enabled;
        }

    }
}