using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserIdentity.Identity
{
    public class AppRole:IdentityRole
    {
        public string Description { get; set; }
    }
}