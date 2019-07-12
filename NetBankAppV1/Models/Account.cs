using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace NetBankAppV1.Models
{
    public class Account
    {
        
        [Key]
        public int AccountNumber { get; set; }
        //public Customer customer { get; set; }
        [Range(0, Int64.MaxValue, ErrorMessage = "Balance should be greater than $0!")]
        public double Balance { get; set; }
        public double InterestRate { get; set; }
        public Customer customer { get; set; }
        
        public bool IsActive { get; set; }

        public virtual bool Deposit(double amount) {
            this.Balance += amount;
            return true;
        }
        public virtual bool Withdraw(double amount) { return false; }
        

        public virtual ICollection<Transaction> Transcations { get; set; }

        public Account()
        {
            Transcations = new HashSet<Transaction>();
        }

    }
}
