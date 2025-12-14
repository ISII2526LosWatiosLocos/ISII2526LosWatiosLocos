using AppForMovies.UIT.Shared;
using AppForSEII2526.UIT.CU_Reparacion;
using AppForSEII2526.UIT.Shared;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;
using OpenQA.Selenium;
using System.Linq;

namespace AppForSEII2526.UIT.CU_Reparacion
{
    public class CURepararacion_UIT : UC_UIT
    {
        private SeleccionarHerramientasParaReparacion_PO seleccionarHerramientasParaReparacion_PO;
        private CrearReparacion_PO crearPO;
        private DetalleReparacion_PO detallePO;

        // --- CONSTANTES DE HERRAMIENTAS ---
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

        // --- CONSTANTES DE DATOS DEL CLIENTE ---
        private const string clienteNombre = "Juan";
        private const string clienteApellidos = "Pérez García";
        private const string telefonoOpcional = "666123456";
        private const string descripcionProblema = "Fallo en el mecanismo";
        private const int cantidad = 1;
        private const String metodoPago = "Tarjeta"; // Asumido como ID/código de pago

        public CURepararacion_UIT(ITestOutputHelper output) : base(output)
        {
            seleccionarHerramientasParaReparacion_PO = new SeleccionarHerramientasParaReparacion_PO(_driver, output);
            crearPO = new CrearReparacion_PO(_driver, _output);
            detallePO = new DetalleReparacion_PO(_driver, _output);
        }

        private void InitialStepsForRepararHerramientas()
        {
            _driver.Navigate().GoToUrl(_URI + "Reparacion/SeleccionHerramientaParaReparaciones");
        }


        // ====================================================================
        // UC2_1 FLUJO BÁSICO - Creación exitosa
        // ====================================================================
        [Theory]
        [InlineData(HerramientaId1, HerramientaNombre1, HerramientaMaterial1, HerramientaFabricante1, HerramientaTiempoReparacion1, PrecioHerramientaReparacion1, "", "")]
        [InlineData(HerramientaId2, HerramientaNombre2, HerramientaMaterial2, HerramientaFabricante2, HerramientaTiempoReparacion2, PrecioHerramientaReparacion2, "", "")]
        [Trait("LevelTesting", "Funcional Testing")]

        public void UC2_1_FlujoBasico(
            int HerramientaId, // CORREGIDO: Se pasa el ID
            string HerramientaNombre,
            string HerramientaMaterial,
            string HerramientaFabricante,
            int HerramientaTiempoReparacion,
            float PrecioHerramientaReparacion,
            string FiltroNombre,
            string FiltroTiempoReparacion
        )
        {
            // PASO 1 y 2
            InitialStepsForRepararHerramientas();
            var expectedHerramientas = new List<string[]> { new string[] { HerramientaNombre, HerramientaMaterial, HerramientaFabricante, PrecioHerramientaReparacion.ToString("F2"), HerramientaTiempoReparacion.ToString() + " días" }, };

            seleccionarHerramientasParaReparacion_PO.BuscarHerramientas(FiltroNombre, FiltroTiempoReparacion, "", "");
            Assert.True(seleccionarHerramientasParaReparacion_PO.CheckListOfHerramientas(expectedHerramientas));

            // PASO 3: Añadir al carrito
            seleccionarHerramientasParaReparacion_PO.AddHerramientaToReparacionCart(HerramientaNombre);

            // PASO 4: Ir al formulario
            seleccionarHerramientasParaReparacion_PO.PressRepararHerramientas();

            // PASO 5: Rellenar formulario
            var fechaEntrega = DateTime.Today.AddDays(5);

            crearPO.RellenarDatosCliente(clienteNombre, clienteApellidos, fechaEntrega, metodoPago, telefonoOpcional);
            crearPO.RellenarDatosHerramienta(HerramientaId, descripcionProblema, cantidad); // CORREGIDO

            // PASO 6: Guardar
            crearPO.PulsarGuardarReparacion();

            // PASO 7: Verificar los detalles de la reparación creada
            var precioTotalEsperado = PrecioHerramientaReparacion * cantidad;
            var nombreCompleto = clienteNombre + " " + clienteApellidos;
            var fechaEntregaStr = fechaEntrega.ToString("dd/MM/yyyy");

            // Verificación usando los métodos de su DetalleReparacion_PO
            Assert.True(detallePO.VerificarNombreCompleto(nombreCompleto), "Error en la verificación del nombre completo del cliente.");
            Assert.True(detallePO.VerificarFechaEntrega(fechaEntregaStr), "Error en la verificación de la fecha de entrega.");
            Assert.True(detallePO.VerificarPrecioTotal(precioTotalEsperado.ToString("F2")), "Error en la verificación del precio total.");

            var expectedReparacionItems = new List<string[]>
            {
                // Nombre, Precio Formateado, Descripción, Cantidad (Ajustado al Flujo Básico)
                new string[] { HerramientaNombre, precioTotalEsperado.ToString("F2"), descripcionProblema, cantidad.ToString() }
            };
            Assert.True(detallePO.CheckHerramientasReparadas(expectedReparacionItems), "Error en la verificación de las herramientas listadas en la reparación.");
        }


        // ====================================================================
        // FLUJOS ALTERNATIVOS
        // ====================================================================

