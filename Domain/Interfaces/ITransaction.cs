using ATM_App.Domain.Enums;

namespace ATM_App.Domain.Interfaces
{
    public interface ITransaction
    {
        void InsertTransaction(long _UserBankAccountId, TransactionType _tranType, decimal _tranAmount, string _desc);

        void ViewTransaction();
    }
}
