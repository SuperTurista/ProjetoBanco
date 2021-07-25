using System;

namespace Domain.Entities.Accounts
{
    public class CheckingAccount : BankAccount
    {
        public CheckingAccount(Guid userId) : base(userId)
        {
            this.InterestRate  = 0.05M;
        }

    }
}
