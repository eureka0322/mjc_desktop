ALTER TABLE dbo.Accounts ADD CONSTRAINT PK_Account PRIMARY KEY (id);
ALTER TABLE dbo.Categories ADD CONSTRAINT PK_Category PRIMARY KEY (id);
ALTER TABLE dbo.CategoryPriceTiers ADD CONSTRAINT PK_CategoryPriceTier PRIMARY KEY (id);
ALTER TABLE dbo.CreditCodes ADD CONSTRAINT PK_CreditCode PRIMARY KEY (id);
ALTER TABLE dbo.SKUCrossReferences ADD CONSTRAINT PK_CrossReference PRIMARY KEY (id);
ALTER TABLE dbo.Customers ADD CONSTRAINT PK_Customer PRIMARY KEY (id);
ALTER TABLE dbo.CustomerCreditCards ADD CONSTRAINT PK_CustomerCreditCard PRIMARY KEY (id);
ALTER TABLE dbo.CustomerPriveLevels ADD PRIMARY KEY (id);
ALTER TABLE dbo.CustomerShipTos ADD PRIMARY KEY (id);
ALTER TABLE dbo.Orders ADD PRIMARY KEY (id);
ALTER TABLE dbo.OrderItems ADD PRIMARY KEY (id);
ALTER TABLE dbo.OrderPayments ADD PRIMARY KEY (id);
ALTER TABLE dbo.Payments ADD PRIMARY KEY (id);
ALTER TABLE dbo.PriceTiers ADD PRIMARY KEY (id);
ALTER TABLE dbo.SalePrices ADD PRIMARY KEY (id);
ALTER TABLE dbo.SalesCostCodes ADD PRIMARY KEY (id);
ALTER TABLE dbo.SKU ADD PRIMARY KEY (id);
ALTER TABLE dbo.SKUCostQtys ADD PRIMARY KEY (id);
ALTER TABLE dbo.SKUPrices ADD PRIMARY KEY (id);
ALTER TABLE dbo.SKUQuantityDiscounts ADD PRIMARY KEY (id);
ALTER TABLE dbo.SKUSerialLots ADD PRIMARY KEY (id);
ALTER TABLE dbo.SKUVendorCosts ADD PRIMARY KEY (id);
ALTER TABLE dbo.SubAssemblies ADD PRIMARY KEY (id);
ALTER TABLE dbo.TaxCodes ADD PRIMARY KEY (id);
ALTER TABLE dbo.Vendors ADD PRIMARY KEY (id);
ALTER TABLE [dbo.Zones] ADD PRIMARY KEY (id);