using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBankAppV1.Models
{
    public class Customer
    {
        [Key]
        public int Id { set; get; }
        [Required]
        public string FirstName { set; get; }
        [Required]
        public string LastName { set; get; }
        public string Address { set; get; }
        public string UserRef { set; get; }
        public int Age { set; get; }

        public virtual User CustomerUser { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
    }
}
