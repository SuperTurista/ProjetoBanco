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

        public decimal Balance { get; set; }
        
        
        
        
        
        
        
        
    }
}