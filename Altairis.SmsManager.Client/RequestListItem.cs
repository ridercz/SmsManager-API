using System;
using System.Collections.Generic;
using System.Text;

namespace Altairis.SmsManager.Client {
    public class RequestListItem {

        public string RequestId { get; set; }

        public Gateway Gateway { get; set; }

        public DateTime Time { get; set; }

        public DateTime? Expiration { get; set; }

        public string Sender { get; set; }

        public int RemainingRecipients { get; set; }

        public ProcessingState Status { get; set; }

    }
}
