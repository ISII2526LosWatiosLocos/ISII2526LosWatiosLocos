using AppForMovies.UIT.Shared;
using AppForSEII2526.UIT.CU_Oferta;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;


namespace AppForSEII2526.UIT.CU_Alquiler
{
    // PARA PROBAR LOS TESTS HAY QUE DAR CLICK DERECHO, VER, ABRIR CON EL NAVEGADOR A LA API Y A LA WEB

    public class UC_AlquilarHerramientas_UIT: UC_UIT
    {
        private const string herramientaNombre1 = "Martillo";
        private const string herramientaMaterial1 = "Madera";
        private const float herramientaPrecio1 = 10.0f;
        private const int herramientaId1 = 1;
        // Page Objects
        private readonly SelectHerramientasParaAlquilar_PO _selectPO;
        private readonly CrearAlquiler_PO _crearPO;
        private readonly DetalleAlquiler_PO _detallePO;
        public UC_AlquilarHerramientas_UIT(ITestOutputHelper output) : base(output)
        {
            Initial_step_opening_the_web_page();
            _selectPO = new SelectHerramientasParaAlquilar_PO(_driver, _output);
            _crearPO = new CrearAlquiler_PO(_driver, _output);
            _detallePO = new DetalleAlquiler_PO(_driver, _output);

        }

        private void InitialStepsForCrearAlquiler_UIT()
        {
            _driver.Navigate().GoToUrl(_URI + "/Alquiler/SeleccionarHerramientasParaAlquilar");
        }

