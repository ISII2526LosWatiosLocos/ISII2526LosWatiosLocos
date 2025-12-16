// Path: test/AppForSEII2526.UIT/CU_Reparacion/DetalleReparacion_PO.cs
using AppForSEII2526.UIT.Shared;
using OpenQA.Selenium;
using System.Collections.Generic;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.CU_Reparacion
{
    public class DetalleReparacion_PO : PageObject
    {
        // Asumiendo que la tabla de elementos en la vista de detalle tiene un ID para CheckBodyTable
        private readonly By _tableReparacionItems = By.Id("TableReparacion");

        public DetalleReparacion_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public bool CheckReparacionDetailTable(List<string[]> expectedItems)
        {
            // Reutiliza el método de la clase base para comprobar el contenido de la tabla
            return CheckBodyTable(expectedItems, _tableReparacionItems);
        }
    }
}