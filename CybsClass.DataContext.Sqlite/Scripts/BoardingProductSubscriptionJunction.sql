-- =================================================================
-- Many-to-many junction for supplemental product subscriptions
--
-- Step 1 makes each product table's BoardingTransactingMerchantId NULLABLE,
-- so subscriptions can exist before they are linked and can be shared
-- across merchants via the junction.
--
-- Step 2 creates BoardingTransactingMerchantProductSubscription, a single
-- polymorphic junction with a ProductType discriminator. A CHECK constraint
-- enforces valid product-type values; the app layer is responsible for
-- resolving ProductSubscriptionId to the right product table.
--
-- Run this script in SSMS after the initial 13-table DDL has been applied.
-- Each section is idempotent.
-- =================================================================

-- -----------------------------------------------------------------
-- Step 1: drop existing FKs, make BoardingTransactingMerchantId NULL, re-add FKs
-- -----------------------------------------------------------------

-- 1a. Digital Payments
IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_BoardingDigitalPayments_Merchant')
    ALTER TABLE dbo.BoardingDigitalPaymentsSubscription DROP CONSTRAINT FK_BoardingDigitalPayments_Merchant;
GO
ALTER TABLE dbo.BoardingDigitalPaymentsSubscription
    ALTER COLUMN BoardingTransactingMerchantId INT NULL;
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_BoardingDigitalPayments_Merchant')
    ALTER TABLE dbo.BoardingDigitalPaymentsSubscription
        ADD CONSTRAINT FK_BoardingDigitalPayments_Merchant FOREIGN KEY (BoardingTransactingMerchantId)
            REFERENCES dbo.BoardingTransactingMerchant (BoardingTransactingMerchantId);
GO

-- 1b. Invoicing
IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_BoardingInvoicing_Merchant')
    ALTER TABLE dbo.BoardingInvoicingSubscription DROP CONSTRAINT FK_BoardingInvoicing_Merchant;
GO
ALTER TABLE dbo.BoardingInvoicingSubscription
    ALTER COLUMN BoardingTransactingMerchantId INT NULL;
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_BoardingInvoicing_Merchant')
    ALTER TABLE dbo.BoardingInvoicingSubscription
        ADD CONSTRAINT FK_BoardingInvoicing_Merchant FOREIGN KEY (BoardingTransactingMerchantId)
            REFERENCES dbo.BoardingTransactingMerchant (BoardingTransactingMerchantId);
GO

-- 1c. Pay By Link
IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_BoardingPayByLink_Merchant')
    ALTER TABLE dbo.BoardingPayByLinkSubscription DROP CONSTRAINT FK_BoardingPayByLink_Merchant;
GO
ALTER TABLE dbo.BoardingPayByLinkSubscription
    ALTER COLUMN BoardingTransactingMerchantId INT NULL;
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_BoardingPayByLink_Merchant')
    ALTER TABLE dbo.BoardingPayByLinkSubscription
        ADD CONSTRAINT FK_BoardingPayByLink_Merchant FOREIGN KEY (BoardingTransactingMerchantId)
            REFERENCES dbo.BoardingTransactingMerchant (BoardingTransactingMerchantId);
GO

-- 1d. Token Management
IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_BoardingTokenMgmt_Merchant')
    ALTER TABLE dbo.BoardingTokenManagementSubscription DROP CONSTRAINT FK_BoardingTokenMgmt_Merchant;
GO
ALTER TABLE dbo.BoardingTokenManagementSubscription
    ALTER COLUMN BoardingTransactingMerchantId INT NULL;
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_BoardingTokenMgmt_Merchant')
    ALTER TABLE dbo.BoardingTokenManagementSubscription
        ADD CONSTRAINT FK_BoardingTokenMgmt_Merchant FOREIGN KEY (BoardingTransactingMerchantId)
            REFERENCES dbo.BoardingTransactingMerchant (BoardingTransactingMerchantId);
GO

