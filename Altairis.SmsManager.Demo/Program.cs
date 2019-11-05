using System;
using System.Threading.Tasks;
using Altairis.SmsManager.Client;

namespace Altairis.SmsManager.Demo {
    class Program {
        private static SmsManagerContext smsContext;

        static void Main(string[] args) {
            Console.WriteLine("Altairis SMS Manager Demo Application");
            Console.WriteLine("https://www.github.com/ridercz/SmsManager-API");
            Console.WriteLine();

            // Read configuration from command line arguments
            if (args.Length > 2) {
                Console.WriteLine("USAGE:   smsdemo <phone> <apikey>");
                Console.WriteLine("EXAMPLE: smsdemo +420123456789 xxxxxxxxxx");
                return;
            }
            var phoneNumber = args[0];
            var apiKey = args[1];

            // Create client
            smsContext = new SmsManagerContext(apiKey);

            // Send SMS
            TestSendAsync(phoneNumber).Wait();

        }

        private static async Task TestSendAsync(string phoneNumber) {
            // Send simple message with default settings
            Console.Write("Sending message with default settings...");
            try {
                var result = await smsContext.SendAsync("This is simple test message sent with default settings.", phoneNumber);
                ShowResult(result);
            }
            catch (Exception ex) {
                Console.WriteLine("Failed!");
                Console.WriteLine(ex.ToString());
            }

            // Send economy message with default settings
            Console.Write("Sending economy message...");
            try {
                var result = await smsContext.SendAsync("This test message was sent via 'Economy' gateway.", phoneNumber, Gateway.Economy);
                ShowResult(result);
            }
            catch (Exception ex) {
                Console.WriteLine("Failed!");
                Console.WriteLine(ex.ToString());
            }

            // Send low-cost message with default settings
            Console.Write("Sending low-cost message...");
            try {
                var result = await smsContext.SendAsync("This test message was sent via 'LowCost' gateway.", phoneNumber, Gateway.LowCost);
                ShowResult(result);
            }
            catch (Exception ex) {
                Console.WriteLine("Failed!");
                Console.WriteLine(ex.ToString());
            }

            // Send messages with custom sender
            Console.Write("Sending message from 'bad-sender' (should fail)...");
            try {
                var result = await smsContext.SendAsync("This is a test message sent from bad-sender.", phoneNumber, Gateway.Economy, sender: "bad-sender");
                ShowResult(result);
            }
            catch (Exception ex) {
                Console.WriteLine("Failed!");
                Console.WriteLine(ex.ToString());
            }
        }

        private static void ShowResult(SmsManagerResult result) {
            if (!result.IsSuccess) {
                Console.WriteLine("Failed!");
                Console.WriteLine($"Error #{result.ErrorCode}: {result.ErrorMessage}");
            } else if (result is SmsManagerResultSend resultSend) {
                Console.WriteLine($"OK, id={resultSend.RequestId}, customid={resultSend.CustomId}");
            } else {
                Console.WriteLine("OK");
            }
        }

    }
}
