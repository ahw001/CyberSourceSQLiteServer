using CybsClass.Cybersource.Models.BaseData;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.Cybersource.Transactions;
using CybsClass.EntityModels;
using CybsClass.WebApi.Service.Services.Configs;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace CybsClass.WebApi.Service.Services.MerchantBoarding;

public static class CallCybsTransactingFromSql
{
    public static async Task<JsonObject> RunAsyncJsonObject(
        BoardingTransactingMerchant merchant,
        BoardingCardProductSubscription? cardSubscription,
        List<string> supplementalProductTypes,
        BoardingValueAddedServicesSubscription? vasSubscription = null)
    {
        JsonObject jsonObject = new();
        string resource = "/boarding/v1/registrations";

        var parentOrg  = merchant.BoardingOrganization;
        var bizContact = parentOrg?.Contacts?.FirstOrDefault(c => c.ContactType == "Business");

        try
        {
            var outboundBoardingRoot = new OutboundBoardingRoot
            {
                RegistrationInformation = new RegistrationInformation
                {
                    BoardingFlow      = "ENTERPRISE",
                    Mode              = "COMPLETE",
                    BoardingPackageId = merchant.BoardingPackageId ?? BoardingConfig.BoardingPackageId,
                },
                OrganizationInformation = new OrganizationInformation
                {
                    OrganizationId = merchant.TransactingOrganizationId,
                    Status         = "TEST",
                    BusinessInformation = new BusinessInformation
                    {
                        Address = new AddressClass
                        {
                            Country            = merchant.AddressCountry      ?? parentOrg?.AddressCountry      ?? "US",
                            Address1           = merchant.Address1             ?? parentOrg?.Address1,
                            PostalCode         = merchant.PostalCode           ?? parentOrg?.PostalCode,
                            AdministrativeArea = merchant.AdministrativeArea   ?? parentOrg?.AdministrativeArea,
                            Locality           = merchant.Locality             ?? parentOrg?.Locality,
                        },
                        BusinessContact = new Contact
                        {
                            FirstName   = bizContact?.FirstName,
                            LastName    = bizContact?.LastName,
                            PhoneNumber = bizContact?.PhoneNumber,
                            Email       = bizContact?.Email,
                        },
                        Name                 = merchant.BusinessName ?? parentOrg?.BusinessName,
                        WebsiteUrl           = merchant.WebsiteUrl   ?? parentOrg?.WebsiteUrl,
                        PhoneNumber          = merchant.BusinessPhoneNumber ?? bizContact?.PhoneNumber,
                        TimeZone             = merchant.TimeZone     ?? parentOrg?.TimeZone ?? "America/Chicago",
                        MerchantCategoryCode = merchant.MerchantCategoryCode ?? "5999",
                    },
                    ParentOrganizationId = parentOrg?.OrganizationId,
                    Type                 = "TRANSACTING",
                    Configurable         = false,
                },
                ProductInformation = new ProductInformation
                {
                    SelectedProducts = new SelectedProducts
                    {
                        Payments       = new Payments(),
                        Risk           = new Risk(),
                        OrganizationId = merchant.TransactingOrganizationId,
                    },
                },
            };

            // Card processing from DB — choose processor builder based on processor name
            if (cardSubscription is not null)
            {
                var firstConfig = cardSubscription.CardProcessingConfigs.FirstOrDefault();
                var firstProc   = firstConfig?.ProcessorConfigs.FirstOrDefault();

                if (firstProc is not null)
                {
                    bool isFdiGlobal = (firstProc.ProcessorName ?? "")
                        .Contains("fdiglobal", StringComparison.OrdinalIgnoreCase);

                    outboundBoardingRoot.ProductInformation.SelectedProducts.Payments.CardProcessing =
                        isFdiGlobal
                            ? BuildFdiGlobalCardProcessing(cardSubscription, firstConfig!, firstProc)
                            : BuildVdcVantivCardProcessing(cardSubscription, firstConfig!, firstProc);
                }
            }

            // Supplemental products from DB product-type list
            if (supplementalProductTypes.Contains(BoardingProductTypes.CustomerInvoicing))
            {
                outboundBoardingRoot.ProductInformation.SelectedProducts.Payments.CustomerInvoicing =
                    new SimpleServiceConfig
                    {
                        SubscriptionInformation = new SubscriptionInformation
                        {
                            Enabled            = true,
                            SelfServiceability = "NOT_SELF_SERVICEABLE",
                        },
                    };
            }
            if (supplementalProductTypes.Contains(BoardingProductTypes.PayByLink))
            {
                outboundBoardingRoot.ProductInformation.SelectedProducts.Payments.PayByLink =
                    new SimpleServiceConfig
                    {
                        SubscriptionInformation = new SubscriptionInformation
                        {
                            Enabled            = true,
                            SelfServiceability = "NOT_SELF_SERVICEABLE",
                        },
                    };
            }
            if (supplementalProductTypes.Contains(BoardingProductTypes.VirtualTerminal))
            {
                outboundBoardingRoot.ProductInformation.SelectedProducts.Payments.VirtualTerminal =
                    new SimpleServiceConfig
                    {
                        SubscriptionInformation = new SubscriptionInformation { Enabled = true },
                        ConfigurationInformation = new ConfigurationInformation
                        {
                            TemplateId     = BoardingConfig.VirtualTerminalTemplateId,
                            Configurations = new Configurations
                            {
                                Common = new Common { CardProcessingType = "CARD_NOT_PRESENT" }
                            },
                        },
                    };
            }
            if (supplementalProductTypes.Contains(BoardingProductTypes.UnifiedCheckout))
            {
                outboundBoardingRoot.ProductInformation.SelectedProducts.Payments.UnifiedCheckout =
                    new SimpleServiceConfig
                    {
                        SubscriptionInformation = new SubscriptionInformation
                        {
                            Enabled            = true,
                            SelfServiceability = "NOT_SELF_SERVICEABLE",
                        },
                    };
            }
            if (supplementalProductTypes.Contains(BoardingProductTypes.PayerAuthentication))
            {
                outboundBoardingRoot.ProductInformation.SelectedProducts.Payments.PayerAuthentication =
                    BuildPayerAuthentication();
            }
            if (supplementalProductTypes.Contains(BoardingProductTypes.TokenManagement))
            {
                outboundBoardingRoot.ProductInformation.SelectedProducts.CommerceSolutions =
                    new CommerceSolutions
                    {
                        TokenManagement = new SimpleServiceConfig
                        {
                            SubscriptionInformation = new SubscriptionInformation
                            {
                                Enabled            = true,
                                SelfServiceability = "NOT_SELF_SERVICEABLE",
                            },
                            ConfigurationInformation = new ConfigurationInformation
                            {
                                TemplateId     = BoardingConfig.TokenManagementTemplateId,
                                Configurations = new Configurations
                                {
                                    Common = new Common { CardProcessingType = "CARD_NOT_PRESENT" }
                                },
                            },
                        },
                    };
            }
            if (supplementalProductTypes.Contains(BoardingProductTypes.ValueAddedServices))
            {
                outboundBoardingRoot.ProductInformation.SelectedProducts.ValueAddedServices =
                    BuildValueAddedServices(vasSubscription);
            }

            var options = new JsonSerializerOptions
            {
                WriteIndented          = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            };
            string jsonString = JsonSerializer.Serialize(outboundBoardingRoot, options);

            Console.WriteLine("\n************* CALLING FOR TRANSACTING LEVEL MERCHANT BOARDING FROM SQL *****\n");
            Console.WriteLine($"\n[SubmitTransactingFromSql] REQUEST JSON: {jsonString}");

            jsonObject = await CallCyberSource.CallCyberSourceApiJson(jsonString, resource, true);
        }
        catch (Exception e)
        {
            Console.WriteLine($"[SubmitTransactingFromSql] ERROR: {e.Message}");
            jsonObject = new JsonObject();
            jsonObject.Add("error", e.Message);
        }

        Console.WriteLine($"\n[SubmitTransactingFromSql] RESPONSE JSON: {jsonObject.ToJsonString()}");
        return jsonObject;
    }

