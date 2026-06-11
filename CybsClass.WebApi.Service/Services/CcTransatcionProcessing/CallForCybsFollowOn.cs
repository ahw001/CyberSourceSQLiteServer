using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Threading.Tasks;
using CybsClass.Cybersource.Transactions;
using CybsClass.Cybersource.Models.OutboundTransObjects;
using CybsClass.Cybersource.Models.BaseData;
using System.Text.Json.Serialization;

namespace CybsClass.WebApi.Service.Services.CcTransatcionProcessing
{
    internal class CallForCybsFollowOn
    {
        public static async Task<JsonObject> RunAsyncFollowOnJsonObject(string id, string amount, string transaction)
        {
            JsonObject jsonObject = new JsonObject();

            string jsonString = string.Empty;

            ReversalData reversalData = new ReversalData();
            CaptureData CaptureData = new CaptureData();
            RefundData refundData = new RefundData();
            VoidData voidData = new VoidData();

            var options = new JsonSerializerOptions { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

            try
            {
                if (transaction == "REVERSAL")
                {
                    reversalData = fillReversalInformation(amount);
                    jsonString = JsonSerializer.Serialize(reversalData, options);

                    Console.WriteLine("\n************* CALLING FOR AUTH REVERSAL *****\n");
                }

                if (transaction == "CAPTURE")
                {
                    Console.WriteLine("\n************* CALLING FOR CAPTURE *****\n");

                    CaptureData = fillCaptureData(amount);
                    jsonString = JsonSerializer.Serialize(CaptureData, options);

                }

                if (transaction.Contains("REFUND_SALE") || transaction.Contains("REFUND_CAPTURE"))
                {
                    if (transaction.Contains("REFUND_SALE"))
                    {
                        Console.WriteLine("\n************* CALLING FOR REFUND_SALE *****\n");
                    }
                    else if (transaction.Contains("REFUND_CAPTURE"))
                    {
                        Console.WriteLine("\n************* CALLING FOR REFUND_CAPTURE *****\n");
                    }

                    refundData = fillRefundData(amount);
                    jsonString = JsonSerializer.Serialize(refundData, options);

                }


                // TODO FIX VOID_CAPTURE
                if (transaction.Contains("VOID_CAPTURE") || transaction.Contains("VOID_CREDIT") || transaction.Contains("VOID_REFUND") || transaction.Contains("VOID_SALE"))
                {
                    if (transaction.Contains("VOID_CAPTURE"))
                    {
                        Console.WriteLine("\n************* CALLING FOR VOID_CAPTURE *****\n");
                    }
                    else if (transaction.Contains("VOID_CREDIT"))
                    {
                        Console.WriteLine("\n************* CALLING FOR VOID_CREDIT *****\n");
                    }

                    voidData = fillVoidData();
                    jsonString = JsonSerializer.Serialize(voidData, options);

                }


                static ReversalData fillReversalInformation(string amount)
                {
                    var reversalData = new ReversalData
                    {
                        ClientReferenceInformation = new ClientReferenceInformation
                        {
                            Code = "ABC123"

                        },

                        ReversalInformation = new ReversalInformation
                        {

                            AmountDetails = new AmountDetails { Currency = "USD", TotalAmount = amount },

                        }
                    };
                    return reversalData;
                }

                static CaptureData fillCaptureData(string amount)
                {
                    var CaptureData = new CaptureData
                    {

                        ClientReferenceInformation = new ClientReferenceInformation
                        {
                            Code = "ABC123"

                        },

                        OrderInformation = new OrderInformation
                        {

                            AmountDetails = new AmountDetails { Currency = "USD", TotalAmount = amount },

                        }

                    };
                    return CaptureData;
                }

                static RefundData fillRefundData(string amount)
                {
                    var refundData = new RefundData
                    {

                        ClientReferenceInformation = new ClientReferenceInformation
                        {
                            Code = "ABC123"

                        },

                        OrderInformation = new OrderInformation
                        {

                            AmountDetails = new AmountDetails { Currency = "USD", TotalAmount = amount },

                        }

                    };
                    return refundData;
                }

                static VoidData fillVoidData()
                {
                    var voidData = new VoidData
                    {

                        ClientReferenceInformation = new ClientReferenceInformation
                        {
                            Code = "ABC123"

                        }

                    };
                    return voidData;
                }

                jsonObject = await CallCyberSource.CallCyberSourceFollowOnJson(jsonString, id, transaction);


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

