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
        // Page Objects refactorizados
        private readonly SeleccionarHerramientasParaReparacion_PO _selectPO;
        private readonly CrearReparacion_PO _crearPO;
        private readonly DetalleReparacion_PO _detallePO;

        // Constantes (Herramienta 1 y 2)
        private const int HerramientaId1 = 1;
        private const string HerramientaNombre1 = "Martillo";
        private const string HerramientaMaterial1 = "Acero";
        private const string HerramientaFabricante1 = "herramientas SA";
        private const int HerramientaTiempoReparacion1 = 33;
        private const float PrecioHerramientaReparacion1 = 57.40f;

        private const int HerramientaId2 = 3;
        private const string HerramientaNombre2 = "LLave";
        private const string HerramientaMaterial2 = "Acero";
        private const string HerramientaFabricante2 = "herramientas SA";
        private const int HerramientaTiempoReparacion2 = 17;
        private const float PrecioHerramientaReparacion2 = 17.50f;

        // Datos del cliente (Flujo Básico 5)
        private const string clienteNombre = "Juan";
        private const string clienteApellidos = "Pérez García";
        private const string telefonoOpcional = "666123456"; // Opcional
        private const string descripcionProblema = "Fallo en el mecanismo"; // Opcional
        private const int cantidadItem = 1; // Obligatoria
        private const string metodoPagoValue = "1"; // Asumido: 1 = Tarjeta Crédito (Obligatorio)

        // Fechas de prueba
        private readonly DateTime fechaEntregaValida = DateTime.Today.AddDays(7);
        private readonly DateTime fechaEntregaInvalida = DateTime.Today.AddDays(-1);


        public CURepararacion_UIT(ITestOutputHelper output) : base(output)
        {
            _selectPO = new SeleccionarHerramientasParaReparacion_PO(_driver, output);
            _crearPO = new CrearReparacion_PO(_driver, output);
            _detallePO = new DetalleReparacion_PO(_driver, output);
        }

        private void InitialStepsForRepararHerramientas()
        {
            // Paso 1: El cliente selecciona Reparar Herramientas (navegación)
            _driver.Navigate().GoToUrl(_URI + "Reparacion/SeleccionHerramientaParaReparaciones");
        }

        // UC2_1 FLUJO BÁSICO - Listado Inicial (Paso 2)
        [Fact]
        [Trait("Category", "UIT")]
        public void UC2_1_FlujoBasico_ListadoInicial_Exito()
        {
            // 1. ARRANGE
            InitialStepsForRepararHerramientas();
            // Expected row format: [Nombre, Material, Fabricante, Precio, TiempoReparacion]
            var expectedHerramientas = new List<string[]> {
                new string[] { HerramientaNombre1, HerramientaMaterial1, HerramientaFabricante1, PrecioHerramientaReparacion1.ToString("F2"), HerramientaTiempoReparacion1.ToString() },
                new string[] { HerramientaNombre2, HerramientaMaterial2, HerramientaFabricante2, PrecioHerramientaReparacion2.ToString("F2"), HerramientaTiempoReparacion2.ToString() }
            };

            // 2. ACT
            _selectPO.BuscarHerramientas("", ""); // Buscar sin filtros (Paso 2)

            // 3. ASSERT
            Assert.True(_selectPO.CheckListOfHerramientas(expectedHerramientas), "Fallo: El listado inicial de herramientas no es correcto.");
        }

        // UC2_2 FLUJO BÁSICO - Creación Exitosa (Pasos 3 al 7)
        [Fact]
        [Trait("Category", "UIT")]
        public void UC2_2_CrearReparacion_FlujoBasico_Exito()
        {
            // 1. ARRANGE
            string nombreHerramienta = HerramientaNombre1;
            string precioTotalEsperado = PrecioHerramientaReparacion1.ToString("F2"); // Con una sola herramienta
            string fechaEntregaFormato = fechaEntregaValida.ToString("dd/MM/yyyy");

            // Expected row format for detail table (Flujo Básico 7): [nombre, precio, descripción, cantidad]
            List<string[]> expectedDetailItem = new List<string[]> {
                new string[] { nombreHerramienta, precioTotalEsperado, descripcionProblema, cantidadItem.ToString() }
            };

            // 2. ACT
            InitialStepsForRepararHerramientas(); // Paso 1 y 2
            _selectPO.BuscarHerramientas("", "");
            _selectPO.AñadirHerramientaAReparacionCart(nombreHerramienta); // Paso 3
            _selectPO.ProcesarReparacion(); // Paso 4

            // Rellenar Formulario (Paso 5)
            _crearPO.RellenarDatosGenerales(clienteNombre, clienteApellidos, telefonoOpcional, fechaEntregaValida, metodoPagoValue);
            _crearPO.RellenarDatosItemReparacion(HerramientaId1, cantidadItem, descripcionProblema);

            // Confirmar (Paso 6)
            _crearPO.PulsarCrearReparacion();
            _crearPO.ConfirmarModal();

            // 3. ASSERT
            // Esperamos redirección a Detalle (Paso 7)
            System.Threading.Thread.Sleep(2000);
            bool urlCorrecta = _driver.Url.Contains("/Reparacion/DetalleReparacion");
            Assert.True(urlCorrecta, $"Fallo: No se redirigió al detalle. URL actual: {_driver.Url}");

            // Comprobamos el resumen y los ítems (Paso 7)
            Assert.True(_detallePO.CheckReparacionSummary($"{clienteNombre} {clienteApellidos}", fechaEntregaFormato, precioTotalEsperado),
                "Fallo: El resumen del detalle de la reparación no es correcto.");
            Assert.True(_detallePO.CheckReparacionItemTable(expectedDetailItem), "Fallo: Los detalles de los ítems de reparación no son correctos.");
        }

        // UC2_FA0 FLUJO ALTERNATIVO 0 - Filtrado por Nombre y Tiempo de Reparación
        [Theory]
        [InlineData(HerramientaNombre1, "")] // Filtrar solo por nombre (Paso 1.1)
        [InlineData("", "33")] // Filtrar solo por tiempo (Paso 1.1)
        [InlineData(HerramientaNombre1, "33")] // Filtrar por ambos (Paso 1.1)
        [Trait("Category", "UIT")]
        public void UC2_FA0_Filtrado_Exito(string filtroNombre, string filtroTiempo)
        {
            // 1. ARRANGE
            InitialStepsForRepararHerramientas();
            // Expected row format: [Nombre, Material, Fabricante, Precio, TiempoReparacion]
            var expectedHerramientas = new List<string[]> {
                new string[] { HerramientaNombre1, HerramientaMaterial1, HerramientaFabricante1, PrecioHerramientaReparacion1.ToString("F2"), HerramientaTiempoReparacion1.ToString() },
            };

            // 2. ACT
            _selectPO.BuscarHerramientas(filtroNombre, filtroTiempo); // Paso 1.2 y 1.3

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
            // Rellenar datos, pero con fecha inválida (Flujo Alternativo 1)
            _crearPO.RellenarDatosGenerales(clienteNombre, clienteApellidos, telefonoOpcional, fechaEntregaInvalida, metodoPagoValue);
            _crearPO.RellenarDatosItemReparacion(HerramientaId1, cantidadItem, descripcionProblema);
            _crearPO.PulsarCrearReparacion();

            // 3. ASSERT
            bool seguimosEnCrear = _driver.Url.Contains("/Reparacion/CreateReparacion");
            Assert.True(seguimosEnCrear, $"El sistema permitió crear reparación con fecha inválida.");

            // Comprobar mensaje de error y que vuelve al paso 5 (Flujo Alternativo 1)
            bool hayError = _crearPO.CheckErrorMessage("debe ser posterior");
            Assert.True(hayError, "El sistema no mostró error por fecha de entrega inválida.");
        }

        // UC2_FA2 FLUJO ALTERNATIVO 2 - Modificar Carrito
        [Fact]
        [Trait("Category", "UIT")]
        public void UC2_FA2_ModificarCarrito_EliminarItem()
        {
            // 1. ARRANGE
            InitialStepsForRepararHerramientas();

            // 2. ACT
            _selectPO.AñadirHerramientaAReparacionCart(HerramientaNombre1);
            _selectPO.AñadirHerramientaAReparacionCart(HerramientaNombre2);
            _selectPO.RemoveHerramientaFromReparacionCart(HerramientaNombre1); // Eliminar (FA2)

            _selectPO.ProcesarReparacion(); // Paso 4

            // 3. ASSERT
            // El flujo debe pasar a la página de creación (Paso 5)
            bool urlCorrecta = _driver.Url.Contains("/Reparacion/CreateReparacion");
            Assert.True(urlCorrecta, $"No se redirigió al formulario de creación.");

            // Intentar rellenar solo la Herramienta 2 (la que queda)
            _crearPO.RellenarDatosGenerales(clienteNombre, clienteApellidos, telefonoOpcional, fechaEntregaValida, metodoPagoValue);
            _crearPO.RellenarDatosItemReparacion(HerramientaId2, cantidadItem, descripcionProblema); // Solo la herramienta 2

            _crearPO.PulsarCrearReparacion();
            _crearPO.ConfirmarModal();
            System.Threading.Thread.Sleep(2000);

            bool urlDetalle = _driver.Url.Contains("/Reparacion/DetalleReparacion");
            Assert.True(urlDetalle, $"Fallo: El flujo no se completó tras modificar el carrito.");
        }

        // UC2_FA3 FLUJO ALTERNATIVO 3 - Carrito Vacío (al Paso 4)
        [Fact]
        [Trait("Category", "UIT")]
        public void UC2_FA3_CarritoVacio_NoPermiteContinuar()
        {
            // 1. ARRANGE
            InitialStepsForRepararHerramientas(); // Paso 1 y 2

            // 2. ACT
            // Comprobamos si el botón 'Procesar Reparación' está inactivo (Flujo Alternativo 3)
            bool isNotAvailable = _selectPO.ReparacionNotAvailable();

            // 3. ASSERT
            Assert.True(isNotAvailable, "El botón 'Procesar Reparación' estaba activo con el carrito vacío.");

            // Aseguramos que la navegación no ha ocurrido (sigue en el paso 2)
            bool seguimosEnSeleccion = _driver.Url.Contains("/Reparacion/SeleccionHerramientaParaReparaciones");
            Assert.True(seguimosEnSeleccion, "El sistema permitió continuar con el carrito vacío.");
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
            // Rellenar datos, pero omitiendo 'Apellidos' (campo obligatorio)
            _crearPO.RellenarDatosGenerales(clienteNombre, "", telefonoOpcional, fechaEntregaValida, metodoPagoValue); // Apellidos: ""
            _crearPO.RellenarDatosItemReparacion(HerramientaId1, cantidadItem, descripcionProblema);
            _crearPO.PulsarCrearReparacion();

            // 3. ASSERT
            bool seguimosEnCrear = _driver.Url.Contains("/Reparacion/CreateReparacion");
            Assert.True(seguimosEnCrear, $"El sistema permitió crear reparación sin campos obligatorios.");

            // Comprobar mensaje de error de validación (Flujo Alternativo 4)
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
            // Rellenar datos, pero con Cantidad = 0 (Flujo Alternativo 5)
            _crearPO.RellenarDatosGenerales(clienteNombre, clienteApellidos, telefonoOpcional, fechaEntregaValida, metodoPagoValue);
            _crearPO.RellenarDatosItemReparacion(HerramientaId1, 0, descripcionProblema); // Cantidad: 0

            // 3. ASSERT
            // El botón debe estar inactivo/deshabilitado (Flujo Alternativo 5)
            Assert.True(_crearPO.IsCrearReparacionButtonDisabled(), "El botón 'Guardar' no se inhabilitó al establecer cantidad 0.");

            // Comprobamos que no navega
            try { _crearPO.PulsarCrearReparacion(); } catch (Exception) { /* Ignorar error de click */ }
            System.Threading.Thread.Sleep(500);

            bool seguimosEnCrear = _driver.Url.Contains("/Reparacion/CreateReparacion");
            Assert.True(seguimosEnCrear, $"El sistema permitió crear reparación con cantidad cero.");
        }
    }
}