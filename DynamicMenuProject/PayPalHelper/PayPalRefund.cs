using PayPalCheckoutSdk.Payments;
using PayPalHttp;
using Samples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicMenuProject.PayPalHelper
{
    public class PayPalRefund
    {
        public static async Task<HttpResponse> CapturesRefund(string CaptureId, bool debug = false)
        {
            var request = new CapturesRefundRequest(CaptureId);
            request.Prefer("return=representation");
            // You can populate the following request body to perform a partial refund.
            // For more details, refer to the Payments API refund captured payment reference.
            //request.RequestBody(BuildRequestBody());
            RefundRequest refundRequest = new RefundRequest()
            {
                Amount = new Money
                {
                    Value = "54.00",
                    CurrencyCode = "USD"
                },

            };
            request.RequestBody(refundRequest);
            //#3. Call PayPal to refund a capture

            var response = await PayPalClient.client().Execute(request);

            if (debug)
            {
                var result = response.Result<Refund>();
                Console.WriteLine("Status: {0}", result.Status);
                Console.WriteLine("Refund Id: {0}", result.Id);
                Console.WriteLine("Links:");
                foreach (LinkDescription link in result.Links)
                {
                    Console.WriteLine("\t{0}: {1}\tCall Type: {2}", link.Rel,
                                            link.Href,
                                            link.Method);
                }
                Console.WriteLine("Response JSON: \n {0}",
                            PayPalClient.ObjectToJSONString(result));
            }
            return response;
        }
    }
    
}
