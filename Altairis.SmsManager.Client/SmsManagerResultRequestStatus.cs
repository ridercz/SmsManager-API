using System;
using System.Collections.Generic;
using System.Text;

namespace Altairis.SmsManager.Client {
    public class SmsManagerResultRequestStatus : SmsManagerResult {

        public string PhoneNumber { get; set; }

        public ProcessingState ProcessingStatus { get; set; }

        public int ExpectedReceipts { get; set; }

        public DeliveryState DeliveryStatus { get; set; }

        protected override void ParseResponseTextInternal(string responseText) {
            if (responseText == null) throw new ArgumentNullException(nameof(responseText));
            if (string.IsNullOrWhiteSpace(responseText)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(responseText));

            var data = responseText.Split('|');
            this.IsSuccess = true;
            this.PhoneNumber = data[0];
            this.ProcessingStatus = (ProcessingState)int.Parse(data[1]);
            this.ExpectedReceipts = int.Parse(data[2]);
            this.DeliveryStatus = (DeliveryState)int.Parse(data[3]);
        }
    }
}
