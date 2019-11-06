using System;
using System.Collections.Generic;
using System.Text;

namespace Altairis.SmsManager.Client {
    public class SmsManagerResultRequestList : SmsManagerResult {

        public IReadOnlyList<RequestListItem> Requests { get; set; }

        protected override void ParseResponseTextInternal(string responseText) {
            this.IsSuccess = true;

            var items = new List<RequestListItem>();
            var lines = responseText.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines) {
                var data = line.Split('|');
                var rli = new RequestListItem {
                    RequestId = data[0],
                    Gateway = (Gateway)Enum.Parse(typeof(Gateway), data[1], ignoreCase: true),
                    Time = DateTime.ParseExact(data[2], "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture),
                    Expiration = string.IsNullOrEmpty(data[3]) ? (DateTime?)null : DateTime.ParseExact(data[3], "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture),
                    Sender = data[4],
                    RemainingRecipients = string.IsNullOrEmpty(data[5]) ? 0 : int.Parse(data[5]),
                    Status = (ProcessingState)int.Parse(data[6])
                };
                items.Add(rli);
            }
            this.Requests = items.AsReadOnly();
        }
    }
}
