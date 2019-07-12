using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBankAppV1.Models
{
    public class CheckingAccount : Account
    {
        
        public CheckingAccount()
        {
           // this.AccountNum = count++;
            this.Balance = 0;
            InterestRate = 3.5;
            IsActive = true;
        }

        public double GetMonthlyInterest()
        {
            return this.Balance + (this.Balance * this.InterestRate/100);
        }


        public override bool Withdraw(double amount)
        {
            if (this.Balance - amount < 0)
            {
                return false;
                
            }
            else
            {
                this.Balance -= amount;
                return true;
            }
        }
    }
}
