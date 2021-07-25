using System;

namespace Domain.Entities.Accounts
{
    public class SavingAccount : BankAccount
    {
        public SavingAccount(Guid userId) : base(userId)
        {
            this.InterestRate = 0.10M;
        }



    }
}
