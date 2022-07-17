using System;
using Square;
using Square.Models;
using Square.Exceptions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ExploreLocationsAPI
{
    public class Program
    {
        private static ISquareClient client;
        private static IConfigurationRoot config;

        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
             .AddJsonFile($"appsettings.json", true, true);

            config = builder.Build();
            var accessToken = config["AppSettings:SandboxAccessToken"];
            Console.WriteLine($"Access Token is: {accessToken}");
            client = new SquareClient.Builder()
                .Environment(Square.Environment.Sandbox)
                .AccessToken(accessToken)
                .Build();

            var GetClass1 = new Class1(); // set up new instance of an external class
            var result = await GetClass1.QuickPayTask(); // access this new instance and get a returned result

            // QuickPayTask().Wait(); // access local Function, with waiting for it to complete before coming back
            //RetrieveLocationsAsync().Wait(); // access local Function, with waiting for it to complete before coming back
        }

        static async Task RetrieveLocationsAsync()
        {
            try
            {
                ListLocationsResponse response = await client.LocationsApi.ListLocationsAsync();
                foreach (Location location in response.Locations)
                {
                    Console.WriteLine("location:\n  country =  {0} name = {1}", location.Country, location.Name);
                }
            }
            catch (ApiException e)
            {
                var errors = e.Errors;
                var statusCode = e.ResponseCode;
                var httpContext = e.HttpContext;
                Console.WriteLine("ApiException occurred:");
                Console.WriteLine("Headers:");
                foreach (var item in httpContext.Request.Headers)
                {
                    //Display all the headers except Authorization
                    if (item.Key != "Authorization")
                    {
                        Console.WriteLine("\t{0}: \t{1}", item.Key, item.Value);
                    }
                }
                Console.WriteLine("Status Code: \t{0}", statusCode);
                foreach (Error error in errors)
                {
                    Console.WriteLine("Error Category:{0} Code:{1} Detail:{2}", error.Category, error.Code, error.Detail);
                }

                // Your error handling code
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occurred");
                // Your error handling code
            }            
        }
      /*  public static async Task QuickPayTask()
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

            try
            {
                var result = await client.CheckoutApi.CreatePaymentLinkAsync(body: body);
                Console.WriteLine(result);
            }
            catch (ApiException e)
            {
                Console.WriteLine("Failed to make the request");
                Console.WriteLine($"Response Code: {e.ResponseCode}");
                Console.WriteLine($"Exception: {e.Message}");
            }
        }*/
    }
}

