using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserIdentity.Identity
{
    // generic olduğu için kullanacağımız ıdentity user ı eklemeliyiz
    public class IdentityDataContext:IdentityDbContext<AppUser>
    {
        // ctor ile constructor oluşturmalıyız
        // base in içerisine connection adını vermeliyiz
        public IdentityDataContext():base("identityConnection")
        {

        }
    }
}