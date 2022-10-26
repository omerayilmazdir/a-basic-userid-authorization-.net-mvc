using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UserIdentity.Controllers
{
    // belirlediğimiz kişilerin girmesi için yapılır
    // bu işlem tek tek de yapılabilr
    // [Authorize]
    public class HomeController : Controller
    {
        // GET: Home
       
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult About()
        {
            return View();
        }
    }
}