using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.CU_Reparacion
{
    internal class GetSelectReparacion_PO : PageObject
    {
        By inputNombreHerramienta = By.Id("inputNombreHerramienta");
        By inputTiempoReparacion = By.Id("tiempoReparacionInput");
        By buttonSearchHerramientas = By.Id("searchHerramientas");
        By tableOfHerramientas = By.Id("TableOfHerramientas");
        By buttonContinuarReparacion = By.Id("createReparacionButton");

        public GetSelectReparacion_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void BuscarHerramientas(string nombre, string tiempo)
        {
            // Filtro por Nombre
            WaitForBeingClickable(inputNombreHerramienta);
            _driver.FindElement(inputNombreHerramienta).SendKeys(nombre);
            Thread.Sleep(1000);
            //Filtro por tiempo de reparación
            WaitForBeingClickable(inputTiempoReparacion);
            _driver.FindElement(inputTiempoReparacion).SendKeys(tiempo);
            Thread.Sleep(1000);
            _driver.FindElement(buttonSearchHerramientas).Click();

        }

        public bool VerificarTablaDeHerramientas(List<string[]> expectedHerramientas)
        {
            return CheckBodyTable(expectedHerramientas, tableOfHerramientas);
        }

        public void AñadirHerramientaAlCarro(string nombreHerramienta)
        {
            // El ID en el razor es: "herramientaForReparar_" + Nombre
            By botonSeleccionar = By.Id("herramientaForReparar_" + nombreHerramienta);
            Thread.Sleep(1000);
            WaitForBeingClickable(botonSeleccionar);
            Thread.Sleep(1000);
            _driver.FindElement(botonSeleccionar).Click();
        }

        public void ContinuarReparacion()
        {
            WaitForBeingClickable(buttonContinuarReparacion);
            _driver.FindElement(buttonContinuarReparacion).Click();
        }

        public bool RepararHerramientaNoDisponible()
        {
            try
            {
                return _driver.FindElement(buttonContinuarReparacion).Displayed == false;
            }
            catch (Exception ex)
            {
                return true;
            }


        }
        public void QuitarDelCarrito(string nombreHerramienta)
        {
            string xpath = $"//div[contains(@class, 'card-body')]//div[contains(@class, 'col-12') and descendant::strong[text()='{nombreHerramienta}']]//button[contains(@class, 'btn-close')]";
            By botonEliminar = By.XPath(xpath);
            WaitForBeingClickable(botonEliminar);
            _driver.FindElement(botonEliminar).Click();
        }
        public string ObtenerTotalEstimado()
        {

            By precioTotal = By.Id("totalEstimado");

            WaitForBeingVisible(precioTotal);
            return _driver.FindElement(precioTotal).Text;
        }


        public void ClickRepararHerramientas()
        {
            WaitForBeingClickable(buttonContinuarReparacion);
            _driver.FindElement(buttonContinuarReparacion).Click();
        }

    }
}