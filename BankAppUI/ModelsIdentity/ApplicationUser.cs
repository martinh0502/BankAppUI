using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankAppUI.ModelsIdentity
{
    public class ApplicationUser:IdentityUser
    {
        public int CustomerId { get; set; }
    }
    public class ApplicationRole:IdentityRole
    {

    }
}
