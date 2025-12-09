using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using AppForSEII2526.UIT.Shared;
using Xunit.Abstractions;
using System.Collections.Generic;

namespace AppForSEII2526.UIT.CU_Compra
{
    internal class SelectHerramientasParaComprar_PO : PageObject
    {
        private By inputMaterial = By.Id("Material");
        private By inputPrecio = By.Id("Precio");
        private By buttonBuscar = By.Id("BuscarHerramientas");
        private By tableCompras = By.Id("TableOfCompras");

        // Nuevo ID más claro
        private By buttonContinuar = By.Id("ComprarHerramientasButton");

        public SelectHerramientasParaComprar_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void BuscarHerramientas(string material, string precio)
        {
            WaitForBeingClickable(inputMaterial);
            if (string.IsNullOrEmpty(material)) material = ""; // String vacío muestra todas, simplemente me aseguro de que no sea null
            new SelectElement(_driver.FindElement(inputMaterial)).SelectByText(material);
            _driver.FindElement(buttonBuscar).Click();

            WaitForBeingClickable(inputPrecio);
            if (string.IsNullOrEmpty(precio)) precio = ""; // String vacío muestra todas, simplemente me aseguro de que no sea null
            new SelectElement(_driver.FindElement(inputPrecio)).SelectByText(precio);
            _driver.FindElement(buttonBuscar).Click();
        }

        public bool CheckListOfHerramientas(List<string[]> expectedHerramientas)
        {
            return CheckBodyTable(expectedHerramientas, tableCompras);
        }

        public void AñadirHerramientasAlCarroDeCompra(string nombreHerramienta)
        {
            By btnAddLocator = By.Id($"btn_add_{nombreHerramienta}");

            // Esperar y clicar
            WaitForBeingClickable(btnAddLocator);
            _driver.FindElement(btnAddLocator).Click();

            By btnRemoveLocator = By.Id($"removeHerramienta_{nombreHerramienta}");

            WaitForBeingVisible(btnRemoveLocator);
        }

        public void Continuar()
        {
            // Como hemos esperado al carrito arriba, este botón ya debería estar habilitado
            WaitForBeingClickable(buttonContinuar);
            _driver.FindElement(buttonContinuar).Click();
        }
    }
}
