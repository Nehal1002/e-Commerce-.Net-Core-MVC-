using System;
using System.Collections.Generic;
using System.Threading.Tasks;
//1. Import the PayPal SDK client that was created in `Set up Server-Side SDK`.
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Payments;
using BraintreeHttp;

namespace Samples
{
  public class CapturesRefundSample
  {

    //2. Set up your server to receive a call from the client
    // Method for refund the capture.
    // Pass a valid capture ID as an argument to this method.

    public async static Task<HttpResponse> CapturesRefund(string CaptureId, bool debug = false)
    {
      var request = new CapturesRefundRequest(CaptureId);
      request.Prefer("return=representation");
      // You can populate the following request body to perform a partial refund.
      // For more details, refer to the Payments API refund captured payment reference.
      request.RequestBody(BuildRequestBody());
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

    // Creating body for partial refund request.
    // Pass empty body for full refund.
    // For more details, refer to the Payments API refund captured payment reference.
    //
    // @return RefundRequest

    private static RefundRequest BuildRequestBody()
    {
      RefundRequest refundRequest = new RefundRequest()
      {
        Amount = new Money
        {
          CurrencyCode = "USD",
          Value = "20.00"
        },

        NoteToPayer = "Partial Refund"
      };

      return refundRequest;
    }

    // Driver function to perform refund on capture.
    // Replace Capture ID with the valid capture ID.

    static void Main(string[] args)
    {
      string CaptureId = "<REPLACE-WITH-VALID-CAPTURE-ID>";
      CapturesRefund(CaptureId, true).Wait();
    }
  }
}