IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [B2cCustomers] (
    [B2cCustomerId] int NOT NULL IDENTITY,
    [FirstName] nvarchar(40) NOT NULL,
    [LastName] nvarchar(40) NOT NULL,
    [Email] nvarchar(60) NULL,
    [Address1] nvarchar(60) NULL,
    [Address2] nvarchar(60) NULL,
    [City] nvarchar(60) NULL,
    [Region] nvarchar(60) NULL,
    [PostalCode] nvarchar(30) NULL,
    [Country] nvarchar(60) NULL,
    [Phone] nvarchar(24) NULL,
    CONSTRAINT [PK_B2cCustomers] PRIMARY KEY ([B2cCustomerId])
);

CREATE TABLE [BoardingRequests] (
    [BoardingRequestId] uniqueidentifier NOT NULL DEFAULT ((newid())),
    [BoardingFlow] nvarchar(50) NOT NULL,
    [Mode] nvarchar(50) NOT NULL,
    [BoardingPackageId] nvarchar(50) NOT NULL,
    [CreatedDateTime] datetime2 NOT NULL DEFAULT ((sysutcdatetime())),
    CONSTRAINT [PK_BoardingRequests] PRIMARY KEY ([BoardingRequestId])
);

CREATE TABLE [Categories] (
    [CategoryId] int NOT NULL IDENTITY,
    [CategoryName] nvarchar(40) NOT NULL,
    [Description] nvarchar(max) NULL,
    [Picture] nvarchar(max) NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY ([CategoryId])
);

CREATE TABLE [CustomerCustomerDemo] (
    [CustomerId] nchar(5) NOT NULL,
    [CustomerTypeID] nchar(10) NOT NULL
);

CREATE TABLE [CustomerDemographics] (
    [CustomerTypeID] nchar(10) NOT NULL,
    [CustomerDesc] ntext NULL
);

CREATE TABLE [Customers] (
    [CustomerId] nchar(5) NOT NULL,
    [CompanyName] nvarchar(40) NOT NULL,
    [ContactName] nvarchar(30) NULL,
    [ContactTitle] nvarchar(30) NULL,
    [Address] nvarchar(60) NULL,
    [City] nvarchar(15) NULL,
    [Region] nvarchar(15) NULL,
    [PostalCode] nvarchar(10) NULL,
    [Country] nvarchar(15) NULL,
    [Phone] nvarchar(24) NULL,
    [Fax] nvarchar(24) NULL,
    CONSTRAINT [PK_Customers] PRIMARY KEY ([CustomerId])
);

CREATE TABLE [ElectronicProducts] (
    [ElectronicProductId] int NOT NULL IDENTITY,
    [ProductName] nvarchar(40) NOT NULL,
    [UnitPrice] money NULL,
    [ProductSku] varchar(6) NULL,
    [Picture] nvarchar(max) NULL,
    [ProductLabel] nvarchar(max) NULL,
    [Brand] nvarchar(40) NULL,
    CONSTRAINT [PK_ElectronicProductId] PRIMARY KEY ([ElectronicProductId])
);

CREATE TABLE [ErrorObjects] (
    [Id] int NOT NULL IDENTITY,
    [Error] nvarchar(max) NULL,
    [Message] nvarchar(max) NULL,
    [Reason] nvarchar(max) NULL,
    [Action] nvarchar(max) NULL,
    [TransactionJson] nvarchar(max) NULL,
    [CreatedAt] datetime2 NOT NULL DEFAULT ((sysutcdatetime())),
    CONSTRAINT [PK__ErrorObj__3214EC0744F783FC] PRIMARY KEY ([Id])
);

CREATE TABLE [Features] (
    [FeatureId] uniqueidentifier NOT NULL DEFAULT ((newid())),
    [ShortName] nvarchar(100) NOT NULL,
    [Name] nvarchar(200) NOT NULL,
    CONSTRAINT [PK_Features] PRIMARY KEY ([FeatureId])
);

CREATE TABLE [IndividualTransactions] (
    [TransactionId] int NOT NULL IDENTITY,
    [RequestId] nvarchar(40) NOT NULL,
    [TransactionType] nvarchar(40) NOT NULL,
    [ReferenceTransactionId] nvarchar(40) NOT NULL,
    [ResponseTransactionJson] nvarchar(max) NULL,
    CONSTRAINT [PK_IndividualTransactions] PRIMARY KEY ([TransactionId])
);

CREATE TABLE [InvoiceResponse] (
    [InvoiceResponseId] int NOT NULL IDENTITY,
    [InvoiceNumber] nvarchar(60) NOT NULL,
    [TransactionJson] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_InvoiceResponse] PRIMARY KEY ([InvoiceResponseId])
);

CREATE TABLE [MerchantSampleData] (
    [SampleMerchantId] int NOT NULL IDENTITY,
    [OrganizationId] varchar(512) NULL,
    [Status] varchar(512) NULL,
    [Type] varchar(512) NULL,
    [Configurable] varchar(512) NULL,
    [Country] varchar(512) NULL,
    [Address1] varchar(512) NULL,
    [PostalCode] varchar(512) NULL,
    [AdministrativeArea] varchar(512) NULL,
    [Locality] varchar(512) NULL,
    [BusinessContactFirstName] varchar(512) NULL,
    [BusinessContactLastName] varchar(512) NULL,
    [BusinessContactPhoneNumber] varchar(512) NULL,
    [BusinessContactEmail] varchar(512) NULL,
    [TechnicalContactFirstName] varchar(512) NULL,
    [TechnicalContactLastName] varchar(512) NULL,
    [TechnicalContactphoneNumber] varchar(512) NULL,
    [TechnicalContactEmail] varchar(512) NULL,
    [EmergencyContactFirstName] varchar(512) NULL,
    [EmergencyContactLastName] varchar(512) NULL,
    [EmergencyContactPhoneNumber] varchar(512) NULL,
    [EmergencyContactEmail] varchar(512) NULL,
    [Name] varchar(512) NULL,
    [WebsiteUrl] varchar(max) NULL,
    [BusinessInformationPhoneNumber] varchar(512) NULL,
    [BusinessInformationTimeZone] varchar(512) NULL,
    [MerchantCategoryCode] int NULL,
    CONSTRAINT [PK_MerchantSampleData] PRIMARY KEY ([SampleMerchantId])
);

