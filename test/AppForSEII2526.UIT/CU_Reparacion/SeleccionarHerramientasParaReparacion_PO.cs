using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.CU_Reparacion
{
    public class SeleccionarHerramientasParaReparacion_PO : PageObject
    {
        By inputTitle = By.Id("Nombreherramienta");
        By inputGenre = By.Id("inputGenre");                    // Realmente es el imput de timpo, si me da tiempo cambio el razor para que tenga más sentido
        By buscarHerramientas = By.Id("buscarHerramientas");
        public SeleccionarHerramientasParaReparacion_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {


        }

        public void SearchMovies(string title)
        {
            //wait for the webelement to be clickable
            WaitForBeingClickable(inputTitle);
            _driver.FindElement(inputTitle).SendKeys(title);
            _driver.FindElement(buscarHerramientas).Click();

        }
    }
}