using System;
using Square;
using Square.Models;
using Square.Exceptions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

public class Class1
{
	private static ISquareClient client;
	private static IConfigurationRoot config;

	public Class1()
	{
		var builder = new ConfigurationBuilder()
			.AddJsonFile($"appsettings.json", true, true);

		config = builder.Build();
		var accessToken = config["AppSettings:SandboxAccessToken"];
		//Console.WriteLine($"Access Token is: {accessToken}");
		client = new SquareClient.Builder()
				.Environment(Square.Environment.Sandbox)
				.AccessToken(accessToken)
				.Build();
	}

    public async Task<string> QuickPayTask()
    {
        var priceMoney = new Money.Builder()
          .Amount(12500L)
          .Currency("GBP")
          .Build();

        var quickPay = new QuickPay.Builder(
            name: "Auto Detailing",
            priceMoney: priceMoney,
            locationId: "LXNPQGX0RXNMV")
          .Build();

        Guid g = Guid.NewGuid();
        string uniqueKey = g.ToString();
        var body = new CreatePaymentLinkRequest.Builder()
          .IdempotencyKey(uniqueKey)
          .QuickPay(quickPay)
          .Build();

        string resultToReturn = "";
        try
        {
            CreatePaymentLinkResponse result = await client.CheckoutApi.CreatePaymentLinkAsync(body: body);
            resultToReturn = result.PaymentLink.Url;
            System.Diagnostics.Debug.WriteLine(resultToReturn);
        }
        catch (ApiException e)
        {
            System.Diagnostics.Debug.WriteLine("Failed to make the request");
            System.Diagnostics.Debug.WriteLine($"Response Code: {e.ResponseCode}");
            System.Diagnostics.Debug.WriteLine($"Exception: {e.Message}");
            resultToReturn = "error";
        }
        string returnedString = resultToReturn;
        return returnedString;
    }
}
