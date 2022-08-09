namespace ATM_App.Domain.Interfaces
{
    public interface IUserAccountActions
    {
        void CheckBalance();

        void PlaceDeposit();

        void MakeWithdrawal();
    }
}
