using CybsClass.Cybersource.Models.BaseData;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.Cybersource.Transactions;
using CybsClass.WebApi.Service.Services.Configs;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;


namespace CybsClass.WebApi.Service.Services.MerchantBoarding
{
    public static class CallCybsMerchantTransactingCreate
    {

        // This class populates the Authorization object for serialization
        public static async Task<JsonObject> RunAsyncJsonObject(MerchantModelDto merchantModelDto)
        {
            JsonObject jsonObject = new JsonObject();
            string resource = "/boarding/v1/registrations";
            string transactingId = string.Empty;

            if (merchantModelDto is not null && merchantModelDto.TransactingOrganizationId is not null)
            {
                transactingId = merchantModelDto.TransactingOrganizationId;
            }
            else
            {
                transactingId = merchantModelDto!.OrganizationId!;
            }

            try
            {
                var outboundBoardingRoot = new OutboundBoardingRoot
                {
                    RegistrationInformation = new RegistrationInformation
                    {
                        BoardingFlow = "ENTERPRISE",
                        Mode = "COMPLETE",
                        BoardingPackageId = BoardingConfig.BoardingPackageId,
                    },
                    OrganizationInformation = new OrganizationInformation
                    {
                        OrganizationId = transactingId,
                        Status = "TEST",
                        BusinessInformation = new BusinessInformation
                        {
                            Address = new AddressClass
                            {
                                Country = merchantModelDto.Country,
                                Address1 = merchantModelDto.Address1,
                                PostalCode = merchantModelDto.PostalCode,
                                AdministrativeArea = merchantModelDto.AdministrativeArea,
                                Locality = merchantModelDto.Locality,
                            },
                            BusinessContact = new Contact
                            {
                                FirstName = merchantModelDto.BusinessContactFirstName,
                                LastName = merchantModelDto.BusinessContactLastName,
                                PhoneNumber = merchantModelDto.BusinessContactPhoneNumber,
                                Email = merchantModelDto.BusinessContactEmail,
                            },
                            Name = merchantModelDto.Name,
                            WebsiteUrl = merchantModelDto.WebsiteUrl,
                            PhoneNumber = merchantModelDto.BusinessContactPhoneNumber,
                            TimeZone = merchantModelDto.BusinessInformationTimeZone,
                            MerchantCategoryCode = "5999"
                        },
                        ParentOrganizationId = merchantModelDto.ParentOrganizationId,
                        Type = merchantModelDto.Type,
                        Configurable = false,
                    },
                    ProductInformation = new ProductInformation
                    {
                        SelectedProducts = new SelectedProducts
                        {
                            Payments = new Payments
                            {

                            },
                            Risk = new Risk { },
                            ValueAddedServices = new ValueAddedServices
                            {
                                TransactionSearch = new TransactionSearch
                                {
                                    SubscriptionInformation = new SubscriptionInformation
                                    {
                                        Enabled = true,
                                        SelfServiceability = "NOT_SELF_SERVICEABLE",

                                    },
                                },
                                Reporting = new Reporting
                                {
                                    SubscriptionInformation = new SubscriptionInformation
                                    {
                                        Enabled = true,
                                        SelfServiceability = "NOT_SELF_SERVICEABLE",

                                    },
                                },
                            },
                            OrganizationId = merchantModelDto.OrganizationId,
                        },
                    },
                };
                if (merchantModelDto.CardProcessing)
                {
                    outboundBoardingRoot.ProductInformation.SelectedProducts.Payments.CardProcessing =
                        BuildVdcVantivCardProcessingAsync(merchantModelDto);
                }
                if (merchantModelDto.CustomerInvoicing)
                {
                    outboundBoardingRoot.ProductInformation.SelectedProducts.Payments.CustomerInvoicing = new SimpleServiceConfig
                    {
                        SubscriptionInformation = new SubscriptionInformation
                        {
                            Enabled = true,
                        },
                    };
                }
                if (merchantModelDto.VirtualTerminal)
                {
                    outboundBoardingRoot.ProductInformation.SelectedProducts.Payments.VirtualTerminal = new SimpleServiceConfig
                    {
                        SubscriptionInformation = new SubscriptionInformation
                        {
                            Enabled = true,
                        },
                        ConfigurationInformation = new ConfigurationInformation
                        {
                            TemplateId = BoardingConfig.VirtualTerminalTemplateId,
                            Configurations = new Configurations
                            {
                                Common = new Common
                                {
                                    CardProcessingType = "CARD_NOT_PRESENT",
                                }
                            },
                        },
                    };
                }
                if (merchantModelDto.PayByLink)
                {
                    outboundBoardingRoot.ProductInformation.SelectedProducts.Payments.PayByLink = new SimpleServiceConfig
                    {
                        SubscriptionInformation = new SubscriptionInformation
                        {
                            Enabled = true,
                        },
                    };
                }
                if (merchantModelDto.UnifiedCheckout)
                {
                    outboundBoardingRoot.ProductInformation.SelectedProducts.Payments.UnifiedCheckout = new SimpleServiceConfig
                    {
                        SubscriptionInformation = new SubscriptionInformation
                        {
                            Enabled = true,
                        },
                    };
                }
                if (merchantModelDto.CybsReadyTerminal)
                {
                    outboundBoardingRoot.ProductInformation.SelectedProducts.Payments.CybsReadyTerminal = new SimpleServiceConfig
                    {
                        SubscriptionInformation = new SubscriptionInformation
                        {
                            Enabled = true,
                        },
                    };
                }
                if (merchantModelDto.Tax)
                {
                    outboundBoardingRoot.ProductInformation.SelectedProducts.Payments.Tax = new SimpleServiceConfig
                    {
                        SubscriptionInformation = new SubscriptionInformation
                        {
                            Enabled = true,
                        },
                    };
                }
                if (merchantModelDto.PayerAuthentication)
                {
                    outboundBoardingRoot.ProductInformation.SelectedProducts.Payments.PayerAuthentication = new PayerAuthentication
                    {
                        SubscriptionInformation = new SubscriptionInformation
                        {
                            Enabled = true,
                        },
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
                                            new Currencies
                                            {
                                                CurrencyCodes = new List<string> { "ALL" },
                                                AcquirerId = "cybersource",
                                                ProcessorMerchantId = "StandardCATestCases"
                                            },
                                        },

                                    },
                                    AmexSafeKey = new PaCardType
                                    {
                                        Currencies = new List<Currencies>
                                        {
                                            new Currencies
                                            {
                                                CurrencyCodes = new List<string> { "ALL" },
                                                AcquirerId = "cybersource",
                                                ProcessorMerchantId = "StandardCATestCases"
                                            },
                                        },
                                    },
                                    MasterCardSecureCode = new PaCardType
                                    {
                                        Currencies = new List<Currencies>
                                        {
                                            new Currencies
                                            {
                                                CurrencyCodes = new List<string> { "ALL" },
                                                AcquirerId = "cybersource",
                                                ProcessorMerchantId = "StandardCATestCases"
                                            },
                                        },
                                    },
                                    DinersClubInternationalProtectBuy = new PaCardType
                                    {
                                        Currencies = new List<Currencies>
                                        {
                                            new Currencies
                                            {
                                                CurrencyCodes = new List<string> { "ALL" },
                                                AcquirerId = "cybersource",
                                                ProcessorMerchantId = "StandardCATestCases"
                                            },
                                        },
                                    },
                                },
                            },
                        },
                    };
                }
                if (merchantModelDto.TokenManagementService)
                {
                    outboundBoardingRoot.ProductInformation.SelectedProducts.CommerceSolutions = new CommerceSolutions
                    {
                        TokenManagement = new SimpleServiceConfig
                        {
                            SubscriptionInformation = new SubscriptionInformation
                            {
                                Enabled = true,
                            },
                            ConfigurationInformation = new ConfigurationInformation
                            {
                                TemplateId = BoardingConfig.TokenManagementTemplateId,
                                Configurations = new Configurations
                                {
                                    Common = new Common
                                    {
                                        CardProcessingType = "CARD_NOT_PRESENT",
                                    }
                                },
                            },
                        },
                    };
                }


                var options = new JsonSerializerOptions { WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
                string jsonString = JsonSerializer.Serialize(outboundBoardingRoot, options);

                /* USE IF YOU NEED TO MODIFIY JSON ---------
                 * 
                JsonNode jsonNode = JsonNode.Parse(jsonString);

                // Navigate to the 'ProcessingInformation' object
                JsonNode processingInfo = jsonNode["ProcessingInformation"];

                // Check if the 'Capture' field exists and remove it
                if (processingInfo["Capture"] != null)
                {
                    processingInfo.AsObject().Remove("Capture");
                }

                // Convert the JsonNode back to a string to see the result
                string modifiedJsonString = jsonNode.ToJsonString();
                Console.WriteLine($"MODIFIED STRING WITHOUT Capture: { modifiedJsonString}");
                *
                */

                Console.WriteLine("\n************* CALLING FOR TRANSACTING LEVEL MERCHANT BOARDING *****\n");

                jsonObject = await CallCyberSource.CallCyberSourceApiJson(jsonString, resource, true);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                jsonObject = new JsonObject();
                jsonObject.Add("error", e.Message);
                return jsonObject;
            }

            return jsonObject;
        }

        // ─────────────────────────────────────────────────────────────
        //  Build CardProcessing: hardcoded defaults.
        // ─────────────────────────────────────────────────────────────
        private static CardProcessing BuildVdcVantivCardProcessingAsync(MerchantModelDto dto)
        {
            return new CardProcessing
            {
                SubscriptionInformation = new SubscriptionInformation
                {
                    Enabled = true,
                    Features = new SubscriptionFeatures
                    {
                        CardNotPresent = new CardNotPresent { Enabled = true },
                        CardPresent = new CardPresent { Enabled = true },
                    },
                },
                ConfigurationInformation = new ConfigurationInformation
                {
                    TemplateId = BoardingConfig.CardProcessingTemplateId,
                    Configurations = new Configurations
                    {
                        Common = new Common
                        {
                            MerchantCategoryCode = "5999",
                            DefaultAuthTypeCode = "FINAL",
                            EnablePartialAuth = false,
                            EnableSplitShipment = false,
                            EnableInterchangeOptimization = false,
                            AllowCapturesGreaterThanAuthorizations = false,
                            EnableDuplicateMerchantReferenceNumberBlocking = false,
                            MerchantDescriptorInformation = new MerchantDescriptorInformation { CountryOfOrigin = null },
                            GovernmentControlled = false,
                            DropBillingInfo = false,
                            Processors = new Processors
                            {
                                VdcVantiv = new VdcVantiv
                                {
                                    Acquirer = new Acquirer
                                    {
                                        CountryCode = "840_usa",
                                        FileDestinationBin = "444500",
                                        InterbankCardAssociationId = "3684",
                                        InstitutionId = "444571",
                                        DiscoverInstitutionId = "1345678911",
                                    },
                                    PaymentTypes = new PaymentTypes
                                    {
                                        MASTERCARD = new CardType { Enabled = true, Currencies = new Dictionary<string, Currency> { { "USD", new Currency { Enabled = true, TerminalId = "12345678", EnabledCardPresent = true, EnabledCardNotPresent = true, MerchantId = "123456789012345" } } } },
                                        DISCOVER   = new CardType { Enabled = true, Currencies = new Dictionary<string, Currency> { { "USD", new Currency { Enabled = true, TerminalId = "12345678", EnabledCardPresent = true, EnabledCardNotPresent = true, MerchantId = "123456789012345" } } } },
                                        VISA       = new CardType { Enabled = true, Currencies = new Dictionary<string, Currency> { { "USD", new Currency { Enabled = true, TerminalId = "12345678", EnabledCardPresent = true, EnabledCardNotPresent = true, MerchantId = "123456789012345" } } } },
                                        AMERICAN_EXPRESS = new CardType { Enabled = true, Currencies = new Dictionary<string, Currency> { { "USD", new Currency { Enabled = true, TerminalId = "12345678", EnabledCardPresent = true, EnabledCardNotPresent = true, MerchantId = "123456789012345" } } } },
                                    },
                                    AcquirerMerchantId = "123456789012345",
                                    AllowMultipleBills = false,
                                    BatchGroup = "vdcvantiv_test",
                                    BusinessApplicationId = "01",
                                    CreditAuthUnsupportedCardTypes = "all",
                                    EnableAutoAuthReversalAfterVoid = true,
                                    EnableExpresspayPanTranslation = true,
                                    QuasiCash = false,
                                    EnableTransactionReferenceNumber = true,
                                    EnableDynamicCurrencyConversion = false,
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
                                        DisablePointOfSaleTerminalIdValidation = true,
                                        EnablePinTranslation = false,
                                        DefaultPointOfSaleTerminalId = "12345678",
                                        PointOfSaleTerminalIds = "12345678",
                                    }
                                },
                            },
                            VantivCardNotPresent = new VatntivCardNotPresent
                            {
                                Processors = new ProcessorsCardNotPresent
                                {
                                    Vdcvantiv = new VdcvantivCardNotPresent
                                    {
                                        EnableEmsTransactionRiskScore = false,
                                        RelaxAddressVerificationSystem = true,
                                        RelaxAddressVerificationSystemAllowExpiredCard = true,
                                        RelaxAddressVerificationSystemAllowZipWithoutCountry = true,
                                    }
                                },
                                VisaStraightThroughProcessingOnly = false,
                                IgnoreAddressVerificationSystem = true,
                            },
                        },
                    },
                },
            };
        }
    }
}