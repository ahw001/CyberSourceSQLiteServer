using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CybsClass.EntityModels;

public partial class B2cNorthwindContext : DbContext
{
    public B2cNorthwindContext()
    {
    }

    public B2cNorthwindContext(DbContextOptions<B2cNorthwindContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AlphabeticalListOfProduct> AlphabeticalListOfProducts { get; set; }

    public virtual DbSet<AuthTransResponse> AuthTransResponses { get; set; }

    public virtual DbSet<B2cCustomer> B2cCustomers { get; set; }

    public virtual DbSet<B2cOrdersQry> B2cOrdersQries { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<CategorySalesFor1997> CategorySalesFor1997s { get; set; }

    public virtual DbSet<CurrentProductList> CurrentProductLists { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerAndSuppliersByCity> CustomerAndSuppliersByCities { get; set; }

    public virtual DbSet<CustomerCustomerDemo> CustomerCustomerDemos { get; set; }

    public virtual DbSet<CustomerDemographic> CustomerDemographics { get; set; }

    public virtual DbSet<ElectronicProduct> ElectronicProducts { get; set; }

    public virtual DbSet<ErrorObject> ErrorObjects { get; set; }

    public virtual DbSet<FollowOnTransResponse> FollowOnTransResponses { get; set; }

    public virtual DbSet<IndividualTransaction> IndividualTransactions { get; set; }

    public virtual DbSet<InvoiceResponse> InvoiceResponses { get; set; }

    public virtual DbSet<MerchantSampleDatum> MerchantSampleData { get; set; }

    public virtual DbSet<NetworkTokenInfo> NetworkTokenInfos { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<OrderDetailsExtended> OrderDetailsExtendeds { get; set; }

    public virtual DbSet<OrderSubtotal> OrderSubtotals { get; set; }

    public virtual DbSet<OrdersQry> OrdersQries { get; set; }

    public virtual DbSet<PayerAuthCardSampleDatum> PayerAuthCardSampleData { get; set; }

    public virtual DbSet<PaymentCardInfo> PaymentCardInfos { get; set; }

    public virtual DbSet<PaymentCardSampleDatum> PaymentCardSampleData { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductSalesFor1997> ProductSalesFor1997s { get; set; }

    public virtual DbSet<ProductsAboveAveragePrice> ProductsAboveAveragePrices { get; set; }

    public virtual DbSet<ProductsByCategory> ProductsByCategories { get; set; }

    public virtual DbSet<QuarterlyOrder> QuarterlyOrders { get; set; }

    public virtual DbSet<Region> Regions { get; set; }

    public virtual DbSet<SalesByCategory> SalesByCategories { get; set; }

    public virtual DbSet<SalesTotalsByAmount> SalesTotalsByAmounts { get; set; }

    public virtual DbSet<SampleCustomerDatum> SampleCustomerData { get; set; }

    public virtual DbSet<SampleInvoiceDetail> SampleInvoiceDetails { get; set; }

    public virtual DbSet<SessionTransactionsStore> SessionTransactionsStores { get; set; }

    public virtual DbSet<Shipper> Shippers { get; set; }

    public virtual DbSet<ShippingInstAddress> ShippingInstAddresses { get; set; }

    public virtual DbSet<StandAloneCredit> StandAloneCredits { get; set; }

    public virtual DbSet<SummaryOfSalesByQuarter> SummaryOfSalesByQuarters { get; set; }

    public virtual DbSet<SummaryOfSalesByYear> SummaryOfSalesByYears { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    // ==================== Boarding Data ====================
    public virtual DbSet<BoardingOrganization> BoardingOrganizations { get; set; }
    public virtual DbSet<BoardingContact> BoardingContacts { get; set; }
    public virtual DbSet<BoardingTransactingMerchant> BoardingTransactingMerchants { get; set; }
    public virtual DbSet<BoardingCardProductSubscription> BoardingCardProductSubscriptions { get; set; }
    public virtual DbSet<BoardingTransactingMerchantSubscription> BoardingTransactingMerchantSubscriptions { get; set; }
    public virtual DbSet<BoardingCardProcessingConfig> BoardingCardProcessingConfigs { get; set; }
    public virtual DbSet<BoardingProcessorConfig> BoardingProcessorConfigs { get; set; }
    public virtual DbSet<BoardingProcessorPaymentType> BoardingProcessorPaymentTypes { get; set; }

    // Supplemental (non-card) products
    public virtual DbSet<BoardingDigitalPaymentsSubscription>        BoardingDigitalPaymentsSubscriptions        { get; set; }
    public virtual DbSet<BoardingInvoicingSubscription>              BoardingInvoicingSubscriptions              { get; set; }
    public virtual DbSet<BoardingPayByLinkSubscription>              BoardingPayByLinkSubscriptions              { get; set; }
    public virtual DbSet<BoardingTokenManagementSubscription>        BoardingTokenManagementSubscriptions        { get; set; }
    public virtual DbSet<BoardingUnifiedCheckoutSubscription>        BoardingUnifiedCheckoutSubscriptions        { get; set; }
    public virtual DbSet<BoardingUnifiedCheckoutAllowedCardNetwork>  BoardingUnifiedCheckoutAllowedCardNetworks  { get; set; }
    public virtual DbSet<BoardingValueAddedServicesSubscription>     BoardingValueAddedServicesSubscriptions     { get; set; }
    public virtual DbSet<BoardingVirtualTerminalSubscription>        BoardingVirtualTerminalSubscriptions        { get; set; }
    public virtual DbSet<BoardingVirtualTerminalGlobalPaymentInfo>   BoardingVirtualTerminalGlobalPaymentInfos   { get; set; }
    public virtual DbSet<BoardingVirtualTerminalAcceptedCardType>    BoardingVirtualTerminalAcceptedCardTypes    { get; set; }
    public virtual DbSet<BoardingVirtualTerminalMerchantDefinedField> BoardingVirtualTerminalMerchantDefinedFields { get; set; }
    public virtual DbSet<BoardingVirtualTerminalReceiptInfo>         BoardingVirtualTerminalReceiptInfos         { get; set; }
    public virtual DbSet<BoardingVirtualTerminalReaderInfo>          BoardingVirtualTerminalReaderInfos          { get; set; }

    public virtual DbSet<BoardingPayerAuthenticationSubscription>  BoardingPayerAuthenticationSubscriptions  { get; set; }
    public virtual DbSet<BoardingPayerAuthenticationCardTypeConfig> BoardingPayerAuthenticationCardTypeConfigs { get; set; }
    public virtual DbSet<BoardingPayerAuthenticationCurrency>       BoardingPayerAuthenticationCurrencies      { get; set; }

    // Polymorphic junction linking any supplemental subscription to a transacting merchant (many-to-many)
    public virtual DbSet<BoardingTransactingMerchantProductSubscription> BoardingTransactingMerchantProductSubscriptions { get; set; }
    // =======================================================

    // ******************** Uncomment this method to use Winhost ********************


    /*
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var csb = new SqlConnectionStringBuilder
        {
            DataSource = "tcp:s31.winhost.com",
            InitialCatalog = "DB_166624_b2cnorthwind",
            UserID = "DB_166624_b2cnorthwind_user",
            Password = "Anetpw!999",
            Encrypt = true,
            TrustServerCertificate = true,
            MultipleActiveResultSets = true
        };

        optionsBuilder.UseSqlServer(csb.ConnectionString, o =>
            o.EnableRetryOnFailure()); // optional: connection resiliency
    }
    */

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite(
            "Data Source=" + Path.Combine(AppContext.BaseDirectory, "Data", "B2CNorthwind.sqlite"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AlphabeticalListOfProduct>(entity =>
        {
            entity.ToView("Alphabetical list of products");
        });

        modelBuilder.Entity<AuthTransResponse>(entity =>
        {
            entity.HasOne(d => d.Order).WithMany(p => p.AuthTransResponses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_OrderId");
        });

        modelBuilder.Entity<B2cOrdersQry>(entity =>
        {
            entity.ToView("B2C Orders Qry");
        });

        modelBuilder.Entity<CategorySalesFor1997>(entity =>
        {
            entity.ToView("Category Sales for 1997");
        });

        modelBuilder.Entity<CurrentProductList>(entity =>
        {
            entity.ToView("Current Product List");

            entity.Property(e => e.ProductId).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(e => e.CustomerId).IsFixedLength();
        });

        modelBuilder.Entity<CustomerAndSuppliersByCity>(entity =>
        {
            entity.ToView("Customer and Suppliers by City");
        });

        modelBuilder.Entity<CustomerCustomerDemo>(entity =>
        {
            entity.Property(e => e.CustomerId).IsFixedLength();
            entity.Property(e => e.CustomerTypeId).IsFixedLength();
        });

        modelBuilder.Entity<CustomerDemographic>(entity =>
        {
            entity.Property(e => e.CustomerTypeId).IsFixedLength();
        });

        modelBuilder.Entity<ElectronicProduct>(entity =>
        {
            entity.HasKey(e => e.ElectronicProductId).HasName("PK_ElectronicProductId");
        });

        modelBuilder.Entity<ErrorObject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ErrorObj__3214EC0744F783FC");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
        });

        modelBuilder.Entity<FollowOnTransResponse>(entity =>
        {
            entity.HasOne(d => d.Order).WithMany(p => p.FollowOnTransResponses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FollowOnTrans_OrderId");
        });

        modelBuilder.Entity<NetworkTokenInfo>(entity =>
        {
            entity.HasKey(e => e.PaymentTokenId).HasName("PK_PaymentTokenId");

            entity.HasOne(d => d.PaymentCard).WithMany(p => p.NetworkTokenInfos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NetworkTokenInfo_PaymentCardId");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.Freight).HasDefaultValue(0m);

            entity.HasOne(d => d.B2cCustomer).WithMany(p => p.Orders).HasConstraintName("FK_Orders_B2cCustomers");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProductId }).HasName("PK_Order_Details");

            entity.Property(e => e.Quantity).HasDefaultValue((short)1);

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Details_Orders");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Details_Products");
        });

        modelBuilder.Entity<OrderDetailsExtended>(entity =>
        {
            entity.ToView("Order Details Extended");
        });

        modelBuilder.Entity<OrderSubtotal>(entity =>
        {
            entity.ToView("Order Subtotals");
        });

        modelBuilder.Entity<OrdersQry>(entity =>
        {
            entity.ToView("Orders Qry");
        });

        modelBuilder.Entity<PaymentCardInfo>(entity =>
        {
            entity.HasOne(d => d.B2cCustomer).WithMany(p => p.PaymentCardInfos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PaymentCardInfo_B2cCustomers");
        });

        modelBuilder.Entity<PaymentCardSampleDatum>(entity =>
        {
            entity.HasKey(e => e.SamplePaymentCardId).HasName("PK_SamplePaymentCardInfo");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.ReorderLevel).HasDefaultValue((short)0);
            entity.Property(e => e.UnitPrice).HasDefaultValue(0m);
            entity.Property(e => e.UnitsInStock).HasDefaultValue((short)0);
            entity.Property(e => e.UnitsOnOrder).HasDefaultValue((short)0);

            entity.HasOne(d => d.Category).WithMany(p => p.Products).HasConstraintName("FK_Products_Categories");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Products).HasConstraintName("FK_Products_Suppliers");
        });

        modelBuilder.Entity<ProductSalesFor1997>(entity =>
        {
            entity.ToView("Product Sales for 1997");
        });

        modelBuilder.Entity<ProductsAboveAveragePrice>(entity =>
        {
            entity.ToView("Products Above Average Price");
        });

        modelBuilder.Entity<ProductsByCategory>(entity =>
        {
            entity.ToView("Products by Category");
        });

        modelBuilder.Entity<QuarterlyOrder>(entity =>
        {
            entity.ToView("Quarterly Orders");
        });

        modelBuilder.Entity<Region>(entity =>
        {
            entity.Property(e => e.RegionDescription).IsFixedLength();
        });

        modelBuilder.Entity<SalesByCategory>(entity =>
        {
            entity.ToView("Sales by Category");
        });

        modelBuilder.Entity<SalesTotalsByAmount>(entity =>
        {
            entity.ToView("Sales Totals by Amount");
        });

        modelBuilder.Entity<SampleInvoiceDetail>(entity =>
        {
            entity.HasKey(e => e.SampleInvoiceId).HasName("PK_SampleSampleInvoiceDetails");
        });

        modelBuilder.Entity<SessionTransactionsStore>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SessionT__3214EC076E547C70");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
        });

        modelBuilder.Entity<ShippingInstAddress>(entity =>
        {
            entity.HasKey(e => e.ShippingInstId).HasName("PK_ShippingInstId");
        });

        modelBuilder.Entity<StandAloneCredit>(entity =>
        {
            entity.HasKey(e => e.StandAloneCreditCardId).HasName("PK_StandAloneCreditCardId");
        });

        modelBuilder.Entity<SummaryOfSalesByQuarter>(entity =>
        {
            entity.ToView("Summary of Sales by Quarter");
        });

        modelBuilder.Entity<SummaryOfSalesByYear>(entity =>
        {
            entity.ToView("Summary of Sales by Year");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
