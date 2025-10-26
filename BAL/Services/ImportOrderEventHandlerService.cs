using BAL.Interfaces;
using BAL.Events.ImportOrderEvents;
using SharedModels.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BAL.Services
{
    /// <summary>
    /// Service responsible for handling ImportOrder events and coordinating inventory updates
    /// </summary>
    public class ImportOrderEventHandlerService
    {
        private readonly IProductService _productService;
        private readonly IRawMaterialService _rawMaterialService;
        private readonly IImportOrderService _importOrderService;

        public ImportOrderEventHandlerService(
            IProductService productService,
            IRawMaterialService rawMaterialService,
            IImportOrderService importOrderService)
        {
            _productService = productService;
            _rawMaterialService = rawMaterialService;
            _importOrderService = importOrderService;

            // Subscribe to ImportOrder events
            _importOrderService.ImportOrderConfirmedEvent += OnImportOrderConfirmed;
        }

        private async Task OnImportOrderConfirmed(object sender, ImportOrderConfirmedEventArgs e)
        {
            try
            {
                // Use union items from the event args
                var unionItems = e.Items;

                // Handle product items (ItemType = 1)
                var productItems = unionItems.Where(item => item.ItemType == 1).ToList();
                if (productItems.Any())
                {
                    await _productService.HandleImportOrderConfirmed(e.ImportOrder.ID, productItems);
                }

                // Handle raw material items (ItemType = 2)
                var rawMaterialItems = unionItems.Where(item => item.ItemType == 2).ToList();
                if (rawMaterialItems.Any())
                {
                    await _rawMaterialService.HandleImportOrderConfirmed(e.ImportOrder.ID, rawMaterialItems);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ImportOrderEventHandlerService.OnImportOrderConfirmed Error: {ex.Message}");
            }
        }
    }
}
