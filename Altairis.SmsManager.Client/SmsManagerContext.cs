using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Altairis.SmsManager.Client {
    public class SmsManagerContext {
        private const string DEFAULT_BASE_URI = "https://http-api.smsmanager.cz/";
        private static readonly string USER_AGENT = string.Format("Altairis.SmsManager.Client/{0:4} (https://github.com/ridercz/SmsManager-API)", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);

        public SmsManagerContext(string apiKey) {
            if (apiKey == null) throw new ArgumentNullException(nameof(apiKey));
            if (string.IsNullOrWhiteSpace(apiKey)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(apiKey));

            this.ApiKey = apiKey;
        }

        // Configuration

        public Uri BaseUri { get; set; } = new Uri(DEFAULT_BASE_URI);

        public string ApiKey { get; set; }

        public Gateway DefaultGateway { get; set; } = Gateway.High;

        public string DefaultSender { get; set; }

        public TimeSpan? DefaultExpiration { get; set; }

        public System.Net.Security.RemoteCertificateValidationCallback CertificateValidationCallback { get; set; }

        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);

        public IWebProxy Proxy { get; set; }

        // Send message

        public Task<SmsManagerResult> SendAsync(string message, params string[] numbers) => this.SendAsync(message, numbers, this.DefaultGateway);

        public Task<SmsManagerResult> SendAsync(string message, IEnumerable<string> numbers, Gateway gateway, string sender = null, string customId = null, DateTime? time = null, DateTime? expiration = null) {
            // Validate arguments
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (string.IsNullOrWhiteSpace(message)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(message));
            if (!numbers.Any()) throw new ArgumentException("Value cannot be null or empty array.", nameof(numbers));
            if (!string.IsNullOrEmpty(customId) && !System.Text.RegularExpressions.Regex.IsMatch(customId, "^[0-9]{1,10}$")) throw new ArgumentException("Value must contain only numbers.", nameof(customId));
            if (time.HasValue && time.Value < DateTime.Now) throw new ArgumentOutOfRangeException(nameof(time));
            if (expiration.HasValue && expiration.Value < DateTime.Now) throw new ArgumentOutOfRangeException(nameof(expiration));

            // Prepare path with query string
            var qsb = new StringBuilder($"/Send?apikey={this.ApiKey}");
            qsb.AppendFormat("&number={0}", Uri.EscapeDataString(string.Join(",", numbers)));
            qsb.AppendFormat("&message={0}", Uri.EscapeDataString(message));
            qsb.AppendFormat("&gateway={0}", Enum.GetName(typeof(Gateway), gateway).ToLower());
            if (!string.IsNullOrWhiteSpace(sender)) qsb.AppendFormat("&sender={0}", Uri.EscapeDataString(sender));
            if (!string.IsNullOrWhiteSpace(customId)) qsb.AppendFormat("&customid={0}", customId);
            if (time.HasValue) qsb.AppendFormat("&time={0:s}", time);
            if (expiration.HasValue) {
                qsb.AppendFormat("&expiration={0:s}", expiration);
            } else if (this.DefaultExpiration.HasValue) {
                qsb.AppendFormat("&expiration={0:s}", DateTime.Now.Add(this.DefaultExpiration.Value));
            }

            // Call API and send response
            return this.GetResponse(qsb.ToString());
        }

        private async Task<SmsManagerResult> GetResponse(string path) {
            if (path == null) throw new ArgumentNullException(nameof(path));
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(path));

            // Construct URI
            var uri = new Uri(this.BaseUri, path);

            // Prepare HTTP request
            var rq = WebRequest.CreateHttp(uri);
            rq.UserAgent = USER_AGENT;
            rq.Timeout = (int)this.Timeout.TotalMilliseconds;
            if (this.CertificateValidationCallback != null) rq.ServerCertificateValidationCallback = this.CertificateValidationCallback;
            if (this.Proxy != null) rq.Proxy = this.Proxy;

            // Send HTTP request and wait for response
            var rp = await rq.GetResponseAsync() as HttpWebResponse;
            return await SmsManagerResult.FromHttpWebResponseAsync(rp);

        }

    }
}
