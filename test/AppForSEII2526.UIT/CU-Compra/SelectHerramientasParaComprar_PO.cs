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
            WaitForBeingClickable(inputMaterial);
            if (string.IsNullOrEmpty(material)) material = ""; // String vacío muestra todas, simplemente me aseguro de que no sea null
            var txtMaterial = _driver.FindElement(inputMaterial);
            txtMaterial.Clear();
            if (!string.IsNullOrEmpty(material))
                txtMaterial.SendKeys(material);

            WaitForBeingClickable(inputPrecio);
            if (string.IsNullOrEmpty(precio)) precio = ""; // String vacío muestra todas, simplemente me aseguro de que no sea null
            var txtPrecio = _driver.FindElement(inputPrecio);
            txtPrecio.Clear();
            if (!string.IsNullOrEmpty(precio))
                txtPrecio.SendKeys(precio);

            // Faltaba pulsar el botón buscar para que el filtro surta efecto
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

                throw new System.Exception("Error: El carrito debería estar vacío, pero el botón 'Continuar' es visible.");
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