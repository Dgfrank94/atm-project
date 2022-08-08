namespace ATM_App.Domain.Entities
{
    public class InternalTransfer
    {
        public decimal TransferAmount { get; set; }

        public long RecipientBankAccountNumber { get; set; }

        public string RecipientBankAccountName { get; set; }

    }
}
