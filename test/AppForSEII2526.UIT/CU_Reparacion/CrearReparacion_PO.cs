// Path: test/AppForSEII2526.UIT/CU_Reparacion/CrearReparacion_PO.cs
using AppForSEII2526.UIT.Shared;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.CU_Reparacion
{
    public class CrearReparacion_PO : PageObject
    {
        // Selectores basados en Flujo Básico 5 y convenciones
        private readonly By _inputNombre = By.Name("Input.ClienteNombre");
        private readonly By _inputApellidos = By.Name("Input.ClienteApellidos");
        private readonly By _inputTelefono = By.Name("Input.TelefonoOpcional");
        private readonly By _inputFechaEntrega = By.Id("Input_FechaEntrega"); // ID asumido para DatePicker
        private readonly By _selectMetodoPago = By.Id("Input_MetodoPago"); // ID asumido para Select/Dropdown
        private readonly By _crearReparacionButton = By.Id("CrearReparacionButton");

        // Selector para mensajes de error de validación en la página (Flujo Alternativo 4)
        private readonly By _validationSummary = By.CssSelector(".text-danger.validation-summary-errors");


        public CrearReparacion_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        // Corresponde a Flujo Básico 5 (Datos generales)
        public void RellenarDatosGenerales(string nombre, string apellidos, string telefono, DateTime fechaEntrega, string metodoPagoValue)
        {
            WaitForBeingVisible(_inputNombre);
            _driver.FindElement(_inputNombre).Clear();
            _driver.FindElement(_inputNombre).SendKeys(nombre);

            WaitForBeingVisible(_inputApellidos);
            _driver.FindElement(_inputApellidos).Clear();
            _driver.FindElement(_inputApellidos).SendKeys(apellidos);

            // Teléfono es opcional
            if (!string.IsNullOrEmpty(telefono))
            {
                WaitForBeingVisible(_inputTelefono);
                _driver.FindElement(_inputTelefono).Clear();
                _driver.FindElement(_inputTelefono).SendKeys(telefono);
            }

            // Fecha de Entrega
            InputDateInDatePicker(_inputFechaEntrega, fechaEntrega);

            // Método de Pago
            WaitForBeingVisible(_selectMetodoPago);
            var selectElement = new SelectElement(_driver.FindElement(_selectMetodoPago));
            selectElement.SelectByValue(metodoPagoValue);
        }

        // Corresponde a Flujo Básico 5 (Datos por herramienta)
        public void RellenarDatosItemReparacion(int herramientaId, int cantidad, string descripcion)
        {
            // IDs/Names por herramienta asumidos
            By inputCantidad = By.Id($"Input_Cantidad_{herramientaId}"); // Requerido - Flujo Alternativo 5
            By inputDescripcion = By.Id($"Input_Descripcion_{herramientaId}"); // Opcional

            WaitForBeingVisible(inputCantidad);
            _driver.FindElement(inputCantidad).Clear();
            _driver.FindElement(inputCantidad).SendKeys(cantidad.ToString());

            if (!string.IsNullOrEmpty(descripcion))
            {
                WaitForBeingVisible(inputDescripcion);
                _driver.FindElement(inputDescripcion).Clear();
                _driver.FindElement(inputDescripcion).SendKeys(descripcion);
            }
        }

        // Corresponde a Flujo Básico 6
        public void PulsarCrearReparacion()
        {
            ClickWithRetry(_crearReparacionButton);
        }

        public void ConfirmarModal()
        {
            PressOkModalDialog();
        }

        // Corresponde a Flujo Alternativo 4
        public bool CheckErrorMessage(string expectedError)
        {
            // 1. Verificar resumen de validación (errores de campo)
            try
            {
                WaitForBeingVisible(_validationSummary);
                if (_driver.FindElement(_validationSummary).Text.Contains(expectedError))
                {
                    _output.WriteLine($"Error de Validación Encontrado: {_driver.FindElement(_validationSummary).Text}");
                    return true;
                }
            }
            catch (Exception) { }

            // 2. Verificar cuerpo del modal (errores de negocio)
            By _modalDialog = By.Id("DialogModal");
            if (CheckModalBodyText(expectedError, _modalDialog))
            {
                _output.WriteLine("Error Encontrado en el Cuerpo del Modal.");
                return true;
            }

            return false;
        }

        // Corresponde a Flujo Alternativo 5
        public bool IsCrearReparacionButtonDisabled()
        {
            // Si no es clickeable, consideramos que está inactivo.
            try
            {
                WaitForBeingClickable(_crearReparacionButton);
                return false; // Si es clickeable, NO está deshabilitado
            }
            catch (WebDriverTimeoutException)
            {
                IWebElement element = _driver.FindElement(_crearReparacionButton);
                return !element.Enabled || element.GetAttribute("disabled") != null;
            }
        }
    }
}