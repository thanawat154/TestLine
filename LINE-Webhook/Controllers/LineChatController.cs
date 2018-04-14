using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using LINE_Webhook.LineServices;

namespace LINE_Webhook.Controllers
{
    [RoutePrefix("api")]
    public class LineChatController : ApiController
    {
        public async Task<Friends[]> GetFriends(string merchantId)
        {
            using (LineServices.ServiceClient ws = new LineServices.ServiceClient())
            {
                return await ws.GetFriendsAsync(merchantId);
            }
        }
    }
}
