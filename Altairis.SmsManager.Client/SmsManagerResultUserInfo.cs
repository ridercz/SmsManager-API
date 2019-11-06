using System;
using System.Collections.Generic;
using System.Text;

namespace Altairis.SmsManager.Client {
    public class SmsManagerResultUserInfo : SmsManagerResult {

        public float Credit { get; set; }

        public string DefaultSender { get; set; }

        public Gateway DefaultGateway { get; set; }

        protected override void ParseResponseTextInternal(string responseText) {
            if (responseText == null) throw new ArgumentNullException(nameof(responseText));
            if (string.IsNullOrWhiteSpace(responseText)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(responseText));

            var data = responseText.Split('|');

            this.IsSuccess = true;
            this.Credit = float.Parse(data[0], System.Globalization.CultureInfo.InvariantCulture);
            this.DefaultSender = data[1];
            this.DefaultGateway = (Gateway)Enum.Parse(typeof(Gateway), data[2], ignoreCase: true);
        }
    }
}
