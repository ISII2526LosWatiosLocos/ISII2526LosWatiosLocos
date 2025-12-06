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
        By tableOfHerramientasBy = By.Id("TableOfOferta");
        By errorShownBy = By.Id("ErrorsShown");
        By buttonAlquilerHerramienta = By.Id("alquilerHerramientaButton");

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

        public bool CheckListOfHerramientas(List<string[]> expectedHerramientas)
        {
            return CheckBodyTable(expectedHerramientas, tableOfHerramientasBy);
        }

        public bool CheckMessageError(string errorMessage)
        {
            IWebElement actualErrorShown = _driver.FindElement(errorShownBy);
            _output.WriteLine($"actual Message shown:{actualErrorShown.Text}");
            return actualErrorShown.Text.Contains(errorMessage);
        }

        public void AddHerramientaToAlquilerCart(string herramientaTitle)
        {
            WaitForBeingClickable(By.Id("herramientaToAlquiler_" + herramientaTitle));

            _driver.FindElement(By.Id("herramientaToAlquiler" + herramientaTitle)).Click();
        }

        public void RemoveHerramientaFromAlquilerCart(string herramientaTitle)
        {
            WaitForBeingClickable(By.Id("removeHerramienta_" + herramientaTitle));
            _driver.FindElement(By.Id("removeHerramienta" + herramientaTitle)).Click();
        }

        public bool AlquilerNotAvailable()
        {
            return _driver.FindElement(buttonAlquilerHerramienta).Displayed == false;
        }
    }
}
