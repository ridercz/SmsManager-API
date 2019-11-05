using System;
using System.Collections.Generic;
using System.Text;

namespace Altairis.SmsManager.Client {
    public class SmsManagerResultSend : SmsManagerResult {

        public string RequestId { get; set; }

        public IEnumerable<string> PhoneNumbers { get; set; }

        public string CustomId { get; set; }

        protected override void ParseResponseTextInternal(string responseText) {
            var data = responseText.Split('|');
            if (data[0].Equals("OK", StringComparison.Ordinal)) {
                // Success state
                this.IsSuccess = true;
                this.RequestId = data[1];
                this.PhoneNumbers = data[2].Split(',');
                if (data.Length == 4) this.CustomId = data[3];
            } else  {
                // Unexpected error
                throw new Exception("Unexpected response from API");
            }

        }
    }
}
