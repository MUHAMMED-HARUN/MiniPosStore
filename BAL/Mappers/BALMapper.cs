using DAL.EF.DTO;
using DAL.EF.Models;
using DAL.Mapper;
using BAL.BALDTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAL.Mappers
{
    public static class BALMappers
    {
        // Customer Mappers
        public static CustomerBALDTO ToCustomerBALDTO(this clsCustomer customer)
        {
            var customerDTO = customer.ToCustomerDTO();
            return new CustomerBALDTO
            {
                CustomerID = customerDTO.CustomerID,
                PersonID = customerDTO.PersonID,
                FirstName = customerDTO.FirstName,
                LastName = customerDTO.LastName,
                PhoneNumber = customerDTO.PhoneNumber
            };
        }

        public static clsCustomer ToCustomerModel(this CustomerBALDTO customerBALDTO)
        {
            var customerDTO = new CustomerDTO
            {
                CustomerID = customerBALDTO.CustomerID,
                PersonID = customerBALDTO.PersonID,
                FirstName = customerBALDTO.FirstName,
                LastName = customerBALDTO.LastName,
                PhoneNumber = customerBALDTO.PhoneNumber
            };
            return customerDTO.ToCustomerModel();
        }

        // Order Mappers
        public static OrderBALDTO ToOrderBALDTO(this clsOrder order)
        {
            var orderDTO = order.ToOrderDTO();
            return new OrderBALDTO
            {
                CustomerID = orderDTO.CustomerID,
                PersonID = orderDTO.PersonID,
                FirstName = orderDTO.FirstName,
                LastName = orderDTO.LastName,
                PhoneNumber = orderDTO.PhoneNumber,
                ID = orderDTO.ID,
                OrderDate = orderDTO.OrderDate,
                TotalAmount = orderDTO.TotalAmount,
                PaidAmount = orderDTO.PaidAmount,
                PaymentStatus = orderDTO.PaymentStatus,
                ActionByUser = orderDTO.ActionByUser,
                ActionType = orderDTO.ActionType,
                ActionDate = orderDTO.ActionDate
            };
        }

        public static clsOrder ToOrderModel(this OrderBALDTO orderBALDTO)
        {
            var orderDTO = new OrderDTO
            {
                CustomerID = orderBALDTO.CustomerID,
                PersonID = orderBALDTO.PersonID,
                FirstName = orderBALDTO.FirstName,
                LastName = orderBALDTO.LastName,
                PhoneNumber = orderBALDTO.PhoneNumber,
                ID = orderBALDTO.ID,
                OrderDate = orderBALDTO.OrderDate,
                TotalAmount = orderBALDTO.TotalAmount,
                PaidAmount = orderBALDTO.PaidAmount,
                PaymentStatus = orderBALDTO.PaymentStatus,
                ActionByUser = orderBALDTO.ActionByUser,
                ActionType = orderBALDTO.ActionType,
                ActionDate = orderBALDTO.ActionDate
            };
            return orderDTO.ToOrderModel();
        }

        // OrderItem Mappers
        public static OrderItemsBALDTO FromDALToOrderItemsBALDTO(this OrderItemsDTO orderItemDTO)
        {
            if (orderItemDTO == null) return null;

            return new OrderItemsBALDTO
            {
                ID = orderItemDTO.ID,
                OrderID = orderItemDTO.OrderID,
                ProductID = orderItemDTO.ProductID,
                ProductName = orderItemDTO.ProductName,
                ProductSaleAmount = orderItemDTO.ProductSaleAmount,
                Quantity = orderItemDTO.Quantity,
                SellingPrice = orderItemDTO.SellingPrice,
                AvailableQuantity = orderItemDTO.AvailableQuantity
            };
        }
        public static OrderItemsBALDTO ToOrderItemsBALDTO(this clsOrderItem orderItem)
        {
            var orderItemDTO = orderItem.ToOrderItemsDTO();
            return new OrderItemsBALDTO
            {
                OrderID = orderItemDTO.OrderID,
                ID = orderItemDTO.ID,
                ProductID = orderItemDTO.ProductID,
                ProductName = orderItemDTO.ProductName,
                ProductSaleAmount = orderItemDTO.ProductSaleAmount,
                Quantity = orderItemDTO.Quantity,
                SellingPrice = orderItemDTO.SellingPrice,
                AvailableQuantity = orderItemDTO.AvailableQuantity
                ,PriceAdjustment=orderItem.PriceAdjustment
            };
        }

        public static clsOrderItem ToOrderItemModel(this OrderItemsBALDTO orderItemBALDTO)
        {
            var orderItemDTO = new OrderItemsDTO
            {
                OrderID = orderItemBALDTO.OrderID,
                ID = orderItemBALDTO.ID,
                ProductID = orderItemBALDTO.ProductID,
                ProductName = orderItemBALDTO.ProductName,
                ProductSaleAmount = orderItemBALDTO.ProductSaleAmount,
                Quantity = orderItemBALDTO.Quantity,
                SellingPrice = orderItemBALDTO.SellingPrice,
                PriceAdjustment = orderItemBALDTO.PriceAdjustment,
                AvailableQuantity = orderItemBALDTO.AvailableQuantity

            };
            return orderItemDTO.ToOrderItemModel();
        }

        // Product Mappers
        public static ProductBALDTO ToProductBALDTO(this clsProduct product)
        {
            var productDTO = product.ToProductDTO();
            return new ProductBALDTO
            {
                ID = productDTO.ID,
                Name = productDTO.Name,
                Description = productDTO.Description,
                RetailPrice = productDTO.RetailPrice,
                WholesalePrice = productDTO.WholesalePrice,
                AvailableQuantity = productDTO.AvailableQuantity,
                CurrencyType = productDTO.CurrencyType,
                CurrencyName= productDTO.CurrencyName,
                ImagePath = productDTO.ImagePath,
                UOMID = productDTO.UOMID,
                UOMName = productDTO.UOMName,
                UOMSymbol = productDTO.UOMSymbol,
                ActionByUser = productDTO.ActionByUser,
                ActionType = productDTO.ActionType,
                ActionDate = productDTO.ActionDate
            };
        }

        public static clsProduct ToProductModel(this ProductBALDTO productBALDTO)
        {
            var productDTO = new ProductDTO
            {
                ID = productBALDTO.ID,
                Name = productBALDTO.Name,
                Description = productBALDTO.Description,
                RetailPrice = productBALDTO.RetailPrice,
                WholesalePrice = productBALDTO.WholesalePrice,
                AvailableQuantity = productBALDTO.AvailableQuantity,
                CurrencyType = productBALDTO.CurrencyType,
                CurrencyName = productBALDTO.CurrencyName,
                ImagePath = productBALDTO.ImagePath,
                UOMID = productBALDTO.UOMID,
                UOMName = productBALDTO.UOMName,
                UOMSymbol = productBALDTO.UOMSymbol,
                ActionByUser = productBALDTO.ActionByUser,
                ActionType = productBALDTO.ActionType,
                ActionDate = productBALDTO.ActionDate
            };
            return productDTO.ToProductModel();
        }

        // ImportOrder Mappers
        public static ImportOrderBALDTO ToImportOrderBALDTO(this clsImportOrder importOrder)
        {
            var importOrderDTO = importOrder.ToImportOrderDTO();
            return new ImportOrderBALDTO
            {
                ImportOrderID = importOrderDTO.ImportOrderID,
                SupplierID = importOrderDTO.SupplierID,
                SupplierName = importOrderDTO.SupplierName,
                SupplierPhone = importOrderDTO.SupplierPhone,
                SupplierAddress = importOrderDTO.SupplierAddress,
                TotalAmount = importOrderDTO.TotalAmount,
                PaidAmount = importOrderDTO.PaidAmount,
                ImportDate = importOrderDTO.ImportDate,
                PaymentStatus = importOrderDTO.PaymentStatus,
                PaymentStatusText = importOrderDTO.PaymentStatusText,
                ActionByUser = importOrderDTO.ActionByUser,
                UserName = importOrderDTO.UserName,
                ActionType = importOrderDTO.ActionType,
                ActionDate = importOrderDTO.ActionDate,
                ItemsCount = importOrderDTO.ItemsCount,
                ImportOrderItems = importOrderDTO.ImportOrderItems?.Select(item => new ImportOrderItemDTO
                {
                    ImportOrderID = item.ImportOrderID,
                    ImportOrderItemID = item.ImportOrderItemID,
                    ProductID = item.ProductID,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    SellingPrice = item.SellingPrice,
                    TotalItemAmount = item.TotalItemAmount,
                    CurrencyType = item.CurrencyType,
                    CurrencyName = item.CurrencyName,
                    UOMName = item.UOMName,
                    UOMSymbol = item.UOMSymbol,
                    ImportedQuantity = item.ImportedQuantity
                }).ToList()
            };
        }

        public static clsImportOrder ToImportOrderModel(this ImportOrderBALDTO importOrderBALDTO)
        {
            var importOrderDTO = new ImportOrderDTO
            {
                ImportOrderID = importOrderBALDTO.ImportOrderID,
                SupplierID = importOrderBALDTO.SupplierID,
                SupplierName = importOrderBALDTO.SupplierName,
                SupplierPhone = importOrderBALDTO.SupplierPhone,
                SupplierAddress = importOrderBALDTO.SupplierAddress,
                TotalAmount = importOrderBALDTO.TotalAmount,
                PaidAmount = importOrderBALDTO.PaidAmount,
                ImportDate = importOrderBALDTO.ImportDate,
                PaymentStatus = importOrderBALDTO.PaymentStatus,
                PaymentStatusText = importOrderBALDTO.PaymentStatusText,
                ActionByUser = importOrderBALDTO.ActionByUser,
                UserName = importOrderBALDTO.UserName,
                ActionType = importOrderBALDTO.ActionType,
                ActionDate = importOrderBALDTO.ActionDate,
                ItemsCount = importOrderBALDTO.ItemsCount,
                ImportOrderItems = importOrderBALDTO.ImportOrderItems?.Select(item => new ImportOrderItemDTO
                {
                    ImportOrderID = item.ImportOrderID,
                    ImportOrderItemID = item.ImportOrderItemID,
                    ProductID = item.ProductID,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    SellingPrice = item.SellingPrice,
                    TotalItemAmount = item.TotalItemAmount,
                    CurrencyType = item.CurrencyType,
                    CurrencyName = item.CurrencyName,
                    UOMName = item.UOMName,
                    UOMSymbol = item.UOMSymbol,
                    ImportedQuantity = item.ImportedQuantity
                }).ToList() ?? new List<ImportOrderItemDTO>()
            };
            return importOrderDTO.ToImportOrderModel();
        }

        // ImportOrderItem Mappers
        public static ImportOrderItemBALDTO ToImportOrderItemBALDTO(this clsImportOrderItem importOrderItem)
        {
            var importOrderItemDTO = importOrderItem.ToImportOrderItemDTO();
            return new ImportOrderItemBALDTO
            {
                ImportOrderID = importOrderItemDTO.ImportOrderID,
                ImportOrderItemID = importOrderItemDTO.ImportOrderItemID,
                ProductID = importOrderItemDTO.ProductID,
                ProductName = importOrderItemDTO.ProductName,
                Quantity = importOrderItemDTO.Quantity,
                SellingPrice = importOrderItemDTO.SellingPrice,
                TotalItemAmount = importOrderItemDTO.TotalItemAmount,
                CurrencyType = importOrderItemDTO.CurrencyType,
                CurrencyName = importOrderItemDTO.CurrencyName,
                UOMName = importOrderItemDTO.UOMName,
                UOMSymbol = importOrderItemDTO.UOMSymbol,
                ImportedQuantity = importOrderItemDTO.ImportedQuantity
            };
        }

        public static clsImportOrderItem ToImportOrderItemModel(this ImportOrderItemBALDTO importOrderItemBALDTO)
        {
            var importOrderItemDTO = new ImportOrderItemDTO
            {
                ImportOrderID = importOrderItemBALDTO.ImportOrderID,
                ImportOrderItemID = importOrderItemBALDTO.ImportOrderItemID,
                ProductID = importOrderItemBALDTO.ProductID,
                ProductName = importOrderItemBALDTO.ProductName,
                Quantity = importOrderItemBALDTO.Quantity,
                SellingPrice = importOrderItemBALDTO.SellingPrice,
                TotalItemAmount = importOrderItemBALDTO.TotalItemAmount,
                CurrencyType = importOrderItemBALDTO.CurrencyType,
                CurrencyName = importOrderItemBALDTO.CurrencyName,
                UOMName = importOrderItemBALDTO.UOMName,
                UOMSymbol = importOrderItemBALDTO.UOMSymbol,
                ImportedQuantity = importOrderItemBALDTO.ImportedQuantity
            };
            return importOrderItemDTO.ToImportOrderItemModel();
        }

        // Supplier Mappers
        public static SupplierBALDTO ToSupplierBALDTO(this clsSupplier supplier)
        {
            var supplierDTO = supplier.ToSupplierDTO();
            return new SupplierBALDTO
            {
                SupplierID = supplierDTO.SupplierID,
                PersonID = supplierDTO.PersonID,
                ShopName = supplierDTO.ShopName,
                Address = supplierDTO.Address,
                FirstName = supplierDTO.FirstName,
                LastName = supplierDTO.LastName,
                PhoneNumber = supplierDTO.PhoneNumber
            };
        }

        public static clsSupplier ToSupplierModel(this SupplierBALDTO supplierBALDTO)
        {
            var supplierDTO = new SupplierDTO
            {
                SupplierID = supplierBALDTO.SupplierID,
                PersonID = supplierBALDTO.PersonID,
                ShopName = supplierBALDTO.ShopName,
                Address = supplierBALDTO.Address,
                FirstName = supplierBALDTO.FirstName,
                LastName = supplierBALDTO.LastName,
                PhoneNumber = supplierBALDTO.PhoneNumber
            };
            return supplierDTO.ToSupplierModel();
        }

        // List Mappers
        public static List<CustomerBALDTO> ToCustomerBALDTOList(this IEnumerable<clsCustomer> customers)
        {
            return customers?.Select(c => c.ToCustomerBALDTO()).ToList() ?? new List<CustomerBALDTO>();
        }

        public static List<clsCustomer> ToCustomerModelList(this IEnumerable<CustomerBALDTO> customerBALDTOs)
        {
            return customerBALDTOs?.Select(c => c.ToCustomerModel()).ToList() ?? new List<clsCustomer>();
        }

        public static List<OrderBALDTO> ToOrderBALDTOList(this IEnumerable<clsOrder> orders)
        {
            return orders?.Select(o => o.ToOrderBALDTO()).ToList() ?? new List<OrderBALDTO>();
        }

        public static List<clsOrder> ToOrderModelList(this IEnumerable<OrderBALDTO> orderBALDTOs)
        {
            return orderBALDTOs?.Select(o => o.ToOrderModel()).ToList() ?? new List<clsOrder>();
        }

        public static List<OrderItemsBALDTO> ToOrderItemsBALDTOList(this IEnumerable<clsOrderItem> orderItems)
        {
            return orderItems?.Select(oi => oi.ToOrderItemsBALDTO()).ToList() ?? new List<OrderItemsBALDTO>();
        }

        public static List<clsOrderItem> ToOrderItemModelList(this IEnumerable<OrderItemsBALDTO> orderItemBALDTOs)
        {
            return orderItemBALDTOs?.Select(oi => oi.ToOrderItemModel()).ToList() ?? new List<clsOrderItem>();
        }

        public static List<ProductBALDTO> ToProductBALDTOList(this IEnumerable<clsProduct> products)
        {
            return products?.Select(p => p.ToProductBALDTO()).ToList() ?? new List<ProductBALDTO>();
        }

        public static List<clsProduct> ToProductModelList(this IEnumerable<ProductBALDTO> productBALDTOs)
        {
            return productBALDTOs?.Select(p => p.ToProductModel()).ToList() ?? new List<clsProduct>();
        }

        public static List<ImportOrderBALDTO> ToImportOrderBALDTOList(this IEnumerable<clsImportOrder> importOrders)
        {
            return importOrders?.Select(io => io.ToImportOrderBALDTO()).ToList() ?? new List<ImportOrderBALDTO>();
        }

        public static List<clsImportOrder> ToImportOrderModelList(this IEnumerable<ImportOrderBALDTO> importOrderBALDTOs)
        {
            return importOrderBALDTOs?.Select(io => io.ToImportOrderModel()).ToList() ?? new List<clsImportOrder>();
        }

        public static List<ImportOrderItemBALDTO> ToImportOrderItemBALDTOList(this IEnumerable<clsImportOrderItem> importOrderItems)
        {
            return importOrderItems?.Select(ioi => ioi.ToImportOrderItemBALDTO()).ToList() ?? new List<ImportOrderItemBALDTO>();
        }

        public static List<clsImportOrderItem> ToImportOrderItemModelList(this IEnumerable<ImportOrderItemBALDTO> importOrderItemBALDTOs)
        {
            return importOrderItemBALDTOs?.Select(ioi => ioi.ToImportOrderItemModel()).ToList() ?? new List<clsImportOrderItem>();
        }

        public static List<SupplierBALDTO> ToSupplierBALDTOList(this IEnumerable<clsSupplier> suppliers)
        {
            return suppliers?.Select(s => s.ToSupplierBALDTO()).ToList() ?? new List<SupplierBALDTO>();
        }

        public static List<clsSupplier> ToSupplierModelList(this IEnumerable<SupplierBALDTO> supplierBALDTOs)
        {
            return supplierBALDTOs?.Select(s => s.ToSupplierModel()).ToList() ?? new List<clsSupplier>();
        }

        // Additional ImportOrder BAL Mappers for different scenarios
        public static ImportOrderBALDTO ToImportOrderSummaryBALDTO(this clsImportOrder importOrder)
        {
            var importOrderDTO = importOrder.ToImportOrderSummaryDTO();
            return new ImportOrderBALDTO
            {
                ImportOrderID = importOrderDTO.ImportOrderID,
                SupplierID = importOrderDTO.SupplierID,
                SupplierName = importOrderDTO.SupplierName,
                SupplierPhone = importOrderDTO.SupplierPhone,
                TotalAmount = importOrderDTO.TotalAmount,
                PaidAmount = importOrderDTO.PaidAmount,
                ImportDate = importOrderDTO.ImportDate,
                PaymentStatus = importOrderDTO.PaymentStatus,
                PaymentStatusText = importOrderDTO.PaymentStatusText,
                ActionByUser = importOrderDTO.ActionByUser,
                UserName = importOrderDTO.UserName,
                ActionType = importOrderDTO.ActionType,
                ActionDate = importOrderDTO.ActionDate,
                ItemsCount = importOrderDTO.ItemsCount
            };
        }

        public static ImportOrderItemBALDTO ToImportOrderItemSummaryBALDTO(this clsImportOrderItem importOrderItem)
        {
            var importOrderItemDTO = importOrderItem.ToImportOrderItemSummaryDTO();
            return new ImportOrderItemBALDTO
            {
                ImportOrderID = importOrderItemDTO.ImportOrderID,
                ImportOrderItemID = importOrderItemDTO.ImportOrderItemID,
                ProductID = importOrderItemDTO.ProductID,
                ProductName = importOrderItemDTO.ProductName,
                Quantity = importOrderItemDTO.Quantity,
                SellingPrice = importOrderItemDTO.SellingPrice,
                TotalItemAmount = importOrderItemDTO.TotalItemAmount,
                CurrencyType = importOrderItemDTO.CurrencyType,
                CurrencyName = importOrderItemDTO.CurrencyName,
                UOMName = importOrderItemDTO.UOMName,
                UOMSymbol = importOrderItemDTO.UOMSymbol,
                ImportedQuantity = importOrderItemDTO.ImportedQuantity
            };
        }

        // List Mappers for Summary DTOs
        public static List<ImportOrderBALDTO> ToImportOrderSummaryBALDTOList(this IEnumerable<clsImportOrder> importOrders)
        {
            return importOrders?.Select(io => io.ToImportOrderSummaryBALDTO()).ToList() ?? new List<ImportOrderBALDTO>();
        }

        public static List<ImportOrderItemBALDTO> ToImportOrderItemSummaryBALDTOList(this IEnumerable<clsImportOrderItem> importOrderItems)
        {
            return importOrderItems?.Select(ioi => ioi.ToImportOrderItemSummaryBALDTO()).ToList() ?? new List<ImportOrderItemBALDTO>();
        }

        // ImportOrderItemDTO to ImportOrderItemBALDTO Mappers
        public static ImportOrderItemBALDTO ToImportOrderItemBALDTO(this ImportOrderItemDTO importOrderItemDTO)
        {
            return new ImportOrderItemBALDTO
            {
                ImportOrderID = importOrderItemDTO.ImportOrderID,
                ImportOrderItemID = importOrderItemDTO.ImportOrderItemID,
                ProductID = importOrderItemDTO.ProductID,
                ProductName = importOrderItemDTO.ProductName,
                Quantity = importOrderItemDTO.Quantity,
                SellingPrice = importOrderItemDTO.SellingPrice,
                TotalItemAmount = importOrderItemDTO.TotalItemAmount,
                CurrencyType = importOrderItemDTO.CurrencyType,
                CurrencyName = importOrderItemDTO.CurrencyName,
                UOMName = importOrderItemDTO.UOMName,
                UOMSymbol = importOrderItemDTO.UOMSymbol,
                ImportedQuantity = importOrderItemDTO.ImportedQuantity
            };
        }

        public static ImportOrderItemDTO ToImportOrderItemDTO(this ImportOrderItemBALDTO importOrderItemBALDTO)
        {
            return new ImportOrderItemDTO
            {
                ImportOrderID = importOrderItemBALDTO.ImportOrderID,
                ImportOrderItemID = importOrderItemBALDTO.ImportOrderItemID,
                ProductID = importOrderItemBALDTO.ProductID,
                ProductName = importOrderItemBALDTO.ProductName,
                Quantity = importOrderItemBALDTO.Quantity,
                SellingPrice = importOrderItemBALDTO.SellingPrice,
                TotalItemAmount = importOrderItemBALDTO.TotalItemAmount,
                CurrencyType = importOrderItemBALDTO.CurrencyType,
                CurrencyName = importOrderItemBALDTO.CurrencyName,
                UOMName = importOrderItemBALDTO.UOMName,
                UOMSymbol = importOrderItemBALDTO.UOMSymbol,
                ImportedQuantity = importOrderItemBALDTO.ImportedQuantity
            };
        }

        // List Mappers for ImportOrderItemDTO
        public static List<ImportOrderItemBALDTO> ToImportOrderItemBALDTOList(this IEnumerable<ImportOrderItemDTO> importOrderItemDTOs)
        {
            return importOrderItemDTOs?.Select(ioi => ioi.ToImportOrderItemBALDTO()).ToList() ?? new List<ImportOrderItemBALDTO>();
        }

        public static List<ImportOrderItemDTO> ToImportOrderItemDTOList(this IEnumerable<ImportOrderItemBALDTO> importOrderItemBALDTOs)
        {
            return importOrderItemBALDTOs?.Select(ioi => ioi.ToImportOrderItemDTO()).ToList() ?? new List<ImportOrderItemDTO>();
        }

        // ImportOrderDTO to ImportOrderBALDTO Mappers
        public static ImportOrderBALDTO ToImportOrderBALDTO(this ImportOrderDTO importOrderDTO)
        {
            return new ImportOrderBALDTO
            {
                ImportOrderID = importOrderDTO.ImportOrderID,
                SupplierID = importOrderDTO.SupplierID,
                SupplierName = importOrderDTO.SupplierName,
                SupplierPhone = importOrderDTO.SupplierPhone,
                SupplierAddress = importOrderDTO.SupplierAddress,
                TotalAmount = importOrderDTO.TotalAmount,
                PaidAmount = importOrderDTO.PaidAmount,
                ImportDate = importOrderDTO.ImportDate,
                PaymentStatus = importOrderDTO.PaymentStatus,
                PaymentStatusText = importOrderDTO.PaymentStatusText,
                ActionByUser = importOrderDTO.ActionByUser,
                UserName = importOrderDTO.UserName,
                ActionType = importOrderDTO.ActionType,
                ActionDate = importOrderDTO.ActionDate,
                ItemsCount = importOrderDTO.ItemsCount,
                ImportOrderItems = new List<ImportOrderItemDTO>()
            };
        }

        // List Mappers for ImportOrderDTO
        public static List<ImportOrderBALDTO> ToImportOrderBALDTOList(this IEnumerable<ImportOrderDTO> importOrderDTOs)
        {
            return importOrderDTOs?.Select(io => io.ToImportOrderBALDTO()).ToList() ?? new List<ImportOrderBALDTO>();
        }
    }
}