CREATE TABLE [Organizations] (
    [OrganizationId] nvarchar(100) NOT NULL,
    [Status] nvarchar(50) NOT NULL,
    [ParentOrganizationId] nvarchar(100) NULL,
    [OrgType] nvarchar(50) NOT NULL,
    [Configurable] bit NOT NULL DEFAULT CAST(0 AS bit),
    [BusinessName] nvarchar(200) NOT NULL,
    [WebsiteUrl] nvarchar(500) NULL,
    [PhoneNumber] nvarchar(30) NULL,
    [TimeZone] nvarchar(100) NULL,
    [MerchantCategoryCode] nvarchar(10) NULL,
    [AddressLine1] nvarchar(200) NULL,
    [AddressLine2] nvarchar(200) NULL,
    [Locality] nvarchar(100) NULL,
    [AdministrativeArea] nvarchar(50) NULL,
    [PostalCode] nvarchar(20) NULL,
    [Country] nchar(2) NULL,
    [ContactFirstName] nvarchar(100) NULL,
    [ContactLastName] nvarchar(100) NULL,
    [ContactPhone] nvarchar(30) NULL,
    [ContactEmail] nvarchar(200) NULL,
    [CreatedDateTime] datetime2 NOT NULL DEFAULT ((sysutcdatetime())),
    [ModifiedDateTime] datetime2 NOT NULL DEFAULT ((sysutcdatetime())),
    CONSTRAINT [PK_Organizations] PRIMARY KEY ([OrganizationId])
);

CREATE TABLE [PayerAuthCardSampleData] (
    [SamplePayAuthPaymentCardId] int NOT NULL IDENTITY,
    [CardBrand] nvarchar(40) NOT NULL,
    [AccountNumber] nvarchar(40) NOT NULL,
    [ExpMonth] nvarchar(2) NULL,
    [ExpYear] nvarchar(4) NULL,
    [Cvv] nvarchar(3) NULL,
    CONSTRAINT [PK_PayerAuthCardSampleData] PRIMARY KEY ([SamplePayAuthPaymentCardId])
);

CREATE TABLE [PaymentCardSampleData] (
    [SamplePaymentCardId] int NOT NULL IDENTITY,
    [CardBrand] nvarchar(40) NOT NULL,
    [AccountNumber] nvarchar(40) NOT NULL,
    [ExpMonth] nvarchar(2) NULL,
    [ExpYear] nvarchar(4) NULL,
    [Cvv] nvarchar(3) NULL,
    [NT] nvarchar(6) NULL,
    CONSTRAINT [PK_SamplePaymentCardInfo] PRIMARY KEY ([SamplePaymentCardId])
);

CREATE TABLE [Processors] (
    [ProcessorKey] nvarchar(50) NOT NULL,
    [DisplayName] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_Processors] PRIMARY KEY ([ProcessorKey])
);

CREATE TABLE [Product] (
    [ProductId] uniqueidentifier NOT NULL DEFAULT ((newid())),
    [ShortName] nvarchar(100) NOT NULL,
    [Name] nvarchar(200) NOT NULL,
    [LifecycleStage] nvarchar(50) NOT NULL,
    [Category] nvarchar(100) NOT NULL,
    [ConfigurationsSupported] bit NOT NULL DEFAULT CAST(0 AS bit),
    [ConfigurationsDataStoreId] nvarchar(100) NULL,
    [ProcessorsSupported] bit NOT NULL DEFAULT CAST(0 AS bit),
    [ProcessorsDataStoreId] nvarchar(100) NULL,
    [Templatable] bit NOT NULL DEFAULT CAST(0 AS bit),
    [SupportsConfiguration] bit NOT NULL DEFAULT CAST(0 AS bit),
    CONSTRAINT [PK_Product] PRIMARY KEY ([ProductId])
);

CREATE TABLE [Region] (
    [RegionID] int NOT NULL,
    [RegionDescription] nchar(50) NOT NULL
);

CREATE TABLE [SampleCustomerData] (
    [SampleCustomerId] int NOT NULL IDENTITY,
    [FirstName] nvarchar(40) NOT NULL,
    [LastName] nvarchar(40) NOT NULL,
    [Phone] nvarchar(24) NULL,
    [Email] nvarchar(100) NULL,
    [Address1] nvarchar(60) NULL,
    [Address2] nvarchar(60) NULL,
    [City] nvarchar(60) NULL,
    [Region] nvarchar(60) NULL,
    [PostalCode] nvarchar(60) NULL,
    [Country] nvarchar(60) NULL,
    [IpAddressV4] nvarchar(60) NULL,
    [IpAddressV6] nvarchar(100) NULL,
    CONSTRAINT [PK_SampleCustomerData] PRIMARY KEY ([SampleCustomerId])
);

