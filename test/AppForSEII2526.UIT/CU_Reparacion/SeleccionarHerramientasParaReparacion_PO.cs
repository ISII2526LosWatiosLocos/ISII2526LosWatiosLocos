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
        By inputFrom = By.Id("fromDate");
        By inputTo = By.Id("toDate");
        By tableReparacion = By.Id("TableReparacion");
        By errorShownBy = By.Id("ErrorsShown");
        By BotonRepararHerramientas= By.Id("ReparacionHerramientaBoton");
        public SeleccionarHerramientasParaReparacion_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {


        }

        public void BuscarHerramientas(string nombre, string tiempoReparacion, string from, string to)
        {
            //wait for the webelement to be clickable
            WaitForBeingClickable(inputTitle);
            _driver.FindElement(inputTitle).SendKeys(nombre);

            _driver.FindElement(inputTitle).SendKeys(nombre);

            if (tiempoReparacion == "")
                tiempoReparacion = "0";

            _driver.FindElement(inputGenre).Clear();
            _driver.FindElement(inputGenre).SendKeys(tiempoReparacion);


            if (from != "")
                _driver.FindElement(inputFrom).SendKeys(from);



            if (to != "")
                _driver.FindElement(inputTo).SendKeys(to);



            _driver.FindElement(BotonbuscarHerramientas).Click();

      


        }

        public bool CheckListOfHerramientas(List<string[]> expectedHerramientas)
        {

            return CheckBodyTable(expectedHerramientas, tableReparacion);
        }



        public bool CheckMessageError(string errorMessage)
        {
            IWebElement actualErrorShown = _driver.FindElement(errorShownBy);
            _output.WriteLine($"actual Message shown:{actualErrorShown.Text}");
            return actualErrorShown.Text.Contains(errorMessage);
        }
        public void AddHerramientaToReparacionCart(string herramientaNombre)
        {
           
            WaitForBeingClickable(By.Id("ReparacionData_" + herramientaNombre));

            _driver.FindElement(By.Id("ReparacionData_" + herramientaNombre)).Click();
        }

        public void RemoveHerramientaFromReparacionCart(string herramientaNombre)
        {
            // Necesito saber el ID exacto de tu botón "Remove"
            WaitForBeingClickable(By.Id("removeHerramienta_" + herramientaNombre));

            _driver.FindElement(By.Id("removeHerramienta_" + herramientaNombre)).Click();
        }

        public bool ReparacionNotAvailable()
        {
            
            return _driver.FindElement(BotonRepararHerramientas).Displayed == false;
        }
    }
}