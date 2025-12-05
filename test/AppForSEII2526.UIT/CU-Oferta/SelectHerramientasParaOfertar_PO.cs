using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppForSEII2526.UIT.CU_Oferta
{
    internal class SelectHerramientasParaOfertar_PO : PageObject
    {
        By inputfabricante = By.Id("fabricanteSelected");
        By inputprecio = By.Id("inputPrecio");
        By buttonSelectHerramientasOferta = By.Id("buscarHerramientas");

        public SelectHerramientasParaOfertar_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
            
        }

        public void searchHerramientas(string Nombrefabricante, float precio)
        {
            //wait for the webelement to be clickable
            WaitForBeingClickable(inputfabricante);
            _driver.FindElement(inputfabricante).SendKeys(Nombrefabricante);
            if (Nombrefabricante == "")
            {
                Nombrefabricante = "All";
            }
            SelectElement selectElement = new SelectElement(_driver.FindElement(inputfabricante));
            selectElement.SelectByText(Nombrefabricante);
            _driver.FindElement(buttonSelectHerramientasOferta).Click();
        }
    }
}
