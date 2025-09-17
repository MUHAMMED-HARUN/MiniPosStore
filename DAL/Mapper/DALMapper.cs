using SharedModels.EF.Models;
using SharedModels.EF.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Mapper
{
    public static class DTOMapper
    {
        // Customer Mappers
        public static CustomerDTO ToCustomerDTO(this clsCustomer customer)
        {
            if (customer == null) return null;
            
            return new CustomerDTO
            {
                CustomerID = customer.ID,
                PersonID = customer.PersonID,
                FirstName = customer.Person?.FirstName ?? "",
                LastName = customer.Person?.LastName ?? "",
                PhoneNumber = customer.Person?.PhoneNumber ?? ""
            };
        }

        public static clsCustomer ToCustomerModel(this CustomerDTO customerDTO)
        {
            if (customerDTO == null) return null;
            
            return new clsCustomer
            {
                ID = customerDTO.CustomerID,
                PersonID = customerDTO.PersonID,

            };
        }

        // Order Mappers
        public static OrderDTO ToOrderDTO(this clsOrder order)
        {
            if (order == null) return null;
            
            return new OrderDTO
            {
                CustomerID = order.CustomerID,
                PersonID = order.Customer?.PersonID ?? 0,
                FirstName = order.Customer?.Person?.FirstName ?? "",
                LastName = order.Customer?.Person?.LastName ?? "",
                PhoneNumber = order.Customer?.Person?.PhoneNumber ?? "",
                ID = order.ID,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                PaidAmount = order.PaidAmount,
                PaymentStatus = order.PaymentStatus,
                ActionByUser = order.ActionByUser,
                ActionType = order.ActionType,
                ActionDate = order.ActionDate
            };
        }

        public static clsOrder ToOrderModel(this OrderDTO orderDTO)
        {
            if (orderDTO == null) return null;
            
            return new clsOrder
            {
                ID = orderDTO.ID,
                CustomerID = orderDTO.CustomerID,
                OrderDate = orderDTO.OrderDate,
                TotalAmount = orderDTO.TotalAmount,
                PaidAmount = orderDTO.PaidAmount,
                PaymentStatus = orderDTO.PaymentStatus,
                ActionByUser = orderDTO.ActionByUser,
                ActionType = orderDTO.ActionType,
                ActionDate = orderDTO.ActionDate,

            };
        }

        // OrderItem Mappers
        public static OrderItemsDTO ToOrderItemsDTO(this clsOrderItem orderItem)
        {
            if (orderItem == null) return null;
            
            return new OrderItemsDTO
            {
                OrderID = orderItem.OrderID,
                ID = orderItem.ID,
                ProductID = orderItem.ProductID,
                ProductName = orderItem.Product?.Name ?? "",
                ProductSaleAmount = orderItem.Quantity * orderItem.SellingPrice,
                Quantity = orderItem.Quantity,
                SellingPrice = orderItem.SellingPrice,
                PriceAdjustment=orderItem.PriceAdjustment
            };
        }

        public static clsOrderItem ToOrderItemModel(this OrderItemsDTO orderItemDTO)
        {
            if (orderItemDTO == null) return null;
            
            return new clsOrderItem
            {
                ID = orderItemDTO.ID,
                OrderID = orderItemDTO.OrderID,
                ProductID = orderItemDTO.ProductID,
                Quantity = orderItemDTO.Quantity,
                SellingPrice = orderItemDTO.SellingPrice,
                PriceAdjustment = orderItemDTO.PriceAdjustment,

            };
        }

        // Product Mappers
        public static ProductDTO ToProductDTO(this clsProduct product)
        {
            if (product == null) return null;
            
            return new ProductDTO
            {
                ID = product.ID,
                Name = product.Name,
                Description = product.Description,
                RetailPrice = product.RetailPrice,
                WholesalePrice = product.WholesalePrice,
                AvailableQuantity = product.AvailableQuantity,
                CurrencyType = product.CurrencyType,
                ImagePath = product.ImagePath,
                UOMID = product.UOMID,
                UOMName = product.UnitOfMeasure?.Name ?? "",
                UOMSymbol = product.UnitOfMeasure?.Seymbol ?? "",
                ActionByUser = product.ActionByUser,
                ActionType = product.ActionType,
                ActionDate = product.ActionDate
            };
        }

        public static clsProduct ToProductModel(this ProductDTO productDTO)
        {
            if (productDTO == null) return null;
            
            return new clsProduct
            {
                ID = productDTO.ID,
                Name = productDTO.Name,
                Description = productDTO.Description,
                RetailPrice = productDTO.RetailPrice,
                WholesalePrice = productDTO.WholesalePrice,
                AvailableQuantity = productDTO.AvailableQuantity,
                CurrencyType = productDTO.CurrencyType,
                ImagePath = productDTO.ImagePath,
                UOMID = productDTO.UOMID,
                ActionByUser = productDTO.ActionByUser,
                ActionType = productDTO.ActionType,
                ActionDate = productDTO.ActionDate,

            };
        }

        // ImportOrder Mappers
        public static ImportOrderDTO ToImportOrderDTO(this clsImportOrder importOrder)
        {
            if (importOrder == null) return null;
            
            return new ImportOrderDTO
            {
                ImportOrderID = importOrder.ID,
                SupplierID = importOrder.SupplierID,
                SupplierName = importOrder.Supplier?.StoreName ?? "",
                SupplierPhone = importOrder.Supplier?.Person?.PhoneNumber ?? "",
                SupplierAddress = importOrder.Supplier?.StoreAddress ?? "",
                TotalAmount = importOrder.TotalAmount,
                PaidAmount = importOrder.PaidAmount,
                ImportDate = importOrder.ImportDate,
                PaymentStatus = importOrder.PaymentStatus,
                PaymentStatusText = GetPaymentStatusText(importOrder.PaymentStatus),
                ActionByUser = importOrder.ActionByUser,
                UserName = importOrder.User?.UserName ?? "",
                ActionType = importOrder.ActionType,
                ActionDate = importOrder.ActionDate,
                ItemsCount = importOrder.ImportOrderItems?.Count ?? 0,
                ImportOrderItems = importOrder.ImportOrderItems?.Select(item => item.ToImportOrderItemDTO()).ToList() ?? new List<ImportOrderItemDTO>()
            };
        }

        public static clsImportOrder ToImportOrderModel(this ImportOrderDTO importOrderDTO)
        {
            if (importOrderDTO == null) return null;
            
            return new clsImportOrder
            {
                ID = importOrderDTO.ImportOrderID,
                SupplierID = importOrderDTO.SupplierID,
                TotalAmount = importOrderDTO.TotalAmount,
                PaidAmount = importOrderDTO.PaidAmount,
                ImportDate = importOrderDTO.ImportDate,
                PaymentStatus = importOrderDTO.PaymentStatus,
                ActionByUser = importOrderDTO.ActionByUser,
                ActionType = importOrderDTO.ActionType,
                ActionDate = importOrderDTO.ActionDate,
                ImportOrderItems = importOrderDTO.ImportOrderItems?.Select(item => item.ToImportOrderItemModel()).ToList()
            };
        }

        // ImportOrderItem Mappers
        public static ImportOrderItemDTO ToImportOrderItemDTO(this clsImportOrderItem importOrderItem)
        {
            if (importOrderItem == null) return null;
            
            return new ImportOrderItemDTO
            {
                ImportOrderID = importOrderItem.ImportOrderID,
                ImportOrderItemID = importOrderItem.ID,
                ProductID = importOrderItem.ProductID,
                ProductName = importOrderItem.Product?.Name ?? "",
                Quantity = importOrderItem.Quantity,
                SellingPrice = importOrderItem.SellingPrice,
                TotalItemAmount = importOrderItem.Quantity * importOrderItem.SellingPrice,
                CurrencyType = importOrderItem.Product?.CurrencyType ?? "",
                CurrencyName = GetCurrencyName(importOrderItem.Product?.CurrencyType ?? ""),
                UOMName = importOrderItem.Product?.UnitOfMeasure?.Name ?? "",
                UOMSymbol = importOrderItem.Product?.UnitOfMeasure?.Seymbol ?? "",
                ImportedQuantity = importOrderItem.Quantity
            };
        }

        public static clsImportOrderItem ToImportOrderItemModel(this ImportOrderItemDTO importOrderItemDTO)
        {
            if (importOrderItemDTO == null) return null;
            
            return new clsImportOrderItem
            {
                ID = importOrderItemDTO.ImportOrderItemID,
                ImportOrderID = importOrderItemDTO.ImportOrderID,
                ProductID = importOrderItemDTO.ProductID,
                Quantity = importOrderItemDTO.Quantity,
                SellingPrice = importOrderItemDTO.SellingPrice
            };
        }

        // Person Mappers
        public static clsPerson ToPersonModel(this CustomerDTO customerDTO)
        {
            if (customerDTO == null) return null;
            
            return new clsPerson
            {
                ID = customerDTO.PersonID,
                FirstName = customerDTO.FirstName,
                LastName = customerDTO.LastName,
                PhoneNumber = customerDTO.PhoneNumber
            };
        }

        // Supplier Mappers
        public static SupplierDTO ToSupplierDTO(this clsSupplier supplier)
        {
            if (supplier == null) return null;
            
            return new SupplierDTO
            {
                SupplierID = supplier.ID,
                PersonID = supplier.PersonID,
                ShopName = supplier.StoreName,
                Address = supplier.StoreAddress ,
                FirstName = supplier.Person.FirstName ,
                LastName = supplier.Person.LastName,
                PhoneNumber = supplier.Person?.PhoneNumber
            };
        }

        public static clsSupplier ToSupplierModel(this SupplierDTO supplierDTO)
        {
            if (supplierDTO == null) return null;
            
            return new clsSupplier
            {
                ID = supplierDTO.SupplierID,
                PersonID = supplierDTO.PersonID,
                StoreName = supplierDTO.ShopName,
                StoreAddress = supplierDTO.Address
            };
        }

        public static clsSupplier ToSupplierModel(this ImportOrderDTO importOrderDTO)
        {
            if (importOrderDTO == null) return null;
            
            return new clsSupplier
            {
                ID = importOrderDTO.SupplierID,
                StoreName = importOrderDTO.SupplierName
            };
        }

        // List Mappers
        public static List<CustomerDTO> ToCustomerDTOList(this IEnumerable<clsCustomer> customers)
        {
            return customers?.Select(c => c.ToCustomerDTO()).ToList() ?? new List<CustomerDTO>();
        }

        public static List<clsCustomer> ToCustomerModelList(this IEnumerable<CustomerDTO> customerDTOs)
        {
            return customerDTOs?.Select(c => c.ToCustomerModel()).ToList() ?? new List<clsCustomer>();
        }

        public static List<OrderDTO> ToOrderDTOList(this IEnumerable<clsOrder> orders)
        {
            return orders?.Select(o => o.ToOrderDTO()).ToList() ?? new List<OrderDTO>();
        }

        public static List<clsOrder> ToOrderModelList(this IEnumerable<OrderDTO> orderDTOs)
        {
            return orderDTOs?.Select(o => o.ToOrderModel()).ToList() ?? new List<clsOrder>();
        }

        public static List<OrderItemsDTO> ToOrderItemsDTOList(this IEnumerable<clsOrderItem> orderItems)
        {
            return orderItems?.Select(oi => oi.ToOrderItemsDTO()).ToList() ?? new List<OrderItemsDTO>();
        }

        public static List<clsOrderItem> ToOrderItemModelList(this IEnumerable<OrderItemsDTO> orderItemDTOs)
        {
            return orderItemDTOs?.Select(oi => oi.ToOrderItemModel()).ToList() ?? new List<clsOrderItem>();
        }

        public static List<ProductDTO> ToProductDTOList(this IEnumerable<clsProduct> products)
        {
            return products?.Select(p => p.ToProductDTO()).ToList() ?? new List<ProductDTO>();
        }

        public static List<clsProduct> ToProductModelList(this IEnumerable<ProductDTO> productDTOs)
        {
            return productDTOs?.Select(p => p.ToProductModel()).ToList() ?? new List<clsProduct>();
        }

        public static List<ImportOrderDTO> ToImportOrderDTOList(this IEnumerable<clsImportOrder> importOrders)
        {
            return importOrders?.Select(io => io.ToImportOrderDTO()).ToList() ?? new List<ImportOrderDTO>();
        }

        public static List<ImportOrderDTO> ToImportOrderSummaryDTOList(this IEnumerable<clsImportOrder> importOrders)
        {
            return importOrders?.Select(io => io.ToImportOrderSummaryDTO()).ToList() ?? new List<ImportOrderDTO>();
        }

        public static List<clsImportOrder> ToImportOrderModelList(this IEnumerable<ImportOrderDTO> importOrderDTOs)
        {
            return importOrderDTOs?.Select(io => io.ToImportOrderModel()).ToList() ?? new List<clsImportOrder>();
        }

        public static List<ImportOrderItemDTO> ToImportOrderItemDTOList(this IEnumerable<clsImportOrderItem> importOrderItems)
        {
            return importOrderItems?.Select(ioi => ioi.ToImportOrderItemDTO()).ToList() ?? new List<ImportOrderItemDTO>();
        }

        public static List<ImportOrderItemDTO> ToImportOrderItemSummaryDTOList(this IEnumerable<clsImportOrderItem> importOrderItems)
        {
            return importOrderItems?.Select(ioi => ioi.ToImportOrderItemSummaryDTO()).ToList() ?? new List<ImportOrderItemDTO>();
        }

        public static List<clsImportOrderItem> ToImportOrderItemModelList(this IEnumerable<ImportOrderItemDTO> importOrderItemDTOs)
        {
            return importOrderItemDTOs?.Select(ioi => ioi.ToImportOrderItemModel()).ToList() ?? new List<clsImportOrderItem>();
        }

        // Additional ImportOrder Mappers for different scenarios
        public static ImportOrderDTO ToImportOrderSummaryDTO(this clsImportOrder importOrder)
        {
            if (importOrder == null) return null;
            
            return new ImportOrderDTO
            {
                ImportOrderID = importOrder.ID,
                SupplierID = importOrder.SupplierID,
                SupplierName = importOrder.Supplier?.StoreName ?? "",
                SupplierPhone = importOrder.Supplier?.Person?.PhoneNumber ?? "",
                TotalAmount = importOrder.TotalAmount,
                PaidAmount = importOrder.PaidAmount,
                ImportDate = importOrder.ImportDate,
                PaymentStatus = importOrder.PaymentStatus,
                PaymentStatusText = GetPaymentStatusText(importOrder.PaymentStatus),
                ActionByUser = importOrder.ActionByUser,
                UserName = importOrder.User?.UserName ?? "",
                ActionType = importOrder.ActionType,
                ActionDate = importOrder.ActionDate,
                ItemsCount = importOrder.ImportOrderItems?.Count ?? 0
            };
        }

        public static ImportOrderItemDTO ToImportOrderItemSummaryDTO(this clsImportOrderItem importOrderItem)
        {
            if (importOrderItem == null) return null;
            
            return new ImportOrderItemDTO
            {
                ImportOrderID = importOrderItem.ImportOrderID,
                ImportOrderItemID = importOrderItem.ID,
                ProductID = importOrderItem.ProductID,
                ProductName = importOrderItem.Product?.Name ?? "",
                Quantity = importOrderItem.Quantity,
                SellingPrice = importOrderItem.SellingPrice,
                TotalItemAmount = importOrderItem.Quantity * importOrderItem.SellingPrice,
                CurrencyType = importOrderItem.Product?.CurrencyType ?? "",
                CurrencyName = GetCurrencyName(importOrderItem.Product?.CurrencyType ?? ""),
                UOMName = importOrderItem.Product?.UnitOfMeasure?.Name ?? "",
                UOMSymbol = importOrderItem.Product?.UnitOfMeasure?.Seymbol ?? "",
                ImportedQuantity = importOrderItem.Quantity
            };
        }

        public static List<SupplierDTO> ToSupplierDTOList(this IEnumerable<clsSupplier> suppliers)
        {
            return suppliers?.Select(s => s.ToSupplierDTO()).ToList() ?? new List<SupplierDTO>();
        }

                public static List<clsSupplier> ToSupplierModelList(this IEnumerable<SupplierDTO> supplierDTOs)
        {
            return supplierDTOs?.Select(s => s.ToSupplierModel()).ToList() ?? new List<clsSupplier>();
        }

        // Helper Methods
        private static string GetPaymentStatusText(byte paymentStatus)
        {
            return paymentStatus switch
            {
                0 => "غير مدفوع",
                1 => "مدفوع جزئياً",
                2 => "مدفوع بالكامل",
                _ => "غير محدد"
            };
        }

        private static string GetCurrencyName(string currencyType)
        {
            return currencyType?.ToUpper() switch
            {
                "USD" => "دولار أمريكي",
                "EUR" => "يورو",
                "GBP" => "جنيه إسترليني",
                "SAR" => "ريال سعودي",
                "AED" => "درهم إماراتي",
                "QAR" => "ريال قطري",
                "KWD" => "دينار كويتي",
                "BHD" => "دينار بحريني",
                "OMR" => "ريال عماني",
                "JOD" => "دينار أردني",
                "EGP" => "جنيه مصري",
                "IQD" => "دينار عراقي",
                "LYD" => "دينار ليبي",
                "TND" => "دينار تونسي",
                "MAD" => "درهم مغربي",
                "DZD" => "دينار جزائري",
                "SDG" => "جنيه سوداني",
                "YER" => "ريال يمني",
                "SYP" => "ليرة سورية",
                "LBP" => "ليرة لبنانية",
                _ => currencyType ?? "غير محدد"
            };
        }
    }
}