    public static async Task<JsonObject> RunAsyncProductsOnlyJsonObject(
        BoardingTransactingMerchant merchant,
        BoardingCardProductSubscription? cardSubscription,
        List<string> supplementalProductTypes,
        BoardingValueAddedServicesSubscription? vasSubscription = null)
    {
        JsonObject jsonObject = new();
        string resource = $"/boarding/v1/registrations/{merchant.TransactingOrganizationId}";

        try
        {
            // PATCH /boarding/v1/registrations/{id} expects the selectedProducts body directly —
            // no registrationInformation or productInformation wrapper.
            var selectedProducts = new SelectedProducts
            {
                Payments       = new Payments(),
                OrganizationId = merchant.TransactingOrganizationId,
            };

            if (cardSubscription is not null)
            {
                var firstConfig = cardSubscription.CardProcessingConfigs.FirstOrDefault();
                var firstProc   = firstConfig?.ProcessorConfigs.FirstOrDefault();

                if (firstProc is not null)
                {
                    bool isFdiGlobal = (firstProc.ProcessorName ?? "")
                        .Contains("fdiglobal", StringComparison.OrdinalIgnoreCase);

                    selectedProducts.Payments.CardProcessing =
                        isFdiGlobal
                            ? BuildFdiGlobalCardProcessing(cardSubscription, firstConfig!, firstProc)
                            : BuildVdcVantivCardProcessing(cardSubscription, firstConfig!, firstProc);
                }
            }

            if (supplementalProductTypes.Contains(BoardingProductTypes.CustomerInvoicing))
            {
                selectedProducts.Payments.CustomerInvoicing =
                    new SimpleServiceConfig { SubscriptionInformation = new SubscriptionInformation { Enabled = true, SelfServiceability = "NOT_SELF_SERVICEABLE" } };
            }
            if (supplementalProductTypes.Contains(BoardingProductTypes.PayByLink))
            {
                selectedProducts.Payments.PayByLink =
                    new SimpleServiceConfig { SubscriptionInformation = new SubscriptionInformation { Enabled = true, SelfServiceability = "NOT_SELF_SERVICEABLE" } };
            }
            if (supplementalProductTypes.Contains(BoardingProductTypes.VirtualTerminal))
            {
                selectedProducts.Payments.VirtualTerminal =
                    new SimpleServiceConfig
                    {
                        SubscriptionInformation = new SubscriptionInformation { Enabled = true },
                        ConfigurationInformation = new ConfigurationInformation
                        {
                            TemplateId     = BoardingConfig.VirtualTerminalTemplateId,
                            Configurations = new Configurations { Common = new Common { CardProcessingType = "CARD_NOT_PRESENT" } },
                        },
                    };
            }
            if (supplementalProductTypes.Contains(BoardingProductTypes.UnifiedCheckout))
            {
                selectedProducts.Payments.UnifiedCheckout =
                    new SimpleServiceConfig { SubscriptionInformation = new SubscriptionInformation { Enabled = true, SelfServiceability = "NOT_SELF_SERVICEABLE" } };
            }
            if (supplementalProductTypes.Contains(BoardingProductTypes.PayerAuthentication))
            {
                selectedProducts.Payments.PayerAuthentication = BuildPayerAuthentication();
            }
            if (supplementalProductTypes.Contains(BoardingProductTypes.TokenManagement))
            {
                selectedProducts.CommerceSolutions =
                    new CommerceSolutions
                    {
                        TokenManagement = new SimpleServiceConfig
                        {
                            SubscriptionInformation = new SubscriptionInformation { Enabled = true, SelfServiceability = "NOT_SELF_SERVICEABLE" },
                            ConfigurationInformation = new ConfigurationInformation
                            {
                                TemplateId     = BoardingConfig.TokenManagementTemplateId,
                                Configurations = new Configurations { Common = new Common { CardProcessingType = "CARD_NOT_PRESENT" } },
                            },
                        },
                    };
            }
            if (supplementalProductTypes.Contains(BoardingProductTypes.ValueAddedServices))
            {
                selectedProducts.ValueAddedServices = BuildValueAddedServices(vasSubscription);
            }

            var options = new JsonSerializerOptions
            {
                WriteIndented          = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            };
            string jsonString = JsonSerializer.Serialize(selectedProducts, options);

            Console.WriteLine("\n************* CALLING PRODUCTS-ONLY PATCH FOR ALREADY-BOARDED TRANSACTING MERCHANT *****\n");
            Console.WriteLine($"\n[SubmitTransactingProductsFromSql] REQUEST JSON: {jsonString}");

            jsonObject = await CallCyberSource.CallCyberSourceApiJsonPatch(jsonString, resource, true);
        }
        catch (Exception e)
        {
            Console.WriteLine($"[SubmitTransactingProductsFromSql] ERROR: {e.Message}");
            jsonObject = new JsonObject();
            jsonObject.Add("error", e.Message);
        }

        Console.WriteLine($"\n[SubmitTransactingProductsFromSql] RESPONSE JSON: {jsonObject.ToJsonString()}");
        return jsonObject;
    }

