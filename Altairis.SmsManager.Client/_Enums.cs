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

    public enum DeliveryState {
        NoReceipt = 0,
        InTransit = 1,
        Delivered = 2,
        Failed = 3,
        Unknown = 4,
        Waiting = 5,
        Expired = 6,
        InvalidNumber = 7,
        Error = 8
    }
}
