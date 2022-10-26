
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserIdentity.Identity;

namespace UserIdentity.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        // sisteme kayıtlı olan tüm kullanıcıları göster
        
        private UserManager<AppUser> userManager;

        public AdminController()
        {
            var userStore = new UserStore<AppUser>(new IdentityDataContext());
            userManager = new UserManager<AppUser>(userStore);
        }
        public ActionResult Index()
        {
            return View(userManager.Users);
        }
    }
}