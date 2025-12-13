using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.CU_Alquiler
{
    public class SelectHerramientasParaAlquilar_PO : PageObject
    {
        By inputNombre = By.Id("inputNombreHerramienta");
        By inputMaterial = By.Id("inputMaterialHeramienta");
        By tablaHerramientas = By.Id("TablaDeHerramientas");
        private By botonContinuar = By.Id("btn_continuar_alquiler");
        public SelectHerramientasParaAlquilar_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }
        public void BuscarHerramientas(string nombre, string material)
        {
            WaitForBeingVisible(inputNombre);
            var nom = _driver.FindElement(inputNombre);
            nom.Clear();
            nom.SendKeys(nombre);

            WaitForBeingVisible(inputMaterial);
            var mat = _driver.FindElement(inputMaterial);
            mat.Clear();
            mat.SendKeys(material);
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