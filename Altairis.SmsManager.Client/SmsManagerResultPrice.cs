using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Altairis.SmsManager.Client {
    public class SmsManagerResultPrice : SmsManagerResult {

        public int ValidRecipients { get; set; }

        public int Messages { get; set; }

        public int Characters { get; set; }

        public float PricePerMessage { get; set; }

        public float Price { get; set; }

        protected override void ParseResponseTextInternal(string responseText) {
            if (responseText == null) throw new ArgumentNullException(nameof(responseText));
            if (string.IsNullOrEmpty(responseText)) throw new ArgumentException("Value cannot be null or empty string.", nameof(responseText));

            var data = responseText.Split('|');

            this.IsSuccess = true;
            this.ValidRecipients = int.Parse(data[0]);
            this.Messages = int.Parse(data[1]);
            this.Characters = int.Parse(data[2]);
            this.PricePerMessage = float.Parse(data[3], CultureInfo.InvariantCulture);
            this.Price = float.Parse(data[4], CultureInfo.InvariantCulture);
        }
    }
}
