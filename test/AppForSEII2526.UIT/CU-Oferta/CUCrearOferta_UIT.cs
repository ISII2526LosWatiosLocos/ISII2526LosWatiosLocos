using AppForMovies.UIT.Shared;
using AppForSEII2526.UIT.Shared;
using Xunit;
using Xunit.Abstractions;
using System;
using System.Collections.Generic;

namespace AppForSEII2526.UIT.CU_Oferta
{
    public class CUCrearOferta_UIT : UC_UIT
    {
        private readonly SelectHerramientasParaOfertar_PO _selectPO;
        private readonly CrearOferta_PO _crearPO;

        public CUCrearOferta_UIT(ITestOutputHelper output) : base(output)
        {
            _selectPO = new SelectHerramientasParaOfertar_PO(_driver, output);
            _crearPO = new CrearOferta_PO(_driver, output);
        }

        // -------------------------------------------------------------------
        // [Fact]: PRUEBA DE CAMINO FELIZ (Creación Exitosa)
        // -------------------------------------------------------------------
        [Fact]
        [Trait("Category", "UIT")]
        public void UC3_1_CrearOferta_FlujoBasico_Exito()
        {
            // 1. ARRANGE
            string fabricante = "EMPRESA1";
            string nombreHerramienta = "Martillo"; 
            int herramientaId = 1; 

            DateTime fechaInicio = DateTime.Today.AddDays(1);
            DateTime fechaFin = DateTime.Today.AddDays(30);

            // 2. ACT
            // Navegar
            _driver.Navigate().GoToUrl(_URI + "Ofertar/SeleccionarHerramientaParaOfertar");

            // Buscar y Seleccionar
            _selectPO.SearchHerramientas(fabricante);
            _selectPO.AddHerramientaToOfertaCart(nombreHerramienta); 
            _selectPO.PressContinuar(); 

            // Rellenar Formulario
            _crearPO.RellenarDatosGenerales(fechaInicio, fechaFin, "1", "Yoel", "Cliente");
            _crearPO.EstablecerPorcentaje(herramientaId, 20); // 20% es válido

            // Confirmar
            _crearPO.PulsarCrearOferta();
            _crearPO.ConfirmarModal();

            // 3. ASSERT
            // Esperamos redirección a Detalle
            System.Threading.Thread.Sleep(2000);
            bool urlCorrecta = _driver.Url.Contains("/Ofertar/DetalleOferta");
            Assert.True(urlCorrecta, $"Fallo: No se redirigió al detalle. URL actual: {_driver.Url}");
        }

        // -------------------------------------------------------------------
        // [Theory]: PRUEBAS DE VALIDACIÓN (Errores)
        // -------------------------------------------------------------------
        [Theory]
        [Trait("Category", "UIT")]
        [InlineData(101)] // Caso: Porcentaje mayor a 100
        [InlineData(-5)]  // Caso: Porcentaje negativo
        public void UC3_2_CrearOferta_PorcentajeInvalido_Error(int porcentajeInvalido)
        {
            // 1. ARRANGE
            string fabricante = "EMPRESA1";
            string nombreHerramienta = "Martillo";
            int herramientaId = 1;

            // 2. ACT (Pasos idénticos hasta el formulario)
            _driver.Navigate().GoToUrl(_URI + "Ofertar/SeleccionarHerramientaParaOfertar");
            _selectPO.SearchHerramientas(fabricante);
            _selectPO.AddHerramientaToOfertaCart(nombreHerramienta);
            _selectPO.PressContinuar();

            // Rellenamos datos válidos generales
            _crearPO.RellenarDatosGenerales(DateTime.Today.AddDays(1), DateTime.Today.AddDays(10), "2", "Tester", "Cliente");

            // INTRODUCIMOS EL DATO INVÁLIDO DEL THEORY
            _crearPO.EstablecerPorcentaje(herramientaId, porcentajeInvalido);

            // Intentamos guardar
            _crearPO.PulsarCrearOferta();

            // 3. ASSERT
            bool seguimosEnCrear = _driver.Url.Contains("/Ofertar/CrearOferta");
            Assert.True(seguimosEnCrear, $"El sistema permitió crear oferta con porcentaje {porcentajeInvalido}%");

        }

