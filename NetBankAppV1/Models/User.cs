using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace NetBankAppV1.Models
{
    public class User : IdentityUser
    {
        //public virtual ICollection<Account> Accounts { get; set; }
        public virtual Customer customer { get; set; }
    }
}
