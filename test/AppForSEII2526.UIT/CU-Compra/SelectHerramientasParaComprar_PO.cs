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
            var txtMaterial = _driver.FindElement(By.Id("inputMaterial"));
            txtMaterial.Clear();
            if (!string.IsNullOrEmpty(material))
                txtMaterial.SendKeys(material);

            WaitForBeingClickable(inputPrecio);
            if (string.IsNullOrEmpty(precio)) precio = ""; // String vacío muestra todas, simplemente me aseguro de que no sea null
            var txtPrecio = _driver.FindElement(By.Id("inputPrecio"));
            txtPrecio.Clear();
            if (!string.IsNullOrEmpty(precio))
                txtPrecio.SendKeys(precio);
        }

        public bool CheckListOfHerramientas(List<string[]> expectedHerramientas)
        {
            return CheckBodyTable(expectedHerramientas, tableCompras);
        }

        public void AñadirHerramientasAlCarroDeCompra(string nombreHerramienta)
        {
            By btnAddLocator = By.Id($"herramientaParaComprar_{nombreHerramienta}");

            // Esperar y clicar
            WaitForBeingClickable(btnAddLocator);
            _driver.FindElement(btnAddLocator).Click();

            By btnRemoveLocator = By.Id($"quitarHerramienta_{nombreHerramienta}");

            WaitForBeingVisible(btnRemoveLocator);
        }

        public bool CheckEmptyCart()
        {
            var botonesContinuar = _driver.FindElements(By.Id("btn_continuar_oferta"));


            bool carritoVisible = botonesContinuar.Count > 0 && botonesContinuar[0].Displayed;

            if (carritoVisible)
            {
                return false;

                throw new Exception("Error: El carrito debería estar vacío, pero el botón 'Continuar' es visible.");
            }
            return true;
        }

        public void Continuar()
        {
            // Como hemos esperado al carrito arriba, este botón ya debería estar habilitado
            WaitForBeingClickable(buttonContinuar);
            _driver.FindElement(buttonContinuar).Click();
        }
    }
}
