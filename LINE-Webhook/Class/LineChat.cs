using Line.Messaging;
using Line.Messaging.Webhooks;
using LINE_Webhook.Data;
using LINE_Webhook.LineServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace LINE_Webhook
{
    public class LineChat
    {
        private LineMessagingClient messagingClient { get; set; }
        private string merchantId { get; set; }

        //public List<UserProfile> GetFriends(string merId, LineMessagingClient client)
        //{
        //    this.messagingClient = client;
        //    this.merchantId = merId;

        //    List<UserProfile> users = new List<UserProfile>();
        //    List<Friends> f = DAO.GetFriends(merId);
        //    //foreach (Friends f in friends)
        //    //{
        //    //    UserProfile user = await client.GetUserProfileAsync(f.SourceId);
        //    //    users.Add(new UserProfile() { UserId = f.SourceId, DisplayName = user.DisplayName, PictureUrl = user.PictureUrl, StatusMessage = user.StatusMessage });
        //    //}

        //    users.Add(new UserProfile() { UserId = "123", DisplayName = "M@RT", PictureUrl = "http://www.test.com", StatusMessage = "Hi" });
        //    return users;
        //}

        public async Task<List<UserProfile>> GetFriends(string merId, LineMessagingClient client)
        {
            this.messagingClient = client;
            this.merchantId = merId;

            List<UserProfile> users = new List<UserProfile>();
            List<Friends> friends = DAO.GetFriends(merId);
            foreach (Friends f in friends)
            {
                UserProfile user = await client.GetUserProfileAsync(f.SourceId);
                var ev = JsonConvert.DeserializeObject<MessageEvent>(f.MessageText);
                var lastMsg = string.Empty;
                switch (ev.Message.Type)
                {
                    case EventMessageType.Text:
                        lastMsg = "Hi";// ((TextEventMessage)ev.Message).Text;
                        break;
                    default:
                        lastMsg = user.DisplayName + " sent a " + ev.Message.Type.ToString().ToLower() + ".";
                        break;
                }                
                
                users.Add(new UserProfile() { UserId = user.UserId, DisplayName = user.DisplayName, PictureUrl = user.PictureUrl, StatusMessage = lastMsg });
            }

            return users;
        }
    }
}