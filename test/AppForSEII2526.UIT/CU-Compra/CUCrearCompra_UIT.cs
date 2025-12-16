using AppForMovies.UIT.Shared;
using AppForSEII2526.UIT.Shared;
using Xunit;
using Xunit.Abstractions;
using System;
using System.Collections.Generic;

// PARA PROBAR LOS TESTS HAY QUE DAR CLICK DERECHO, VER, ABRIR CON EL NAVEGADOR A LA API Y A LA WEB

namespace AppForSEII2526.UIT.CU_Compra
{
    public class CUCrearCompra_UIT : UC_UIT
    {
        private readonly SelectHerramientasParaComprar_PO _selectPO;
        private readonly CrearCompra_PO _crearPO;
        private readonly DetalleCompra_PO _detallePO;

        public CUCrearCompra_UIT(ITestOutputHelper output) : base(output)
        {
            _selectPO = new SelectHerramientasParaComprar_PO(_driver, output);
            _crearPO = new CrearCompra_PO(_driver, output);
            _detallePO = new DetalleCompra_PO(_driver, output);
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

            // Datos esperados en la tabla de Detalles (Nombre, Apellidos, Direccion)
            var expectedDetails = new List<string[]>
            {
                new string[] { nombre, apellidos, direccionEnvio }
            };
            var expectedItems = new List<string> { nombreHerramienta };

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
            // 3.1 Verificar detalles del comprador
            Assert.True(_detallePO.CheckDetallesCompra(expectedDetails),
                "Fallo: Los detalles del comprador en la tabla de detalles no coinciden.");
            // 3.2 Verificar ítems comprados
            Assert.True(_detallePO.CheckItemsComprados(expectedItems),
                "Fallo: La lista de ítems comprados no coincide con lo esperado.");
        }

        // -------------------------------------------------------------------
        // [Theory]: DESCRIPCIÓN INVÁLIDA
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
            System.Threading.Thread.Sleep(1000); // DEJAMOS UN POCO DE TIEMPO PARA PROCESAR
            bool hayError = _crearPO.CheckErrorMessage("no tiene descipción") || _crearPO.CheckErrorMessage("Error");
            Assert.True(hayError, "El sistema mostró error al faltar descripción.");

        }

        // -------------------------------------------------------------------
        // [Fact]: USUARIO NO EXISTE
        // -------------------------------------------------------------------
        [Fact]
        [Trait("Category", "UIT")]
        public void UC1_2_UsuarioNoExistente_Error()
        {
            // 1. ARRANGE
            string nombre = "Eloy"; // Claramente no Yoel
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
            Assert.True(hayError, "El sistema mostró error al usar un usuario inexistente.");
        }

        // -------------------------------------------------------------------
        // [Fact]: CARRITO VACÍO
        // -------------------------------------------------------------------
        [Fact]
        [Trait("Category", "UIT")]
        public void UC1_3_CarritoVacio1_Error()
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

        // -------------------------------------------------------------------
        // [Fact]: CARRITO VACÍO (despues de añadir y quitar herramientas)
        // -------------------------------------------------------------------
        [Fact]
        [Trait("Category", "UIT")]
        public void UC1_4_CarritoVacio2_Error()
        {
            // 1. ARRANGE
            _driver.Navigate().GoToUrl(_URI + "Compra/SelectHerramientasParaCompra");

            // 2. ACT
            _selectPO.BuscarHerramientas("", "");
            _selectPO.AñadirHerramientasAlCarroDeCompra("Llave");
            _selectPO.AñadirHerramientasAlCarroDeCompra("Martillo");
            _selectPO.AñadirHerramientasAlCarroDeCompra("Llave");
            _selectPO.AñadirHerramientasAlCarroDeCompra("Martillo");
            _selectPO.ClearCart();
            _selectPO.AñadirHerramientasAlCarroDeCompra("Llave");
            _selectPO.AñadirHerramientasAlCarroDeCompra("Martillo");
            _selectPO.borrarHerramienta("Llave");
            _selectPO.borrarHerramienta("Martillo");

            // Intentar pulsar continuar SIN quedar nada en el carrito
            try
            {
                _selectPO.Continuar();
            }
            catch (Exception) { /* Ignoramos si falla el click por estar disabled */ }

            // 3. ASSERT
            bool seguimosEnSeleccion = _driver.Url.Contains("/Compra/SelectHerramientasParaCompra");
            Assert.True(seguimosEnSeleccion, "El sistema permitió continuar con el carrito vacío.");
        }

