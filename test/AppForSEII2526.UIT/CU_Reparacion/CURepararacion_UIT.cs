
using AppForMovies.UIT.Shared;
using AppForSEII2526.UIT.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.CU_Reparacion
{
    public class CURepararacion_UIT: UC_UIT
         
    {

        private SeleccionarHerramientasParaReparacion_PO seleccionarHerramientasParaReparacion_PO;

        public CURepararacion_UIT(ITestOutputHelper output) : base(output)
        {
        }

        private void Precondition_perform_login()
        {
            Perform_login("elena@uclm.es", "Password1234%");
        }

        private void InitialStepsForRentalMovies()
        {
            Precondition_perform_login();
            //we wait for the option of the menu to be visible
           seleccionarHerramientasParaReparacion_PO.WaitForBeingVisible(By.Id("CrearReparacion"));
            //we click on the menu
            _driver.FindElement(By.Id("CrearReparacion")).Click();
        }

    }
}