CREATE TABLE [SampleInvoiceDetails] (
    [SampleInvoiceId] int NOT NULL IDENTITY,
    [CustomerInformationName] nvarchar(255) NULL,
    [CustomerInformationEmail] nvarchar(255) NULL,
    [CustomerInformationMerchantCustomerId] nvarchar(50) NULL,
    [CustomerInformationCompanyName] nvarchar(255) NULL,
    [InvoiceInformationInvoiceNumber] nvarchar(50) NULL,
    [InvoiceInformationDueDate] date NULL,
    [InvoiceInformationSendImmediately] nvarchar(50) NULL,
    [InvoiceInformationAllowPartialPayments] nvarchar(50) NULL,
    [InvoiceInformationDeliveryMode] nvarchar(50) NULL,
    [OrderInformationAmountDetailsTotalAmount] decimal(18,2) NULL,
    [OrderInformationAmountDetailsCurrency] nvarchar(3) NULL,
    [OrderInformationAmountDetailsDiscountAmount] decimal(18,2) NULL,
    [OrderInformationAmountDetailsDiscountPercent] decimal(5,2) NULL,
    [OrderInformationAmountDetailsSubAmount] decimal(18,2) NULL,
    [OrderInformationAmountDetailsMinimumPartialAmount] decimal(18,2) NULL,
    [OrderInformationAmountDetailsTaxDetailsType] nvarchar(50) NULL,
    [OrderInformationAmountDetailsTaxDetailsAmount] decimal(18,2) NULL,
    [OrderInformationAmountDetailsTaxDetailsRate] decimal(5,2) NULL,
    [OrderInformationAmountDetailsFreightAmount] decimal(18,2) NULL,
    [OrderInformationAmountDetailsFreightTaxable] nvarchar(50) NULL,
    [OrderInformationLineItemsProductSku] nvarchar(50) NULL,
    [OrderInformationLineItemsQuantity] int NULL,
    [OrderInformationLineItemsUnitPrice] decimal(18,2) NULL,
    [OrderInformationLineItemsDiscountAmount] decimal(18,2) NULL,
    [OrderInformationLineItemsDiscountRate] decimal(5,2) NULL,
    [OrderInformationLineItemsTaxAmount] decimal(18,2) NULL,
    [OrderInformationLineItemsTaxRate] decimal(5,2) NULL,
    [OrderInformationLineItemsTotalAmount] decimal(18,2) NULL,
    CONSTRAINT [PK_SampleSampleInvoiceDetails] PRIMARY KEY ([SampleInvoiceId])
);

CREATE TABLE [SessionTransactionsStore] (
    [Id] uniqueidentifier NOT NULL,
    [SerializedData] nvarchar(max) NOT NULL,
    [CreatedAt] datetime2 NOT NULL DEFAULT ((sysutcdatetime())),
    CONSTRAINT [PK__SessionT__3214EC076E547C70] PRIMARY KEY ([Id])
);

CREATE TABLE [Shippers] (
    [ShipperId] int NOT NULL IDENTITY,
    [CompanyName] nvarchar(40) NOT NULL,
    [Phone] nvarchar(24) NULL,
    CONSTRAINT [PK_Shippers] PRIMARY KEY ([ShipperId])
);

CREATE TABLE [ShippingInstAddress] (
    [ShippingInstId] nvarchar(60) NOT NULL,
    [CustomerInstId] nvarchar(60) NOT NULL,
    [FirstName] nvarchar(40) NULL,
    [LastName] nvarchar(40) NULL,
    [Phone] nvarchar(24) NULL,
    [Email] nvarchar(100) NULL,
    [Address1] nvarchar(60) NULL,
    [Address2] nvarchar(60) NULL,
    [City] nvarchar(60) NULL,
    [Region] nvarchar(60) NULL,
    [PostalCode] nvarchar(60) NULL,
    [Country] nvarchar(60) NULL,
    [Company] nvarchar(100) NULL,
    CONSTRAINT [PK_ShippingInstId] PRIMARY KEY ([ShippingInstId])
);

CREATE TABLE [StandAloneCredit] (
    [StandAloneCreditCardId] int NOT NULL IDENTITY,
    [OrderId] int NULL,
    [B2cCustomerId] int NULL,
    [AccountNumber] nvarchar(40) NULL,
    [TokenValue] nvarchar(40) NULL,
    [ExpMonth] nvarchar(2) NULL,
    [ExpYear] nvarchar(4) NULL,
    [Cvv] nvarchar(3) NULL,
    [CreditAmount] nvarchar(40) NULL,
    [ResponseTransactionJson] nvarchar(max) NULL,
    CONSTRAINT [PK_StandAloneCreditCardId] PRIMARY KEY ([StandAloneCreditCardId])
);

CREATE TABLE [Suppliers] (
    [SupplierId] int NOT NULL IDENTITY,
    [CompanyName] nvarchar(40) NOT NULL,
    [ContactName] nvarchar(30) NULL,
    [ContactTitle] nvarchar(30) NULL,
    [Address] nvarchar(60) NULL,
    [City] nvarchar(15) NULL,
    [Region] nvarchar(15) NULL,
    [PostalCode] nvarchar(10) NULL,
    [Country] nvarchar(15) NULL,
    [Phone] nvarchar(24) NULL,
    [Fax] nvarchar(24) NULL,
    [HomePage] ntext NULL,
    CONSTRAINT [PK_Suppliers] PRIMARY KEY ([SupplierId])
);

CREATE TABLE [Orders] (
    [OrderId] int NOT NULL IDENTITY,
    [B2cCustomerId] int NULL,
    [OrderDate] datetime NULL,
    [RequiredDate] datetime NULL,
    [ShippedDate] datetime NULL,
    [ShipVia] int NULL,
    [Freight] money NULL DEFAULT 0.0,
    [ShipName] nvarchar(40) NULL,
    [ShipAddress] nvarchar(60) NULL,
    [ShipCity] nvarchar(15) NULL,
    [ShipRegion] nvarchar(15) NULL,
    [ShipPostalCode] nvarchar(10) NULL,
    [ShipCountry] nvarchar(15) NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY ([OrderId]),
    CONSTRAINT [FK_Orders_B2cCustomers] FOREIGN KEY ([B2cCustomerId]) REFERENCES [B2cCustomers] ([B2cCustomerId])
);