        // -------------------------------------------------------------------
        // [Fact]: Faltan datos obligatorios
        // -------------------------------------------------------------------
        [Fact]
        [Trait("Category", "UIT")]
        public void UC1_5_DatosObligatoriosFaltantes_Error()
        {
            // 1. ARRANGE
            string nombre = "Yoel";
            string apellidos = "CS";
            string direccionEnvio = ""; // falta dirección
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
            System.Threading.Thread.Sleep(1000); // DEJAMOS UN POCO DE TIEMPO PARA PROCESAR
            bool hayError = _crearPO.CheckErrorMessage("La compra debe tener una dirección de envío.") || _crearPO.CheckErrorMessage("Error");
            Assert.True(hayError, "El sistema no mostró error al ser la dirección inexistente.");
        }

        // -------------------------------------------------------------------
        // [Fact]: COMPRAR MULTIPLES ITEMS: (Creación Exitosa)
        // -------------------------------------------------------------------
        [Fact]
        [Trait("Category", "UIT")]
        public void UC1_6_CompraMultiplesItems_Exito()
        {
            // 1. ARRANGE
            string nombre = "Yoel";
            string apellidos = "CS";
            string direccionEnvio = "casa de yoel";
            string pagoValue = "0";

            string material = "";
            string precio = "";

            // 2. ACT
            // Navegar
            _driver.Navigate().GoToUrl(_URI + "Compra/SelectHerramientasParaCompra");

            // Buscar y Seleccionar
            _selectPO.BuscarHerramientas(material, precio);
            _selectPO.AñadirHerramientasAlCarroDeCompra("Martillo");
            _selectPO.AñadirHerramientasAlCarroDeCompra("Martillo");
            _selectPO.AñadirHerramientasAlCarroDeCompra("Llave");
            _selectPO.borrarHerramienta("Llave");
            _selectPO.AñadirHerramientasAlCarroDeCompra("Llave");
            _selectPO.AñadirHerramientasAlCarroDeCompra("Llave");
            _selectPO.Continuar();

            // Rellenar Formulario
            _crearPO.RellenarDatosGenerales(nombre, apellidos, direccionEnvio, pagoValue);
            _crearPO.RellenarDescripcion(1, "Descripción martillo cualquiera"); // cualquier descripción no nula es válida
            _crearPO.RellenarDescripcion(2, "Descripción martillo cualquiera"); // cualquier descripción no nula es válida

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
        // [Fact]: PRUEBA DE CANCELAR EL DIÁLOGO DE CONFIRMACIÓN (Creación Exitosa)
        // -------------------------------------------------------------------
        [Fact]
        [Trait("Category", "UIT")]
        public void UC1_7_NavegacionCancelarDialogoCompra_Exito()
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
            // Retroceder
            _crearPO.RechazarModal();
            // Confirmar, ahora sí
            _crearPO.PulsarCrearCompra();
            _crearPO.ConfirmarModal();

            // 3. ASSERT
            // Esperamos redirección a Detalle
            System.Threading.Thread.Sleep(2000); // DEJAMOS UN POCO DE TIEMPO PARA PROCESAR
            bool urlCorrecta = _driver.Url.Contains("/Compra/DetailParaCompra");
            Assert.True(urlCorrecta, $"Fallo: No se redirigió al detalle. URL actual: {_driver.Url}");
        }

        // -------------------------------------------------------------------
        // [Fact]: PRUEBA DE REGRESAR AL CARRITO (Creación Exitosa)
        // -------------------------------------------------------------------
        [Fact]
        [Trait("Category", "UIT")]
        public void UC1_8_NavegacionModificarCarrito_Exito()
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

            // Retrocedo al carrito
            _crearPO.PulsarModificarCarrito();
            // Continuar
            _selectPO.Continuar();

            // Confirmar
            _crearPO.PulsarCrearCompra();
            _crearPO.ConfirmarModal();

            // 3. ASSERT
            // Esperamos redirección a Detalle
            System.Threading.Thread.Sleep(2000); // DEJAMOS UN POCO DE TIEMPO PARA PROCESAR
            bool urlCorrecta = _driver.Url.Contains("/Compra/DetailParaCompra");
            Assert.True(urlCorrecta, $"Fallo: No se redirigió al detalle. URL actual: {_driver.Url}");
        }
    }
}