using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBankAppV1.Models
{
    public class TermDeposit: Account
    {
        public bool termEnded { get; set; }
        public double penalty = 10;
        public DateTime TermEndedDate { get; set; }
        //due date=true
        public TermDeposit()
        {
            //AccountNumId = count++;
            Balance = 0;
            termEnded = false;
            IsActive = true;
            InterestRate = 3.5;
        }

        public double Earning()
        {
            double earning = 0;
            if (termEnded == true)
            {
                earning=this.Balance+(this.Balance * (InterestRate/100));
            }
            else
            {
                earning = this.Balance - (this.Balance * (penalty/100));
            }
            return earning;
        }

        public override bool Withdraw(double amount)
        {
            if (this.Balance == 0|| amount!=this.Balance)
            {
                return false;
            }
            this.Balance = 0;
            return true;          
        }
    }
}