    // ── VdcVantiv card processing from DB processor config ────────────────────
    private static CardProcessing BuildVdcVantivCardProcessing(
        BoardingCardProductSubscription sub,
        BoardingCardProcessingConfig config,
        BoardingProcessorConfig proc)
    {
        var paymentTypes = new PaymentTypes();
        foreach (var pt in proc.PaymentTypes.Where(p => p.Enabled == true))
        {
            var currency  = new Dictionary<string, Currency>
            {
                ["USD"] = new Currency
                {
                    Enabled               = true,
                    TerminalId            = pt.TerminalId,
                    EnabledCardPresent    = pt.EnabledCardPresent    ?? true,
                    EnabledCardNotPresent = pt.EnabledCardNotPresent ?? true,
                    MerchantId            = pt.MerchantId,
                }
            };
            var cardType = new CardType { Enabled = true, Currencies = currency };
            switch (pt.PaymentType?.ToUpperInvariant())
            {
                case "VISA":             paymentTypes.VISA             = cardType; break;
                case "MASTERCARD":       paymentTypes.MASTERCARD       = cardType; break;
                case "AMERICAN_EXPRESS": paymentTypes.AMERICAN_EXPRESS = cardType; break;
                case "DISCOVER":         paymentTypes.DISCOVER         = cardType; break;
            }
        }

        return new CardProcessing
        {
            SubscriptionInformation = new SubscriptionInformation
            {
                Enabled  = sub.SubscriptionEnabled,
                Features = new SubscriptionFeatures
                {
                    CardNotPresent = new CardNotPresent { Enabled = sub.CardNotPresentEnabled },
                    CardPresent    = new CardPresent    { Enabled = sub.CardPresentEnabled    },
                },
            },
            ConfigurationInformation = new ConfigurationInformation
            {
                TemplateId     = sub.TemplateId ?? BoardingConfig.CardProcessingTemplateId,
                Configurations = new Configurations
                {
                    Common = new Common
                    {
                        MerchantCategoryCode                           = config.MerchantCategoryCode ?? "5999",
                        DefaultAuthTypeCode                            = config.DefaultAuthTypeCode  ?? "FINAL",
                        EnablePartialAuth                              = config.EnablePartialAuth    ?? false,
                        EnableDuplicateMerchantReferenceNumberBlocking = config.EnableDuplicateRefNumBlocking ?? false,
                        AuthMerchantRetryDisabled                      = config.AuthMerchantRetryDisabled     ?? false,
                        AllowCapturesGreaterThanAuthorizations         = false,
                        EnableInterchangeOptimization                  = false,
                        EnableSplitShipment                            = false,
                        MerchantDescriptorInformation                  = new MerchantDescriptorInformation { CountryOfOrigin = null },
                        GovernmentControlled                           = false,
                        DropBillingInfo                                = false,
                        Processors = new Processors
                        {
                            VdcVantiv = new VdcVantiv
                            {
                                Acquirer = new Acquirer
                                {
                                    CountryCode                = proc.AcquirerCountryCode,
                                    FileDestinationBin         = proc.AcquirerFileDestinationBin,
                                    InterbankCardAssociationId = proc.AcquirerInterbankCardAssociationId,
                                    InstitutionId              = proc.AcquirerInstitutionId,
                                    DiscoverInstitutionId      = proc.AcquirerDiscoverInstitutionId,
                                },
                                PaymentTypes          = paymentTypes,
                                AcquirerMerchantId    = proc.AcquirerMerchantId,
                                AllowMultipleBills    = proc.AllowMultipleBills    ?? false,
                                BatchGroup            = proc.BatchGroup,
                                BusinessApplicationId = proc.BusinessApplicationId,
                                CreditAuthUnsupportedCardTypes  = "all",
                                EnableAutoAuthReversalAfterVoid = proc.EnableAutoAuthReversalAfterVoid ?? true,
                                EnableExpresspayPanTranslation  = proc.EnableExpresspayPanTranslation  ?? true,
                                QuasiCash                       = proc.QuasiCash                       ?? false,
                                EnableTransactionReferenceNumber = proc.EnableTransactionReferenceNumber ?? true,
                                EnableDynamicCurrencyConversion  = false,
                            },
                        },
                    },
                    Features = new Features
                    {
                        VantivCardPresent = new VantivCardPresent
                        {
                            Processors = new ProcessorsCardPresent
                            {
                                Vdcvantiv = new VdcvantivCardPresent
                                {
                                    DisablePointOfSaleTerminalIdValidation = proc.DisablePointOfSaleTerminalIdValidation ?? true,
                                    EnablePinTranslation                   = proc.EnablePinTranslation                   ?? false,
                                    DefaultPointOfSaleTerminalId           = proc.DefaultPointOfSaleTerminalId,
                                    PointOfSaleTerminalIds                 = proc.PointOfSaleTerminalIds,
                                }
                            },
                        },
                        VantivCardNotPresent = new VatntivCardNotPresent
                        {
                            Processors = new ProcessorsCardNotPresent
                            {
                                Vdcvantiv = new VdcvantivCardNotPresent
                                {
                                    EnableEmsTransactionRiskScore                        = proc.EnableEmsTransactionRiskScore          ?? false,
                                    RelaxAddressVerificationSystem                       = proc.RelaxAddressVerificationSystem         ?? true,
                                    RelaxAddressVerificationSystemAllowExpiredCard       = proc.RelaxAvsAllowExpiredCard               ?? true,
                                    RelaxAddressVerificationSystemAllowZipWithoutCountry = proc.RelaxAvsAllowZipWithoutCountry          ?? true,
                                }
                            },
                            VisaStraightThroughProcessingOnly = config.VisaStraightThroughProcessingOnly ?? false,
                            IgnoreAddressVerificationSystem   = config.IgnoreAddressVerificationSystem   ?? true,
                        },
                    },
                },
            },
        };
    }

