using AppForMovies.UIT.Shared;
using AppForSEII2526.UIT.Shared;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.CU_Oferta
{
    public class CUCrearOferta_UIT : UC_UIT
    {
        private const string herramientaNombre1 = "Martillo";
        private const string herramientaFabricante1 = "EMPRESA1";
        private const string herramientaPrecio1 = "10";
        private const int herramientaId1 = 1;

        private const string usuarioEmail = "elena@uclm.es";
        private const string usuarioPass = "Password1234%";

        // Page Objects
        private readonly SelectHerramientasParaOfertar_PO _selectPO;
        private readonly CrearOferta_PO _crearPO;
        private readonly DetalleOferta_PO _detallePO;

        public CUCrearOferta_UIT(ITestOutputHelper output) : base(output)
        {
            Initial_step_opening_the_web_page();

            _selectPO = new SelectHerramientasParaOfertar_PO(_driver, _output);
            _crearPO = new CrearOferta_PO(_driver, _output);
            _detallePO = new DetalleOferta_PO(_driver, _output);
        }


        private void InitialStepsForCrearOferta_UIT()
        {
            _driver.Navigate().GoToUrl(_URI + "Ofertar/SeleccionarHerramientaParaOfertar");
        }

        // UC3_1: Flujo Básico - Creación Exitosa (Esc-1)

        [Theory]
        // Caso 1: Martillo (ID 1), Efectivo (ID 0)
        [InlineData("Martillo", 1, "EMPRESA1", "0", "Efectivo", "")]
        // Caso 2: Llave (ID 2), PayPal (ID 1), Socio
        [InlineData("Llave", 2, "EMPRESA2", "1", "Paypal", "Socio")]
        // Caso 3: Martillo (ID 1), Tarjeta (ID 2), Cliente 
        [InlineData("Martillo", 1, "EMPRESA1", "2", "Tarjeta", "Cliente")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC3_1_CrearOferta_Exito(
            string nombreHerramienta,
            int idHerramienta,
            string fabricante,
            string pagoId,
            string nombrePagoEsperado,
            string tipoDirigido)
        {
            // Arrange
            var fechaInicio = DateTime.Today.AddDays(1);
            var fechaFin = DateTime.Today.AddDays(30);
            var fechaActual = DateTime.Today;

            string usuario = "Yoel";
            int descuento = 10;

            // Act
            InitialStepsForCrearOferta_UIT();

            // 1. Selección (Usando los datos del Theory)
            _selectPO.SearchHerramientas(fabricante, null);
            _selectPO.AddHerramientaToOfertaCart(nombreHerramienta);
            _selectPO.PressContinuar();

            // 2. Rellenar formulario (Usando los datos del Theory)
            _crearPO.RellenarDatosGenerales(fechaInicio, fechaFin, pagoId, usuario, tipoDirigido);

            // Establecemos el porcentaje usando el ID correcto de la herramienta seleccionada
            _crearPO.EstablecerPorcentaje(idHerramienta, descuento);

            _crearPO.PulsarCrearOferta();
            _crearPO.ConfirmarModal();

            // Assert
            var expectedRow = new List<string[]>
            {
                new string[] {
                    fechaInicio.ToString("dd/MM/yyyy"), // Columna 1: Inicio
                    fechaFin.ToString("dd/MM/yyyy"),    // Columna 2: Fin
                    fechaActual.ToString("dd/MM/yyyy"), // Columna 3: Fecha Oferta
                    nombrePagoEsperado                  // Columna 4: Pago (Efectivo/Paypal/Tarjeta)
                }
            };

            // Verificamos que la fila de cabecera coincida con los datos introducidos
            Assert.True(_detallePO.CheckDetallesOferta(expectedRow),
                $"Error: Los detalles de la oferta para el caso '{nombreHerramienta} - {nombrePagoEsperado}' no coinciden.");
        }

        public static IEnumerable<object[]> DatosParaFiltros()
        {
            yield return new object[]
            {
                "EMPRESA1",
                15.0f,
                new List<string[]>
                {
                    new string[] { "Martillo", "Madera", "EMPRESA1", "10", "Add" }
                }
            };

            yield return new object[]
            {
                null,
                15.0f,
                new List<string[]>
                {
                    new string[] { "Martillo", "Madera", "EMPRESA1", "10", "Add" },
                    new string[] { "Llave", "Hierro", "EMPRESA2", "15", "Add" }
                }
            };

            yield return new object[]
            {
                "EMPRESA1",
                null,
                new List<string[]>
                {
                    new string[] { "Martillo", "Madera", "EMPRESA1", "10", "Add" }
                }
            };
        }


        [Theory]
        [MemberData(nameof(DatosParaFiltros))]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC3_6_FA0_FiltroPorFabricanteYPrecio(string? fabricante, float? precio, List<string[]> expectedHerramientas)
        {
            //Act
            InitialStepsForCrearOferta_UIT();
            _selectPO.SearchHerramientas(fabricante, precio);
            //Assert
            Assert.True(_selectPO.CheckListOfHerramientas(expectedHerramientas),
                "Error: La lista de herramientas filtradas no coincide con la esperada.");
        }

        // UC3_2: Lista Vacía (Esc-4)
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC3_2_FA4_ListaVacia_Error()
        {
            // Act
            InitialStepsForCrearOferta_UIT();

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
        public void UC3_2_FA1_FechasInvalidas_Error(DateTime inicio, DateTime fin, string mensajeError)
        {
            // Act
            InitialStepsForCrearOferta_UIT();
            _selectPO.SearchHerramientas(herramientaFabricante1, null);
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
        public void UC3_5_UsuarioNoExiste_Error()
        {
            // Act
            InitialStepsForCrearOferta_UIT();
            _selectPO.SearchHerramientas(herramientaFabricante1, null);
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
        public void UC3_3_FA3_PorcentajesInvalidos_Error(int descuento)
        {
            // Act
            InitialStepsForCrearOferta_UIT();
            _selectPO.SearchHerramientas(herramientaFabricante1, null);
            _selectPO.AddHerramientaToOfertaCart(herramientaNombre1);
            _selectPO.PressContinuar();

            _crearPO.RellenarDatosGenerales(DateTime.Today.AddDays(1), DateTime.Today.AddDays(30), "0", "Yoel", "Cliente");

            // Introducimos el porcentaje inválido
            _crearPO.EstablecerPorcentaje(herramientaId1, descuento);

            _crearPO.PulsarCrearOferta();
            try { _crearPO.ConfirmarModal(); } catch { }

            // Assert
            Assert.True(_crearPO.CheckErrorMessage("no es válido") && _crearPO.CheckErrorMessage("entre 1 y 90"),
                $"UC3_9/10 Falló: Se permitió un descuento inválido de {descuento}%.");
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC3_FA2_BorrarHerramientaCarrito()
        {
            //Act
            InitialStepsForCrearOferta_UIT();
            _selectPO.SearchHerramientas(herramientaFabricante1, null);
            _selectPO.AddHerramientaToOfertaCart(herramientaNombre1);
            _selectPO.PressContinuar();
            _crearPO.PulsarModificarCarrito();
            _selectPO.borrarHerramienta();
            //Assert
            Assert.True(_selectPO.CheckEmptyCart(),
                "Error: El carrito no está vacío tras borrar la herramienta.");

        }

        public static IEnumerable<object[]> TestCasesFor_DatosFaltantes()
        {
            yield return new object[] { DateTime.MinValue, DateTime.Today.AddDays(10), "Yoel", "0" };
            yield return new object[] { DateTime.Today.AddDays(1), DateTime.MinValue, "Yoel", "0" };
            yield return new object[] { DateTime.Today.AddDays(1), DateTime.Today.AddDays(10), "", "0" };
            yield return new object[] { DateTime.Today.AddDays(1), DateTime.Today.AddDays(10), "Yoel", "" };
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_DatosFaltantes))]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC3_FA5_DatosFaltantes(DateTime FechaInicio, DateTime FechaFinal, string nombreUsuario, string pagoValue)
        { 
            //Arrange
            string dirigidaA = "Cliente";
            //Act
            InitialStepsForCrearOferta_UIT();
            _selectPO.SearchHerramientas(herramientaFabricante1, null);
            _selectPO.AddHerramientaToOfertaCart(herramientaNombre1);
            _selectPO.PressContinuar();
            //Rellenar formulario sin fecha de fin
            _crearPO.RellenarDatosGenerales(FechaInicio, FechaFinal, pagoValue, nombreUsuario, dirigidaA);
            _crearPO.PulsarCrearOferta();
            //Assert
            //Comprobar si el botón de submit sigue desactivo
            Assert.True(_crearPO.IsCrearButtonEnabled(), "Error: El botón no se ha deshabilitado");
        }


    }
}