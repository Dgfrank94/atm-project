using ATM_App.Domain.Enums;
using System;

namespace ATM_App.Domain.Entities
{
    public class Transaction
    {
        public long TransactionID { get; set; }

        public long UserBankAccountID { get; set; }

        public DateTime TransactionDate { get; set; }

        public TransactionType TransactionType { get; set; }

        public string Description { get; set; }

        public decimal TransactionAmount { get; set; }
    }
}
