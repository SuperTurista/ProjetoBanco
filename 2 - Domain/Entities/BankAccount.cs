using System;

namespace Domain.Entities
{
    public class BankAccount : Entity
    {
        public BankAccount(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; set; }

        public long Number { get; set; }

        public string Agency { get; set; }

        public string Type { get; set; }

        public decimal Balance { get; private set; }

        public void BankDeposit(decimal deposit)
        {
            if (deposit > 0)
                Balance += deposit;
        }

        public void BankDraft(decimal draft) 
        { 
            if(Balance > 0 && draft > 0 && Balance >= draft)
                Balance -= draft;
        }
        
        
    }
}