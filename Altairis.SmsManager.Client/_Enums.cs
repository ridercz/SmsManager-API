using System;
using System.Collections.Generic;
using System.Text;

namespace Altairis.SmsManager.Client {
    public enum Gateway {
        Default = 0,
        High = 1,
        Economy = 2,
        LowCost = 3
    }

    public enum ProcessingState {
        Waiting = 0,
        Sending = 1,
        Sent = 2,
        Partial = 3,
        Invalid = 4,
        Failed = 5,
        Cancelled = 6,
        Rejected = 7,
        Processing = 9
    }
}
