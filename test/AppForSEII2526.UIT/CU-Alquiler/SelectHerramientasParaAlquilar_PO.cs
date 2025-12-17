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
        By inputMaterial = By.Id("inputMaterialHerramienta");
        By tablaHerramientas = By.Id("TablaDeHerramientas");
        By buscarHerramientas = By.Id("buscarHerramientas");
        private By botonContinuar = By.Id("btn_continuar_alquiler");
        public SelectHerramientasParaAlquilar_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }
        public void BuscarHerramientas(string? nombre, string? material)
        {

            if (nombre != null)
            {
                if (material == null)
                {
                    WaitForBeingVisible(inputMaterial);
                    var mat = _driver.FindElement(inputMaterial);
                    mat.Clear();
                }
                WaitForBeingVisible(inputNombre);
                var nom = _driver.FindElement(inputNombre);
                nom.Clear();
                nom.SendKeys(nombre);
            }


            if (material != null)
            {
                if (nombre == null)
                {
                    WaitForBeingVisible(inputNombre);
                    var nom = _driver.FindElement(inputNombre);
                    nom.Clear();
                }
                WaitForBeingVisible(inputMaterial);
                var mat = _driver.FindElement(inputMaterial);
                mat.Clear();
                mat.SendKeys(material);
            }
            _driver.FindElement(buscarHerramientas).Click();
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
        public void borrarHerramienta(string nombreHerramienta)
        {
            By botonBorrarHerramienta = By.Id($"quitarHerramienta_{nombreHerramienta}");
            WaitForBeingClickable(botonBorrarHerramienta);
            _driver.FindElement(botonBorrarHerramienta).Click();
        }
        public bool CheckEmptyCart()
        {
            var botonesContinuar = _driver.FindElements(botonContinuar);


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
            WaitForBeingClickable(botonContinuar);
            _driver.FindElement(botonContinuar).Click();
        }
    }
}