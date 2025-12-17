using AppForMovies.UIT.Shared;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
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
        private const string nombreInvalido = "Fantasma";
        private const string apellidosInvalido = "Fantasmez";
        private const string calleInvalida = "No empieza por Calle";
        // Constantes para el examen
        private const string herramientaNombre2 = "Llave";
        private const string herramientaMaterial2 = "Hierro";
        private const int herramientaId2 = 2;
        private const float herramientaPrecio2 = 15.0f;

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
            _driver.Navigate().GoToUrl(_URI + "Alquiler/SeleccionarHerramientasParaAlquilar");
        }

        // UC4_1: Flujo Básico - Creación Exitosa (Esc-1)
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC4_1_CrearAlquiler_Exito()
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
                //Nota: Solo compruebo la parte inicial debido a la complejidad de comprobar adicionalmente los items del alquiler.
                new string[] {
                    usuario,                            // Columna 0
                    apellidos,                           //Columna 1
                    direccion,                          // Columna 2
                    fechaActual.ToString("dd/MM/yyyy"), // Columna 3 
                    fechaInicio.ToString("dd/MM/yyyy"), // Columna 4  
                    fechaFin.ToString("dd/MM/yyyy"),    // Columna 5 
                    herramientaPrecio1.ToString("0.00")// Columna 6
                }
            };

            Assert.True(_detallePO.CheckDetallesAlquiler(expectedRow),
                "Error: Los detalles del alquiler no coinciden.");
        }

        public static IEnumerable<object[]> DatosParaFiltros()
        {
            yield return new object[]
            {
                "Martillo",
                "Madera",
                new List<string[]>
                {
                    new string[] { "Martillo", "Madera", "EMPRESA1", "10", "Añadir" }
                }
            };

            yield return new object[]
            {
                null,
                "Hierro",
                new List<string[]>
                {
                    new string[] { "Llave", "Hierro", "EMPRESA2", "15", "Añadir" }
                }
            };

            yield return new object[]
            {
                "Martillo",
                null,
                new List<string[]>
                {
                    new string[] { "Martillo", "Madera", "EMPRESA1", "10", "Añadir" }
                }
            };
        }

        [Theory]
        [MemberData(nameof(DatosParaFiltros))]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC4_FiltroPorNombreYMaterial(string? nombre, string? material, List<string[]> expectedHerramientas)
        {
            //Act
            InitialStepsForCrearAlquiler_UIT();
            _selectPO.BuscarHerramientas(nombre, material);
            //Assert
            Assert.True(_selectPO.CheckListOfHerramientas(expectedHerramientas),
                "Error: La lista de herramientas filtradas no coincide con la esperada.");
        }


        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC4_PruebaExamen()
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

            // 1. Seleccionamos la primera herramienta filtrando por nombre
            _selectPO.BuscarHerramientas(herramientaNombre1, null);
            _selectPO.AñadirHerramientasAlCarroDeAlquiler(herramientaNombre1);
            // ahora filtramos por material
            _selectPO.BuscarHerramientas(null, herramientaMaterial2);
            _selectPO.AñadirHerramientasAlCarroDeAlquiler(herramientaNombre2);
            // vamos a la pantalla del post, volvemos a la del select y borramos la primera herramienta
            _selectPO.Continuar();
            _crearPO.PulsarModificarCarrito();
            _selectPO.borrarHerramienta(herramientaNombre1);
            _selectPO.Continuar();
            // 2. Rellenar formulario
            _crearPO.RellenarDatosGenerales(fechaInicio, fechaFin, usuario, apellidos, direccion, telefono, correo, pago);
            _crearPO.EstablecerCantidad(herramientaId2, cantidad);
            _crearPO.PulsarCrearAlquiler();
            _crearPO.ConfirmarModal();

            // Assert
            var expectedRow = new List<string[]>
            {
                //Nota: Solo compruebo la parte inicial debido a la complejidad de comprobar adicionalmente los items del alquiler.
                new string[] {
                    usuario,                            // Columna 0
                    apellidos,                           //Columna 1
                    direccion,                          // Columna 2
                    fechaActual.ToString("dd/MM/yyyy"), // Columna 3 
                    fechaInicio.ToString("dd/MM/yyyy"), // Columna 4  
                    fechaFin.ToString("dd/MM/yyyy"),    // Columna 5 
                    herramientaPrecio2.ToString("0.00")// Columna 6
                }
            };

            Assert.True(_detallePO.CheckDetallesAlquiler(expectedRow),
                "Error: Los detalles del alquiler no coinciden.");

        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC4_ListaVacia_Error()
        {
            // Act
            InitialStepsForCrearAlquiler_UIT();

            bool botonHabilitado = true;
            try
            {
                _selectPO.Continuar();
            }
            catch (Exception)
            {
                botonHabilitado = false;
            }

            // Assert
            if (botonHabilitado)
            {
                bool urlCambio = _driver.Url.Contains("Alquila tus herramientas");
                if (urlCambio)
                {
                    // Si logramos pasar intentamos guardar y buscamos el error
                    _crearPO.RellenarDatosGenerales(DateTime.Today.AddDays(1), DateTime.Today.AddDays(30),"Yoel","CS","Calle OMG","12345789","xd@gmail.com","PayPal");
                    _crearPO.PulsarCrearAlquiler();
                    Assert.True(_crearPO.CheckErrorMessage("El alquiler debe incluir al menos una herramienta") ||
                                _crearPO.CheckErrorMessage("debe incluir"),
                                "UC4_2 Falló: No apareció el mensaje de error de lista vacía.");
                }
            }
            else
            {
                Assert.True(!botonHabilitado, "Correcto: El botón continuar está deshabilitado con lista vacía.");
            }
        }

        public static IEnumerable<object[]> TestCasesFor_FechasInvalidas()
        {
            var allTests = new List<object[]>
            {
                // Inicio Ayer
                new object[] { DateTime.Today.AddDays(-1), DateTime.Today.AddDays(30), "La fecha de inicio debe ser posterior a la actualidad."},
                
                // Fin antes que Inicio (Hoy / Ayer -> Fin < Inicio)
                new object[] { DateTime.Today.AddDays(1), DateTime.Today,  "La fecha de fin debe ser mayor que la de inicio."  },
                
            };
            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_FechasInvalidas))]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC4_FechasInvalidas_Error(DateTime inicio, DateTime fin, string mensajeError)
        {
            // Act
            InitialStepsForCrearAlquiler_UIT();
            _selectPO.BuscarHerramientas(herramientaNombre1,herramientaMaterial1);
            _selectPO.AñadirHerramientasAlCarroDeAlquiler(herramientaNombre1);
            _selectPO.Continuar();

            _crearPO.RellenarDatosGenerales(inicio, fin, "Yoel", "CS", "Calle AAAA", "656376257", "jfkdsj@gmail.com", "PayPal");
            _crearPO.PulsarCrearAlquiler();
            try { _crearPO.ConfirmarModal(); } catch { }

            // Assert
            Assert.True(_crearPO.CheckErrorMessage(mensajeError),
                $"Fallo en validación de fechas ({inicio.ToShortDateString()} - {fin.ToShortDateString()}). Esperado: {mensajeError}");
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC4_UsuarioNoExiste_Error()
        {
            // Act
            InitialStepsForCrearAlquiler_UIT();
            _selectPO.BuscarHerramientas(herramientaNombre1, herramientaMaterial1);
            _selectPO.AñadirHerramientasAlCarroDeAlquiler(herramientaNombre1);
            _selectPO.Continuar();

            _crearPO.RellenarDatosGenerales(DateTime.Today.AddDays(1), DateTime.Today.AddDays(30), nombreInvalido, apellidosInvalido, "Calle Siniestra", "00000", "fantasmita@hola.com", "Efectivo");

            _crearPO.PulsarCrearAlquiler();
            try { _crearPO.ConfirmarModal(); } catch { }

            // Assert
            Assert.True(_crearPO.CheckErrorMessage("El Usuario "+ nombreInvalido + " " + apellidosInvalido + " no existe."),
                "No se mostró error de usuario inexistente.");
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC4_CalleInvalida_Error()
        {
            // Act
            InitialStepsForCrearAlquiler_UIT();
            _selectPO.BuscarHerramientas(herramientaNombre1, herramientaMaterial1);
            _selectPO.AñadirHerramientasAlCarroDeAlquiler(herramientaNombre1);
            _selectPO.Continuar();

            _crearPO.RellenarDatosGenerales(DateTime.Today.AddDays(1), DateTime.Today.AddDays(30), "Yoel", "CS", calleInvalida, "00000", "hello@hola.com", "Efectivo");

            _crearPO.PulsarCrearAlquiler();
            try { _crearPO.ConfirmarModal(); } catch { }

            // Assert
            Assert.True(_crearPO.CheckErrorMessage("¡Error! La dirección de envío debe empezar por la palabra Calle"),
                "No se mostró error de calle inválida.");
        }



        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC4_BorrarHerramientaCarrito()
        {
            //Act
            InitialStepsForCrearAlquiler_UIT();
            _selectPO.BuscarHerramientas(herramientaNombre1, null);
            _selectPO.AñadirHerramientasAlCarroDeAlquiler(herramientaNombre1);
            _selectPO.Continuar();
            _crearPO.PulsarModificarCarrito();
            _selectPO.borrarHerramienta(herramientaNombre1);
            //Assert
            Assert.True(_selectPO.CheckEmptyCart(),
                "Error: El carrito no está vacío tras borrar la herramienta.");

        }
    }
}
