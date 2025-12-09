using AppForMovies.UIT.Shared;
using AppForSEII2526.UIT.Shared;
using Xunit;
using Xunit.Abstractions;
using System;
using System.Collections.Generic;

namespace AppForSEII2526.UIT.CU_Compra
{
    public class CUCrearCompra_UIT : UC_UIT
    {
        private readonly SelectHerramientasParaComprar_PO _selectPO;
        private readonly CrearCompra_PO _crearPO;

        public CUCrearCompra_UIT(ITestOutputHelper output) : base(output)
        {
            _selectPO = new SelectHerramientasParaComprar_PO(_driver, output);
            _crearPO = new CrearCompra_PO(_driver, output);
        }

        // -------------------------------------------------------------------
        // [Fact]: PRUEBA DE CAMINO FELIZ (Creación Exitosa)
        // -------------------------------------------------------------------
        [Fact]
        [Trait("Category", "UIT")]
        public void UC1_0_CrearCompra_FlujoBasico_Exito()
        {
            // 1. ARRANGE
            string nombre = "Yoel";
            string apellidos = "CS";
            string direccionEnvio = "casa de yoel";
            string pagoValue = "0";

            int herramientaId = 1;
            string nombreHerramienta = "Martillo";
            string material = "Madera";
            string precio = "10";

            // 2. ACT
            // Navegar
            _driver.Navigate().GoToUrl(_URI + "Compra/SelectHerramientasParaCompra");

            // Buscar y Seleccionar
            _selectPO.BuscarHerramientas(material, precio);
            _selectPO.AñadirHerramientasAlCarroDeCompra(nombreHerramienta);
            _selectPO.Continuar();

            // Rellenar Formulario
            _crearPO.RellenarDatosGenerales(nombre, apellidos, direccionEnvio, pagoValue);
            _crearPO.RellenarDescripcion(herramientaId, "Descripción cualquiera"); // cualquier descripción no nula es válida

            // Confirmar
            _crearPO.PulsarCrearCompra();
            _crearPO.ConfirmarModal();

            // 3. ASSERT
            // Esperamos redirección a Detalle
            System.Threading.Thread.Sleep(2000); // DEJAMOS UN POCO DE TIEMPO PARA PROCESAR
            bool urlCorrecta = _driver.Url.Contains("/Compra/DetailParaCompra");
            Assert.True(urlCorrecta, $"Fallo: No se redirigió al detalle. URL actual: {_driver.Url}");
        }
        
        // -------------------------------------------------------------------
        // [Theory]: ESCENARIO 1: DESCRIPCIÓN INVÁLIDA
        // -------------------------------------------------------------------
        [Theory]
        [Trait("Category", "UIT")]
        [InlineData("")] // Caso: String vacío
        [InlineData(null)]  // Caso: null
        public void UC1_1_CrearCompra_DescripcionInvalida_Error(string descripcionInvalida)
        {
            // 1. ARRANGE
            string nombre = "Yoel";
            string apellidos = "CS";
            string direccionEnvio = "casa de yoel";
            string pagoValue = "0";

            int herramientaId = 1;
            string nombreHerramienta = "Martillo";
            string material = "Madera";
            string precio = "10";

            // 2. ACT (Pasos idénticos hasta el formulario)
            _driver.Navigate().GoToUrl(_URI + "Compra/SelectHerramientasParaCompra");
            _selectPO.BuscarHerramientas(material, precio);
            _selectPO.AñadirHerramientasAlCarroDeCompra(nombreHerramienta);
            _selectPO.Continuar();

            // Rellenamos datos válidos generales
            _crearPO.RellenarDatosGenerales(nombre, apellidos, direccionEnvio, pagoValue);
            _crearPO.RellenarDescripcion(herramientaId, descripcionInvalida); // INTRODUCIMOS EL DATO INVÁLIDO DEL THEORY

            // Intentamos guardar
            _crearPO.PulsarCrearCompra();
            _crearPO.ConfirmarModal();

            // 3. ASSERT
            bool seguimosEnCrear = _driver.Url.Contains("/Compra/CrearCompra");
            Assert.True(seguimosEnCrear, $"El sistema permitió crear oferta con descripción {descripcionInvalida}%");

        }

        // -------------------------------------------------------------------
        // [Fact]: ESCENARIO 2: USUARIO NO EXISTE
        // -------------------------------------------------------------------
        [Fact]
        [Trait("Category", "UIT")]
        public void UC1_2_UsuarioNoExistente_Error()
        {
            // 1. ARRANGE
            string nombre = "Eloy";
            string apellidos = "CS";
            string direccionEnvio = "casa de yoel";
            string pagoValue = "0";

            int herramientaId = 1;
            string nombreHerramienta = "Martillo";
            string material = "Madera";
            string precio = "10";

            // 2. ACT
            _driver.Navigate().GoToUrl(_URI + "Compra/SelectHerramientasParaCompra");
            _selectPO.BuscarHerramientas(material, precio);
            _selectPO.AñadirHerramientasAlCarroDeCompra(nombreHerramienta);
            _selectPO.Continuar();

            // Rellenar Formulario
            _crearPO.RellenarDatosGenerales(nombre, apellidos, direccionEnvio, pagoValue); // EL USUARIO CON NOMBRE "Eloy" NO EXISTE
            _crearPO.RellenarDescripcion(herramientaId, "Descripción cualquiera"); // cualquier descripción no nula es válida

            // Intentamos guardar
            _crearPO.PulsarCrearCompra();
            try { _crearPO.ConfirmarModal(); } catch { /* Si no sale modal, seguimos */ }

            // 3. ASSERT
            System.Threading.Thread.Sleep(1000); // DEJAMOS UN POCO DE TIEMPO PARA PROCESAR
            bool hayError = _crearPO.CheckErrorMessage("no existe") || _crearPO.CheckErrorMessage("Error");
            Assert.True(hayError, "El sistema no mostró error al usar un usuario inexistente.");
        }

        // -------------------------------------------------------------------
        // [Fact]: ESCENARIO 3: CARRITO VACÍO
        // -------------------------------------------------------------------
        [Fact]
        [Trait("Category", "UIT")]
        public void UC1_3_CarritoVacio_Error()
        {
            // 1. ARRANGE
            _driver.Navigate().GoToUrl(_URI + "Compra/SelectHerramientasParaCompra");

            // 2. ACT - Intentar pulsar continuar SIN añadir nada
            try
            {
                _selectPO.Continuar();
            }
            catch (Exception) { /* Ignoramos si falla el click por estar disabled */ }

            // 3. ASSERT
            bool seguimosEnSeleccion = _driver.Url.Contains("/Compra/SelectHerramientasParaCompra");
            Assert.True(seguimosEnSeleccion, "El sistema permitió continuar con el carrito vacío.");
        }
    }
}