CREATE TABLE [PaymentCardInfo] (
    [PaymentCardId] int NOT NULL IDENTITY,
    [B2cCustomerId] int NOT NULL,
    [AccountNumber] nvarchar(40) NULL,
    [TokenValue] nvarchar(40) NULL,
    [ExpMonth] nvarchar(2) NULL,
    [ExpYear] nvarchar(4) NULL,
    [Cvv] nvarchar(3) NULL,
    [PaymentAccountReferenceNumber] nvarchar(40) NULL,
    [TokenizedCardType] nvarchar(40) NULL,
    [InstrumentidentifierNew] nvarchar(40) NULL,
    [InstrumentIdentifierId] nvarchar(40) NULL,
    [InstrumentIdentifierState] nvarchar(40) NULL,
    [PaymentInstrumentId] nvarchar(40) NULL,
    [CustomerInstrumentId] nvarchar(40) NULL,
    [MerchantCustomerID] nvarchar(40) NULL,
    [ShippingInstrumentId] nvarchar(40) NULL,
    [ResponseTransactionJson] nvarchar(max) NULL,
    CONSTRAINT [PK_PaymentCardInfo] PRIMARY KEY ([PaymentCardId]),
    CONSTRAINT [FK_PaymentCardInfo_B2cCustomers] FOREIGN KEY ([B2cCustomerId]) REFERENCES [B2cCustomers] ([B2cCustomerId])
);

CREATE TABLE [ActiveProductSets] (
    [ActiveProductSetId] uniqueidentifier NOT NULL DEFAULT ((newid())),
    [OrganizationId] nvarchar(100) NOT NULL,
    [CreatedDateTime] datetime2 NOT NULL DEFAULT ((sysutcdatetime())),
    [ModifiedDateTime] datetime2 NOT NULL DEFAULT ((sysutcdatetime())),
    CONSTRAINT [PK_ActiveProductSets] PRIMARY KEY ([ActiveProductSetId]),
    CONSTRAINT [FK_ActiveProductSets_Organizations_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [Organizations] ([OrganizationId]) ON DELETE CASCADE
);

CREATE TABLE [ProductSubscriptions] (
    [Id] uniqueidentifier NOT NULL DEFAULT ((newid())),
    [OrganizationId] nvarchar(100) NOT NULL,
    [ProductShortName] nvarchar(100) NOT NULL,
    [ProductCategory] nvarchar(50) NOT NULL,
    [Enabled] bit NOT NULL DEFAULT CAST(1 AS bit),
    [SelfServiceability] nvarchar(50) NULL,
    [TemplateId] nvarchar(100) NULL,
    CONSTRAINT [PK_ProductSubscriptions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ProductSubscriptions_Organizations_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [Organizations] ([OrganizationId]) ON DELETE CASCADE
);

CREATE TABLE [ProductNotifications] (
    [Id] uniqueidentifier NOT NULL DEFAULT ((newid())),
    [ProductId] uniqueidentifier NOT NULL,
    [Channel] nvarchar(20) NOT NULL,
    [Enabled] bit NOT NULL DEFAULT CAST(0 AS bit),
    CONSTRAINT [PK_ProductNotifications] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ProductNotifications_Product_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Product] ([ProductId]) ON DELETE CASCADE
);

CREATE TABLE [ProductWebhookEventTypes] (
    [Id] uniqueidentifier NOT NULL DEFAULT ((newid())),
    [ProductId] uniqueidentifier NOT NULL,
    [EventType] nvarchar(200) NOT NULL,
    CONSTRAINT [PK_ProductWebhookEventTypes] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ProductWebhookEventTypes_Product_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Product] ([ProductId]) ON DELETE CASCADE
);

CREATE TABLE [ProductWebhookRetryPolicies] (
    [ProductId] uniqueidentifier NOT NULL,
    [Algorithm] nvarchar(50) NOT NULL,
    [FirstRetry] int NOT NULL,
    [Interval] int NOT NULL,
    [NumberOfRetries] int NOT NULL,
    [RepeatInterval] int NOT NULL DEFAULT 0,
    CONSTRAINT [PK_ProductWebhookRetryPolicies] PRIMARY KEY ([ProductId]),
    CONSTRAINT [FK_ProductWebhookRetryPolicy_Product] FOREIGN KEY ([ProductId]) REFERENCES [Product] ([ProductId]) ON DELETE CASCADE
);

CREATE TABLE [Products] (
    [ProductId] int NOT NULL IDENTITY,
    [ProductName] nvarchar(40) NOT NULL,
    [SupplierId] int NULL,
    [CategoryId] int NULL,
    [QuantityPerUnit] nvarchar(20) NULL,
    [UnitPrice] money NULL DEFAULT 0.0,
    [UnitsInStock] smallint NULL DEFAULT CAST(0 AS smallint),
    [UnitsOnOrder] smallint NULL DEFAULT CAST(0 AS smallint),
    [ReorderLevel] smallint NULL DEFAULT CAST(0 AS smallint),
    [Discontinued] bit NOT NULL,
    [ProductSku] varchar(6) NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY ([ProductId]),
    CONSTRAINT [FK_Products_Categories] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([CategoryId]),
    CONSTRAINT [FK_Products_Suppliers] FOREIGN KEY ([SupplierId]) REFERENCES [Suppliers] ([SupplierId])
);

