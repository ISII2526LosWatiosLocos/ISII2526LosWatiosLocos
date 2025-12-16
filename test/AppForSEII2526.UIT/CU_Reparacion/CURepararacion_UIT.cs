using AppForMovies.UIT.Shared;
using AppForSEII2526.UIT.Shared;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.CU_Reparacion
{
    public class UC_Reparacion_UIT : UC_UIT
    {
        private GetSelectReparacion_PO getSelectReparacion_PO;
        private GetDetailsReparacion_PO getDetailsReparacion_PO;
        private CreateReparacion_PO createReparacion_PO;

        private const string llave = "Llave inglesa";
        private const int llaveid = 1;
        private const string materialLlave = "Acero";
        private const string FabricanteLlave = "BOSCH";
        private const string tiempoReparacionLlave = "30";
        private const float precioLlave = 150;

        private const string destornillador = "Destornillador de estrella";
        private const int destornilladorid = 3;
        private const string materialDestornillador = "Acero y Plástico";
        private const string FabricanteDestornillador = "BOSCH";
        private const string tiempoReparacionDestornillador = "5";
        private const float precioDestornillador = 45;

        public UC_Reparacion_UIT(ITestOutputHelper output) : base(output)
        {
            getSelectReparacion_PO = new GetSelectReparacion_PO(_driver, _output);
            getDetailsReparacion_PO = new GetDetailsReparacion_PO(_driver, _output);
            createReparacion_PO = new CreateReparacion_PO(_driver, _output);
        }

        public void Primer_Paso_Seleccionar_Herramienta()
        {
            Initial_step_opening_the_web_page();
            getSelectReparacion_PO.WaitForBeingVisible(By.Id("Reparación"));
            _driver.FindElement(By.Id("Reparación")).Click();
            Thread.Sleep(1000);
        }




        // PASOS 1-7, FLUJO BÁSICO COMPLETO
        [Theory]
        [InlineData(nombreCliente1, apellidosCliente1, "+34 600111222", tiempoReparacionLlave, "Efectivo")]
        [InlineData(nombreCliente2, apellidosCliente2, "+34 700333444", tiempoReparacionLlave, "PayPal")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void FlujoBásico(string nombreCliente, string apellidosCliente, string telefono, string dias, string metodoPago)
        {
            // Arrange
            Primer_Paso_Seleccionar_Herramienta();
            Thread.Sleep(1000);

            // Act
            getSelectReparacion_PO.BuscarHerramientas(llave, tiempoReparacionLlave);
            Thread.Sleep(1000);
            getSelectReparacion_PO.AñadirHerramientaAlCarro(llave);
            getSelectReparacion_PO.ContinuarReparacion();
            Thread.Sleep(1000);
            DateTime fechaEntrega = DateTime.Now;
            DateTime fechaRecogidaCalculada = fechaEntrega.AddDays(int.Parse(dias));

            // Rellenamos el formulario con la fecha de inicio
            createReparacion_PO.RellenarFormularioReparacion(nombreCliente, apellidosCliente, telefono, fechaEntrega, metodoPago);
            Thread.Sleep(1000);

            createReparacion_PO.ClickRegistrarButton();
            Thread.Sleep(1000);
            createReparacion_PO.ConfirmDialog();
            Thread.Sleep(1000);

            // Assert
            Assert.True(getDetailsReparacion_PO.CheckDetallesReparacion(
                nombreCliente + " " + apellidosCliente,
                fechaEntrega.Date,
                fechaRecogidaCalculada.Date,

                metodoPago,
                "150,00 €"
            ));
        }


        // PASOS 2 y 3, FLUJO ALTERNATIVO 0 - Filtros
        [Theory]
        [InlineData(llave, tiempoReparacionLlave, llave, FabricanteLlave, materialLlave, precioLlave, tiempoReparacionLlave)] // Filtro por Nombre y Tiempo
        [InlineData(llave, "", llave, FabricanteLlave, materialLlave, precioLlave, tiempoReparacionLlave)] // Filtro por Nombre
        [InlineData("", tiempoReparacionDestornillador, destornillador, FabricanteDestornillador, materialDestornillador, precioDestornillador, tiempoReparacionDestornillador)] // Filtro por Tiempo

        public void FB_P2_P3_FA0_FiltroNombreYTiempo(
            string nombreHerramientaFiltro,
            string tiempoReparacionFiltro,
            string nombreEsperado,
            string fabricanteEsperado,
            string materialEsperado,
            float precioEsperado,
            string tiempoReparacionEsperado)
        {
            //Arrange
            Primer_Paso_Seleccionar_Herramienta();

            var herramientasEsperadas = new List<string[]>
            {
              new string[] {
              nombreEsperado,
              fabricanteEsperado,
              materialEsperado,
              precioEsperado.ToString() + " €",
              tiempoReparacionEsperado,
              "Añadir"
      }
            };

            //Act
            getSelectReparacion_PO.BuscarHerramientas(nombreHerramientaFiltro, tiempoReparacionFiltro);
            Thread.Sleep(1000); // Esperar a que la tabla se refresque

            //Assert
            Assert.True(getSelectReparacion_PO.VerificarTablaDeHerramientas(herramientasEsperadas));

        }

        //PASO 6 - FLUJO ALTERNATIVO 1 - Fecha Entrega mala
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void FB_P6_FA1_FechaEntregaMala()
        {
            Primer_Paso_Seleccionar_Herramienta();
            Thread.Sleep(1000);
            getSelectReparacion_PO.AñadirHerramientaAlCarro(llave);
            getSelectReparacion_PO.ContinuarReparacion();

            DateTime fechaMala = DateTime.Now.AddDays(-5);

            //ACT
            createReparacion_PO.RellenarFormularioReparacion(nombreCliente1, apellidosCliente1, "", fechaMala, "PayPal");
            Thread.Sleep(1000);
            createReparacion_PO.ClickRegistrarButton();
            Thread.Sleep(1000);
            createReparacion_PO.ConfirmDialog();
            Thread.Sleep(1000);

            //ASSERT
            Assert.True(createReparacion_PO.CheckValidationError("Errors: (*) La fecha de entrega no puede ser anterior a hoy"));

        }



        // PASO 3, FLUJO ALTERNATIVO 2 - Modificar carrito desde Select
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void FB_P3_FA2_ModificarCarritoDesdeSelect()
        {
            // Arrange
            Primer_Paso_Seleccionar_Herramienta();
            getSelectReparacion_PO.BuscarHerramientas("", "");
            Thread.Sleep(1000);

            // Act - 1. Añadimos dos herramientas
            getSelectReparacion_PO.AñadirHerramientaAlCarro(llave); // Precio: 150
            getSelectReparacion_PO.AñadirHerramientaAlCarro(destornillador); // Precio: 45
            //Eliminamos la llave
            Thread.Sleep(1000);
            getSelectReparacion_PO.QuitarDelCarrito(llave);
            Thread.Sleep(1000);

            // Assert
            string precioEsperado = "45,00 €";
            string precioActual = getSelectReparacion_PO.ObtenerTotalEstimado();

            Assert.Equal(precioEsperado, precioActual);

        }

        // PASO 5 - FLUJO ALTERNATIVO 2 - MODIFICAR CARRITO DESDE POST
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void FB_P5_FA2_ModificarCarritoDesdePost()
        {
            // Arrange
            Primer_Paso_Seleccionar_Herramienta();
            Thread.Sleep(500);
            getSelectReparacion_PO.AñadirHerramientaAlCarro(llave);
            getSelectReparacion_PO.AñadirHerramientaAlCarro(destornillador);
            getSelectReparacion_PO.ContinuarReparacion();
            Thread.Sleep(1000);
            createReparacion_PO.ClickModificarHerramientas();
            Thread.Sleep(1000);
            getSelectReparacion_PO.QuitarDelCarrito(llave);
            Thread.Sleep(1000);

            // Assert
            string precioEsperado = "45,00 €";
            string precioActual = getSelectReparacion_PO.ObtenerTotalEstimado();

            Assert.Equal(precioEsperado, precioActual);



        }


        // PASO 4, FLUJO ALTERNATIVO 3 - Carrito vacío
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void FB_P4_FA3_CarritoVacio()
        {
            //Arrange
            Primer_Paso_Seleccionar_Herramienta();
            getSelectReparacion_PO.BuscarHerramientas("", "");
            Thread.Sleep(500);

            //Act
            // Sin herramientas en el carrito

            //Assert
            Assert.True(getSelectReparacion_PO.RepararHerramientaNoDisponible());
        }

        private const string nombreCliente1 = "Billy";
        private const string nombreCliente2 = "Amador";
        private const string nombreCliente3 = "Shawn";
        private DateTime fechaBuena = DateTime.Now.AddDays(new Random().Next(1, 11));
        private const string apellidosCliente1 = "Chalabi";
        private const string apellidosCliente2 = "Rivas";
        private const string apellidosCliente3 = "Frost";

        private const string descripcionLlave = "Mu inglesa no parece la llave eh";
        private const string descripcionDestornillador = "Era de punta plana pero bueno";






        // PASO 6 - FLUJO ALTERNATIVO 4 - Datos no rellenados
        [Theory]
        [InlineData("", apellidosCliente1, "Errors: (*) The NombreCliente field is required.")]
        [InlineData(nombreCliente1, "", "Errors: (*) The ApellidoCliente field is required.")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void FB_P6_FA4_DatosNoRellenados(
            string nombreCliente,
            string apellidosCliente,
            string errorEsperado)
        {
            Primer_Paso_Seleccionar_Herramienta();
            Thread.Sleep(1000);
            getSelectReparacion_PO.AñadirHerramientaAlCarro(destornillador);
            Thread.Sleep(1000);
            getSelectReparacion_PO.ContinuarReparacion();
            DateTime fechaValida = DateTime.Now.AddDays(5);
            Thread.Sleep(1000);
            //ACT
            createReparacion_PO.RellenarFormularioReparacion(nombreCliente, apellidosCliente, "", fechaValida, "Efectivo");
            Thread.Sleep(1000);
            createReparacion_PO.RellenarDescripcionReparacion(descripcionDestornillador, destornilladorid);
            createReparacion_PO.ClickRegistrarButton();
            Thread.Sleep(1000);
            createReparacion_PO.ConfirmDialog();
            Thread.Sleep(1000);
            //ASSERT
            Assert.True(createReparacion_PO.CheckValidationError(errorEsperado));
        }



        //PASO 6, FLUJO ALTERNATIVO 5 - Carrito vacío, 0 herramientas seleccionadas 
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void FB_P6_FA5_CarritoVacio()
        {
            //Arrange
            Primer_Paso_Seleccionar_Herramienta();
            Thread.Sleep(1000);
            getSelectReparacion_PO.AñadirHerramientaAlCarro(destornillador);
            getSelectReparacion_PO.ContinuarReparacion();
            Thread.Sleep(1000);
            createReparacion_PO.RellenarDescripcionReparacion(descripcionDestornillador, destornilladorid);
            Thread.Sleep(1000);
            createReparacion_PO.ClickModificarHerramientas();
            Thread.Sleep(1000);
            getSelectReparacion_PO.QuitarDelCarrito(destornillador);
            Thread.Sleep(1000);

            //Act
            // Sin herramientas en el carrito
            //Assert
            Assert.True(getSelectReparacion_PO.RepararHerramientaNoDisponible());
        }








    }
}