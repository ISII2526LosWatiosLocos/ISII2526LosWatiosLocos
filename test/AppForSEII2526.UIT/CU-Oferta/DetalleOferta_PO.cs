using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using AppForSEII2526.UIT.Shared;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.CU_Oferta
{
    public class DetalleOferta_PO : PageObject
    {
        public DetalleOferta_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }


        public bool CheckDetallesOferta(List<string[]> expectedDetails)
        {
            return CheckBodyTable(expectedDetails, By.Id("TableOfOfertas"));
        }
    }
}