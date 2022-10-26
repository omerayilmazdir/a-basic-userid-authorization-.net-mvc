using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserIdentity.Identity;
using UserIdentity.Models;

namespace UserIdentity.Controllers
{
    // admin rolüne sahip olan kullanıcı görebilir
    [Authorize(Roles = "admin")]
    public class RoleAdminController : Controller
    {
        // identity role ü genişlettik
        // temel sınıf kullanıldı
        private RoleManager<IdentityRole> roleManager;
        private UserManager<AppUser> userManager;

        // önce role store daha sonra üzeirnde çalışılacak data context i gireriz
        public RoleAdminController()
        {
            roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new IdentityDataContext()));
            userManager = new UserManager<AppUser>(new UserStore<AppUser>(new IdentityDataContext()));
        }


        // GET: RoleAdmin
        public ActionResult Index()
        {
            return View(roleManager.Roles);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(string name)
        {   
            // eğer model state geçerliyse ıdentity role de rol oluştur
            if (ModelState.IsValid)
            {
                var result = roleManager.Create(new IdentityRole(name));
                // eğer işlem başarılıysa ındex e yönlendir
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item);
                    }
                }
            }
            // valid değil ise tekrar create sayfasına geri döner
            return View(name);
        }
        // index teki delete butonundan post ile gelen veriyi işle
        [HttpPost]
        public ActionResult Delete(string id)
        {
            var role = roleManager.FindById(id);
            // role içerisi boş değilse ıd ye göre sil
            if (role != null)
            {
                var result = roleManager.Delete(role);
                // eğer işlem başarılıysa tekrar listeye gönder
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Error", result.Errors);
                }
            }
            // eğer rol yoksa rol olmadığına dair bir hata gönderilir
            else
            {
                return View("Error", new string[] { "Role bulunamadı" });
            }
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            // id ye göre role u bulalım
            var role = roleManager.FindById(id);

            // üye ve üye olmayanların listesini tutalım
            var members = new List<AppUser>();
            var nonMembers = new List<AppUser>();

            // user ları test edelim
            foreach (var user in userManager.Users.ToList())
            {
                // eğer role sahipse üyeler listesine ekle değilse üye olmayanlar listesine ekle
                var list = userManager.IsInRole(user.Id, role.Name) ? members : nonMembers;

                list.Add(user);
            }

            return View(new RoleEditModel()
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            });
        }

        [HttpPost]
        public ActionResult Edit(RoleUpdateModel model)
        {
            // hata ya da başarılı sonuca bakmak için oluşturulur 
            IdentityResult result;

            if (ModelState.IsValid)
            {
                // boş gelirse hata vermesin diye diziye atama yaparız
                foreach (var userId in model.IdsToAdd??new string[] { })
                {
                    // user id yi model içerisinden gelen role name atarız
                    result = userManager.AddToRole(userId, model.RoleName);
                    // eğer sonuç doğru değil ise hata ver
                    if (!result.Succeeded)
                    {
                        return View("Error", result.Errors);
                    }
                }

                foreach (var userId in model.IdsToDelete ?? new string[] { })
                {
                    // user id yi model içerisinden gelen role name atarız
                    result = userManager.RemoveFromRole(userId, model.RoleName);
                    // eğer sonuç doğru değil ise hata ver
                    if (!result.Succeeded)
                    {
                        return View("Error", result.Errors);
                    }
                }
                // eğer hiçbir problem yoksa index sayfasına yönlendir
                return RedirectToAction("Index");


            }

            return View("Error", new string[] { "Aranılan rol yok" });
        }

    }
}