    // ── FdiGlobal card processing from DB processor config ────────────────────
    private static CardProcessing BuildFdiGlobalCardProcessing(
        BoardingCardProductSubscription sub,
        BoardingCardProcessingConfig config,
        BoardingProcessorConfig proc)
    {
        var fdiPaymentTypes = new FdiPaymentTypes();
        foreach (var pt in proc.PaymentTypes.Where(p => p.Enabled == true))
        {
            var flag = new EnabledFlag { Enabled = true };
            switch (pt.PaymentType?.ToUpperInvariant())
            {
                case "VISA":             fdiPaymentTypes.Visa            = flag; break;
                case "MASTERCARD":       fdiPaymentTypes.MasterCard      = flag; break;
                case "AMERICAN_EXPRESS": fdiPaymentTypes.AmericanExpress = flag; break;
                case "DISCOVER":         fdiPaymentTypes.Discover        = flag; break;
                case "DINERS_CLUB":      fdiPaymentTypes.DinersClub      = flag; break;
                case "PIN_DEBIT":
                    fdiPaymentTypes.PinDebit = new PinDebit
                    {
                        Enabled = true,
                        Currencies = new PinDebitCurrencies
                        {
                            USD = new PinDebitUsd
                            {
                                Enabled               = true,
                                TerminalId            = pt.TerminalId,
                                EnabledCardPresent    = pt.EnabledCardPresent    ?? false,
                                EnabledCardNotPresent = pt.EnabledCardNotPresent ?? false,
                                MerchantId            = pt.MerchantId,
                            }
                        }
                    };
                    break;
            }
        }

        return new CardProcessing
        {
            SubscriptionInformation = new SubscriptionInformation
            {
                Enabled  = sub.SubscriptionEnabled,
                Features = new SubscriptionFeatures
                {
                    CardNotPresent = new CardNotPresent { Enabled = sub.CardNotPresentEnabled },
                    CardPresent    = new CardPresent    { Enabled = sub.CardPresentEnabled    },
                },
            },
            ConfigurationInformation = new ConfigurationInformation
            {
                TemplateId     = sub.TemplateId ?? BoardingConfig.CardProcessingTemplateId,
                Configurations = new Configurations
                {
                    Common = new Common
                    {
                        MerchantCategoryCode                           = config.MerchantCategoryCode ?? "5999",
                        DefaultAuthTypeCode                            = config.DefaultAuthTypeCode  ?? "final",
                        EnablePartialAuth                              = config.EnablePartialAuth    ?? true,
                        EnableDuplicateMerchantReferenceNumberBlocking = config.EnableDuplicateRefNumBlocking ?? false,
                        AuthMerchantRetryDisabled                      = config.AuthMerchantRetryDisabled     ?? false,
                        Processors = new Processors
                        {
                            FdiGlobal = new FdiGlobalProcessor
                            {
                                Acquirer  = new Dictionary<string, object?>(),
                                Currencies = new ProcessorCurrencies
                                {
                                    USD = new Currency
                                    {
                                        Enabled               = proc.CurrencyUsdEnabled               ?? true,
                                        EnabledCardPresent    = proc.CurrencyUsdEnabledCardPresent    ?? true,
                                        EnabledCardNotPresent = proc.CurrencyUsdEnabledCardNotPresent ?? true,
                                        MerchantId            = proc.CurrencyUsdMerchantId,
                                        TerminalId            = proc.CurrencyUsdTerminalId,
                                        TerminalIds           = string.IsNullOrWhiteSpace(proc.CurrencyUsdTerminalIds)
                                            ? null
                                            : proc.CurrencyUsdTerminalIds
                                                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                                                .Select(t => (string?)t)
                                                .ToList(),
                                    },
                                },
                                PaymentTypes                     = fdiPaymentTypes,
                                BatchGroup                       = proc.BatchGroup,
                                EnablePosNetworkSwitching        = proc.EnablePosNetworkSwitching        ?? false,
                                EnableTransactionReferenceNumber = proc.EnableTransactionReferenceNumber ?? true,
                            },
                        },
                    },
                    Features = new Features
                    {
                        VantivCardPresent = new VantivCardPresent
                        {
                            Processors = new ProcessorsCardPresent
                            {
                                FdiGlobal = new CardPresentFdiGlobal
                                {
                                    PinDebitEnablePartialAuth = proc.PinDebitEnablePartialAuth ?? false,
                                }
                            },
                        },
                        VantivCardNotPresent = new VatntivCardNotPresent
                        {
                            Processors = new ProcessorsCardNotPresent
                            {
                                FdiGlobal = new CardNotPresentFdiGlobal
                                {
                                    RelaxAddressVerificationSystem                       = proc.RelaxAddressVerificationSystem ?? true,
                                    RelaxAddressVerificationSystemAllowExpiredCard       = proc.RelaxAvsAllowExpiredCard        ?? true,
                                    RelaxAddressVerificationSystemAllowZipWithoutCountry = proc.RelaxAvsAllowZipWithoutCountry  ?? false,
                                }
                            },
                            VisaStraightThroughProcessingOnly = config.VisaStraightThroughProcessingOnly ?? false,
                            IgnoreAddressVerificationSystem   = config.IgnoreAddressVerificationSystem   ?? false,
                        },
                    },
                },
            },
        };
    }

