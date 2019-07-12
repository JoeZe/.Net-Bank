using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace NetBankAppV1.Models
{
    public class Transaction
    {
        [Key]
        public int TranId { get; set; }
        [Range(1, Int64.MaxValue, ErrorMessage = "Amount should be greater than $1")]
        public double amount { get; set; }
        
        public DateTime date { get; set; }
        public string TranscationType{ get; set; }
        //public int AccountId { get; set; }
        public virtual Account account { get; set; }
    }
}
