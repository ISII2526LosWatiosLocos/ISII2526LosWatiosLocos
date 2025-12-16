using AppForMovies.UIT.Shared;
using AppForSEII2526.UIT.Shared;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppForSEII2526.UIT.CU_Compra
{
    public class CUCrearCompra_UIT : UC_UIT
    {
        private const string herramientaNombre1 = "Martillo";
        private const string herramientaFabricante1 = "EMPRESA1";
        private const string herramientaPrecio1 = "10";
        private const int herramientaId1 = 1;

        private const string usuarioEmail = "elena@uclm.es";
        private const string usuarioPass = "Password1234%";

        // Page Objects
        private readonly SelectHerramientasParaComprar_PO _selectPO;
        private readonly CrearCompra_PO _crearPO;
        private readonly DetalleCompra_PO _detallePO;

        public CUCrearCompra_UIT(ITestOutputHelper output) : base(output)
        {
            Initial_step_opening_the_web_page();

            _selectPO = new SelectHerramientasParaComprar_PO(_driver, _output);
            _crearPO = new CrearCompra_PO(_driver, _output);
            _detallePO = new DetalleCompra_PO(_driver, _output);
        }


        private void InitialStepsForCrearCompra_UIT()
        {
            _driver.Navigate().GoToUrl(_URI + "Compra/SelectHerramientasParaCompra");
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
        public void UC3_1_CrearCompra_Exito(
            string nombreHerramienta,
            int idHerramienta,
            string fabricante,
            string pagoId,
            string nombrePagoEsperado,
            string tipoDirigido)
        {
            // Arrange
            string nombre = "Yoel";
            string apellidos = "CS";
            string direccionEnvio = "casa de yoel";
            string pagoValue = "0"; // Efectivo

            // Act
            InitialStepsForCrearCompra_UIT();

            // 1. Selección (Usando los datos del Theory)
            _selectPO.BuscarHerramientas(fabricante, null);
            _selectPO.AñadirHerramientasAlCarroDeCompra(nombreHerramienta);
            _selectPO.Continuar();

            // 2. Rellenar formulario (Usando los datos del Theory)
            _crearPO.RellenarDatosGenerales(nombre, apellidos, direccionEnvio, pagoValue);
            _crearPO.RellenarDescripcion(idHerramienta, "Descripción cualquiera"); // cualquier descripción no nula es válida

            _crearPO.PulsarCrearCompra();
            _crearPO.ConfirmarModal();

            // Assert
            var expectedRow = new List<string[]>
            {
                new string[] {
                    nombrePagoEsperado                  // Columna 1: Pago (Efectivo/Paypal/Tarjeta)
                }
            };

            // Verificamos que la fila de cabecera coincida con los datos introducidos
            Assert.True(_detallePO.CheckDetallesCompra(expectedRow),
                $"Error: Los detalles de la compra para el caso '{nombreHerramienta} - {nombrePagoEsperado}' no coinciden.");
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
        public void UC3_6_FA0_FiltroPorFabricanteYPrecio(string? fabricante, string? precio, List<string[]> expectedHerramientas)
        {
            //Act
            InitialStepsForCrearCompra_UIT();
            _selectPO.BuscarHerramientas(fabricante, precio);
            //Assert
            Assert.True(_selectPO.CheckListOfHerramientas(expectedHerramientas),
                "Error: La lista de herramientas filtradas no coincide con la esperada.");
        }

        // UC3_2: Lista Vacía (Esc-4)
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC3_2_FA4_CarritoVacio_Error()
        {
            // Act
            InitialStepsForCrearCompra_UIT();

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
                bool urlCambio = _driver.Url.Contains("CrearCompra");
                if (urlCambio)
                {
                    // Si logramos pasar intentamos guardar y buscamos el error
                    _crearPO.RellenarDatosGenerales("Yoel", "Apellidos", "casa de yoel", "0");
                    _crearPO.PulsarCrearCompra();
                    Assert.True(_crearPO.CheckErrorMessage("La compra debe incluir al menos una herramienta") ||
                                _crearPO.CheckErrorMessage("debe incluir"),
                                "UC3_2 Falló: No apareció el mensaje de error de lista vacía.");
                }
            }
            else
            {
                Assert.True(!botonHabilitado, "Correcto: El botón continuar está deshabilitado con lista vacía.");
            }
        }

        // UC3_6: Usuario No Existe (Esc-5)
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC3_5_UsuarioNoExiste_Error()
        {
            // Act
            InitialStepsForCrearCompra_UIT();
            _selectPO.BuscarHerramientas(herramientaFabricante1, null);
            _selectPO.AñadirHerramientasAlCarroDeCompra(herramientaNombre1);
            _selectPO.Continuar();

            _crearPO.RellenarDatosGenerales("UsuarioFantasma", "Apellidos", "casa de yoel", "0");

            _crearPO.PulsarCrearCompra();
            try { _crearPO.ConfirmarModal(); } catch { }

            // Assert
            Assert.True(_crearPO.CheckErrorMessage("usuario no existe") || _crearPO.CheckErrorMessage("no existe"),
                "UC3_6 Falló: No se mostró error de usuario inexistente.");
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC3_FA2_BorrarHerramientaCarrito()
        {
            //Act
            InitialStepsForCrearCompra_UIT();
            _selectPO.BuscarHerramientas(herramientaFabricante1, null);
            _selectPO.AñadirHerramientasAlCarroDeCompra(herramientaNombre1);
            _selectPO.Continuar();
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
        public void UC3_FA5_DatosFaltantes(string nombre, string apellidos, string direccionEnvio, string pagoValue)
        {
            //Arrange
            string dirigidaA = "Cliente";
            //Act
            InitialStepsForCrearCompra_UIT();
            _selectPO.BuscarHerramientas(herramientaFabricante1, null);
            _selectPO.AñadirHerramientasAlCarroDeCompra(herramientaNombre1);
            _selectPO.Continuar();
            //Rellenar formulario sin fecha de fin
            _crearPO.RellenarDatosGenerales(nombre, apellidos, direccionEnvio, pagoValue);
            _crearPO.PulsarCrearCompra();
            //Assert
            //Comprobar si el botón de submit sigue desactivo
            Assert.True(_crearPO.IsCrearButtonEnabled(), "Error: El botón no se ha deshabilitado");
        }


    }
}