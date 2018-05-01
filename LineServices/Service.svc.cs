using Line.Messaging.Webhooks;
using LineServices.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace LineServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service : IService
    {
        public decimal SaveEvents(string channelId, string eventType, string sourceType, string sourceId, string sender, string messageType, string messageText, string replyToken)
        {
            decimal result = 0;
            using (LineContext ctx = new LineContext())
            {
                result = ctx.SaveEvents(channelId, eventType, sourceType, sourceId, sender, messageType, messageText, replyToken);
            }
            return result;
        }

        //public List<Friends> GetChats(string merchantId)
        //{
        //    var friends = new List<Friends>();
        //    using (AccountContext ctx = new AccountContext())
        //    {
        //        friends = ctx.GetFriends(merchantId);
        //    }

        //    return friends;
        //}

        public bool RegisterMerchant(string merchantId, string channelId, string channelSecret, string channelAccessToken, string descriptions)
        {

            return true;
        }

        public Merchant GetMerchant(string channelId, string zortId)
        {
            var mer = new Merchant();
            using (LineContext ctx = new LineContext())
            {
                mer = ctx.GetMerchant(channelId, zortId);
            }

            return mer;
        }
    }
}
