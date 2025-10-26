using SharedModels.EF.DTO;
using SharedModels.EF.Models;
using DAL.Mapper;

using System;
using System.Collections.Generic;
using System.Linq;

namespace BAL.Mappers
{
    public static class BALMappers
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

        public static clsCustomer ToCustomerModel(this CustomerDTO CustomerDTO)
        {
            if (CustomerDTO == null) return null;
            
            return new clsCustomer
            {
                ID = CustomerDTO.CustomerID,
                PersonID = CustomerDTO.PersonID
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

        public static clsOrder ToOrderModel(this OrderDTO OrderDTO)
        {
            if (OrderDTO == null) return null;
            
            return new clsOrder
            {
                ID = OrderDTO.ID,
                CustomerID = OrderDTO.CustomerID,
                OrderDate = OrderDTO.OrderDate,
                TotalAmount = OrderDTO.TotalAmount,
                PaidAmount = OrderDTO.PaidAmount,
                PaymentStatus = OrderDTO.PaymentStatus,
                ActionByUser = OrderDTO.ActionByUser,
                ActionType = OrderDTO.ActionType,
                ActionDate = OrderDTO.ActionDate
            };
        }

        // OrderItem Mappers
        public static OrderItemsDTO FromDALToOrderItemsDTO(this OrderItemsDTO orderItemDTO)
        {
            if (orderItemDTO == null) return null;

            return new OrderItemsDTO
            {
                ID = orderItemDTO.ID,
                OrderID = orderItemDTO.OrderID,
                ProductID = orderItemDTO.ProductID,
                ProductName = orderItemDTO.ProductName,
                ProductSaleAmount = orderItemDTO.ProductSaleAmount,
                Quantity = orderItemDTO.Quantity,
                SellingPrice = orderItemDTO.SellingPrice,
                AvailableQuantity = orderItemDTO.AvailableQuantity,
                WholesalePrice=orderItemDTO.WholesalePrice
            };
        }
        
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
                PriceAdjustment = orderItem.PriceAdjustment,
                WholesalePrice= orderItem.WholesalePrice
            };
        }

