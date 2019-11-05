using System;
using Altairis.SmsManager.Client;

namespace Altairis.SmsManager.Demo {
    class Program {
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
            var client = new SmsManagerContext(apiKey);

            // Send simple message with default settings
            Console.Write("Sending message with default settings...");
            try {
                var result = client.SendAsync("This is simple test message sent with default settings.", phoneNumber).Result;
                ShowResult(result);
            }
            catch (Exception ex) {
                Console.WriteLine("Failed!");
                Console.WriteLine(ex.ToString());
            }

            // Send economy message with default settings
            Console.Write("Sending economy message...");
            try {
                var result = client.SendAsync("This test message sent via 'Economy' gateway.", phoneNumber, Gateway.Economy).Result;
                ShowResult(result);
            }
            catch (Exception ex) {
                Console.WriteLine("Failed!");
                Console.WriteLine(ex.ToString());
            }

            // Send low-cost message with default settings
            Console.Write("Sending low-cost message...");
            try {
                var result = client.SendAsync("This test message sent via 'LowCost' gateway.", phoneNumber, Gateway.LowCost).Result;
                ShowResult(result);
            }
            catch (Exception ex) {
                Console.WriteLine("Failed!");
                Console.WriteLine(ex.ToString());
            }

            // Send messages with custom senders
            Console.Write("Sending message from 'bad-sender' (should fail)...");
            try {
                var result = client.SendAsync("This is a test message sent from bad-sender.", phoneNumber, Gateway.Economy, sender: "bad-sender").Result;
                ShowResult(result);
            }
            catch (Exception ex) {
                Console.WriteLine("Failed!");
                Console.WriteLine(ex.ToString());
            }

        }

        private static void ShowResult(SmsManagerResult result) {
            if (result.IsSuccess) {
                Console.WriteLine($"OK, id={result.RequestId}, customid={result.CustomId}");
            } else {
                Console.WriteLine("Failed!");
                Console.WriteLine($"Error #{result.ErrorCode}: {result.ErrorMessage}");
            }
        }
    }
}
