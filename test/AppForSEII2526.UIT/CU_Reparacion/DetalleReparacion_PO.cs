using System;
using OpenQA.Selenium;
using Xunit.Abstractions;
using AppForSEII2526.UIT.Shared;
using System.Collections.Generic;

namespace AppForSEII2526.UIT.CU_Reparacion
{
    public class GetDetailsReparacion_PO : PageObject
    {
        public GetDetailsReparacion_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        private By tablaReparacionItemBy = By.Id("HerramientasEnReparacion");

        public bool CheckDetallesReparacion(string nombreCompleto, DateTime fechaEntrega, DateTime fechaRecogida, string metodoPago, string totalCoste)
        {
            WaitForBeingVisible(By.Id("Name"));

            bool result = true;

            result &= _driver.FindElement(By.Id("Name")).Text.Contains(nombreCompleto);
            result &= _driver.FindElement(By.Id("FechaEntrega")).Text.Contains(fechaEntrega.ToString("dd/MM/yyyy"));
            result &= _driver.FindElement(By.Id("FechaRecogida")).Text.Contains(fechaRecogida.ToString("dd/MM/yyyy"));
            result &= _driver.FindElement(By.Id("MetodoPago")).Text.Contains(metodoPago);
            result &= _driver.FindElement(By.Id("TotalCoste")).Text.Contains(totalCoste);

            return result;
        }

    }
}