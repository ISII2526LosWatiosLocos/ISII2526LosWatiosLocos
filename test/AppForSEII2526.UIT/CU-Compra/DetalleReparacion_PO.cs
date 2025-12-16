using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using AppForSEII2526.UIT.Shared;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.CU_Compra
{
    public class DetalleReparacion_PO : PageObject
    {
        private readonly By tableCompras = By.Id("TableOfCompras");
        private readonly By itemsListLocator = By.CssSelector("#TableOfCompras td:last-child ul.list-group");

        public DetalleReparacion_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        // Verifica la tabla de detalles generales (Nombre, Apellidos, Dirección)
        public bool CheckDetallesCompra(List<string[]> expectedDetails)
        {
            return CheckBodyTable(expectedDetails, tableCompras);
        }

        public bool CheckItemsDetails(List<string[]> expectedItemDetails)
        {
            try
            {
                WaitForBeingVisible(itemsListLocator);

                IWebElement itemsListElement = _driver.FindElement(itemsListLocator);
                var actualItems = itemsListElement.FindElements(By.CssSelector("li"));

                // 1. Verificar la cantidad total de ítems
                if (actualItems.Count != expectedItemDetails.Count)
                {
                    _output.WriteLine($"ERROR: Se esperaban {expectedItemDetails.Count} ítems, pero se encontraron {actualItems.Count}.");
                    return false;
                }

                // 2. Verificar los detalles de cada ítem esperado
                foreach (var expectedDetail in expectedItemDetails)
                {
                    string expectedName = expectedDetail[0];
                    string expectedMaterial = expectedDetail[1];
                    string expectedPrecio = expectedDetail[2];
                    string expectedCantidad = expectedDetail[3];
                    string expectedDescripcion = expectedDetail[4];

                    // Texto esperado de la segunda línea de detalles, concatenado con '|'
                    string expectedDetailsTextPart = $"{expectedMaterial} | {expectedPrecio} | {expectedCantidad} | {expectedDescripcion}";

                    // Intentar encontrar el <li> que contiene el Nombre esperado
                    var itemElement = actualItems.FirstOrDefault(item =>
                        item.FindElements(By.XPath($".//div[contains(@class, 'fw-bold') and text()=normalize-space('{expectedName}')]")).Any());

                    if (itemElement == null)
                    {
                        _output.WriteLine($"ERROR: El ítem con nombre '{expectedName}' no fue encontrado en la lista de compras.");
                        return false;
                    }

                    // Obtener el bloque de detalles dentro de ese <li>
                    var detailsElement = itemElement.FindElement(By.CssSelector("div.small.text-muted"));

                    // Normalizar el texto (eliminar saltos de línea y espacios excesivos)
                    string actualDetailsText = detailsElement.Text.Replace("\r", "").Replace("\n", " ").Trim();

                    // Verificar si la parte clave del detalle está contenida en el texto real
                    if (!actualDetailsText.Contains(expectedDetailsTextPart))
                    {
                        _output.WriteLine($"ERROR: Los detalles del ítem '{expectedName}' no coinciden.");
                        _output.WriteLine($"  Esperado (parte clave): '{expectedDetailsTextPart}'");
                        _output.WriteLine($"  Encontrado (todo el bloque): '{actualDetailsText}'");
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                _output.WriteLine($"ERROR Inesperado al verificar ítems: {ex.Message}");
                return false;
            }
        }
    }
}