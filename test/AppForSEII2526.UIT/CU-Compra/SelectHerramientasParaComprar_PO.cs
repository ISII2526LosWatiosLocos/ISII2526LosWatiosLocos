using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using AppForSEII2526.UIT.Shared;
using Xunit.Abstractions;
using System.Collections.Generic;
using System.Threading;

namespace AppForSEII2526.UIT.CU_Compra
{
    internal class SelectHerramientasParaComprar_PO : PageObject
    {
        private By inputMaterial = By.Id("inputMaterial");
        private By inputPrecio = By.Id("inputPrecio");
        private By buttonBuscar = By.Id("BuscarHerramientas");
        private By tableCompras = By.Id("TableOfCompras");

        private By _borrarHerramientaMartilloButton = By.Id("quitarHerramienta_Martillo");
        private By _borrarHerramientaLlaveButton = By.Id("quitarHerramienta_Llave");
        private By cartItemRemoveButtons = By.XPath("//button[starts-with(@id, 'quitarHerramienta_')]");

        private By buttonContinuar = By.Id("ComprarHerramientasButton");

        public SelectHerramientasParaComprar_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void BuscarHerramientas(string material, string precio)
        {
            WaitForBeingVisible(inputMaterial);
            if (string.IsNullOrEmpty(material)) material = "";
            var txtMaterial = _driver.FindElement(inputMaterial);
            txtMaterial.Clear();
            if (!string.IsNullOrEmpty(material))
            {
                txtMaterial.SendKeys(material);
                txtMaterial.SendKeys(Keys.Tab);
            }

            WaitForBeingVisible(inputPrecio);
            if (string.IsNullOrEmpty(precio)) precio = "";
            var txtPrecio = _driver.FindElement(inputPrecio);
            txtPrecio.Clear();
            if (!string.IsNullOrEmpty(precio))
            {
                txtPrecio.SendKeys(precio);
                txtPrecio.SendKeys(Keys.Tab);
            }

            WaitForBeingClickable(buttonBuscar);
            _driver.FindElement(buttonBuscar).Click();
        }

        public bool CheckListOfHerramientas(List<string[]> expectedHerramientas)
        {
            return CheckBodyTable(expectedHerramientas, tableCompras);
        }

        public void AñadirHerramientasAlCarroDeCompra(string nombreHerramienta)
        {
            By btnAddLocator = By.Id($"herramientaParaComprar_{nombreHerramienta}");

            WaitForBeingClickable(btnAddLocator);
            _driver.FindElement(btnAddLocator).Click();

            By btnRemoveLocator = By.Id($"quitarHerramienta_{nombreHerramienta}");

            WaitForBeingVisible(btnRemoveLocator);
        }

        public bool CheckEmptyCart()
        {
            var botonesContinuar = _driver.FindElements(buttonContinuar);

            bool carritoVisible = botonesContinuar.Count > 0 && botonesContinuar[0].Displayed;

            if (carritoVisible)
            {
                return false;
            }
            return true;
        }

        public bool TryBorrarHerramienta(string nombreHerramienta)
        {
            By locator = By.Id($"quitarHerramienta_{nombreHerramienta}");

            var buttons = _driver.FindElements(locator);

            if (buttons.Count > 0)
            {
                WaitForBeingClickable(locator);
                buttons[0].Click();
                return true;
            }

            return false;
        }

        public void borrarHerramienta(string nombreHerramienta)
        {
            TryBorrarHerramienta(nombreHerramienta);
        }

        public int CountItemsInCart()
        {
            var removeButtons = _driver.FindElements(cartItemRemoveButtons);
            return removeButtons.Count;
        }

        public void ClearCart()
        {
            int count = CountItemsInCart();

            while (count > 0)
            {
                var removeButtons = _driver.FindElements(cartItemRemoveButtons);

                if (removeButtons.Count > 0)
                {
                    IWebElement firstRemoveButton = removeButtons[0];

                    WaitForBeingClickable(cartItemRemoveButtons);

                    firstRemoveButton.Click();

                    Thread.Sleep(500);

                    count = CountItemsInCart();
                }
                else
                {
                    break;
                }
            }
        }

        public void Continuar()
        {
            WaitForBeingClickable(buttonContinuar);
            _driver.FindElement(buttonContinuar).Click();
        }
    }
}