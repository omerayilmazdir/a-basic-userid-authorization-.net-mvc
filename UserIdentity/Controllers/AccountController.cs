using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserIdentity.Identity;
using UserIdentity.Models;

namespace UserIdentity.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<AppUser> userManager;

        public AccountController()
        {
            var userStore = new UserStore<AppUser>(new IdentityDataContext());
            userManager = new UserManager<AppUser>(userStore);

            // benzersiz email
            userManager.UserValidator = new UserValidator<AppUser>(userManager)
            {
                RequireUniqueEmail = true
            };
        }
        // GET: Account
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return View("Error", new string[] { "Erişim hakkınız yok" });
            }
            ViewBag.returnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Login(LoginModel model, string returnUrl)
        {


            if (ModelState.IsValid)
            {

                var user = userManager.Find(model.Username, model.Password);
                // eğer kullanıcı yoksa hata ver
                if (user == null)
                {
                    ModelState.AddModelError("", "Yanlış kullanıcı adı veya parola");
                }
                else
                {
                    var authManager = HttpContext.GetOwinContext().Authentication;

                    // authmanager giriş çıkış işlemlerini yerine getiren nesne

                    var identity = userManager.CreateIdentity(user, "ApplicationCookie");
                    var authProperties = new AuthenticationProperties()
                    {
                        IsPersistent = true
                    };
                    // önceden giriş yapılmışsa çıkışı sağlar
                    authManager.SignOut();
                    authManager.SignIn(authProperties, identity);

                    // eğer returnUrl boşa eşitse ana dizine gönder
                    return Redirect(string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl);
                }
            }
            ViewBag.returnUrl = returnUrl;

            return View(model);
        }
        // register metodu aktif hale getirildi
        // diğer bütün her şey kapalıdır
        // fakat register ı açık bırakmalıyız
        // [AllowAnonymous] metodu ile gerçekleştiririz
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Register(Register model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser();
                user.UserName = model.Username;
                user.Email = model.Email;

                var result = userManager.Create(user, model.Password);

                // eğer başarılı bir şekilde result gönderildiyse login sayfasına göndeririz
                if (result.Succeeded)
                {
                    userManager.AddToRole(user.Id, "User");
                    return RedirectToAction("Login");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }

            // kullanıcı eğer hatalıysa girdiği bilgiler aynı kalır 
            // ve kayıt sayfasına 
            return View(model);

            
        }

        public ActionResult Logout()
        {
            var authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignOut();

            return RedirectToAction("Login");
        }
    }
}