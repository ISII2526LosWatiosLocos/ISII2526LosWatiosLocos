using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using AppForSEII2526.UIT.Shared;
using Xunit.Abstractions;
using System.Collections.Generic;

namespace AppForSEII2526.UIT.CU_Oferta
{
    public class SelectHerramientasParaOfertar_PO : PageObject
    {
        private By inputfabricante = By.Id("fabricanteSelected");
        private By inputPrecio = By.Id("inputPrecio");
        private By buttonBuscar = By.Id("buscarHerramientas");
        private By tableOferta = By.Id("TableOfOferta");

        private By buttonContinuar = By.Id("btn_continuar_oferta");

        public SelectHerramientasParaOfertar_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void SearchHerramientas(string? nombreFabricante, float? precio)
        {
            WaitForBeingClickable(inputfabricante);
            if (nombreFabricante != null)
            {
                var fabricanteInputElement = _driver.FindElement(inputfabricante);
                fabricanteInputElement.SendKeys(nombreFabricante);
            }
            if (precio != null)
            {
                var precioInputElement = _driver.FindElement(inputPrecio);
                precioInputElement.Clear();
                precioInputElement.SendKeys(precio.ToString());
            }
            _driver.FindElement(buttonBuscar).Click();

        }


        public bool CheckListOfHerramientas(List<string[]> expectedHerramientas)
        {
            return CheckBodyTable(expectedHerramientas, tableOferta);
        }

        public void AddHerramientaToOfertaCart(string nombreHerramienta)
        {
            By btnAddLocator = By.Id($"btn_add_{nombreHerramienta}");

            ClickWithRetry(btnAddLocator);
            // Esperar y clicar
            WaitForBeingClickable(btnAddLocator);
            _driver.FindElement(btnAddLocator).Click();

            By btnRemoveLocator = By.Id($"removeHerramienta_{nombreHerramienta}");

            WaitForBeingVisible(btnRemoveLocator);
        }

        public void borrarHerramienta(string nombreHerramienta)
        {
            By btnBorrarLocator = By.Id($"removeHerramienta_{nombreHerramienta}");
            WaitForBeingClickable(btnBorrarLocator);
            _driver.FindElement(btnBorrarLocator).Click();
        }

        public void PressContinuar()
        {
            // Como hemos esperado al carrito arriba, este botón ya debería estar habilitado
            WaitForBeingClickable(buttonContinuar);
            _driver.FindElement(buttonContinuar).Click();
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
    }
}