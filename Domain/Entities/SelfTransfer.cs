namespace ATM_App.Domain.Entities
{
    public class SelfTransfer
    {
        public decimal TransferAmount { get; set; }

        public long BankAccountNumber { get; set; }

        public string BankAccountName { get; set; }
    }
}
