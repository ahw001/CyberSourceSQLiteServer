using CybsClass.Cybersource.Models.DTOs;
using CybsClass.Cybersource.Models.OutboundTransObjects;
using CybsClass.Cybersource.Models.BaseData;
using System.Text.Json;
using System.Text.Json.Nodes;


namespace CybsClass.Cybersource.Transactions
{
    public static class CallForCybsStandAloneCredit
    {

        // This class populates the Stand Alone Credit object for serialization
        public static async Task<JsonObject> RunAsyncJsonObject(B2cCustomerDto b2cCustomerDto)
        {
            JsonObject jsonObject = new JsonObject();
            string resource = $"/pts/v2/credits";

            try
            {

                var creditData = new CreditData
                {


                    ClientReferenceInformation = new ClientReferenceInformation
                    {
                        Code = "ABC123"

                    },

                    PaymentInformation = new Dictionary<string, FullCard>
                    {

                        ["card"] = new FullCard
                        {
                            Number = b2cCustomerDto.AccountNumber,
                            ExpirationMonth = b2cCustomerDto.ExpMonth,
                            ExpirationYear = b2cCustomerDto.ExpYear,
                            SecurityCode = b2cCustomerDto.Cvv,
                            Type = b2cCustomerDto.CardType
                        },

                    },

                    OrderInformation = new OrderInformation
                    {

                        AmountDetails = new AmountDetails { Currency = "USD", TotalAmount = Convert.ToString(b2cCustomerDto.TotalAmount) }, //Convert.ToString(b2cCustomerDto.TotalAmount)

                        BillTo = new BillTo { FirstName = b2cCustomerDto.FirstName, LastName = b2cCustomerDto.LastName, Address1 = b2cCustomerDto.Address1, Locality = b2cCustomerDto.City, AdministrativeArea = b2cCustomerDto.AdministrativeArea, PostalCode = b2cCustomerDto.PostalCode, Country = "US", Email = b2cCustomerDto.Email, PhoneNumber = b2cCustomerDto.Phone }

                    }

                };

                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(creditData, options);

                Console.WriteLine("\n************* CALLING FOR STAND ALONE CREDIT *****\n");

                jsonObject = await CallCyberSource.CallCyberSourceApiJson(jsonString, resource, false);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                jsonObject = new JsonObject();
                jsonObject["error"] = e.Message;
                return jsonObject;
            }

            return jsonObject;
        }




    }
}