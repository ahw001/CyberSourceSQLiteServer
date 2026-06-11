using CybsClass.Cybersource.Models.BaseData;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.Cybersource.Transactions;
using CybsClass.WebApi.Service.Services.Configs;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;


namespace CybsClass.WebApi.Service.Services.MerchantBoarding
{
    public static class CallCybsMerchantTransactingCreateFdiGlobal
    {

        // This class populates the Authorization object for serialization (fdiglobal processor)
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
                            Payments       = new Payments(),
                            Risk           = new Risk(),
                            OrganizationId = merchantModelDto.OrganizationId,
                        },
                    },
                };
                if (merchantModelDto.CardProcessing)
                {
                    outboundBoardingRoot.ProductInformation.SelectedProducts.Payments.CardProcessing =
                        BuildFdiGlobalCardProcessingAsync(merchantModelDto);
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

                Console.WriteLine("\n************* CALLING FOR TRANSACTING LEVEL MERCHANT BOARDING (fdiglobal) *****\n");

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
        private static CardProcessing BuildFdiGlobalCardProcessingAsync(MerchantModelDto dto)
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
                                FdiGlobal = new FdiGlobalProcessor
                                {
                                    Acquirer = new Dictionary<string, object?>(),
                                    Currencies = new ProcessorCurrencies
                                    {
                                        USD = new Currency { Enabled = true }
                                    },
                                    PaymentTypes = new FdiPaymentTypes
                                    {
                                        Visa = new EnabledFlag { Enabled = true },
                                        MasterCard = new EnabledFlag { Enabled = true },
                                        AmericanExpress = new EnabledFlag { Enabled = true },
                                        Discover = new EnabledFlag { Enabled = true },
                                        DinersClub = new EnabledFlag { Enabled = true },
                                    },
                                    BatchGroup = "fdiglobal_test",
                                    EnablePosNetworkSwitching = false,
                                    EnableTransactionReferenceNumber = true,
                                },
                            },
                        },
                        Features = new Features
                        {
                            VantivCardPresent = new VantivCardPresent
                            {
                                Processors = new ProcessorsCardPresent
                                {
                                    FdiGlobal = new CardPresentFdiGlobal { PinDebitEnablePartialAuth = false }
                                },
                            },
                            VantivCardNotPresent = new VatntivCardNotPresent
                            {
                                Processors = new ProcessorsCardNotPresent
                                {
                                    FdiGlobal = new CardNotPresentFdiGlobal
                                    {
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