        // UC4_1: Flujo Básico - Creación Exitosa (Esc-1)
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC4_1_CrearOferta_Exito()
        {
            // Arrange
            var fechaInicio = DateTime.Today.AddDays(1);
            var fechaFin = DateTime.Today.AddDays(30);
            var fechaActual = DateTime.Today;
            string usuario = "Yoel";
            string apellidos = "CS";
            string direccion = "Calle Verdadera";
            string telefono = "123456789";
            string correo = "hola@gmail.com";
            string pago = "Efectivo"; // ID 0
            int cantidad = 1;
            // Act
            InitialStepsForCrearAlquiler_UIT();

            // 1. Selección
            _selectPO.BuscarHerramientas(herramientaNombre1, herramientaMaterial1);
            _selectPO.AñadirHerramientasAlCarroDeAlquiler(herramientaNombre1);
            _selectPO.Continuar();

            // 2. Rellenar formulario
            _crearPO.RellenarDatosGenerales(fechaInicio,fechaFin,usuario,apellidos,direccion,telefono,correo,pago);
            _crearPO.EstablecerCantidad(herramientaId1, cantidad);
            _crearPO.PulsarCrearAlquiler();
            _crearPO.ConfirmarModal();

            // Assert
            var expectedRow = new List<string[]>
            {
                //Nota: Solo compruebo la parte inicial debido a la complejidad de comprobar adicionalmente los items de la oferta.
                new string[] {
                    usuario,                            // Columna 0
                    apellidos,                           //Columna 1
                    direccion,                          // Columna 2
                    fechaActual.ToString("dd/MM/yyyy"), // Columna 3 
                    fechaInicio.ToString("dd/MM/yyyy"), // Columna 4  
                    fechaFin.ToString("dd/MM/yyyy"),    // Columna 5 
                    "$"+herramientaPrecio1.ToString("0.00")// Columna 6
                }
            };

            Assert.True(_detallePO.CheckDetallesAlquiler(expectedRow),
                "Error: Los detalles de la oferta (Fechas, Pago, Tipo) no coinciden.");
        }
        /** 
        // UC3_2: Lista Vacía (Esc-4)
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC3_2_ListaVacia_Error()
        {
            // Act
            InitialStepsForCrearAlquiler_UIT();

            bool botonHabilitado = true;
            try
            {
                _selectPO.PressContinuar();
            }
            catch (Exception)
            {
                botonHabilitado = false;
            }

            // Assert
            if (botonHabilitado)
            {
                bool urlCambio = _driver.Url.Contains("CrearOferta");
                if (urlCambio)
                {
                    // Si logramos pasar intentamos guardar y buscamos el error
                    _crearPO.RellenarDatosGenerales(DateTime.Today.AddDays(1), DateTime.Today.AddDays(30), "0", "Yoel", "Cliente");
                    _crearPO.PulsarCrearOferta();
                    Assert.True(_crearPO.CheckErrorMessage("La oferta debe incluir al menos una herramienta") ||
                                _crearPO.CheckErrorMessage("debe incluir"),
                                "UC3_2 Falló: No apareció el mensaje de error de lista vacía.");
                }
            }
            else
            {
                Assert.True(!botonHabilitado, "Correcto: El botón continuar está deshabilitado con lista vacía.");
            }
        }

        // UC3_3, UC3_4, UC3_5: Errores de Fechas (Esc-2)
        public static IEnumerable<object[]> TestCasesFor_FechasInvalidas()
        {
            var allTests = new List<object[]>
            {
                // UC3_3: Inicio Ayer
                new object[] { DateTime.Today.AddDays(-1), DateTime.Today.AddDays(30), "La fecha de inicio debe ser posterior a hoy" },
                
                // UC3_4: Fin antes que Inicio (Hoy / Ayer -> Fin < Inicio)
                new object[] { DateTime.Today.AddDays(1), DateTime.Today, "La fecha final debe ser posterior a la fecha de inicio" },
                
                // UC3_5: Duración < 1 semana
                new object[] { DateTime.Today.AddDays(1), DateTime.Today.AddDays(2), "La oferta debe durar al menos una semana" }
            };
            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_FechasInvalidas))]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC3_FechasInvalidas_Error(DateTime inicio, DateTime fin, string mensajeError)
        {
            // Act
            InitialStepsForCrearAlquiler_UIT();
            _selectPO.SearchHerramientas(herramientaFabricante1);
            _selectPO.AddHerramientaToOfertaCart(herramientaNombre1);
            _selectPO.PressContinuar();

            _crearPO.RellenarDatosGenerales(inicio, fin, "0", "Yoel", "Cliente");
            _crearPO.PulsarCrearOferta();
            try { _crearPO.ConfirmarModal(); } catch { }

            // Assert
            Assert.True(_crearPO.CheckErrorMessage(mensajeError),
                $"Fallo en validación de fechas ({inicio.ToShortDateString()} - {fin.ToShortDateString()}). Esperado: {mensajeError}");
        }

        // UC3_6: Usuario No Existe (Esc-5)
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC3_6_UsuarioNoExiste_Error()
        {
            // Act
            InitialStepsForCrearAlquiler_UIT();
            _selectPO.SearchHerramientas(herramientaFabricante);
            _selectPO.AddHerramientaToOfertaCart(herramientaNombre1);
            _selectPO.PressContinuar();

            _crearPO.RellenarDatosGenerales(DateTime.Today.AddDays(1), DateTime.Today.AddDays(30), "0", "UsuarioFantasma", "Cliente");

            _crearPO.PulsarCrearOferta();
            try { _crearPO.ConfirmarModal(); } catch { }

            // Assert
            Assert.True(_crearPO.CheckErrorMessage("usuario no existe") || _crearPO.CheckErrorMessage("no existe"),
                "UC3_6 Falló: No se mostró error de usuario inexistente.");
        }

        // UC3_9, UC3_10: Porcentajes Inválidos (Esc-3)
        [Theory]
        [InlineData(91)] // UC3_9
        [InlineData(0)]  // UC3_10
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC3_PorcentajesInvalidos_Error(int descuento)
        {
            // Act
            InitialStepsForCrearAlquiler_UIT();
            _selectPO.SearchHerramientas(herramientaFabricante1);
            _selectPO.AddHerramientaToOfertaCart(herramientaNombre1);
            _selectPO.PressContinuar();

            _crearPO.RellenarDatosGenerales(DateTime.Today.AddDays(1), DateTime.Today.AddDays(30), "0", "Yoel", "Cliente");

            // Introducimos el porcentaje inválido
            _crearPO.EstablecerPorcentaje(herramientaId1, descuento);

            _crearPO.PulsarCrearOferta();
            try { _crearPO.ConfirmarModal(); } catch { }

            // Assert
            // El mensaje del PDF dice: "El porcentaje X% para 'Herramienta' no es válido. Debe estar entre 1 y 90."
            Assert.True(_crearPO.CheckErrorMessage("no es válido") && _crearPO.CheckErrorMessage("entre 1 y 90"),
                $"UC3_9/10 Falló: Se permitió un descuento inválido de {descuento}%.");
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC3_7_BorrarHerramientaCarrito()
        {
            //Act
            InitialStepsForCrearAlquiler_UIT();
            _selectPO.SearchHerramientas(herramientaFabricante1);
            _selectPO.AddHerramientaToOfertaCart(herramientaNombre1);
            _selectPO.PressContinuar();
            _crearPO.PulsarModificarCarrito();
            _selectPO.borrarHerramienta();
            //Assert
            Assert.True(_selectPO.CheckEmptyCart(),
                "Error: El carrito no está vacío tras borrar la herramienta.");

        }
        **/
    }
}
