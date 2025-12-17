// Path: test/AppForSEII2526.UIT/CU_Reparacion/CURepararacion_UIT.cs
using AppForMovies.UIT.Shared;
using AppForSEII2526.UIT.Shared;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.CU_Reparacion
{
    public class CURepararacion_UIT : UC_UIT
    {
        private readonly SeleccionarHerramientasParaReparacion_PO _selectPO;
        private readonly CrearReparacion_PO _crearPO;
        private readonly DetalleReparacion_PO _detallePO;

        // ... (Constantes de datos) ...
        private const int HerramientaId1 = 1;
        private const string HerramientaNombre1 = "Martillo";
        private const string HerramientaMaterial1 = "Acero";
        private const string HerramientaFabricante1 = "herramientas SA";
        private const float PrecioHerramientaReparacion1 = 57.40f;
        private const int HerramientaTiempoReparacion1 = 33;

        private const int HerramientaId2 = 3;
        private const string HerramientaNombre2 = "LLave";
        private const string HerramientaMaterial2 = "Acero";
        private const string HerramientaFabricante2 = "herramientas SA";
        private const float PrecioHerramientaReparacion2 = 17.50f;
        private const int HerramientaTiempoReparacion2 = 17;

        private const string clienteNombre = "Juan";
        private const string clienteApellidos = "Pérez García";
        private const string telefonoOpcional = "666123456";
        private const string descripcionProblema = "Fallo en el mecanismo";
        private const int cantidadItem = 1;
        private const string metodoPagoValue = "1";

        private readonly DateTime fechaEntregaValida = DateTime.Today.AddDays(7);
        private readonly DateTime fechaEntregaInvalida = DateTime.Today.AddDays(-1);


        public CURepararacion_UIT(ITestOutputHelper output) : base(output)
        {
            _selectPO = new SeleccionarHerramientasParaReparacion_PO(_driver, output);
            _crearPO = new CrearReparacion_PO(_driver, output);
            _detallePO = new DetalleReparacion_PO(_driver, output);
        }

        protected void InitialStepsForRepararHerramientas()
        {
            // Ya no incluimos Perform_login aquí, basándonos en su última indicación.
            _driver.Navigate().GoToUrl(_URI + "Reparacion/SeleccionHerramientaParaReparaciones");
        }

        // UC2_1 FLUJO BÁSICO - Listado Inicial (Paso 2)
        [Fact]
        [Trait("Category", "UIT")]
        public void UC2_1_FlujoBasico_ListadoInicial_Exito()
        {
            // 1. ARRANGE
            InitialStepsForRepararHerramientas();

            var expectedHerramientas = new List<string[]> {
                new string[] { HerramientaNombre1, HerramientaMaterial1, HerramientaFabricante1, PrecioHerramientaReparacion1.ToString("F2") },
                new string[] { HerramientaNombre2, HerramientaMaterial2, HerramientaFabricante2, PrecioHerramientaReparacion2.ToString("F2") }
            };

            // 2. ACT
            _selectPO.BuscarHerramientas("", "");

            // 3. ASSERT
            Assert.True(_selectPO.CheckListOfHerramientas(expectedHerramientas), "Fallo: El listado inicial de herramientas no es correcto.");
        }

        // UC2_2 FLUJO BÁSICO - Creación Exitosa (End-to-end)
        [Fact]
        [Trait("Category", "UIT")]
        public void UC2_2_CrearReparacion_FlujoBasico_Exito()
        {
            // 1. ARRANGE
            string nombreHerramienta = HerramientaNombre1;
            float precioTotal = PrecioHerramientaReparacion1 * cantidadItem;
            string precioTotalEsperado = precioTotal.ToString("N2");
            string fechaEntregaFormato = fechaEntregaValida.ToString("dd/MM/yyyy");

            var expectedDetailItem = new List<string[]> {
                new string[] { HerramientaId1.ToString(), nombreHerramienta, PrecioHerramientaReparacion1.ToString("N2") + "€", cantidadItem.ToString(), descripcionProblema }
            };

            // 2. ACT
            InitialStepsForRepararHerramientas();
            _selectPO.BuscarHerramientas("", "");
            _selectPO.AñadirHerramientaAReparacionCart(nombreHerramienta);
            _selectPO.ProcesarReparacion();

            // Rellenar Formulario (Paso 5)
            _crearPO.RellenarDatosGenerales(clienteNombre, clienteApellidos, telefonoOpcional, fechaEntregaValida, metodoPagoValue);

            // CORRECCIÓN: Usar RellenarDatosItemReparacion que maneja la descripción
            _crearPO.RellenarDatosItemReparacion(HerramientaId1, cantidadItem, descripcionProblema);

            // Confirmar (Paso 6)
            _crearPO.PulsarCrearReparacion();
            _crearPO.ConfirmarModal();

            // 3. ASSERT
            System.Threading.Thread.Sleep(2000);
            bool urlCorrecta = _driver.Url.Contains("/Reparacion/DetalleReparacion");
            Assert.True(urlCorrecta, $"Fallo: No se redirigió al detalle. URL actual: {_driver.Url}");

            Assert.True(_detallePO.CheckReparacionSummary($"{clienteNombre} {clienteApellidos}", fechaEntregaFormato, precioTotalEsperado),
                "Fallo: El resumen del detalle de la reparación no es correcto.");
            Assert.True(_detallePO.CheckReparacionItemTable(expectedDetailItem), "Fallo: Los detalles de los ítems de reparación no son correctos.");
        }

        // UC2_FA0 FLUJO ALTERNATIVO 0 - Filtrado
        [Theory]
        [InlineData(HerramientaNombre1, "")]
        [InlineData("", "33")]
        [InlineData(HerramientaNombre1, "33")]
        [Trait("Category", "UIT")]
        public void UC2_FA0_Filtrado_Exito(string filtroNombre, string filtroTiempo)
        {
            // 1. ARRANGE
            InitialStepsForRepararHerramientas();
            var expectedHerramientas = new List<string[]> {
                new string[] { HerramientaNombre1, HerramientaMaterial1, HerramientaFabricante1, PrecioHerramientaReparacion1.ToString("F2") },
            };

            // 2. ACT
            _selectPO.BuscarHerramientas(filtroNombre, filtroTiempo);

            // 3. ASSERT
            Assert.True(_selectPO.CheckListOfHerramientas(expectedHerramientas), $"Fallo al filtrar por Nombre:'{filtroNombre}' y Tiempo:'{filtroTiempo}'.");
        }

        // UC2_FA1 FLUJO ALTERNATIVO 1 - Fecha de Entrega Anterior a Hoy (Paso 5)
        [Fact]
        [Trait("Category", "UIT")]
        public void UC2_FA1_FechaEntregaInvalida_Error()
        {
            // 1. ARRANGE
            InitialStepsForRepararHerramientas();
            _selectPO.AñadirHerramientaAReparacionCart(HerramientaNombre1);
            _selectPO.ProcesarReparacion();

            // 2. ACT
            _crearPO.RellenarDatosGenerales(clienteNombre, clienteApellidos, telefonoOpcional, fechaEntregaInvalida, metodoPagoValue);

            // CORRECCIÓN: Usar RellenarDatosItemReparacion
            _crearPO.RellenarDatosItemReparacion(HerramientaId1, cantidadItem, descripcionProblema);

            _crearPO.PulsarCrearReparacion();

            // 3. ASSERT
            bool seguimosEnCrear = _driver.Url.Contains("/Reparacion/CreateReparacion");
            Assert.True(seguimosEnCrear, $"El sistema permitió crear reparación con fecha inválida.");

            bool hayError = _crearPO.CheckErrorMessage("debe ser posterior");
            Assert.True(hayError, "El sistema no mostró error por fecha de entrega inválida.");
        }

        // UC2_FA2 FLUJO ALTERNATIVO 2 - Modificar Carrito
        [Fact]
        [Trait("Category", "UIT")]
        public void UC2_FA2_ModificarCarrito_EliminarItem()
        {
            // 1. ARRANGE
            string nombreHerramienta2 = HerramientaNombre2;
            float precioTotal = PrecioHerramientaReparacion2 * cantidadItem;
            string precioTotalEsperado = precioTotal.ToString("N2");
            string fechaEntregaFormato = fechaEntregaValida.ToString("dd/MM/yyyy");

            var expectedDetailItem = new List<string[]> {
                new string[] { HerramientaId2.ToString(), nombreHerramienta2, PrecioHerramientaReparacion2.ToString("N2") + "€", cantidadItem.ToString(), descripcionProblema }
            };

            // 2. ACT
            InitialStepsForRepararHerramientas();
            _selectPO.AñadirHerramientaAReparacionCart(HerramientaNombre1);
            _selectPO.AñadirHerramientaAReparacionCart(HerramientaNombre2);
            _selectPO.RemoveHerramientaFromReparacionCart(HerramientaNombre1);

            _selectPO.ProcesarReparacion();

            // Rellenar formulario (Paso 5)
            _crearPO.RellenarDatosGenerales(clienteNombre, clienteApellidos, telefonoOpcional, fechaEntregaValida, metodoPagoValue);

            // CORRECCIÓN: Usar RellenarDatosItemReparacion para la herramienta que queda
            _crearPO.RellenarDatosItemReparacion(HerramientaId2, cantidadItem, descripcionProblema);

            _crearPO.PulsarCrearReparacion();
            _crearPO.ConfirmarModal();
            System.Threading.Thread.Sleep(2000);

            // 3. ASSERT
            bool urlDetalle = _driver.Url.Contains("/Reparacion/DetalleReparacion");
            Assert.True(urlDetalle, $"Fallo: El flujo no se completó tras modificar el carrito.");

            Assert.True(_detallePO.CheckReparacionSummary($"{clienteNombre} {clienteApellidos}", fechaEntregaFormato, precioTotalEsperado),
                "Fallo: El precio total o cliente en el detalle no refleja la modificación del carrito.");
            Assert.True(_detallePO.CheckReparacionItemTable(expectedDetailItem), "Fallo: Los ítems en el detalle no reflejan la modificación del carrito.");
        }

        // UC2_FA3 FLUJO ALTERNATIVO 3 - Carrito Vacío (al Paso 4)
        [Fact]
        [Trait("Category", "UIT")]
        public void UC2_FA3_CarritoVacio_NoPermiteContinuar()
        {
            // 1. ARRANGE
            InitialStepsForRepararHerramientas();

            // 2. ACT
            bool isNotAvailable = _selectPO.ReparacionNotAvailable();

            // 3. ASSERT
            Assert.True(isNotAvailable, "El botón 'Procesar Reparación' estaba activo con el carrito vacío.");
        }

        // UC2_FA4 FLUJO ALTERNATIVO 4 - Campo Obligatorio Faltante (Paso 6)
        [Fact]
        [Trait("Category", "UIT")]
        public void UC2_FA4_CrearReparacion_CampoObligatorioFaltante_Error()
        {
            // 1. ARRANGE
            InitialStepsForRepararHerramientas();
            _selectPO.AñadirHerramientaAReparacionCart(HerramientaNombre1);
            _selectPO.ProcesarReparacion();

            // 2. ACT
            // Omitiendo 'Apellidos' (campo obligatorio)
            _crearPO.RellenarDatosGenerales(clienteNombre, "", telefonoOpcional, fechaEntregaValida, metodoPagoValue);

            // CORRECCIÓN: Usar RellenarDatosItemReparacion
            _crearPO.RellenarDatosItemReparacion(HerramientaId1, cantidadItem, descripcionProblema);

            _crearPO.PulsarCrearReparacion();

            // 3. ASSERT
            bool seguimosEnCrear = _driver.Url.Contains("/Reparacion/CreateReparacion");
            Assert.True(seguimosEnCrear, $"El sistema permitió crear reparación sin campos obligatorios.");

            bool hayError = _crearPO.CheckErrorMessage("El campo Apellidos es obligatorio");
            Assert.True(hayError, "El sistema no mostró error por campo obligatorio faltante.");
        }

        // UC2_FA5 FLUJO ALTERNATIVO 5 - Cantidad Cero (Paso 6)
        [Fact]
        [Trait("Category", "UIT")]
        public void UC2_FA5_CrearReparacion_CantidadCero_BotonInactivo()
        {
            // 1. ARRANGE
            InitialStepsForRepararHerramientas();
            _selectPO.AñadirHerramientaAReparacionCart(HerramientaNombre1);
            _selectPO.ProcesarReparacion();

            // 2. ACT
            // Rellenar datos, pero con Cantidad = 0
            _crearPO.RellenarDatosGenerales(clienteNombre, clienteApellidos, telefonoOpcional, fechaEntregaValida, metodoPagoValue);

            // CORRECCIÓN: Usar RellenarDatosItemReparacion
            _crearPO.RellenarDatosItemReparacion(HerramientaId1, 0, descripcionProblema);

            // 3. ASSERT
            // El botón debe estar inactivo/deshabilitado
            Assert.True(_crearPO.IsCrearReparacionButtonDisabled(), "El botón 'Guardar' no se inhabilitó al establecer cantidad 0.");
        }




        //UC2_Modificación 


        [Fact]
        [Trait("Category", "UIT")]



        public void UC2_Mod_CrearReparacion()
        {

            string FechaEntrega = "25/12/2025";
            string Precio = "10";



            // Datos esperados:
            var expectedDetails = new List<string[]>
            {
                new string[] { clienteNombre, Precio,FechaEntrega }
            };
            var expectedItems = new List<string[]>
            {
                new string[] { HerramientaNombre1, descripcionProblema }
            };

            InitialStepsForRepararHerramientas();
            Thread.Sleep(1000);

            _selectPO.BuscarHerramientas("Martillo", "");
            Thread.Sleep(1000);

            _selectPO.AñadirHerramientaAReparacionCart(HerramientaNombre1);
            Thread.Sleep(1000);

            _selectPO.BuscarHerramientas("", "44");
            Thread.Sleep(1000);

            _selectPO.AñadirHerramientaAReparacionCart(HerramientaNombre2);
            Thread.Sleep(1000);

            _selectPO.ProcesarReparacion();
            Thread.Sleep(1000);



            _crearPO.RellenarDatosGenerales(clienteNombre, clienteApellidos, telefonoOpcional, fechaEntregaValida, metodoPagoValue);
            Thread.Sleep(1000);

            _crearPO.RellenarDatosItemReparacion(HerramientaId1, cantidadItem, descripcionProblema);
            Thread.Sleep(1000);

            _crearPO.RellenarDatosItemReparacion(HerramientaId1, cantidadItem, descripcionProblema);
            Thread.Sleep(1000);

            _crearPO.PulsarModificarCarrito();
            Thread.Sleep(1000);
            _selectPO.RemoveHerramientaFromReparacionCart(HerramientaNombre1);



            _selectPO.ProcesarReparacion();
            Thread.Sleep(1000);



            _crearPO.RellenarDatosGenerales(clienteNombre, clienteApellidos, telefonoOpcional, fechaEntregaValida, metodoPagoValue);
            Thread.Sleep(1000);

            _crearPO.RellenarDatosItemReparacion(HerramientaId2, cantidadItem, descripcionProblema);
            Thread.Sleep(1000);


            _crearPO.ConfirmarModal();

            Thread.Sleep(1000);
            _crearPO.PulsarCrearReparacion();







            // 3. ASSERT
            // 3.1 Verificar detalles del comprador
            Assert.True(_detallePO.CheckReparacionItemTable(expectedDetails),
                "Fallo: Los detalles del comprador en la tabla de detalles no coinciden.");
            // 3.2 Verificar ítems comprados
            // Assert.True(_detallePO.CheckReparacionSummary(expectedDetails),
            //"Fallo: La lista de ítems reparados o sus detalles no coinciden con lo esperado.");







        }



































    }
}