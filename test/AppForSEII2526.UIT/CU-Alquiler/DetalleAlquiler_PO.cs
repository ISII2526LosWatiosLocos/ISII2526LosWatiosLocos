using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.CU_Alquiler
{
    public class DetalleAlquiler_PO: PageObject
    {
        public DetalleAlquiler_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public bool CheckDetallesAlquiler(List<string[]> expectedDetails)
        {
            return CheckBodyTable(expectedDetails, By.Id("TableOfAlquileres"));
        }
    }
}
