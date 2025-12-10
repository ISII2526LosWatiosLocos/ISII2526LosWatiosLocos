using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppForSEII2526.UIT.CU_Reparacion
{
    public class SeleccionarHerramientasParaReparacion_PO : PageObject
    {
        By inputTitle = By.Id("Nombreherramienta");
        By inputGenre = By.Id("inputTeimporeparacion");                    // Realmente es el imput de timpo, si me da tiempo cambio el razor para que tenga más sentido
        By BotonbuscarHerramientas = By.Id("buscarHerramientas");
        public SeleccionarHerramientasParaReparacion_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {


        }

        public void BuscarHerramientas(string nombre, string tiempoReparacion)
        {
            //wait for the webelement to be clickable
            WaitForBeingClickable(inputTitle);
            _driver.FindElement(inputTitle).SendKeys(nombre);
            _driver.FindElement(BotonbuscarHerramientas).Click();

            // aquí en el código proporcionado debería ir el genre, pero no tengo nada que siga la misma lógica

            _driver.FindElement(inputTitle).SendKeys(nombre);

            if (tiempoReparacion == "")
                tiempoReparacion = "0";

            _driver.FindElement(inputGenre).Clear();
            _driver.FindElement(inputGenre).SendKeys(tiempoReparacion);


        }
    }
}