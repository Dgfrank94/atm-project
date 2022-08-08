

namespace ATM_App.Domain.Entities
{
    public class UserAccount
    {
        public int Id { get; set; }

        public string AccountType { get; set; }

        public long CardNumber { get; set; }

        public int CardPin { get; set; }

        public long AccountNumber { get; set; }

        public string FullName { get; set; }

        public decimal AccountBalance { get; set; }

        public int TotalLogin { get; set; }

        public bool IsLocked { get; set; }
    }
}
