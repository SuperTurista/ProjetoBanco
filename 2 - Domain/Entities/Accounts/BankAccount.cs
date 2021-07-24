using System;
using System.Collections.Generic;
using Domain.Entities.Transactions;

namespace Domain.Entities.Accounts
{
    public class BankAccount : Entity
    {
        public BankAccount(Guid userId)
        {
            UserId = userId;
            FinancialTransactions = new List<FinancialTransaction>();
        }

        public Guid UserId { get; set; }

        public long Number { get; set; }

        public string Agency { get; set; }

        public BankAccountType Type { get; set; }

        public decimal Balance { get; private set; }

        public List<FinancialTransaction> FinancialTransactions { get; set; }

        private void BankDeposit(FinancialTransaction financialTransaction)
        {
            if (financialTransaction.Value > 0)
            {
                FinancialTransactions.Add(financialTransaction);
                Balance += financialTransaction.Value;
            }

        }

        private void BankDraft(FinancialTransaction financialTransaction)
        {
            if (Balance > 0 && financialTransaction.Value > 0 && Balance >= financialTransaction.Value)
            {
                FinancialTransactions.Add(financialTransaction);
                Balance -= financialTransaction.Value;
            }
        }

        public BankStatement GetBankStatement()
        {
            return new BankStatement(this.Id, this.FinancialTransactions);
        }

        public void ProcessTransaction(FinancialTransaction financialTransaction)
        {
            if (financialTransaction != null && financialTransaction.Value > 0)
            {
                if (financialTransaction.Type == FinancialTransactionType.BankDeposit)
                    BankDeposit(financialTransaction);
                if (financialTransaction.Type == FinancialTransactionType.BankDraft)
                    BankDraft(financialTransaction);
            }

        }

    }
}