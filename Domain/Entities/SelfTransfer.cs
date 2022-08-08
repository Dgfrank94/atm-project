using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM_App.Domain.Entities
{
    public class SelfTransfer
    {
        public decimal TransferAmount { get; set; }

        public long BankAccountNumber { get; set; }

        public string BankAccountName { get; set; }
    }
}