-- 1e. Unified Checkout
IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_BoardingUnifiedCheckout_Merchant')
    ALTER TABLE dbo.BoardingUnifiedCheckoutSubscription DROP CONSTRAINT FK_BoardingUnifiedCheckout_Merchant;
GO
ALTER TABLE dbo.BoardingUnifiedCheckoutSubscription
    ALTER COLUMN BoardingTransactingMerchantId INT NULL;
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_BoardingUnifiedCheckout_Merchant')
    ALTER TABLE dbo.BoardingUnifiedCheckoutSubscription
        ADD CONSTRAINT FK_BoardingUnifiedCheckout_Merchant FOREIGN KEY (BoardingTransactingMerchantId)
            REFERENCES dbo.BoardingTransactingMerchant (BoardingTransactingMerchantId);
GO

-- 1f. Value Added Services
IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_BoardingVas_Merchant')
    ALTER TABLE dbo.BoardingValueAddedServicesSubscription DROP CONSTRAINT FK_BoardingVas_Merchant;
GO
ALTER TABLE dbo.BoardingValueAddedServicesSubscription
    ALTER COLUMN BoardingTransactingMerchantId INT NULL;
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_BoardingVas_Merchant')
    ALTER TABLE dbo.BoardingValueAddedServicesSubscription
        ADD CONSTRAINT FK_BoardingVas_Merchant FOREIGN KEY (BoardingTransactingMerchantId)
            REFERENCES dbo.BoardingTransactingMerchant (BoardingTransactingMerchantId);
GO

-- 1g. Virtual Terminal
IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_BoardingVt_Merchant')
    ALTER TABLE dbo.BoardingVirtualTerminalSubscription DROP CONSTRAINT FK_BoardingVt_Merchant;
GO
ALTER TABLE dbo.BoardingVirtualTerminalSubscription
    ALTER COLUMN BoardingTransactingMerchantId INT NULL;
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_BoardingVt_Merchant')
    ALTER TABLE dbo.BoardingVirtualTerminalSubscription
        ADD CONSTRAINT FK_BoardingVt_Merchant FOREIGN KEY (BoardingTransactingMerchantId)
            REFERENCES dbo.BoardingTransactingMerchant (BoardingTransactingMerchantId);
GO

-- -----------------------------------------------------------------
-- Step 2: create the many-to-many junction
-- -----------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'BoardingTransactingMerchantProductSubscription')
BEGIN
    CREATE TABLE dbo.BoardingTransactingMerchantProductSubscription
    (
        BoardingTransactingMerchantProductSubscriptionId INT IDENTITY(1,1) NOT NULL,
        BoardingTransactingMerchantId                    INT               NOT NULL,
        ProductType                                      NVARCHAR(50)      NOT NULL,
        ProductSubscriptionId                            INT               NOT NULL,
        AssignedAt                                       DATETIME2(3)      NOT NULL CONSTRAINT DF_BoardingTmps_AssignedAt DEFAULT SYSUTCDATETIME(),

        CONSTRAINT PK_BoardingTransactingMerchantProductSubscription
            PRIMARY KEY CLUSTERED (BoardingTransactingMerchantProductSubscriptionId),

        CONSTRAINT FK_BoardingTmps_TransactingMerchant
            FOREIGN KEY (BoardingTransactingMerchantId)
            REFERENCES dbo.BoardingTransactingMerchant (BoardingTransactingMerchantId)
            ON DELETE CASCADE,

        CONSTRAINT CK_BoardingTmps_ProductType
            CHECK (ProductType IN (
                'digitalPayments',
                'customerInvoicing',
                'payByLink',
                'tokenManagement',
                'unifiedCheckout',
                'valueAddedServices',
                'virtualTerminal'
            )),

        CONSTRAINT UQ_BoardingTmps_Triple
            UNIQUE (BoardingTransactingMerchantId, ProductType, ProductSubscriptionId)
    );

    CREATE INDEX IX_BoardingTmps_ProductLookup
        ON dbo.BoardingTransactingMerchantProductSubscription (ProductType, ProductSubscriptionId);
END
GO
