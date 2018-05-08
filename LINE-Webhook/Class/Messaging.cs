using Line.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace LINE_Webhook.Class
{
    public class Messaging
    {
        public bool ReplyMessage(string channelId, string zortId, string replyToken, IList<ISendMessage> messages)
        {           
            var mer = new LineServices.Merchant();
            using (LineServices.ServiceClient ws = new LineServices.ServiceClient())
            {
                mer = ws.GetMerchant(channelId, zortId);
            }
            var client = new LineMessagingClient(mer.ChannelAccessToken);

            var task = Task.Factory.StartNew(() => client.ReplyMessageAsync(replyToken,messages));
            task.Start();

            return true;
        }

        public void SendMessage(string channelId, string zortId, string replyToken, string msg)
        {
            ISendMessage replyMessage = new TextMessage(msg);
            ReplyMessage(channelId, zortId, replyToken, new List<ISendMessage> { replyMessage });
        }
        public void SendLocation(string channelId, string zortId, string replyToken, string address, decimal latitude, decimal longitude)
        {
            ISendMessage replyMessage = new LocationMessage("Location", address,latitude, longitude);
            ReplyMessage(channelId, zortId, replyToken, new List<ISendMessage> { replyMessage });
        }
        public void SendSticker(string channelId, string zortId, string replyToken, string packageId, string stickerId)
        {
            ISendMessage replyMessage = new StickerMessage(packageId, stickerId);
            ReplyMessage(channelId, zortId, replyToken, new List<ISendMessage> { replyMessage });
        }
    }
}