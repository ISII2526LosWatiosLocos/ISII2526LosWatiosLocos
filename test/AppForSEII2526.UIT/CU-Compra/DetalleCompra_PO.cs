using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using AppForSEII2526.UIT.Shared;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.CU_Compra
{
    public class DetalleCompra_PO : PageObject
    {
        public DetalleCompra_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }


        public bool CheckDetallesCompra(List<string[]> expectedDetails)
        {
            return CheckBodyTable(expectedDetails, By.Id("TableOfCompras"));
        }
    }
}