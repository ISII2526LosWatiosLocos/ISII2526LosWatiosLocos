
using AppForMovies.UIT.Shared;
using AppForSEII2526.UIT.CU_Reparacion;
using AppForSEII2526.UIT.Shared;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppForSEII2526.UIT.CU_Reparacion
{
    public class CURepararacion_UIT : UC_UIT

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



        // Datos del cliente (PASO 5)
        private const string clienteNombre = "Juan";
        private const string clienteApellidos = "Pérez García";
        private const string telefonoOpcional = "666123456";
        private const string descripcionProblema = "Fallo en el mecanismo";
        private const int cantidad = 1;




        // Page Objects

        private readonly SeleccionarHerramientasParaReparacion_PO _selectPO;
        

        public CURepararacion_UIT(ITestOutputHelper output) : base(output)
        {

            seleccionarHerramientasParaReparacion_PO = new SeleccionarHerramientasParaReparacion_PO(_driver, output);
        // METER AQUI LUEGO 
        }

       
    


        private void InitialStepsForRepararHerramientas()
        {
            _driver.Navigate().GoToUrl(_URI + "Reparacion/SeleccionHerramientaParaReparaciones");
        }


        //UC2_1 FLUJO BÁSICO - Creacioón exitosa
        [Theory]
        [InlineData( HerramientaNombre1, HerramientaMaterial1, HerramientaFabricante1, HerramientaTiempoReparacion1, PrecioHerramientaReparacion1, "", "")]



        [InlineData( HerramientaNombre2, HerramientaMaterial2, HerramientaFabricante2, HerramientaTiempoReparacion2, PrecioHerramientaReparacion2, "", "")]
        [Trait("LevelTesting", "Funcional Testing")]

        public void UC2_1_FlujoBasico(string HerramientaNombre, string HerramientaMaterial, string HerramientaFabricante, int HerramientaTiempoReparacion, float PrecioHerramientaReparacion, string FiltroNombre, string FiltroTiempoReparacion
     )
        {
            //paso 1 cliente selecciona Reparar Herrmainetas
            //Arrange
            InitialStepsForRepararHerramientas();
            var expectedHerramientas = new List<string[]> { new string[] { HerramientaNombre, HerramientaMaterial, HerramientaFabricante, PrecioHerramientaReparacion.ToString("F2"), HerramientaTiempoReparacion.ToString() }, };

         
            seleccionarHerramientasParaReparacion_PO.BuscarHerramientas(FiltroNombre, FiltroTiempoReparacion, "", "");

            //Assert

            Assert.True(seleccionarHerramientasParaReparacion_PO.CheckListOfHerramientas(expectedHerramientas));

        }


        [Fact(Skip = "first run dbo.Movies.data.UpdateQuantityAvailable.sql, after running the test case run dbo.Movies.data.UpdateQuantityAvailableto100")]
        [Trait("LevelTesting", "Funcional Testing")]



        public void UC2_AF1_UC2_6_filtering()
        {
            //Arrange


            InitialStepsForRepararHerramientas();
            var expectedHerramientas = new List<string[]> { new string[] { HerramientaNombre2, HerramientaMaterial2, HerramientaFabricante2 }, };

            string desde = DateTime.Today.AddDays(2).ToString("dd/MM/yyyy");
            string hasta = DateTime.Today.AddDays(3).ToString("dd/MM/yyyy");

            //Act
            seleccionarHerramientasParaReparacion_PO.BuscarHerramientas("", "", desde, hasta);

            //Assert

            Assert.True(seleccionarHerramientasParaReparacion_PO.CheckListOfHerramientas(expectedHerramientas));




        }


        public static IEnumerable<object[]> TestCasesFor_UC2_4_5_AF2_errorEnFechas()
        {
            var allTests = new List<object[]>
    {
        new object[] {
            DateTime.Today.AddDays(-1).ToString("dd/MM/yyyy"),
            DateTime.Today.AddDays(2).ToString("dd/MM/yyyy"),
            "Tu período de reparación debe ser posterior"
        },
        new object[] {
            DateTime.Today.AddDays(-2).ToString("dd/MM/yyyy"),
            DateTime.Today.AddDays(-1).ToString("dd/MM/yyyy"),
            "Tu período de reparación debe ser posterior"
        },
        new object[] {
            DateTime.Today.AddDays(7).ToString("dd/MM/yyyy"),
            DateTime.Today.AddDays(5).ToString("dd/MM/yyyy"),
            "La reparación debe terminar después de comenzar"
        },
    };

            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_UC2_4_5_AF2_errorEnFechas))]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_7_8_9_AF2_errorEnFechas(string fechaDesde, string fechaHasta, string errorEsperado)
        {
            // Arrange

            // Act

            InitialStepsForRepararHerramientas();

            // Assert



            Assert.True(seleccionarHerramientasParaReparacion_PO.CheckMessageError(errorEsperado), $"Error in the message box for test {fechaDesde} - {fechaHasta}");




        }



        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]

        public void UC2_AF1_UC2_11_RentingNotavailable()
        {
            //Arrange
            InitialStepsForRepararHerramientas();
            //Act
            seleccionarHerramientasParaReparacion_PO.AddHerramientaToReparacionCart(HerramientaNombre1);
            seleccionarHerramientasParaReparacion_PO.RemoveHerramientaFromReparacionCart(HerramientaNombre1);

            //Assert

            Assert.True(seleccionarHerramientasParaReparacion_PO.ReparacionNotAvailable());
        }
    }
}