CREATE TABLE [AuthTransResponses] (
    [AuthTransResponsesId] int NOT NULL IDENTITY,
    [Id] nvarchar(255) NOT NULL,
    [OrderId] int NOT NULL,
    [ClientReferenceCode] nvarchar(255) NULL,
    [ConsumerAuthenticationToken] nvarchar(255) NULL,
    [IssuerResponseRaw] nvarchar(max) NULL,
    [ReconciliationId] nvarchar(255) NULL,
    [Status] nvarchar(50) NULL,
    [SubmitTimeUtc] datetime2 NULL,
    [Links] nvarchar(max) NULL,
    [AuthorizedAmount] decimal(18,2) NULL,
    [Currency] nvarchar(10) NULL,
    [CardType] nvarchar(50) NULL,
    [ConsumerAuthInfoToken] nvarchar(50) NULL,
    [ClientRefInfoCode] nvarchar(50) NULL,
    [POSInfoTerminalId] nvarchar(50) NULL,
    [ProcInfoPayAcctReferenceNumber] nvarchar(50) NULL,
    [ProcInfoMerchantNumber] nvarchar(50) NULL,
    [ProcInfoApprovalCode] nvarchar(50) NULL,
    [ProcInfoNetworkTransactionId] nvarchar(50) NULL,
    [ProcInfoTransactionId] nvarchar(50) NULL,
    [ProcInfoResponseCode] nvarchar(50) NULL,
    [AvsCode] nvarchar(50) NULL,
    [AvsCodeRaw] nvarchar(50) NULL,
    [TokenInformationInstIdNew] nvarchar(50) NULL,
    [TokenInformationInstId] nvarchar(50) NULL,
    [InstrumentIdentifierState] nvarchar(50) NULL,
    [InstrumentIdentifierId] nvarchar(50) NULL,
    [PaymentInstrumentId] nvarchar(50) NULL,
    [ResponseTransactionJson] nvarchar(max) NULL,
    CONSTRAINT [PK_AuthTransResponses] PRIMARY KEY ([AuthTransResponsesId]),
    CONSTRAINT [FK_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([OrderId])
);

CREATE TABLE [FollowOnTransResponses] (
    [TransResponseId] int NOT NULL IDENTITY,
    [FollowOnTransResponseId] nvarchar(100) NULL,
    [OriginalTransactionId] nvarchar(255) NOT NULL,
    [OrderId] int NOT NULL,
    [TransactionType] nvarchar(255) NOT NULL,
    [ResponseTransactionJson] nvarchar(max) NULL,
    CONSTRAINT [PK_FollowOnTransResponses] PRIMARY KEY ([TransResponseId]),
    CONSTRAINT [FK_FollowOnTrans_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([OrderId])
);

CREATE TABLE [NetworkTokenInfo] (
    [PaymentTokenId] int NOT NULL IDENTITY,
    [PaymentCardId] int NOT NULL,
    [TokenAccountNumber] nvarchar(40) NOT NULL,
    [TokenValue] nvarchar(40) NULL,
    [TokenExpMonth] nvarchar(2) NULL,
    [TokenExpYear] nvarchar(4) NULL,
    [OriginalAccountSuffix] nvarchar(40) NULL,
    [OriginalAccountExpMonth] nvarchar(40) NULL,
    [OriginalAccountExpYear] nvarchar(40) NULL,
    [OriginalAccountNumber] nvarchar(40) NULL,
    [PaymentAccountReferenceNumber] nvarchar(200) NULL,
    [TokenizedCardType] nvarchar(40) NULL,
    [TokenState] nvarchar(40) NULL,
    [TokenRequestorId] nvarchar(200) NULL,
    [EnrollmentId] nvarchar(200) NULL,
    [MITPreviousTransactionId] nvarchar(200) NULL,
    [ResponseTransactionJson] nvarchar(max) NULL,
    CONSTRAINT [PK_PaymentTokenId] PRIMARY KEY ([PaymentTokenId]),
    CONSTRAINT [FK_NetworkTokenInfo_PaymentCardId] FOREIGN KEY ([PaymentCardId]) REFERENCES [PaymentCardInfo] ([PaymentCardId])
);

CREATE TABLE [ActiveProductSetProducts] (
    [Id] uniqueidentifier NOT NULL DEFAULT ((newid())),
    [ActiveProductSetId] uniqueidentifier NOT NULL,
    [ProductId] uniqueidentifier NOT NULL,
    [SelfServiceability] nvarchar(50) NOT NULL,
    [EnablementStatus] nvarchar(50) NOT NULL,
    [Distributability] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_ActiveProductSetProducts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ActiveProductSetProducts_ActiveProductSets_ActiveProductSetId] FOREIGN KEY ([ActiveProductSetId]) REFERENCES [ActiveProductSets] ([ActiveProductSetId]) ON DELETE CASCADE,
    CONSTRAINT [FK_ActiveProductSetProducts_Product_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Product] ([ProductId]) ON DELETE CASCADE
);

CREATE TABLE [CardProcessingConfigs] (
    [Id] uniqueidentifier NOT NULL DEFAULT ((newid())),
    [ProductSubscriptionId] uniqueidentifier NOT NULL,
    [MerchantCategoryCode] nvarchar(10) NULL,
    [DefaultAuthTypeCode] nvarchar(50) NULL,
    [EnablePartialAuth] bit NOT NULL DEFAULT CAST(0 AS bit),
    [EnableInterchangeOptimization] bit NOT NULL DEFAULT CAST(0 AS bit),
    [EnableSplitShipment] bit NOT NULL DEFAULT CAST(0 AS bit),
    [AllowCapturesGreaterThanAuthorizations] bit NOT NULL DEFAULT CAST(0 AS bit),
    [EnableDuplicateMerchantReferenceNumberBlocking] bit NOT NULL DEFAULT CAST(0 AS bit),
    [GovernmentControlled] bit NOT NULL DEFAULT CAST(0 AS bit),
    [DropBillingInfo] bit NOT NULL DEFAULT CAST(0 AS bit),
    CONSTRAINT [PK_CardProcessingConfigs] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_CardProcessingConfig_ProductSubscription] FOREIGN KEY ([ProductSubscriptionId]) REFERENCES [ProductSubscriptions] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [FeatureConfigs] (
    [Id] uniqueidentifier NOT NULL DEFAULT ((newid())),
    [ProductSubscriptionId] uniqueidentifier NOT NULL,
    [FeatureShortName] nvarchar(100) NOT NULL,
    [VisaStraightThroughProcessingOnly] bit NULL,
    [IgnoreAddressVerificationSystem] bit NULL,
    CONSTRAINT [PK_FeatureConfigs] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_FeatureConfigs_ProductSubscriptions_ProductSubscriptionId] FOREIGN KEY ([ProductSubscriptionId]) REFERENCES [ProductSubscriptions] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [FeatureProcessorConfigs] (
    [Id] uniqueidentifier NOT NULL DEFAULT ((newid())),
    [ProductSubscriptionId] uniqueidentifier NOT NULL,
    [FeatureShortName] nvarchar(100) NOT NULL,
    [ProcessorKey] nvarchar(50) NOT NULL,
    [AttributeKey] nvarchar(100) NOT NULL,
    [AttributeValue] nvarchar(500) NULL,
    CONSTRAINT [PK_FeatureProcessorConfigs] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_FeatureProcessorConfigs_Processors_ProcessorKey] FOREIGN KEY ([ProcessorKey]) REFERENCES [Processors] ([ProcessorKey]) ON DELETE CASCADE,
    CONSTRAINT [FK_FeatureProcessorConfigs_ProductSubscriptions_ProductSubscriptionId] FOREIGN KEY ([ProductSubscriptionId]) REFERENCES [ProductSubscriptions] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [ProductSubscriptionFeatures] (
    [Id] uniqueidentifier NOT NULL DEFAULT ((newid())),
    [ProductSubscriptionId] uniqueidentifier NOT NULL,
    [FeatureShortName] nvarchar(100) NOT NULL,
    [Enabled] bit NOT NULL DEFAULT CAST(1 AS bit),
    CONSTRAINT [PK_ProductSubscriptionFeatures] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ProductSubscriptionFeatures_ProductSubscriptions_ProductSubscriptionId] FOREIGN KEY ([ProductSubscriptionId]) REFERENCES [ProductSubscriptions] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Order Details] (
    [OrderId] int NOT NULL,
    [ProductId] int NOT NULL,
    [UnitPrice] money NOT NULL,
    [Quantity] smallint NOT NULL DEFAULT CAST(1 AS smallint),
    [Discount] real NOT NULL,
    CONSTRAINT [PK_Order_Details] PRIMARY KEY ([OrderId], [ProductId]),
    CONSTRAINT [FK_Order_Details_Orders] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([OrderId]),
    CONSTRAINT [FK_Order_Details_Products] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([ProductId])
);

