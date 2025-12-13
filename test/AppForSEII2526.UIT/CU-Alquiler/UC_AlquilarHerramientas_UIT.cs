using AppForMovies.UIT.Shared;
using AppForSEII2526.UIT.CU_Oferta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.CU_Alquiler
{
    // PARA PROBAR LOS TESTS HAY QUE DAR CLICK DERECHO, VER, ABRIR CON EL NAVEGADOR A LA API Y A LA WEB

    public class UC_AlquilarHerramientas_UIT: UC_UIT
    {
        private const string herramientaNombre1 = "Martillo";
        private const string herramientaMaterial1 = "Madera";

        private readonly SelectHerramientasParaAlquilar_PO _selectPO;

        public UC_AlquilarHerramientas_UIT(ITestOutputHelper output) : base(output)
        {
            _selectPO = new SelectHerramientasParaAlquilar_PO(_driver, output);

        }
    }
}
