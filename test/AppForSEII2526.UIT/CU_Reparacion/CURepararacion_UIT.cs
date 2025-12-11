
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

        private const int HerramientaId1 = 1;
        private const string HerramientaNombre1 = "Martillo";
        private const string HerramientaMaterial1 = "Acero";
        private const string HerramientaFabricante1 = "herramientas SA";
        private const int HerramientaTiempoReparacion1 = 33;
        private const float PrecioHerramientaReparacion1 = 57.4f;



        private const int HerramientaId2 = 3;
        private const string HerramientaNombre2 = "LLave";
        private const string HerramientaMaterial2 = "Acero";
        private const string HerramientaFabricante2 = "herramientas SA";
        private const int HerramientaTiempoReparacion2 = 17;
        private const float PrecioHerramientaReparacion2 = 17.5f;


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


        [InlineData(HerramientaId1, HerramientaNombre1, HerramientaMaterial1, HerramientaFabricante1,HerramientaTiempoReparacion1,PrecioHerramientaReparacion1, "", "")]
        [Trait("LevelTesting", "Funcional Testing")]


        [InlineData(HerramientaId2, HerramientaNombre2, HerramientaMaterial2, HerramientaFabricante2, HerramientaTiempoReparacion2, PrecioHerramientaReparacion2, "", "")]
        [Trait("LevelTesting", "Funcional Testing")]

        public void UC2_AF1_UC2_4_5_6_filtering(string HerramientaNombre, string HerramientaMaterial, string HerramientaFabricante, int HerramientaTiempoReparacion, int PrecioHerramientaReparacion, string FiltroNombre, string FiltroTiempoReparacion
     )
        {
            //Arrange
            InitialStepsForRentalMovies();
            var expectedHerramientas= new List<string[]> { new string[] {  HerramientaNombre, HerramientaMaterial, HerramientaFabricante }, };

            //Act
            seleccionarHerramientasParaReparacion_PO.BuscarHerramientas(FiltroNombre, FiltroTiempoReparacion, "", "");

            //Assert

            Assert.True(seleccionarHerramientasParaReparacion_PO.CheckListOfHerramientas(expectedHerramientas));

        }
    }
}
