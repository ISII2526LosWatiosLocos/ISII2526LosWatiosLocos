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
        // Selectores CORREGIDOS
        private readonly By _inputNombre = By.Id("Nombre"); 
        private readonly By _inputApellidos = By.Id("Apellidos"); 
        private readonly By _inputTelefono = By.Id("Telefono"); 
        private readonly By _inputFechaEntrega = By.Id("FechaEntrega"); 
        private readonly By _selectMetodoPago = By.Id("MetodoPago"); 
        private readonly By _pulsarCrearReparacion = By.Id("Submit"); // Corregido: id="Submit"
        private readonly By _validationSummary = By.CssSelector(".validation-summary-errors");
        private By _modificarButton = By.Id("ModificarHerramientas");

        public CrearReparacion_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void RellenarDatosGenerales(string nombre, string apellidos, string telefono, DateTime fechaEntrega, string metodoPagoValue)
        {
            WaitForBeingVisible(_inputNombre);
            _driver.FindElement(_inputNombre).Clear();
            _driver.FindElement(_inputNombre).SendKeys(nombre);

            WaitForBeingVisible(_inputApellidos);
            _driver.FindElement(_inputApellidos).Clear();
            _driver.FindElement(_inputApellidos).SendKeys(apellidos);

            if (!string.IsNullOrEmpty(telefono))
            {
                WaitForBeingVisible(_inputTelefono);
                _driver.FindElement(_inputTelefono).Clear();
                _driver.FindElement(_inputTelefono).SendKeys(telefono);
            }
            
            InputDateInDatePicker(_inputFechaEntrega, fechaEntrega);
            
            WaitForBeingVisible(_selectMetodoPago);
            var selectElement = new SelectElement(_driver.FindElement(_selectMetodoPago));
            selectElement.SelectByValue(metodoPagoValue);
        }
        
        public void RellenarDatosItemReparacion(int herramientaId, int cantidad, string descripcion)
        {
            // IDs CORREGIDOS: cantidad_{id} y descripcion_{id}
            By inputCantidad = By.Id($"cantidad_{herramientaId}"); 
            By inputDescripcion = By.Id($"descripcion_{herramientaId}"); 
            
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
        
        public void PulsarCrearReparacion()
        {
            ClickWithRetry(_pulsarCrearReparacion);
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
        
        // MÉTODO CheckErrorMessage CONFIRMADO Y CORREGIDO
        public bool CheckErrorMessage(string expectedError)
        {
            // 1. Verificar resumen de validación
            try
            {
                WaitForBeingVisible(_validationSummary);
                IWebElement validationElement = _driver.FindElement(_validationSummary);
                if (validationElement.Text.Contains(expectedError))
                {
                    _output.WriteLine($"Mensaje de Validación Encontrado: {validationElement.Text}");
                    return true;
                }
            }
            catch (Exception) 
            {
            }
            
            // 2. Verificar cuerpo del modal (si el error viene del servidor)
            By _modalDialog = By.Id("DialogModal"); 
            if (CheckModalBodyText(expectedError, _modalDialog))
            {
                _output.WriteLine("Error Encontrado en el Cuerpo del Modal.");
                return true;
            }
            
            return false;
        }

        public bool IsCrearReparacionButtonDisabled()
        {
            try
            {
                IWebElement element = _driver.FindElement(_pulsarCrearReparacion);
                return !element.Enabled || element.GetAttribute("disabled") != null;
            }
            catch (Exception)
            {
                return true; 
            }
        }
    }
}