CREATE TABLE [ActiveProductSetProductFeatures] (
    [Id] uniqueidentifier NOT NULL DEFAULT ((newid())),
    [ActiveProductSetProductId] uniqueidentifier NOT NULL,
    [FeatureId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_ActiveProductSetProductFeatures] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ActiveProductSetProductFeatures_ActiveProductSetProducts_ActiveProductSetProductId] FOREIGN KEY ([ActiveProductSetProductId]) REFERENCES [ActiveProductSetProducts] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ActiveProductSetProductFeatures_Features_FeatureId] FOREIGN KEY ([FeatureId]) REFERENCES [Features] ([FeatureId]) ON DELETE CASCADE
);

CREATE TABLE [CardProcessingProcessors] (
    [Id] uniqueidentifier NOT NULL DEFAULT ((newid())),
    [CardProcessingConfigId] uniqueidentifier NOT NULL,
    [ProcessorKey] nvarchar(50) NOT NULL,
    [AcquirerMerchantId] nvarchar(50) NULL,
    [BatchGroup] nvarchar(100) NULL,
    [BusinessApplicationId] nvarchar(10) NULL,
    [AllowMultipleBills] bit NOT NULL DEFAULT CAST(0 AS bit),
    [QuasiCash] bit NOT NULL DEFAULT CAST(0 AS bit),
    [EnableAutoAuthReversalAfterVoid] bit NOT NULL DEFAULT CAST(0 AS bit),
    [EnableExpresspayPanTranslation] bit NOT NULL DEFAULT CAST(0 AS bit),
    [EnableTransactionReferenceNumber] bit NOT NULL DEFAULT CAST(0 AS bit),
    [EnableDynamicCurrencyConversion] bit NOT NULL DEFAULT CAST(0 AS bit),
    [CreditAuthUnsupportedCardTypes] nvarchar(50) NULL,
    CONSTRAINT [PK_CardProcessingProcessors] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_CardProcessingProcessors_CardProcessingConfigs_CardProcessingConfigId] FOREIGN KEY ([CardProcessingConfigId]) REFERENCES [CardProcessingConfigs] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_CardProcessingProcessors_Processors_ProcessorKey] FOREIGN KEY ([ProcessorKey]) REFERENCES [Processors] ([ProcessorKey]) ON DELETE CASCADE
);

