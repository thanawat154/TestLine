using Line.Messaging;
using LINE_Webhook.Data;
using LINE_Webhook.LineServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LINE_Webhook
{
    public class LineChat
    {
        private LineMessagingClient messagingClient { get; set; }
        private string merchantId { get; set; }

        public List<Friends> GetFriends(string merId, LineMessagingClient client)
        {
            this.messagingClient = client;
            this.merchantId = merId;

            return DAO.GetFriends(merId);
        }


    }
}