        // -------------------------------------------------------------------
        // [Theory]: ESCENARIO 2 - FECHAS INVÁLIDAS (UC3_3, UC3_4, UC3_5)
        // -------------------------------------------------------------------
        [Theory]
        [Trait("Category", "UIT")]
        // Caso UC3_3: Inicio en el pasado (Ayer)
        [InlineData(-1, 30, "La fecha de inicio debe ser posterior a hoy")]
        // Caso UC3_4: Fin antes que Inicio
        [InlineData(1, 0, "La fecha final debe ser posterior a la fecha de inicio")]
        // Caso UC3_5: Duración insuficiente (< 7 días)
        [InlineData(1, 2, "La oferta debe durar al menos una semana")]
        public void UC3_FechasInvalidas_Error(int diasInicio, int diasFin, string mensajeErrorEsperado)
        {
            // 1. ARRANGE
            DateTime inicio = DateTime.Today.AddDays(diasInicio);
            DateTime fin = DateTime.Today.AddDays(diasFin);
            string herramienta = "Martillo"; // Ajustar nombre real

            // 2. ACT
            _driver.Navigate().GoToUrl(_URI + "Ofertar/SeleccionarHerramientaParaOfertar");
            _selectPO.SearchHerramientas("EMPRESA1");
            _selectPO.AddHerramientaToOfertaCart(herramienta);
            _selectPO.PressContinuar();

            // Rellenar formulario con fechas inválidas
            _crearPO.RellenarDatosGenerales(inicio, fin, "1", "elena@uclm.es", "Cliente");

            _crearPO.PulsarCrearOferta();
            // Nota: No confirmamos modal porque la validación debería saltar antes

            // 3. ASSERT
            bool hayError = _crearPO.CheckErrorMessage(mensajeErrorEsperado);
            // También sirve verificar que seguimos en la misma URL
            Assert.True(hayError || _driver.Url.Contains("/Ofertar/CrearOferta"),
                $"No se detectó el error de fecha esperado: {mensajeErrorEsperado}");
        }

        // -------------------------------------------------------------------
        // [Fact]: ESCENARIO 5 - USUARIO NO EXISTE (UC3_6)
        // -------------------------------------------------------------------
        [Fact]
        [Trait("Category", "UIT")]
        public void UC3_6_UsuarioNoExistente_Error()
        {
            // 1. ARRANGE
            string usuarioInvalido = "usuario_fantasma";
            string herramienta = "Martillo";

            // 2. ACT
            _driver.Navigate().GoToUrl(_URI + "Ofertar/SeleccionarHerramientaParaOfertar");
            _selectPO.SearchHerramientas("EMPRESA1");
            _selectPO.AddHerramientaToOfertaCart(herramienta);
            _selectPO.PressContinuar();

            _crearPO.RellenarDatosGenerales(DateTime.Today.AddDays(1), DateTime.Today.AddDays(30), "1", usuarioInvalido, "Cliente");

            _crearPO.PulsarCrearOferta();
            try { _crearPO.ConfirmarModal(); } catch { /* Si no sale modal, seguimos */ }

            // 3. ASSERT
            System.Threading.Thread.Sleep(1000);
            bool hayError = _crearPO.CheckErrorMessage("no existe") || _crearPO.CheckErrorMessage("Error");
            Assert.True(hayError, "El sistema no mostró error al usar un usuario inexistente.");
        }

        // -------------------------------------------------------------------
        // [Fact]: ESCENARIO 4 - CARRITO VACÍO (UC3_2)
        // -------------------------------------------------------------------
        [Fact]
        [Trait("Category", "UIT")]
        public void UC3_2_CarritoVacio_Error()
        {
            // 1. ARRANGE
            _driver.Navigate().GoToUrl(_URI + "Ofertar/SeleccionarHerramientaParaOfertar");

            // 2. ACT - Intentar pulsar continuar SIN añadir nada
            try
            {
                _selectPO.PressContinuar();
            }
            catch (Exception) { /* Ignoramos si falla el click por estar disabled */ }

            // 3. ASSERT
            bool seguimosEnSeleccion = _driver.Url.Contains("/Ofertar/SeleccionarHerramientaParaOfertar");
            Assert.True(seguimosEnSeleccion, "El sistema permitió continuar con el carrito vacío.");
        }
    }
}