CREATE TABLE [ProcessorAcquirerAttributes] (
    [Id] uniqueidentifier NOT NULL DEFAULT ((newid())),
    [CardProcessingProcessorId] uniqueidentifier NOT NULL,
    [AttributeKey] nvarchar(100) NOT NULL,
    [AttributeValue] nvarchar(200) NULL,
    CONSTRAINT [PK_ProcessorAcquirerAttributes] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ProcessorAcquirerAttributes_CardProcessingProcessors_CardProcessingProcessorId] FOREIGN KEY ([CardProcessingProcessorId]) REFERENCES [CardProcessingProcessors] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [ProcessorPaymentTypes] (
    [Id] uniqueidentifier NOT NULL DEFAULT ((newid())),
    [CardProcessingProcessorId] uniqueidentifier NOT NULL,
    [PaymentTypeCode] nvarchar(50) NOT NULL,
    [Enabled] bit NOT NULL DEFAULT CAST(1 AS bit),
    CONSTRAINT [PK_ProcessorPaymentTypes] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ProcessorPaymentTypes_CardProcessingProcessors_CardProcessingProcessorId] FOREIGN KEY ([CardProcessingProcessorId]) REFERENCES [CardProcessingProcessors] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [ProcessorPaymentTypeCurrencies] (
    [Id] uniqueidentifier NOT NULL DEFAULT ((newid())),
    [ProcessorPaymentTypeId] uniqueidentifier NOT NULL,
    [CurrencyCode] nchar(3) NOT NULL,
    [Enabled] bit NOT NULL DEFAULT CAST(1 AS bit),
    [EnabledCardPresent] bit NOT NULL DEFAULT CAST(0 AS bit),
    [EnabledCardNotPresent] bit NOT NULL DEFAULT CAST(0 AS bit),
    [MerchantId] nvarchar(50) NULL,
    [TerminalId] nvarchar(50) NULL,
    CONSTRAINT [PK_ProcessorPaymentTypeCurrencies] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ProcessorPaymentTypeCurrencies_ProcessorPaymentTypes_ProcessorPaymentTypeId] FOREIGN KEY ([ProcessorPaymentTypeId]) REFERENCES [ProcessorPaymentTypes] ([Id]) ON DELETE CASCADE
);

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ProcessorKey', N'DisplayName') AND [object_id] = OBJECT_ID(N'[Processors]'))
    SET IDENTITY_INSERT [Processors] ON;
INSERT INTO [Processors] ([ProcessorKey], [DisplayName])
VALUES (N'fdiglobal', N'FDI Global'),
(N'vdcvantiv', N'VDC Vantiv');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ProcessorKey', N'DisplayName') AND [object_id] = OBJECT_ID(N'[Processors]'))
    SET IDENTITY_INSERT [Processors] OFF;

CREATE UNIQUE INDEX [IX_ActiveProductSetProductFeature_ProductId_FeatureId] ON [ActiveProductSetProductFeatures] ([ActiveProductSetProductId], [FeatureId]);

CREATE UNIQUE INDEX [IX_ActiveProductSetProductFeatures_ActiveProductSetProductId_FeatureId] ON [ActiveProductSetProductFeatures] ([ActiveProductSetProductId], [FeatureId]);

CREATE INDEX [IX_ActiveProductSetProductFeatures_FeatureId] ON [ActiveProductSetProductFeatures] ([FeatureId]);

CREATE UNIQUE INDEX [IX_ActiveProductSetProduct_SetId_ProductId] ON [ActiveProductSetProducts] ([ActiveProductSetId], [ProductId]);

CREATE UNIQUE INDEX [IX_ActiveProductSetProducts_ActiveProductSetId_ProductId] ON [ActiveProductSetProducts] ([ActiveProductSetId], [ProductId]);

CREATE INDEX [IX_ActiveProductSetProducts_ProductId] ON [ActiveProductSetProducts] ([ProductId]);

CREATE INDEX [IX_ActiveProductSets_OrganizationId] ON [ActiveProductSets] ([OrganizationId]);

CREATE INDEX [IX_AuthTransResponses_OrderId] ON [AuthTransResponses] ([OrderId]);

CREATE INDEX [City] ON [B2cCustomers] ([City]);

CREATE INDEX [PostalCode] ON [B2cCustomers] ([PostalCode]);

CREATE INDEX [Region] ON [B2cCustomers] ([Region]);

CREATE UNIQUE INDEX [IX_CardProcessingConfig_ProductSubscriptionId] ON [CardProcessingConfigs] ([ProductSubscriptionId]);

CREATE UNIQUE INDEX [IX_CardProcessingConfigs_ProductSubscriptionId] ON [CardProcessingConfigs] ([ProductSubscriptionId]);

CREATE UNIQUE INDEX [IX_CardProcessingProcessor_ConfigId_ProcessorKey] ON [CardProcessingProcessors] ([CardProcessingConfigId], [ProcessorKey]);

CREATE UNIQUE INDEX [IX_CardProcessingProcessors_CardProcessingConfigId_ProcessorKey] ON [CardProcessingProcessors] ([CardProcessingConfigId], [ProcessorKey]);

CREATE INDEX [IX_CardProcessingProcessors_ProcessorKey] ON [CardProcessingProcessors] ([ProcessorKey]);

CREATE INDEX [CategoryName] ON [Categories] ([CategoryName]);

CREATE INDEX [City] ON [Customers] ([City]);

CREATE INDEX [CompanyName] ON [Customers] ([CompanyName]);

CREATE INDEX [PostalCode] ON [Customers] ([PostalCode]);

CREATE INDEX [Region] ON [Customers] ([Region]);

CREATE UNIQUE INDEX [IX_FeatureConfig_SubId_FeatureShortName] ON [FeatureConfigs] ([ProductSubscriptionId], [FeatureShortName]);

CREATE UNIQUE INDEX [IX_FeatureConfigs_ProductSubscriptionId_FeatureShortName] ON [FeatureConfigs] ([ProductSubscriptionId], [FeatureShortName]);

