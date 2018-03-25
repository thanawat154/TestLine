using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LINE_Webhook.Models
{
    public class LineUserModels
    {
        public LineUserModels(string merchantId, string partitionKey, string rowKey)
        {
            this.MerchantId = merchantId;
            this.PartitionKey = partitionKey;
            this.RowKey = rowKey;
        }

        public string MerchantId { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}