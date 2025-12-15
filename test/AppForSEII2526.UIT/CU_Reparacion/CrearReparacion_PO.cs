using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using AppForSEII2526.UIT.Shared;
using Xunit.Abstractions;
using System;
using System.Linq;
using System.Collections.Generic;
// Añadido para métodos base temporales. Asegúrate de tener la referencia:
using SeleniumExtras.WaitHelpers;

namespace AppForSEII2526.UIT.CU_Reparacion
{
    public class CrearReparacion_PO : PageObject
    {
        // IDs mapeados exactamente a CreateReparacion.razor
        By inputNombre = By.Id("Nombre");
        By inputApellidos = By.Id("Apellidos");
        By inputFechaEntrega = By.Id("FechaEntrega");
        By selectMetodoPago = By.Id("MetodoPago");
        By inputTelefono = By.Id("Telefono");

        // ID REAL del botón de Guardar/Submit
        By buttonGuardar = By.Id("Submit");

        // ID REAL del botón de Modificar Carrito (AF2)
        By buttonModificarCarrito = By.Id("ModificarHerramientas");

        // ID REAL del div/p que muestra los errores de validación (AF4)
        By errorsShown = By.Id("ErrorsShown");

        // El constructor necesita IWebDriver y ITestOutputHelper (asumido de PageObject)
        public CrearReparacion_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output) { }

        // ======================================================================
        // MÉTODOS BASE TEMPORALES PARA RESOLVER EL ERROR DE COMPILACIÓN
        // SI ESTOS MÉTODOS EXISTEN EN TU CLASE BASE (PageObject), BÓRRALOS DE AQUÍ.
        // ======================================================================

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

        // ======================================================================
        // FLUJO BÁSICO (UC2_1) Y FLUJOS ALTERNATIVOS
        // ======================================================================

        // Recibe metodoPagoId como string, como lo exige SelectByValue
        public void RellenarDatosCliente(string nombre, string apellidos, DateTime fechaEntrega, string metodoPagoId, string telefono = "")
        {
            WaitForBeingClickable(inputNombre);

            _driver.FindElement(inputNombre).Clear();
            _driver.FindElement(inputNombre).SendKeys(nombre);

            _driver.FindElement(inputApellidos).Clear();
            _driver.FindElement(inputApellidos).SendKeys(apellidos);

            // Asumiendo que InputDateInDatePicker existe en la base (o se usa SendKeys para la fecha)
            // InputDateInDatePicker(inputFechaEntrega, fechaEntrega);

            SelectElement metodoPagoSelect = new SelectElement(_driver.FindElement(selectMetodoPago));
            metodoPagoSelect.SelectByValue(metodoPagoId);

            if (!string.IsNullOrEmpty(telefono))
            {
                _driver.FindElement(inputTelefono).Clear();
                _driver.FindElement(inputTelefono).SendKeys(telefono);
            }
        }

        // Rellena la descripción y cantidad para la herramienta dinámica
        public void RellenarDatosHerramienta(int herramientaId, string descripcion, int cantidad)
        {
            // IDs dinámicos mapeados a HerramientaData_{id}
            By inputDescripcion = By.Id($"descripcion_{herramientaId}");
            By inputCantidad = By.Id($"cantidad_{herramientaId}");

            if (!string.IsNullOrEmpty(descripcion))
            {
                _driver.FindElement(inputDescripcion).Clear();
                _driver.FindElement(inputDescripcion).SendKeys(descripcion);
            }

            _driver.FindElement(inputCantidad).Clear();
            _driver.FindElement(inputCantidad).SendKeys(cantidad.ToString());
        }

        // PASO 6: Pulsar el botón de Guardar/Submit
        public void PulsarGuardarReparacion()
        {
            WaitForBeingClickable(buttonGuardar);
            _driver.FindElement(buttonGuardar).Click();
        }

        // AF2: Pulsar el botón Modificar Herramientas
        public void PulsarModificarCarrito()
        {
            WaitForBeingClickable(buttonModificarCarrito);
            _driver.FindElement(buttonModificarCarrito).Click();
        }

        // AF2: Verifica si la herramienta con el ID específico está presente en la tabla de ítems
        public bool CheckHerramientaPresente(int herramientaId)
        {
            By inputCantidad = By.Id($"cantidad_{herramientaId}");
            return _driver.FindElements(inputCantidad).Any();
        }

        // AF5: Verifica si el botón de Guardar está habilitado
        public bool IsGuardarButtonEnabled()
        {
            try
            {
                return _driver.FindElement(buttonGuardar).Enabled;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        // AF4, AF1: Verifica errores
        public bool CheckErrorMessage(string message)
        {
            try
            {
                WaitForBeingVisible(errorsShown, timeoutSeconds: 5);
                return _driver.FindElement(errorsShown).Text.Contains(message);
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }
    }
}