CREATE UNIQUE INDEX [IX_FeatureProcessorConfig_SubId_Feature_Proc_Key] ON [FeatureProcessorConfigs] ([ProductSubscriptionId], [FeatureShortName], [ProcessorKey], [AttributeKey]);

CREATE INDEX [IX_FeatureProcessorConfigs_ProcessorKey] ON [FeatureProcessorConfigs] ([ProcessorKey]);

CREATE UNIQUE INDEX [IX_FeatureProcessorConfigs_ProductSubscriptionId_FeatureShortName_ProcessorKey_AttributeKey] ON [FeatureProcessorConfigs] ([ProductSubscriptionId], [FeatureShortName], [ProcessorKey], [AttributeKey]);

CREATE UNIQUE INDEX [IX_Feature_ShortName] ON [Features] ([ShortName]);

CREATE UNIQUE INDEX [IX_Features_ShortName] ON [Features] ([ShortName]);

CREATE INDEX [IX_FollowOnTransResponses_OrderId] ON [FollowOnTransResponses] ([OrderId]);

CREATE INDEX [IX_NetworkTokenInfo_PaymentCardId] ON [NetworkTokenInfo] ([PaymentCardId]);

CREATE INDEX [OrderId] ON [Order Details] ([OrderId]);

CREATE INDEX [OrdersOrder_Details] ON [Order Details] ([OrderId]);

CREATE INDEX [ProductId] ON [Order Details] ([ProductId]);

CREATE INDEX [ProductsOrder_Details] ON [Order Details] ([ProductId]);

CREATE INDEX [B2cCustomersOrders] ON [Orders] ([B2cCustomerId]);

CREATE INDEX [OrderDate] ON [Orders] ([OrderDate]);

CREATE INDEX [ShippedDate] ON [Orders] ([ShippedDate]);

CREATE INDEX [ShippersOrders] ON [Orders] ([ShipVia]);

CREATE INDEX [ShipPostalCode] ON [Orders] ([ShipPostalCode]);

CREATE INDEX [IX_PaymentCardInfo_B2cCustomerId] ON [PaymentCardInfo] ([B2cCustomerId]);

CREATE UNIQUE INDEX [IX_ProcessorAcquirerAttribute_ProcessorId_Key] ON [ProcessorAcquirerAttributes] ([CardProcessingProcessorId], [AttributeKey]);

CREATE UNIQUE INDEX [IX_ProcessorAcquirerAttributes_CardProcessingProcessorId_AttributeKey] ON [ProcessorAcquirerAttributes] ([CardProcessingProcessorId], [AttributeKey]);

CREATE UNIQUE INDEX [IX_ProcessorPaymentTypeCurrencies_ProcessorPaymentTypeId_CurrencyCode] ON [ProcessorPaymentTypeCurrencies] ([ProcessorPaymentTypeId], [CurrencyCode]);

CREATE UNIQUE INDEX [IX_ProcessorPaymentTypeCurrency_TypeId_CurrencyCode] ON [ProcessorPaymentTypeCurrencies] ([ProcessorPaymentTypeId], [CurrencyCode]);

CREATE UNIQUE INDEX [IX_ProcessorPaymentType_ProcessorId_PaymentTypeCode] ON [ProcessorPaymentTypes] ([CardProcessingProcessorId], [PaymentTypeCode]);

CREATE UNIQUE INDEX [IX_ProcessorPaymentTypes_CardProcessingProcessorId_PaymentTypeCode] ON [ProcessorPaymentTypes] ([CardProcessingProcessorId], [PaymentTypeCode]);

CREATE UNIQUE INDEX [IX_Product_ShortName] ON [Product] ([ShortName]);

CREATE UNIQUE INDEX [IX_ProductNotification_ProductId_Channel] ON [ProductNotifications] ([ProductId], [Channel]);

CREATE UNIQUE INDEX [IX_ProductNotifications_ProductId_Channel] ON [ProductNotifications] ([ProductId], [Channel]);

CREATE INDEX [CategoriesProducts] ON [Products] ([CategoryId]);

CREATE INDEX [CategoryId] ON [Products] ([CategoryId]);

CREATE INDEX [ProductName] ON [Products] ([ProductName]);

CREATE INDEX [SupplierId] ON [Products] ([SupplierId]);

CREATE INDEX [SuppliersProducts] ON [Products] ([SupplierId]);

CREATE UNIQUE INDEX [IX_ProductSubscriptionFeature_SubId_FeatureName] ON [ProductSubscriptionFeatures] ([ProductSubscriptionId], [FeatureShortName]);

CREATE UNIQUE INDEX [IX_ProductSubscriptionFeatures_ProductSubscriptionId_FeatureShortName] ON [ProductSubscriptionFeatures] ([ProductSubscriptionId], [FeatureShortName]);

CREATE UNIQUE INDEX [IX_ProductSubscription_OrgId_ProductShortName] ON [ProductSubscriptions] ([OrganizationId], [ProductShortName]);

CREATE UNIQUE INDEX [IX_ProductSubscriptions_OrganizationId_ProductShortName] ON [ProductSubscriptions] ([OrganizationId], [ProductShortName]);

CREATE UNIQUE INDEX [IX_ProductWebhookEventType_ProductId_EventType] ON [ProductWebhookEventTypes] ([ProductId], [EventType]);

CREATE UNIQUE INDEX [IX_ProductWebhookEventTypes_ProductId_EventType] ON [ProductWebhookEventTypes] ([ProductId], [EventType]);

CREATE INDEX [CompanyName] ON [Suppliers] ([CompanyName]);

CREATE INDEX [PostalCode] ON [Suppliers] ([PostalCode]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260407015657_AddBoardingTables', N'10.0.5');

COMMIT;
GO

