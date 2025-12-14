using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.CU_Reparacion
{
    public class CrearReparacion_PO : PageObject
    {
        // Elementos del formulario según tu flujo PASO 5
        By inputNombre = By.Id("Nombre");
        By inputApellidos = By.Id("Apellidos");
        By inputFechaEntrega = By.Id("FechaEntrega");
        By selectMetodoPago = By.Id("MetodoPago");
        By inputTelefono = By.Id("Telefono"); // Opcional

        // Elementos por herramienta (PASO 5)
        By inputDescripcionTemplate = By.Id("descripcion"); 
        By inputCantidadTemplate = By.Id("cantidad"); 

        // Botones (PASO 6)
        By buttonGuardar = By.Id("btnGuardarReparacion");
        By modalConfirmar = By.Id("modalConfirmar");
        By buttonConfirmarModal = By.Id("btnConfirmarModal");

        // Mensajes de error
        By errorMessages = By.ClassName("Error");


        By buttonModificarCarrito = By.Id("btnModificarCarrito");

        public CrearReparacion_PO(IWebDriver driver, ITestOutputHelper output)
            : base(driver, output)
        {
        }

        // PASO 5: Rellenar datos del cliente
        public void RellenarDatosCliente(string nombre, string apellidos,
                                        DateTime fechaEntrega, string metodoPagoId,
                                        string telefono = "")
        {
            WaitForBeingClickable(inputNombre);

            _driver.FindElement(inputNombre).Clear();
            _driver.FindElement(inputNombre).SendKeys(nombre);

            _driver.FindElement(inputApellidos).Clear();
            _driver.FindElement(inputApellidos).SendKeys(apellidos);

            // Fecha usando el método del PageObject base
            InputDateInDatePicker(inputFechaEntrega, fechaEntrega);

            // Método de pago (dropdown)
            SelectElement metodoPagoSelect = new SelectElement(_driver.FindElement(selectMetodoPago));
            metodoPagoSelect.SelectByValue(metodoPagoId); // "0", "1", "2"

            // Teléfono opcional
            if (!string.IsNullOrEmpty(telefono))
            {
                _driver.FindElement(inputTelefono).Clear();
                _driver.FindElement(inputTelefono).SendKeys(telefono);
            }
        }

        // PASO 5: Rellenar datos por herramienta
        public void RellenarDatosHerramienta(int herramientaId, string descripcion, int cantidad)
        {
            // Descripción (opcional)
            if (!string.IsNullOrEmpty(descripcion))
            {
                By inputDescripcion = By.Id($"descripcion_{herramientaId}");
                WaitForBeingClickable(inputDescripcion);
                _driver.FindElement(inputDescripcion).Clear();
                _driver.FindElement(inputDescripcion).SendKeys(descripcion);
            }

            // Cantidad (obligatorio)
            By inputCantidad = By.Id($"cantidad_{herramientaId}");
            WaitForBeingClickable(inputCantidad);
            _driver.FindElement(inputCantidad).Clear();
            _driver.FindElement(inputCantidad).SendKeys(cantidad.ToString());
        }

        // PASO 6: Guardar reparación
        public void PulsarGuardarReparacion()
        {
            WaitForBeingClickable(buttonGuardar);
            _driver.FindElement(buttonGuardar).Click();
        }

        // Confirmar modal si aparece
        public void ConfirmarModal()
        {
            try
            {
                WaitForBeingVisible(modalConfirmar, timeoutSeconds: 5);
                WaitForBeingClickable(buttonConfirmarModal);
                _driver.FindElement(buttonConfirmarModal).Click();
            }
            catch (WebDriverTimeoutException)
            {
                // No hay modal, continuar
                _output.WriteLine("No apareció modal de confirmación");
            }
        }

        // Verificar mensajes de error (
        public bool CheckErrorMessage(string errorMessage)
        {
            try
            {
                WaitForBeingVisible(errorMessages, timeoutSeconds: 3);
                var errors = _driver.FindElements(errorMessages);
                return errors.Any(e => e.Text.Contains(errorMessage));
            }
            catch
            {
                return false;
            }
        }

       
        private void WaitForBeingVisible(By locator, int timeoutSeconds = 30)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutSeconds));
            wait.Until(ExpectedConditions.ElementIsVisible(locator));
        }




        // Dentro de la clase CrearReparacion_PO:

        // --- AF2: Modificar Carrito (Volver a la selección de herramientas) ---
        public void PulsarModificarCarrito()
        {
            WaitForBeingClickable(buttonModificarCarrito);
            _driver.FindElement(buttonModificarCarrito).Click();
        }

        // --- AF2: Verificar si una herramienta está presente en el formulario ---
        public bool CheckHerramientaPresente(int herramientaId)
        {
            // Usamos el ID de la Cantidad para verificar que la sección de la herramienta exista
            By inputCantidad = By.Id($"cantidad_{herramientaId}");

            // Si encuentra al menos un elemento con ese ID, la herramienta está presente
            return _driver.FindElements(inputCantidad).Any();
        }


        // --- AF5: Verificar si el botón Guardar está habilitado ---
        public bool IsGuardarButtonEnabled()
        {
            // Verifica si el botón Guardar está presente y habilitado
            try
            {
                WaitForBeingVisible(buttonGuardar, timeoutSeconds: 3); // Esperamos a que aparezca
                return _driver.FindElement(buttonGuardar).Enabled;
            }
            catch (WebDriverTimeoutException)
            {
                return false; // Si no lo encuentra, lo consideramos deshabilitado/no visible
            }
        }
    }


}