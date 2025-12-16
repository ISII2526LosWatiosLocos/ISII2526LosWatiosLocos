using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using AppForSEII2526.UIT.Shared;
using Xunit.Abstractions;
using System.Collections.Generic;

namespace AppForSEII2526.UIT.CU_Compra
{
    internal class SelectHerramientasParaComprar_PO : PageObject
    {
        // Corrección: selectores apuntando a ID de inputs, no etiquetas span
        private By inputMaterial = By.Id("inputMaterial");
        private By inputPrecio = By.Id("inputPrecio");
        private By buttonBuscar = By.Id("BuscarHerramientas");
        private By tableCompras = By.Id("TableOfCompras");
        private By _borrarHerramientaButton = By.Id("quitarHerramienta_Martillo");

        // Nuevo ID más claro (Corregido al ID real del botón en Razor)
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
                // Tabular para asegurar que el evento OnChange se dispara antes de buscar
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

            // Faltaba pulsar el botón buscar para que el filtro surta efecto y esperar a que sea clickeable
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

            // Esperar y clicar
            WaitForBeingClickable(btnAddLocator);
            _driver.FindElement(btnAddLocator).Click();

            By btnRemoveLocator = By.Id($"quitarHerramienta_{nombreHerramienta}");

            WaitForBeingVisible(btnRemoveLocator);
        }

        public bool CheckEmptyCart()
        {
            // Corregido: Buscamos el botón correcto.
            var botonesContinuar = _driver.FindElements(buttonContinuar);

            // El botón está en un div con hidden="@hideCompraCart", por tanto si está oculto el carro está vacío
            bool carritoVisible = botonesContinuar.Count > 0 && botonesContinuar[0].Displayed;

            if (carritoVisible)
            {
                return false;
                // Eliminado código inalcanzable (throw) que había aquí para evitar confusiones lógicas
            }
            return true;
        }

        public void borrarHerramienta()
        {
            WaitForBeingClickable(_borrarHerramientaButton);
            _driver.FindElement(_borrarHerramientaButton).Click();
        }

        public void Continuar()
        {
            // Como hemos esperado al carrito arriba, este botón ya debería estar habilitado
            WaitForBeingClickable(buttonContinuar);
            _driver.FindElement(buttonContinuar).Click();
        }
    }
}