    private static ValueAddedServices BuildValueAddedServices(BoardingValueAddedServicesSubscription? vas) =>
        new ValueAddedServices
        {
            TransactionSearch = new TransactionSearch
            {
                SubscriptionInformation = new SubscriptionInformation
                {
                    Enabled            = vas?.TransactionSearchEnabled ?? true,
                    SelfServiceability = vas?.TransactionSearchSelfServiceability ?? "NOT_SELF_SERVICEABLE",
                },
            },
            Reporting = new Reporting
            {
                SubscriptionInformation = new SubscriptionInformation
                {
                    Enabled            = vas?.ReportingEnabled ?? true,
                    SelfServiceability = vas?.ReportingSelfServiceability ?? "NOT_SELF_SERVICEABLE",
                },
            },
        };

    private static PayerAuthentication BuildPayerAuthentication() =>
        new PayerAuthentication
        {
            SubscriptionInformation = new SubscriptionInformation { Enabled = true },
            PaConfigurationInformation = new PaConfigurationInformation
            {
                TemplateId = BoardingConfig.PayerAuthenticationTemplateId,
                PaConfigurations = new PaConfigurations
                {
                    PaCardTypes = new PaCardTypes
                    {
                        VerifiedByVisa = new PaCardType
                        {
                            Currencies = new List<Currencies>
                            {
                                new Currencies { CurrencyCodes = new List<string> { "ALL" }, AcquirerId = "cybersource", ProcessorMerchantId = "StandardCATestCases" },
                            },
                        },
                        AmexSafeKey = new PaCardType
                        {
                            Currencies = new List<Currencies>
                            {
                                new Currencies { CurrencyCodes = new List<string> { "ALL" }, AcquirerId = "cybersource", ProcessorMerchantId = "StandardCATestCases" },
                            },
                        },
                        MasterCardSecureCode = new PaCardType
                        {
                            Currencies = new List<Currencies>
                            {
                                new Currencies { CurrencyCodes = new List<string> { "ALL" }, AcquirerId = "cybersource", ProcessorMerchantId = "StandardCATestCases" },
                            },
                        },
                        DinersClubInternationalProtectBuy = new PaCardType
                        {
                            Currencies = new List<Currencies>
                            {
                                new Currencies { CurrencyCodes = new List<string> { "ALL" }, AcquirerId = "cybersource", ProcessorMerchantId = "StandardCATestCases" },
                            },
                        },
                    },
                },
            },
        };
}
