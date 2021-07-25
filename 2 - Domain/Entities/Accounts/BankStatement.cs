using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities.Transactions;

namespace Domain.Entities.Accounts
{
    public class BankStatement
    {
        public BankStatement(Guid bankAccountId, List<FinancialTransaction> financialTransactions)
        {
            BankAccountId = bankAccountId;
            Transactions = financialTransactions;
            CreatedAt = DateTime.Now;
        }

        public Guid BankAccountId { get; private set; }

        public List<FinancialTransaction> Transactions { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public bool Contains(FinancialTransaction financialTransaction)
        {
            return Transactions.Any(statement => statement.Id == financialTransaction.Id);
        }

    }
}
