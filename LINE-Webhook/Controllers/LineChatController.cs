using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LINE_Webhook.Controllers
{
    public class LineChatController : Controller
    {
        // GET: LineChat
        public ActionResult Index()
        {
            return View();
        }
    }
}