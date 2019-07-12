using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBankAppV1.Models
{
    public class Loan: Account
    {
        public double Interest = 3.5;

        public Loan()
        {
            InterestRate = Interest;
            Balance = 0;
        }

        public override bool Deposit(double amount)
        {
            this.Balance -= amount;
            return true;
        }


        public override bool Withdraw(double amount)
        {
            return false;
        }
    }
}