        public static clsOrderItem ToOrderItemModel(this OrderItemsDTO orderItemBALDTO)
        {
            if (orderItemBALDTO == null) return null;
            
            return new clsOrderItem
            {
                ID = orderItemBALDTO.ID,
                OrderID = orderItemBALDTO.OrderID,
                ProductID = orderItemBALDTO.ProductID,
                Quantity = orderItemBALDTO.Quantity,
                SellingPrice = orderItemBALDTO.SellingPrice,
                PriceAdjustment = orderItemBALDTO.PriceAdjustment,
                WholesalePrice=orderItemBALDTO.WholesalePrice
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

        public static clsProduct ToProductModel(this ProductDTO ProductDTO)
        {
            if (ProductDTO == null) return null;
            
            return new clsProduct
            {
                ID = ProductDTO.ID,
                Name = ProductDTO.Name,
                Description = ProductDTO.Description,
                RetailPrice = ProductDTO.RetailPrice,
                WholesalePrice = ProductDTO.WholesalePrice,
                AvailableQuantity = ProductDTO.AvailableQuantity,
                CurrencyType = ProductDTO.CurrencyType,
                ImagePath = ProductDTO.ImagePath,
                UOMID = ProductDTO.UOMID,
                ActionByUser = ProductDTO.ActionByUser,
                ActionType = ProductDTO.ActionType,
                ActionDate = ProductDTO.ActionDate
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
                ImportOrderItems = importOrder.ImportOrderItems?.Select(item => new ImportOrderItemDTO
                {
                    ImportOrderID = item.ImportOrderID,
                    ImportOrderItemID = item.ID,
                    ProductID = item.ProductID,
                    ProductName = item.Product?.Name ?? "",
                    Quantity = item.Quantity,
                    SellingPrice = item.SellingPrice,
                    TotalItemAmount = item.Quantity * item.SellingPrice,
                    CurrencyType = item.Product?.CurrencyType ?? "",
                    CurrencyName = GetCurrencyName(item.Product?.CurrencyType ?? ""),
                    UOMName = item.Product?.UnitOfMeasure?.Name ?? "",
                    UOMSymbol = item.Product?.UnitOfMeasure?.Seymbol ?? "",
                    ImportedQuantity = item.Quantity
                }).ToList()
            };
        }

        public static clsImportOrder ToImportOrderModel(this ImportOrderDTO ImportOrderDTO)
        {
            if (ImportOrderDTO == null) return null;
            
            return new clsImportOrder
            {
                ID = ImportOrderDTO.ImportOrderID,
                SupplierID = ImportOrderDTO.SupplierID,
                TotalAmount = ImportOrderDTO.TotalAmount,
                PaidAmount = ImportOrderDTO.PaidAmount,
                ImportDate = ImportOrderDTO.ImportDate,
                PaymentStatus = ImportOrderDTO.PaymentStatus,
                ActionByUser = ImportOrderDTO.ActionByUser,
                ActionType = ImportOrderDTO.ActionType,
                ActionDate = ImportOrderDTO.ActionDate,
                ImportOrderItems = ImportOrderDTO.ImportOrderItems?.Select(item => new clsImportOrderItem
                {
                    ID = item.ImportOrderItemID,
                    ImportOrderID = item.ImportOrderID,
                    ProductID = item.ProductID,
                    Quantity = item.Quantity,
                    SellingPrice = item.SellingPrice
                }).ToList()
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

        public static clsImportOrderItem ToImportOrderItemModel(this ImportOrderItemDTO ImportOrderItemDTO)
        {
            if (ImportOrderItemDTO == null) return null;
            
            return new clsImportOrderItem
            {
                ID = ImportOrderItemDTO.ImportOrderItemID,
                ImportOrderID = ImportOrderItemDTO.ImportOrderID,
                ProductID = ImportOrderItemDTO.ProductID,
                Quantity = ImportOrderItemDTO.Quantity,
                SellingPrice = ImportOrderItemDTO.SellingPrice
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
                Address = supplier.StoreAddress,
                FirstName = supplier.Person?.FirstName ?? "",
                LastName = supplier.Person?.LastName ?? "",
                PhoneNumber = supplier.Person?.PhoneNumber ?? ""
            };
        }

        public static clsSupplier ToSupplierModel(this SupplierDTO SupplierDTO)
        {
            if (SupplierDTO == null) return null;
            
            return new clsSupplier
            {
                ID = SupplierDTO.SupplierID,
                PersonID = SupplierDTO.PersonID,
                StoreName = SupplierDTO.ShopName,
                StoreAddress = SupplierDTO.Address
            };
        }

        // RawMaterial Mappers
        public static RawMaterialDTO ToRawMaterialDTO(this clsRawMaterial rawMaterial)
        {
            if (rawMaterial == null) return null;

            return new RawMaterialDTO
            {
                ID = rawMaterial.ID,
                Name = rawMaterial.Name,
                Description = rawMaterial.Description,
                PurchasePrice = rawMaterial.PurchasePrice,
                AvailableQuantity = rawMaterial.AvailableQuantity,
                ProductionLossQuantity = 0,
                UOMID = rawMaterial.UOMID,
                UOMName = rawMaterial.unitOfMeasure?.Name ?? "",
                CurrencyTypeID = rawMaterial.CurrencyTypeID,
                MaterialSupplier = rawMaterial.MaterialSupplier,
                SupplierName = rawMaterial.Supplier?.StoreName ?? "",
                ActionDate = rawMaterial.ActionDate,
                ReservedQuantity = rawMaterial.ReservedQuantity ?? 0
            };
        }

        public static clsRawMaterial ToRawMaterialModel(this RawMaterialDTO rawMaterialDTO)
        {
            if (rawMaterialDTO == null) return null;

            return new clsRawMaterial
            {
                ID = rawMaterialDTO.ID,
                Name = rawMaterialDTO.Name,
                Description = rawMaterialDTO.Description,
                PurchasePrice = rawMaterialDTO.PurchasePrice,
                AvailableQuantity = rawMaterialDTO.AvailableQuantity,
                UOMID = rawMaterialDTO.UOMID,
                CurrencyTypeID = rawMaterialDTO.CurrencyTypeID,
                MaterialSupplier = rawMaterialDTO.MaterialSupplier,
                ActionDate = rawMaterialDTO.ActionDate,
                ReservedQuantity = rawMaterialDTO.ReservedQuantity 
            };
        }

        // Recipe Mappers
        public static RecipeDTO ToRecipeDTO(this clsRecipe recipe)
        {
            if (recipe == null) return null;

            return new RecipeDTO
            {
                ID = recipe.ID,
                Name = recipe.Name,
                Description = recipe.Description,
                ProductID = recipe.ProductID,
                ProductName = recipe.Product?.Name ?? "",
                YieldQuantity = recipe.YieldQuantity,
                ActionDate = recipe.ActionDate
            };
        }

        public static clsRecipe ToRecipeModel(this RecipeDTO recipeDTO)
        {
            if (recipeDTO == null) return null;

            return new clsRecipe
            {
                ID = recipeDTO.ID,
                Name = recipeDTO.Name,
                Description = recipeDTO.Description,
                ProductID = recipeDTO.ProductID,
                YieldQuantity = recipeDTO.YieldQuantity,
                ActionDate = recipeDTO.ActionDate
            };
        }

        // RecipeInfo Mappers
        public static RecipeInfoDTO ToRecipeInfoDTO(this clsRecipeInfo recipeInfo)
        {
            if (recipeInfo == null) return null;

            return new RecipeInfoDTO
            {
                ID = recipeInfo.ID,
                RecipeID = recipeInfo.RecipeID,
                RecipeName = recipeInfo.Recipe?.Name ?? "",
                MaterialID = recipeInfo.RawMaterialID,
                MaterialName = recipeInfo.RawMaterial?.Name ?? "",
                RequiredMaterialQuantity = recipeInfo.RequiredMaterialQuantity,
                ActionDate = recipeInfo.ActionDate
            };
        }

        public static clsRecipeInfo ToRecipeInfoModel(this RecipeInfoDTO recipeInfoDTO)
        {
            if (recipeInfoDTO == null) return null;

            return new clsRecipeInfo
            {
                ID = recipeInfoDTO.ID,
                RecipeID = recipeInfoDTO.RecipeID,
                RawMaterialID = recipeInfoDTO.MaterialID,
                RequiredMaterialQuantity = recipeInfoDTO.RequiredMaterialQuantity,
                ActionDate = recipeInfoDTO.ActionDate
            };
        }

        // List Mappers
        public static List<CustomerDTO> ToCustomerDTOList(this IEnumerable<clsCustomer> customers)
        {
            return customers?.Select(c => c.ToCustomerDTO()).ToList() ?? new List<CustomerDTO>();
        }

        public static List<clsCustomer> ToCustomerModelList(this IEnumerable<CustomerDTO> CustomerDTOs)
        {
            return CustomerDTOs?.Select(c => c.ToCustomerModel()).ToList() ?? new List<clsCustomer>();
        }

        public static List<OrderDTO> ToOrderDTOList(this IEnumerable<clsOrder> orders)
        {
            return orders?.Select(o => o.ToOrderDTO()).ToList() ?? new List<OrderDTO>();
        }

        public static List<clsOrder> ToOrderModelList(this IEnumerable<OrderDTO> OrderDTOs)
        {
            return OrderDTOs?.Select(o => o.ToOrderModel()).ToList() ?? new List<clsOrder>();
        }

        public static List<OrderItemsDTO> ToOrderItemsDTOList(this IEnumerable<clsOrderItem> orderItems)
        {
            return orderItems?.Select(oi => oi.ToOrderItemsDTO()).ToList() ?? new List<OrderItemsDTO>();
        }

        public static List<clsOrderItem> ToOrderItemModelList(this IEnumerable<OrderItemsDTO> orderItemBALDTOs)
        {
            return orderItemBALDTOs?.Select(oi => oi.ToOrderItemModel()).ToList() ?? new List<clsOrderItem>();
        }

        public static List<ProductDTO> ToProductDTOList(this IEnumerable<clsProduct> products)
        {
            return products?.Select(p => p.ToProductDTO()).ToList() ?? new List<ProductDTO>();
        }

        public static List<clsProduct> ToProductModelList(this IEnumerable<ProductDTO> ProductDTOs)
        {
            return ProductDTOs?.Select(p => p.ToProductModel()).ToList() ?? new List<clsProduct>();
        }

        public static List<ImportOrderDTO> ToImportOrderDTOList(this IEnumerable<clsImportOrder> importOrders)
        {
            return importOrders?.Select(io => io.ToImportOrderDTO()).ToList() ?? new List<ImportOrderDTO>();
        }

        public static List<clsImportOrder> ToImportOrderModelList(this IEnumerable<ImportOrderDTO> ImportOrderDTOs)
        {
            return ImportOrderDTOs?.Select(io => io.ToImportOrderModel()).ToList() ?? new List<clsImportOrder>();
        }

        public static List<ImportOrderItemDTO> ToImportOrderItemDTOList(this IEnumerable<clsImportOrderItem> importOrderItems)
        {
            return importOrderItems?.Select(ioi => ioi.ToImportOrderItemDTO()).ToList() ?? new List<ImportOrderItemDTO>();
        }

        public static List<clsImportOrderItem> ToImportOrderItemModelList(this IEnumerable<ImportOrderItemDTO> ImportOrderItemDTOs)
        {
            return ImportOrderItemDTOs?.Select(ioi => ioi.ToImportOrderItemModel()).ToList() ?? new List<clsImportOrderItem>();
        }

        public static List<SupplierDTO> ToSupplierDTOList(this IEnumerable<clsSupplier> suppliers)
        {
            return suppliers?.Select(s => s.ToSupplierDTO()).ToList() ?? new List<SupplierDTO>();
        }

        public static List<clsSupplier> ToSupplierModelList(this IEnumerable<SupplierDTO> SupplierDTOs)
        {
            return SupplierDTOs?.Select(s => s.ToSupplierModel()).ToList() ?? new List<clsSupplier>();
        }

        public static List<RawMaterialDTO> ToRawMaterialDTOList(this IEnumerable<clsRawMaterial> rawMaterials)
        {
            return rawMaterials?.Select(rm => rm.ToRawMaterialDTO()).ToList() ?? new List<RawMaterialDTO>();
        }

        public static List<clsRawMaterial> ToRawMaterialModelList(this IEnumerable<RawMaterialDTO> rawMaterialDTOs)
        {
            return rawMaterialDTOs?.Select(rm => rm.ToRawMaterialModel()).ToList() ?? new List<clsRawMaterial>();
        }

        public static List<RecipeDTO> ToRecipeDTOList(this IEnumerable<clsRecipe> recipes)
        {
            return recipes?.Select(r => r.ToRecipeDTO()).ToList() ?? new List<RecipeDTO>();
        }

        public static List<clsRecipe> ToRecipeModelList(this IEnumerable<RecipeDTO> recipeDTOs)
        {
            return recipeDTOs?.Select(r => r.ToRecipeModel()).ToList() ?? new List<clsRecipe>();
        }

        public static List<RecipeInfoDTO> ToRecipeInfoDTOList(this IEnumerable<clsRecipeInfo> recipeInfos)
        {
            return recipeInfos?.Select(ri => ri.ToRecipeInfoDTO()).ToList() ?? new List<RecipeInfoDTO>();
        }

        public static List<clsRecipeInfo> ToRecipeInfoModelList(this IEnumerable<RecipeInfoDTO> recipeInfoDTOs)
        {
            return recipeInfoDTOs?.Select(ri => ri.ToRecipeInfoModel()).ToList() ?? new List<clsRecipeInfo>();
        }

        // Union Order Item Mappers
        // Maps a union DTO to a concrete order item model depending on ItemType
        public static clsOrderItem ToOrderItemModel(this OrderItemUnionDTO unionDto)
        {
            if (unionDto == null) return null;

            // Treat as order product item
            return new clsOrderItem
            {
                ID = unionDto.OrderItemID,
                OrderID = unionDto.OrderID,
                ProductID = unionDto.ItemID,
                Quantity = unionDto.Quantity,
                SellingPrice = unionDto.SellingPrice,
                PriceAdjustment = unionDto.PriceAdjustment,
                WholesalePrice = unionDto.WholesalePrice
            };
        }

        // Maps a union DTO to a raw material order item
        public static clsRawMaterialOrderItem ToRawMaterialOrderItemModel(this OrderItemUnionDTO unionDto)
        {
            if (unionDto == null) return null;

            return new clsRawMaterialOrderItem
            {
                ID = unionDto.OrderItemID,
                OrderID = unionDto.OrderID,
                RawMaterialID = unionDto.ItemID,
                Quantity = unionDto.Quantity,
                SellingPrice = unionDto.SellingPrice,
                WholesalePrice = unionDto.WholesalePrice
            };
        }

        // Import Order Item Union Mappers
        // Maps a union DTO to a concrete import order item model depending on ItemType
        public static clsImportOrderItem ToImportOrderItemModel(this ImportOrderItemUnionDTO unionDto)
        {
            if (unionDto == null) return null;

            // Treat as import order product item
            return new clsImportOrderItem
            {
                ID = unionDto.ImportOrderItemID,
                ImportOrderID = unionDto.ImportOrderID,
                ProductID = unionDto.ItemID,
                Quantity = unionDto.Quantity,
                SellingPrice = unionDto.SellingPrice
            };
        }

        // Maps a union DTO to a raw material import order item
        public static clsImportRawMaterialItem ToImportRawMaterialItemModel(this ImportOrderItemUnionDTO unionDto)
        {
            if (unionDto == null) return null;

            return new clsImportRawMaterialItem
            {
                ID = unionDto.ImportOrderItemID,
                ImportOrderID = unionDto.ImportOrderID,
                RawMaterialID = unionDto.ItemID,
                Quantity = unionDto.Quantity,
                SellingPrice = unionDto.SellingPrice,
      
            };
        }

        // Additional ImportOrder BAL Mappers for different scenarios
        public static ImportOrderDTO ToImportOrderSummaryBALDTO(this clsImportOrder importOrder)
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

        public static ImportOrderItemDTO ToImportOrderItemSummaryBALDTO(this clsImportOrderItem importOrderItem)
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

        // List Mappers for Summary DTOs
        public static List<ImportOrderDTO> ToImportOrderSummaryBALDTOList(this IEnumerable<clsImportOrder> importOrders)
        {
            return importOrders?.Select(io => io.ToImportOrderSummaryBALDTO()).ToList() ?? new List<ImportOrderDTO>();
        }

        public static List<ImportOrderItemDTO> ToImportOrderItemSummaryBALDTOList(this IEnumerable<clsImportOrderItem> importOrderItems)
        {
            return importOrderItems?.Select(ioi => ioi.ToImportOrderItemSummaryBALDTO()).ToList() ?? new List<ImportOrderItemDTO>();
        }

        // ImportOrderItemDTO to ImportOrderItemDTO Mappers
        public static ImportOrderItemDTO ToImportOrderItemDTO(this ImportOrderItemDTO importOrderItemDTO)
        {
            return new ImportOrderItemDTO
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

        //public static ImportOrderItemDTO ToImportOrderItemDTO(this ImportOrderItemDTO ImportOrderItemDTO)
        //{
        //    return new ImportOrderItemDTO
        //    {
        //        ImportOrderID = ImportOrderItemDTO.ImportOrderID,
        //        ImportOrderItemID = ImportOrderItemDTO.ImportOrderItemID,
        //        ProductID = ImportOrderItemDTO.ProductID,
        //        ProductName = ImportOrderItemDTO.ProductName,
        //        Quantity = ImportOrderItemDTO.Quantity,
        //        SellingPrice = ImportOrderItemDTO.SellingPrice,
        //        TotalItemAmount = ImportOrderItemDTO.TotalItemAmount,
        //        CurrencyType = ImportOrderItemDTO.CurrencyType,
        //        CurrencyName = ImportOrderItemDTO.CurrencyName,
        //        UOMName = ImportOrderItemDTO.UOMName,
        //        UOMSymbol = ImportOrderItemDTO.UOMSymbol,
        //        ImportedQuantity = ImportOrderItemDTO.ImportedQuantity
        //    };
        //}

        // List Mappers for ImportOrderItemDTO
        //public static List<ImportOrderItemDTO> ToImportOrderItemDTOList(this IEnumerable<ImportOrderItemDTO> importOrderItemDTOs)
        //{
        //    return importOrderItemDTOs?.Select(ioi => ioi.ToImportOrderItemDTO()).ToList() ?? new List<ImportOrderItemDTO>();
        //}

        public static List<ImportOrderItemDTO> ToImportOrderItemDTOList(this IEnumerable<ImportOrderItemDTO> ImportOrderItemDTOs)
        {
            return ImportOrderItemDTOs?.Select(ioi => ioi.ToImportOrderItemDTO()).ToList() ?? new List<ImportOrderItemDTO>();
        }

        // ImportOrderDTO to ImportOrderDTO Mappers
        public static ImportOrderDTO ToImportOrderDTO(this ImportOrderDTO importOrderDTO)
        {
            return new ImportOrderDTO
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
        public static List<ImportOrderDTO> ToImportOrderDTOList(this IEnumerable<ImportOrderDTO> importOrderDTOs)
        {
            return importOrderDTOs?.Select(io => io.ToImportOrderDTO()).ToList() ?? new List<ImportOrderDTO>();
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

