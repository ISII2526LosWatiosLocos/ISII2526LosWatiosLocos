// Path: test/AppForSEII2526.UIT/CU_Reparacion/CrearReparacion_PO.cs
using AppForSEII2526.UIT.Shared;
using OpenQA.Selenium;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.CU_Reparacion
{
    public class CrearReparacion_PO : PageObject
    {
        // Selectores asumidos basados en convenciones de formularios Blazor/Identity (Input.Propiedad)
        private readonly By _inputNombre = By.Name("Input.ClienteNombre");
        private readonly By _inputApellidos = By.Name("Input.ClienteApellidos");
        private readonly By _inputTelefono = By.Name("Input.TelefonoOpcional");
        private readonly By _inputDescripcionProblema = By.Name("Input.DescripcionProblema");

        private readonly By _pulsarCrearReparacion = By.Id("CrearReparacionButton");

        // Selectores para mensajes de error de validación en la página
        private readonly By _validationSummary = By.CssSelector(".text-danger.validation-summary-errors");


        public CrearReparacion_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void RellenarDatosGenerales(string nombre, string apellidos, string telefono)
        {
            WaitForBeingVisible(_inputNombre);
            _driver.FindElement(_inputNombre).SendKeys(nombre);

            WaitForBeingVisible(_inputApellidos);
            _driver.FindElement(_inputApellidos).SendKeys(apellidos);

            WaitForBeingVisible(_inputTelefono);
            _driver.FindElement(_inputTelefono).SendKeys(telefono);
        }

        public void RellenarDescripcionProblema(string descripcion)
        {
            WaitForBeingVisible(_inputDescripcionProblema);
            _driver.FindElement(_inputDescripcionProblema).SendKeys(descripcion);
        }

        public void PulsarCrearReparacion()
        {
            WaitForBeingClickable(_pulsarCrearReparacion);
            _driver.FindElement(_pulsarCrearReparacion).Click();
        }

        public void ConfirmarModal()
        {
            // Usa el método del PageObject base para el modal
            PressOkModalDialog();
        }

        public bool CheckErrorMessage(string expectedError)
        {
            // Comprueba si el mensaje de error aparece en el resumen de validación o en el cuerpo del modal.

            // 1. Verificar resumen de validación
            try
            {
                // Espera a que el resumen de validación esté visible y contenga el error
                WaitForBeingVisible(_validationSummary);
                if (_driver.FindElement(_validationSummary).Text.Contains(expectedError))
                {
                    _output.WriteLine($"Error de Validación Encontrado: {_driver.FindElement(_validationSummary).Text}");
                    return true;
                }
            }
            catch (NoSuchElementException)
            {
                // No hay resumen de validación, continúa con la comprobación del modal
            }
            catch (WebDriverTimeoutException)
            {
                // No hay resumen de validación en el tiempo esperado
            }

            // 2. Verificar cuerpo del modal (si no hay resumen de validación)
            // Se asume que el modal tiene el ID "DialogModal" si no se ha definido otro selector.
            // Uso una convención común para el ID del modal si se usa el componente Dialog.razor
            By _modalDialog = By.Id("DialogModal");
            if (CheckModalBodyText(expectedError, _modalDialog))
            {
                _output.WriteLine("Error Encontrado en el Cuerpo del Modal.");
                return true;
            }

            return false;
        }
    }
}