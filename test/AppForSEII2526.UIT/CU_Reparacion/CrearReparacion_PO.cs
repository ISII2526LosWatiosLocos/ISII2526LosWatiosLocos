using AppForSEII2526.UIT.Shared;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text.RegularExpressions;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.CU_Reparacion
{
    public class CreateReparacion_PO : PageObject
    {

        private By inputNombre = By.Id("Nombre");
        private By inputApellidos = By.Id("Apellido");
        private By inputTelefono = By.Id("Telefono");
        private By inputFechaEntrega = By.Id("FechaEntrega");
        private By inputMetodoPago = By.Id("MetodoPago");
        private By btnRegistrarReparacion = By.Id("Submit");
        private By btnModificarHerramientas = By.Id("ModifyReparacion");
        private By dialogOkButton = By.Id("Button_DialogOK");
        private By tableOfReparacionItems = By.Id("TableOfReparacionItems");
        public CreateReparacion_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }
        public void RellenarFormularioReparacion(string nombreC, string apellidosC, string telefono, DateTime fechaEntrega, string metodoPago)
        {
            //Nombre
            WaitForBeingClickable(inputNombre);
            _driver.FindElement(inputNombre).Clear();
            _driver.FindElement(inputNombre).SendKeys(nombreC);

            //Apellidos
            WaitForBeingClickable(inputApellidos);
            _driver.FindElement(inputApellidos).Clear();
            _driver.FindElement(inputApellidos).SendKeys(apellidosC);

            //Teléfono
            WaitForBeingClickable(inputTelefono);
            _driver.FindElement(inputTelefono).Clear();
            _driver.FindElement(inputTelefono).SendKeys(telefono);

            //Fecha
            WaitForBeingClickable(inputFechaEntrega);
            _driver.FindElement(inputFechaEntrega).SendKeys(fechaEntrega.ToString("dd/MM/yyyy"));

            //Método de Pago
            WaitForBeingClickable(inputMetodoPago);
            var selectElement = new SelectElement(_driver.FindElement(inputMetodoPago));
            selectElement.SelectByText(metodoPago);
        }

        public void RellenarDescripcionReparacion(string descripcion, int idHerramienta)
        {
            By descripcionInput = By.Id($"Descripcion_{idHerramienta}");
            WaitForBeingClickable(descripcionInput);
            _driver.FindElement(descripcionInput).Clear();
            _driver.FindElement(descripcionInput).SendKeys(descripcion);
        }

        public void ClickRegistrarButton()
        {
            WaitForBeingClickable(btnRegistrarReparacion);
            _driver.FindElement(btnRegistrarReparacion).Click();
        }

        public void ConfirmDialog()
        {
            WaitForBeingClickable(dialogOkButton);
            _driver.FindElement(dialogOkButton).Click();
        }

        public void ClickModificarHerramientas()
        {
            WaitForBeingClickable(btnModificarHerramientas);
            _driver.FindElement(btnModificarHerramientas).Click();
        }

        public bool CheckValidationError(string error)
        {
            return _driver.PageSource.Contains(error);
        }
    }
}