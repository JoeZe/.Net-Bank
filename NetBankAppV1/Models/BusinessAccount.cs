using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NetBankAppV1.Models
{
    public class BusinessAccount : Account
    {
       // public Customer customer { get; set; }
       // public int AccoutNum { get; set; }
       // public double Balance { get; set; }
        public double OverdraftAmount { get; set; }
        public bool AllowLoan { get; set; }

        public BusinessAccount()
        {
           // this.AccountNum = count++;
            this.Balance = 0;
            OverdraftAmount = 100;
            AllowLoan = true;
            IsActive = true;
            InterestRate = 0;
        }

        public override bool Withdraw(double amount)
        {
            if (this.Balance - amount < - OverdraftAmount)
            {
                return false;
            }
            else if(this.Balance - amount >= -OverdraftAmount && this.Balance - amount < 0)
            {
                //loan is - payment 
                this.OverdraftAmount = this.OverdraftAmount + (this.Balance - amount);
      
                this.Balance = 0;

                return true;
            }
            else
            {
                this.Balance -= amount;
                return true;
            }
            
        }
    }
}
