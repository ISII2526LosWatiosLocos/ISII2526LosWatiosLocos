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
            //wait for the webelement to be clickable
            WaitForBeingClickable(inputNombre);
            /**
            _driver.FindElement(inputTitle).SendKeys(title);
            _driver.FindElement(buttonSearchMovies).Click(); **/


        }
    }
}
