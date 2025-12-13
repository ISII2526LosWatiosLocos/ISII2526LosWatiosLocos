using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.CU_Alquiler
{
    public class SelectHerramientasParaAlquilar_PO : PageObject
    {
        By inputNombre = By.Id("inputNombreHeramienta");
        By inputMaterial = By.Id("inputMaterialHeramienta");
        By tablaHerramientas = By.Id("TablaDeHerramientas");
        private By botonContinuar = By.Id("btn_continuar_alquiler");
        public SelectHerramientasParaAlquilar_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }
        public void BuscarHerramientas(string nombre, string material)
        {
            WaitForBeingClickable(inputNombre);
           if (string.IsNullOrEmpty(nombre)) nombre = ""; // String vacío muestra todas, simplemente me aseguro de que no sea null
            var txtNombre = _driver.FindElement(By.Id("inputNombreHeramienta"));
            txtNombre.Clear();
            if (!string.IsNullOrEmpty(nombre))
                txtNombre.SendKeys(nombre);
            WaitForBeingClickable(inputMaterial);
            if (string.IsNullOrEmpty(material)) material = ""; // String vacío muestra todas, simplemente me aseguro de que no sea null
            var txtMaterial = _driver.FindElement(By.Id("inputMaterialHeramienta"));
            txtMaterial.Clear();
            if (!string.IsNullOrEmpty(material))
                txtMaterial.SendKeys(material);
        }

        public bool CheckListOfHerramientas(List<string[]> expectedHerramientas)
        {
            return CheckBodyTable(expectedHerramientas, tablaHerramientas);
        }
        public void AñadirHerramientasAlCarroDeAlquiler(string nombreHerramienta)
        {
            By btnAddLocator = By.Id($"herramientaParaAlquilar_{nombreHerramienta}");
            // Esperar y clicar
            WaitForBeingClickable(btnAddLocator);
            _driver.FindElement(btnAddLocator).Click();
            By btnRemoveLocator = By.Id($"quitarHerramienta_{nombreHerramienta}");
            WaitForBeingVisible(btnRemoveLocator);
        }
        public void Continuar()
        {
            // Como hemos esperado al carrito arriba, este botón ya debería estar habilitado
            WaitForBeingClickable(botonContinuar);
            _driver.FindElement(botonContinuar).Click();
        }
    }
}