        // --- AF0 / Búsqueda por rango de tiempo (Test original) ---
        [Fact(Skip = "first run dbo.Movies.data.UpdateQuantityAvailable.sql, after running the test case run dbo.Movies.data.UpdateQuantityAvailableto100")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_AF1_UC2_6_filtering()
        {
            InitialStepsForRepararHerramientas();
            var expectedHerramientas = new List<string[]> { new string[] { HerramientaNombre2, HerramientaMaterial2, HerramientaFabricante2 }, };

            string desde = DateTime.Today.AddDays(2).ToString("dd/MM/yyyy");
            string hasta = DateTime.Today.AddDays(3).ToString("dd/MM/yyyy");

            seleccionarHerramientasParaReparacion_PO.BuscarHerramientas("", "", desde, hasta);
            Assert.True(seleccionarHerramientasParaReparacion_PO.CheckListOfHerramientas(expectedHerramientas));
        }

        // --- AF0 / Validación de Fechas en el Filtro ---
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
            InitialStepsForRepararHerramientas();
            seleccionarHerramientasParaReparacion_PO.BuscarHerramientas("", "", fechaDesde, fechaHasta);
            Assert.True(seleccionarHerramientasParaReparacion_PO.CheckMessageError(errorEsperado),
                $"Error en la caja de mensaje para el test {fechaDesde} - {fechaHasta}. Mensaje esperado: {errorEsperado}");
        }

        // --- AF3: Botón Continuar Inactivo si Carrito Vacío (al Paso 4) ---
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_AF3_BotonContinuarInactivoSiCarritoVacio()
        {
            InitialStepsForRepararHerramientas();
            // Utilizamos el método que verifica si el botón está habilitado
            Assert.False(seleccionarHerramientasParaReparacion_PO.IsRepararButtonEnabled(),
                "El botón 'Reparar herramientas' (Continuar) debería estar inactivo con el carrito vacío.");
        }

        // --- AF2: Modificar Carrito (al Paso 5) ---
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_AF2_ModificarCarritoBorrarHerramienta()
        {
            // 1. Arrange: Añadir herramientas e ir al formulario
            InitialStepsForRepararHerramientas();
            seleccionarHerramientasParaReparacion_PO.AddHerramientaToReparacionCart(HerramientaNombre1);
            seleccionarHerramientasParaReparacion_PO.AddHerramientaToReparacionCart(HerramientaNombre2);
            seleccionarHerramientasParaReparacion_PO.PressRepararHerramientas();

            // 2. Act: Pulsar Modificar y borrar una herramienta (vuelve a la página de selección)
            crearPO.PulsarModificarCarrito();
            seleccionarHerramientasParaReparacion_PO.RemoveHerramientaFromReparacionCart(HerramientaNombre2);

            // 3. Volver al formulario
            seleccionarHerramientasParaReparacion_PO.PressRepararHerramientas();

            // 4. Assert: Verificar que solo queda la Herramienta 1
            Assert.True(crearPO.CheckHerramientaPresente(HerramientaId1), "La Herramienta 1 debería estar presente.");
            Assert.False(crearPO.CheckHerramientaPresente(HerramientaId2), "La Herramienta 2 debería haber sido eliminada.");
        }

        // --- AF4: Validación de Campos Obligatorios (al Paso 6) ---
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_AF4_ValidacionCamposObligatorios()
        {
            InitialStepsForRepararHerramientas();
            seleccionarHerramientasParaReparacion_PO.AddHerramientaToReparacionCart(HerramientaNombre1);
            seleccionarHerramientasParaReparacion_PO.PressRepararHerramientas();

            var fechaEntrega = DateTime.Today.AddDays(5);

            // Rellenamos sin Nombre (campo obligatorio)
            crearPO.RellenarDatosCliente("", clienteApellidos, fechaEntrega, metodoPago, telefonoOpcional);
            crearPO.RellenarDatosHerramienta(HerramientaId1, descripcionProblema, cantidad);

            crearPO.PulsarGuardarReparacion();

            Assert.True(crearPO.CheckErrorMessage("El campo Nombre es obligatorio."),
                "Debería aparecer un error por campo de cliente obligatorio faltante.");
        }

        // --- AF5: Cantidad Cero Desactiva Guardar (al Paso 6) ---
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_AF5_CantidadCeroDesactivaGuardar()
        {
            InitialStepsForRepararHerramientas();
            seleccionarHerramientasParaReparacion_PO.AddHerramientaToReparacionCart(HerramientaNombre1);
            seleccionarHerramientasParaReparacion_PO.PressRepararHerramientas();

            var fechaEntrega = DateTime.Today.AddDays(5);
            crearPO.RellenarDatosCliente(clienteNombre, clienteApellidos, fechaEntrega, metodoPago, telefonoOpcional);

            const int cantidadCero = 0;
            crearPO.RellenarDatosHerramienta(HerramientaId1, descripcionProblema, cantidadCero); // Cantidad 0

            // Verifica que el botón de Guardar está deshabilitado
            Assert.False(crearPO.IsGuardarButtonEnabled(),
                "El botón 'Guardar' debería estar inactivo si la cantidad de reparación es 0.");
        }

        // --- Test original de Carrito No Disponible ---
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_AF1_UC2_11_RentingNotavailable()
        {
            InitialStepsForRepararHerramientas();
            seleccionarHerramientasParaReparacion_PO.AddHerramientaToReparacionCart(HerramientaNombre1);
            seleccionarHerramientasParaReparacion_PO.RemoveHerramientaFromReparacionCart(HerramientaNombre1);

            Assert.True(seleccionarHerramientasParaReparacion_PO.ReparacionNotAvailable());
        }
    }
}