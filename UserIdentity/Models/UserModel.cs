using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using UserIdentity.Identity;

namespace UserIdentity.Models
{
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class Register
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class RoleEditModel
    {
        // ıdentityrole den rolleri alıyoruz
        public IdentityRole Role { get; set; }
        // kullanılcar listesinden üyeler(members), ve üye olmayanların listesini(nonmembers)
        // role göre alacağız
        public IEnumerable<AppUser> Members { get; set; }
        public IEnumerable<AppUser> NonMembers { get; set; }
    }

    // posta gelecek olan bilgiler
    public class RoleUpdateModel
    {
        [Required]
        public string RoleName { get; set; }
        public string RoleId { get; set; }
        // role eklenmesini istediğimiz user id lerin checkbox larını diziye atarız
        public string[] IdsToAdd { get; set; }
        public string[] IdsToDelete { get; set; }
    }

}