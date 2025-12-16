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
        // Selectores CORREGIDOS basados en CreateReparacion.razor
        private readonly By _inputNombre = By.Id("Nombre"); // ID Corregido
        private readonly By _inputApellidos = By.Id("Apellidos"); // ID Corregido
        private readonly By _inputTelefono = By.Id("Telefono"); // ID Corregido
        private readonly By _inputFechaEntrega = By.Id("FechaEntrega"); // ID Corregido
        private readonly By _selectMetodoPago = By.Id("MetodoPago"); // ID Corregido

        private readonly By _pulsarCrearReparacion = By.Id("Submit"); // ID Corregido

        // Selector para el resumen de validación (usado en FA4)
        private readonly By _validationSummary = By.CssSelector(".validation-summary-errors");


        public CrearReparacion_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        // Flujo Básico 5
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

        // Flujo Básico 5 (Items)
        public void RellenarDatosItemReparacion(int herramientaId, int cantidad, string descripcion)
        {
            // IDs CORREGIDOS
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

        // Flujo Básico 6
        public void PulsarCrearReparacion()
        {
            ClickWithRetry(_pulsarCrearReparacion);
        }

        public void ConfirmarModal()
        {
            PressOkModalDialog();
        }

        // <<<<<<<<<< AQUÍ ESTÁ EL MÉTODO CheckErrorMessage >>>>>>>>>>
        // Flujo Alternativo 4 (Datos obligatorios faltantes) y Flujo Alternativo 1 (Fecha inválida)
        public bool CheckErrorMessage(string expectedError)
        {
            // 1. Verificar resumen de validación (errores de campo)
            try
            {
                // Espera a que el resumen de validación esté visible
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
                // No se encontró el resumen de validación, se ignora.
            }

            // 2. Verificar cuerpo del modal (errores de negocio, como FA1 si el error viene del backend)
            By _modalDialog = By.Id("DialogModal"); // ID asumido para el modal
            if (CheckModalBodyText(expectedError, _modalDialog))
            {
                _output.WriteLine("Error Encontrado en el Cuerpo del Modal.");
                return true;
            }

            return false;
        }
        // <<<<<<<<<< FIN DEL MÉTODO CheckErrorMessage >>>>>>>>>>

        // Flujo Alternativo 5
        public bool IsCrearReparacionButtonDisabled()
        {
            try
            {
                IWebElement element = _driver.FindElement(_pulsarCrearReparacion);
                // Retorna true si el elemento no está habilitado o tiene el atributo 'disabled'
                return !element.Enabled || element.GetAttribute("disabled") != null;
            }
            catch (Exception)
            {
                return true;
            }
        }
    }
}