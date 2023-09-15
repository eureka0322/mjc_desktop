ALTER TABLE dbo.OrderItems DROP CONSTRAINT FKOrderItems457399;
ALTER TABLE dbo.OrderItems DROP CONSTRAINT FKOrderItems822669;
ALTER TABLE dbo.SalePrices DROP CONSTRAINT FKSalePrices197745;
ALTER TABLE dbo.SalePrices DROP CONSTRAINT FKSalePrices241755;
ALTER TABLE dbo.SalePrices DROP CONSTRAINT FKSalePrices95451;
ALTER TABLE dbo.CustomerPriveLevels DROP CONSTRAINT FKCustomerPr787677;
ALTER TABLE dbo.CustomerPriveLevels DROP CONSTRAINT FKCustomerPr743667;
ALTER TABLE dbo.CustomerPriveLevels DROP CONSTRAINT FKCustomerPr426091;
ALTER TABLE dbo.SKUVendorCosts DROP CONSTRAINT FKSKUVendorC178741;
ALTER TABLE dbo.SKUVendorCosts DROP CONSTRAINT FKSKUVendorC473243;
ALTER TABLE dbo.SubAssemblies DROP CONSTRAINT FKSubAssembl108397;
ALTER TABLE dbo.SubAssemblies DROP CONSTRAINT FKSubAssembl844279;
ALTER TABLE dbo.SubAssemblies DROP CONSTRAINT FKSubAssembl483669;
ALTER TABLE dbo.SKUSerialLots DROP CONSTRAINT FKSKUSerialL812766;
ALTER TABLE dbo.Payments DROP CONSTRAINT FKPayments292791;
ALTER TABLE dbo.SKUQuantityDiscounts DROP CONSTRAINT FKSKUQuantit734253;
ALTER TABLE dbo.CustomerShipTos DROP CONSTRAINT FKCustomerSh958082;
ALTER TABLE dbo.CustomerCreditCards DROP CONSTRAINT FKCustomerCr626626;
ALTER TABLE dbo.SKUCostQtys DROP CONSTRAINT FKSKUCostQty945968;
ALTER TABLE dbo.SKUCrossReferences DROP CONSTRAINT FKSKUCrossRe604433;
ALTER TABLE dbo.SKUCrossReferences DROP CONSTRAINT FKSKUCrossRe757229;
ALTER TABLE dbo.CategoryPriceTiers DROP CONSTRAINT FKCategoryPr626469;
ALTER TABLE dbo.CategoryPriceTiers DROP CONSTRAINT FKCategoryPr710736;
ALTER TABLE dbo.SKUPrices DROP CONSTRAINT FKSKUPrices243260;
ALTER TABLE dbo.SKUPrices DROP CONSTRAINT FKSKUPrices49936;
ALTER TABLE dbo.OrderPayments DROP CONSTRAINT FKOrderPayme782288;
ALTER TABLE dbo.SystemSettings DROP CONSTRAINT FKSystemSett697825;
ALTER TABLE dbo.InventorySettings DROP CONSTRAINT FKInventoryS701844;
ALTER TABLE dbo.Payments DROP CONSTRAINT FKPayments704381;
ALTER TABLE dbo.Customers DROP CONSTRAINT FKCustomers404946;
ALTER TABLE dbo.Customers DROP CONSTRAINT FKCustomers163918;
ALTER TABLE dbo.SKU DROP CONSTRAINT FKSKU46533;
ALTER TABLE dbo.SKU DROP CONSTRAINT FKSKU356193;
ALTER TABLE dbo.Orders DROP CONSTRAINT FKOrders445723;
ALTER TABLE dbo.OrderPayments DROP CONSTRAINT FKOrderPayme928650;
ALTER TABLE dbo.Orders DROP CONSTRAINT FKOrders631067;
ALTER TABLE dbo.Orders DROP CONSTRAINT FKOrders937796;
ALTER TABLE dbo.OrderItems DROP CONSTRAINT FKOrderItems427225;
ALTER TABLE dbo.OrderItems DROP CONSTRAINT FKOrderItems141639;
ALTER TABLE dbo.SalePrices DROP CONSTRAINT FKSalePrices52150;
ALTER TABLE dbo.SalePrices DROP CONSTRAINT FKSalePrices516714;
ALTER TABLE dbo.CustomerPriveLevels DROP CONSTRAINT FKCustomerPr934017;
ALTER TABLE dbo.CustomerPriveLevels DROP CONSTRAINT FKCustomerPr468708;
ALTER TABLE dbo.SKUVendorCosts DROP CONSTRAINT FKSKUVendorC542954;
ALTER TABLE dbo.SKUVendorCosts DROP CONSTRAINT FKSKUVendorC859771;
ALTER TABLE dbo.SubAssemblies DROP CONSTRAINT FKSubAssembl810235;
ALTER TABLE dbo.SubAssemblies DROP CONSTRAINT FKSubAssembl758628;
ALTER TABLE dbo.SKUSerialLots DROP CONSTRAINT FKSKUSerialL91071;
ALTER TABLE dbo.SKUSerialLots DROP CONSTRAINT FKSKUSerialL493797;
ALTER TABLE dbo.Payments DROP CONSTRAINT FKPayments800717;
ALTER TABLE dbo.Payments DROP CONSTRAINT FKPayments602008;
ALTER TABLE dbo.SKUQuantityDiscounts DROP CONSTRAINT FKSKUQuantit455949;
ALTER TABLE dbo.SKUQuantityDiscounts DROP CONSTRAINT FKSKUQuantit53223;
ALTER TABLE dbo.CustomerShipTos DROP CONSTRAINT FKCustomerSh450156;
ALTER TABLE dbo.CustomerShipTos DROP CONSTRAINT FKCustomerSh118708;
ALTER TABLE dbo.CustomerCreditCards DROP CONSTRAINT FKCustomerCr134553;
ALTER TABLE dbo.CustomerCreditCards DROP CONSTRAINT FKCustomerCr268173;
ALTER TABLE dbo.SKUCostQtys DROP CONSTRAINT FKSKUCostQty775726;
ALTER TABLE dbo.SKUCostQtys DROP CONSTRAINT FKSKUCostQty626999;
ALTER TABLE dbo.SalesCostCodes DROP CONSTRAINT FKSalesCostC241156;
ALTER TABLE dbo.SalesCostCodes DROP CONSTRAINT FKSalesCostC327708;
ALTER TABLE dbo.PriceTiers DROP CONSTRAINT FKPriceTiers326546;
ALTER TABLE dbo.PriceTiers DROP CONSTRAINT FKPriceTiers76180;
ALTER TABLE dbo.CreditCodes DROP CONSTRAINT FKCreditCode550330;
ALTER TABLE dbo.CreditCodes DROP CONSTRAINT FKCreditCode852395;
ALTER TABLE dbo.SKUCrossReferences DROP CONSTRAINT FKSKUCrossRe773427;
ALTER TABLE dbo.SKUCrossReferences DROP CONSTRAINT FKSKUCrossRe370701;
ALTER TABLE dbo.TaxCodes DROP CONSTRAINT FKTaxCodes624853;
ALTER TABLE dbo.TaxCodes DROP CONSTRAINT FKTaxCodes777872;
ALTER TABLE dbo.CategoryPriceTiers DROP CONSTRAINT FKCategoryPr304155;
ALTER TABLE dbo.CategoryPriceTiers DROP CONSTRAINT FKCategoryPr901428;
ALTER TABLE dbo.SKUPrices DROP CONSTRAINT FKSKUPrices6635;
ALTER TABLE dbo.SKUPrices DROP CONSTRAINT FKSKUPrices562229;
ALTER TABLE dbo.Categories DROP CONSTRAINT FKCategories856083;
ALTER TABLE dbo.Categories DROP CONSTRAINT FKCategories712780;
ALTER TABLE dbo.OrderPayments DROP CONSTRAINT FKOrderPayme158315;
ALTER TABLE dbo.OrderPayments DROP CONSTRAINT FKOrderPayme755588;
ALTER TABLE dbo.Vendors DROP CONSTRAINT FKVendors999524;
ALTER TABLE dbo.Vendors DROP CONSTRAINT FKVendors569339;
ALTER TABLE dbo.OrderItems ADD CONSTRAINT FKOrderItems457399 FOREIGN KEY (orderId) REFERENCES dbo.Orders (id);
ALTER TABLE dbo.OrderItems ADD CONSTRAINT FKOrderItems822669 FOREIGN KEY (skuId) REFERENCES dbo.SKU (id);
ALTER TABLE dbo.SalePrices ADD CONSTRAINT FKSalePrices197745 FOREIGN KEY (skuId) REFERENCES dbo.SKU (id);
ALTER TABLE dbo.SalePrices ADD CONSTRAINT FKSalePrices241755 FOREIGN KEY (categoryId) REFERENCES dbo.Categories (id);
ALTER TABLE dbo.SalePrices ADD CONSTRAINT FKSalePrices95451 FOREIGN KEY (priceTierId) REFERENCES dbo.PriceTiers (id);
ALTER TABLE dbo.CustomerPriveLevels ADD CONSTRAINT FKCustomerPr787677 FOREIGN KEY (skuId) REFERENCES dbo.SKU (id);
ALTER TABLE dbo.CustomerPriveLevels ADD CONSTRAINT FKCustomerPr743667 FOREIGN KEY (categoryId) REFERENCES dbo.Categories (id);
ALTER TABLE dbo.CustomerPriveLevels ADD CONSTRAINT FKCustomerPr426091 FOREIGN KEY (customerId) REFERENCES dbo.Customers (id);
ALTER TABLE dbo.SKUVendorCosts ADD CONSTRAINT FKSKUVendorC178741 FOREIGN KEY (skuId) REFERENCES dbo.SKU (id);
ALTER TABLE dbo.SKUVendorCosts ADD CONSTRAINT FKSKUVendorC473243 FOREIGN KEY (vendorId) REFERENCES dbo.Vendors (id);
ALTER TABLE dbo.SubAssemblies ADD CONSTRAINT FKSubAssembl108397 FOREIGN KEY (targetSkuId) REFERENCES dbo.SKU (id);
ALTER TABLE dbo.SubAssemblies ADD CONSTRAINT FKSubAssembl844279 FOREIGN KEY (subAssemblySkuId) REFERENCES dbo.SKU (id);
ALTER TABLE dbo.SubAssemblies ADD CONSTRAINT FKSubAssembl483669 FOREIGN KEY (categoryId) REFERENCES dbo.Categories (id);
ALTER TABLE dbo.SKUSerialLots ADD CONSTRAINT FKSKUSerialL812766 FOREIGN KEY (skuId) REFERENCES dbo.SKU (id);
ALTER TABLE dbo.Payments ADD CONSTRAINT FKPayments292791 FOREIGN KEY (customerId) REFERENCES dbo.Customers (id);
ALTER TABLE dbo.SKUQuantityDiscounts ADD CONSTRAINT FKSKUQuantit734253 FOREIGN KEY (skuId) REFERENCES dbo.SKU (id);
ALTER TABLE dbo.CustomerShipTos ADD CONSTRAINT FKCustomerSh958082 FOREIGN KEY (customerId) REFERENCES dbo.Customers (id);
ALTER TABLE dbo.CustomerCreditCards ADD CONSTRAINT FKCustomerCr626626 FOREIGN KEY (customerId) REFERENCES dbo.Customers (id);
ALTER TABLE dbo.SKUCostQtys ADD CONSTRAINT FKSKUCostQty945968 FOREIGN KEY (skuId) REFERENCES dbo.SKU (id);
ALTER TABLE dbo.SKUCrossReferences ADD CONSTRAINT FKSKUCrossRe604433 FOREIGN KEY (SkuId) REFERENCES dbo.SKU (id);
ALTER TABLE dbo.SKUCrossReferences ADD CONSTRAINT FKSKUCrossRe757229 FOREIGN KEY (vendorId) REFERENCES dbo.Vendors (id);
ALTER TABLE dbo.CategoryPriceTiers ADD CONSTRAINT FKCategoryPr626469 FOREIGN KEY (categoryId) REFERENCES dbo.Categories (id);
ALTER TABLE dbo.CategoryPriceTiers ADD CONSTRAINT FKCategoryPr710736 FOREIGN KEY (priceTierId) REFERENCES dbo.PriceTiers (id);
ALTER TABLE dbo.SKUPrices ADD CONSTRAINT FKSKUPrices243260 FOREIGN KEY (skuId) REFERENCES dbo.SKU (id);
ALTER TABLE dbo.SKUPrices ADD CONSTRAINT FKSKUPrices49936 FOREIGN KEY (priceTierId) REFERENCES dbo.PriceTiers (id);
ALTER TABLE dbo.OrderPayments ADD CONSTRAINT FKOrderPayme782288 FOREIGN KEY (paymentId) REFERENCES dbo.Payments (id);
ALTER TABLE dbo.SystemSettings ADD CONSTRAINT FKSystemSett697825 FOREIGN KEY (taxCodeId) REFERENCES dbo.TaxCodes (id);
ALTER TABLE dbo.InventorySettings ADD CONSTRAINT FKInventoryS701844 FOREIGN KEY (salesCostCodeId) REFERENCES dbo.SalesCostCodes (id);
ALTER TABLE dbo.Payments ADD CONSTRAINT FKPayments704381 FOREIGN KEY (creditCardId) REFERENCES dbo.CustomerCreditCards (id);
ALTER TABLE dbo.Customers ADD CONSTRAINT FKCustomers404946 FOREIGN KEY (createdBy) REFERENCES dbo.Accounts (id);
ALTER TABLE dbo.Customers ADD CONSTRAINT FKCustomers163918 FOREIGN KEY (updatedBy) REFERENCES dbo.Accounts (id);
ALTER TABLE dbo.SKU ADD CONSTRAINT FKSKU46533 FOREIGN KEY (createdBy) REFERENCES dbo.Accounts (id);
ALTER TABLE dbo.SKU ADD CONSTRAINT FKSKU356193 FOREIGN KEY (updatedBy) REFERENCES dbo.Accounts (id);
ALTER TABLE dbo.Orders ADD CONSTRAINT FKOrders445723 FOREIGN KEY (customerId) REFERENCES dbo.Customers (id);
ALTER TABLE dbo.OrderPayments ADD CONSTRAINT FKOrderPayme928650 FOREIGN KEY (orderId) REFERENCES dbo.Orders (id);
ALTER TABLE dbo.Orders ADD CONSTRAINT FKOrders631067 FOREIGN KEY (updatedBy) REFERENCES dbo.Accounts (id);
ALTER TABLE dbo.Orders ADD CONSTRAINT FKOrders937796 FOREIGN KEY (createdBy) REFERENCES dbo.Accounts (id);
ALTER TABLE dbo.OrderItems ADD CONSTRAINT FKOrderItems427225 FOREIGN KEY (createdBy) REFERENCES dbo.Accounts (id);
ALTER TABLE dbo.OrderItems ADD CONSTRAINT FKOrderItems141639 FOREIGN KEY (updatedBy) REFERENCES dbo.Accounts (id);
ALTER TABLE dbo.SalePrices ADD CONSTRAINT FKSalePrices52150 FOREIGN KEY (createdBy) REFERENCES dbo.Accounts (id);
ALTER TABLE dbo.SalePrices ADD CONSTRAINT FKSalePrices516714 FOREIGN KEY (updatedBy) REFERENCES dbo.Accounts (id);
ALTER TABLE dbo.CustomerPriveLevels ADD CONSTRAINT FKCustomerPr934017 FOREIGN KEY (createdBy) REFERENCES dbo.Accounts (id);
ALTER TABLE dbo.CustomerPriveLevels ADD CONSTRAINT FKCustomerPr468708 FOREIGN KEY (updatedBy) REFERENCES dbo.Accounts (id);
ALTER TABLE dbo.SKUVendorCosts ADD CONSTRAINT FKSKUVendorC542954 FOREIGN KEY (createdBy) REFERENCES dbo.Accounts (id);
ALTER TABLE dbo.SKUVendorCosts ADD CONSTRAINT FKSKUVendorC859771 FOREIGN KEY (updatedBy) REFERENCES dbo.Accounts (id);
ALTER TABLE dbo.SubAssemblies ADD CONSTRAINT FKSubAssembl810235 FOREIGN KEY (createdBy) REFERENCES dbo.Accounts (id);
ALTER TABLE dbo.SubAssemblies ADD CONSTRAINT FKSubAssembl758628 FOREIGN KEY (updatedBy) REFERENCES dbo.Accounts (id);
ALTER TABLE dbo.SKUSerialLots ADD CONSTRAINT FKSKUSerialL91071 FOREIGN KEY (createdBy) REFERENCES dbo.Accounts (id);
ALTER TABLE dbo.SKUSerialLots ADD CONSTRAINT FKSKUSerialL493797 FOREIGN KEY (updatedBy) REFERENCES dbo.Accounts (id);
ALTER TABLE dbo.Payments ADD CONSTRAINT FKPayments800717 FOREIGN KEY (createdBy) REFERENCES dbo.Accounts (id);
ALTER TABLE dbo.Payments ADD CONSTRAINT FKPayments602008 FOREIGN KEY (updatedBy) REFERENCES dbo.Accounts (id);
ALTER TABLE dbo.SKUQuantityDiscounts ADD CONSTRAINT FKSKUQuantit455949 FOREIGN KEY (createdBy) REFERENCES dbo.Accounts (id);
ALTER TABLE dbo.SKUQuantityDiscounts ADD CONSTRAINT FKSKUQuantit53223 FOREIGN KEY (updatedBy) REFERENCES dbo.Accounts (id);
ALTER TABLE dbo.CustomerShipTos ADD CONSTRAINT FKCustomerSh450156 FOREIGN KEY (createdBy) REFERENCES dbo.Accounts (id);
ALTER TABLE dbo.CustomerShipTos ADD CONSTRAINT FKCustomerSh118708 FOREIGN KEY (updatedBy) REFERENCES dbo.Accounts (id);
ALTER TABLE dbo.CustomerCreditCards ADD CONSTRAINT FKCustomerCr134553 FOREIGN KEY (createdBy) REFERENCES dbo.Accounts (id);
ALTER TABLE dbo.CustomerCreditCards ADD CONSTRAINT FKCustomerCr268173 FOREIGN KEY (updatedBy) REFERENCES dbo.Accounts (id);
ALTER TABLE dbo.SKUCostQtys ADD CONSTRAINT FKSKUCostQty775726 FOREIGN KEY (createdBy) REFERENCES dbo.Accounts (id);
ALTER TABLE dbo.SKUCostQtys ADD CONSTRAINT FKSKUCostQty626999 FOREIGN KEY (updatedBy) REFERENCES dbo.Accounts (id);
ALTER TABLE dbo.SalesCostCodes ADD CONSTRAINT FKSalesCostC241156 FOREIGN KEY (createdBy) REFERENCES dbo.Accounts (id);
ALTER TABLE dbo.SalesCostCodes ADD CONSTRAINT FKSalesCostC327708 FOREIGN KEY (updatedBy) REFERENCES dbo.Accounts (id);
ALTER TABLE dbo.PriceTiers ADD CONSTRAINT FKPriceTiers326546 FOREIGN KEY (createdBy) REFERENCES dbo.Accounts (id);
ALTER TABLE dbo.PriceTiers ADD CONSTRAINT FKPriceTiers76180 FOREIGN KEY (updatedBy) REFERENCES dbo.Accounts (id);
ALTER TABLE dbo.CreditCodes ADD CONSTRAINT FKCreditCode550330 FOREIGN KEY (createdBy) REFERENCES dbo.Accounts (id);
ALTER TABLE dbo.CreditCodes ADD CONSTRAINT FKCreditCode852395 FOREIGN KEY (updatedBy) REFERENCES dbo.Accounts (id);
ALTER TABLE dbo.SKUCrossReferences ADD CONSTRAINT FKSKUCrossRe773427 FOREIGN KEY (createdBy) REFERENCES dbo.Accounts (id);
ALTER TABLE dbo.SKUCrossReferences ADD CONSTRAINT FKSKUCrossRe370701 FOREIGN KEY (updatedBy) REFERENCES dbo.Accounts (id);
ALTER TABLE dbo.TaxCodes ADD CONSTRAINT FKTaxCodes624853 FOREIGN KEY (createdBy) REFERENCES dbo.Accounts (id);
ALTER TABLE dbo.TaxCodes ADD CONSTRAINT FKTaxCodes777872 FOREIGN KEY (updatedBy) REFERENCES dbo.Accounts (id);
ALTER TABLE dbo.CategoryPriceTiers ADD CONSTRAINT FKCategoryPr304155 FOREIGN KEY (createdBy) REFERENCES dbo.Accounts (id);
ALTER TABLE dbo.CategoryPriceTiers ADD CONSTRAINT FKCategoryPr901428 FOREIGN KEY (updatedBy) REFERENCES dbo.Accounts (id);
ALTER TABLE dbo.SKUPrices ADD CONSTRAINT FKSKUPrices6635 FOREIGN KEY (createdBy) REFERENCES dbo.Accounts (id);
ALTER TABLE dbo.SKUPrices ADD CONSTRAINT FKSKUPrices562229 FOREIGN KEY (updatedBy) REFERENCES dbo.Accounts (id);
ALTER TABLE dbo.Categories ADD CONSTRAINT FKCategories856083 FOREIGN KEY (createdBy) REFERENCES dbo.Accounts (id);
ALTER TABLE dbo.Categories ADD CONSTRAINT FKCategories712780 FOREIGN KEY (updatedBy) REFERENCES dbo.Accounts (id);
ALTER TABLE dbo.OrderPayments ADD CONSTRAINT FKOrderPayme158315 FOREIGN KEY (createdBy) REFERENCES dbo.Accounts (id);
ALTER TABLE dbo.OrderPayments ADD CONSTRAINT FKOrderPayme755588 FOREIGN KEY (updatedBy) REFERENCES dbo.Accounts (id);
ALTER TABLE dbo.Vendors ADD CONSTRAINT FKVendors999524 FOREIGN KEY (createdBy) REFERENCES dbo.Accounts (id);
ALTER TABLE dbo.Vendors ADD CONSTRAINT FKVendors569339 FOREIGN KEY (updatedBy) REFERENCES dbo.Accounts (id);