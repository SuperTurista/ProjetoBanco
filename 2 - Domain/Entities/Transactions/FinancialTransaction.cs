using System;

namespace Domain.Entities.Transactions
{
    public class FinancialTransaction
    {
        
        public FinancialTransaction(decimal value, Guid originId)
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.Now;
            Value = value;
            OriginId = originId;
        }

        public Guid Id { get; private set; }
        
        
        public DateTime CreatedAt { get; private set; }
        
        public decimal Value { get; set; }
        
        public FinancialTransactionType Type { get; set; }
        
        public Guid OriginId { get; set; }
        
        
    }
}
