using Line.Messaging.Webhooks;
using LINE_Webhook.LineServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LINE_Webhook.Data
{
    public class DAO
    {
        public static decimal SaveEvents(string merchantId, MessageEvent ev)
        {
            using (LineServices.ServiceClient ws = new LineServices.ServiceClient())
            {
                var json = JsonConvert.SerializeObject(ev);
                return ws.SaveEvents(merchantId, ev.Type.ToString(), ev.Source.Type.ToString(), ev.Source.Id.ToString(), ev.Source.Id.ToString(), ev.Message.Type.ToString(), json, ev.ReplyToken);
            }
        }

        //public static List<Friends> GetFriends(string merchantId)
        //{
        //    using (LineServices.ServiceClient ws = new LineServices.ServiceClient())
        //    {
        //        return ws.GetFriends(merchantId).ToList();
        //    }
        //}

        public static Merchant GetMerchant(string channelId, string zortId)
        {
            using (LineServices.ServiceClient ws = new LineServices.ServiceClient())
            {
                return ws.GetMerchant(channelId, zortId);
            }
        }
    }
}