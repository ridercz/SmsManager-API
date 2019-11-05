using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Altairis.SmsManager.Client {
    public class SmsManagerResult {
        private static HttpStatusCode[] KNOWN_STATUS_CODES = {
            HttpStatusCode.OK,
            HttpStatusCode.BadRequest,
            HttpStatusCode.Unauthorized,
            HttpStatusCode.PaymentRequired,
            HttpStatusCode.InternalServerError
        };

        public bool IsSuccess { get; set; }

        public string RequestId { get; set; }

        public IEnumerable<string> PhoneNumbers { get; set; }

        public string CustomId { get; set; }

        public HttpStatusCode HttpStatusCode { get; set; }

        public string HttpStatusDescription { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMessage {
            get {
                switch (this.ErrorCode) {
                    case 101: return "Neexistující data požadavku (chybí XMLDATA parametr u XML API)";
                    case 102: return "Zaslaná data nejsou ve správném formátu";
                    case 103: return "Neplatné uživatelské jméno nebo heslo";
                    case 104: return "Neplatný parametr gateway";
                    case 105: return "Nedostatek kreditu pro prepaid";
                    case 109: return "Požadavek neobsahuje všechna vyžadovaná data";
                    case 201: return "Žádná platná telefonní čísla v požadavku";
                    case 202: return "Text zprávy neexistuje nebo je příliš dlouhý";
                    case 203: return "Neplatný parametr sender (odesílatele nejprve nastavte ve webovém rozhraní)";
                    default: return null;
                }
            }
        }

        internal static async Task<SmsManagerResult> FromHttpWebResponseAsync(HttpWebResponse rp) {
            if (rp == null) throw new ArgumentNullException(nameof(rp));

            // Process API response
            var result = new SmsManagerResult {
                HttpStatusCode = rp.StatusCode,
                HttpStatusDescription = rp.StatusDescription
            };

            if (KNOWN_STATUS_CODES.Contains(rp.StatusCode)) {
                // Get response as text
                using (var rs = rp.GetResponseStream())
                using (var sr = new StreamReader(rs)) {
                    var responseText = await sr.ReadToEndAsync();
                    result.ParseResponseText(responseText);
                }
            } else {
                // Unknown status code
                result.IsSuccess = false;
            }

            return result;
        }

        protected void ParseResponseText(string responseText) {
            var data = responseText.Split('|');
            if (data[0].Equals("OK", StringComparison.Ordinal)) {
                // Success state
                this.IsSuccess = true;
                this.RequestId = data[1];
                this.PhoneNumbers = data[2].Split(',');
                if (data.Length == 4) this.CustomId = data[3];
            } else if (data[0].Equals("ERROR", StringComparison.Ordinal)) {
                // Expected error
                this.IsSuccess = false;
                this.ErrorCode = int.Parse(data[1]);
            } else {
                // Unexpected error
                throw new Exception("Unexpected response from API");
            }
        }
    }

}

