using System;
using Domain.Entities.Accounts;
using Domain.Entities.Transactions;
using Xunit;

namespace Domain.Tests
{
    public class BankAccountTests
    {
        [Fact]
        public void ProcessTransaction_AddingNewTransaction_ShouldSetTransactionInBankStatement()
        {
            //Given
            var originId = Guid.NewGuid();
            var bankAccount = new BankAccount(Guid.NewGuid());
            var depositTransaction = new FinancialTransaction(10, originId);


            //When
            bankAccount.ProcessTransaction(depositTransaction);
            BankStatement bankStatement = bankAccount.GetBankStatement();

            //Then
            Assert.True(bankStatement.Contains(depositTransaction));
        }

        [Fact]
        public void ProcessDepositTransaction_AddingNewBankDepositTransaction_ShouldIncrementBalanceAndBankStatement()
        {
            //Given
            decimal deposit = 200;
            var originId = Guid.NewGuid();
            var bankAccount = new BankAccount(Guid.NewGuid());
            var depositTransaction = new FinancialTransaction(deposit, originId);
            var originalBalance = bankAccount.GetBalance();

            //When
            bankAccount.ProcessTransaction(depositTransaction);
            BankStatement bankStatement = bankAccount.GetBankStatement();

            //Then
            Assert.True(bankAccount.GetBalance() > originalBalance);
            Assert.True(bankStatement.Contains(depositTransaction));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-3)]
        public void ProcessDepositTransaction_AddingNegativeBankDepositTransaction_ShouldNotIncrementBalanceAndBankStatement(decimal deposit)
        {
            //Given
            var originId = Guid.NewGuid();
            var bankAccount = new BankAccount(Guid.NewGuid());
            var depositTransaction = new FinancialTransaction(deposit, originId);
            var originalBalance = bankAccount.GetBalance();

            //When
            bankAccount.ProcessTransaction(depositTransaction);
            BankStatement bankStatement = bankAccount.GetBankStatement();

            //Then
            Assert.True(bankAccount.GetBalance() == originalBalance);
            Assert.False(bankStatement.Contains(depositTransaction));
        }

        [Fact]
        public void ProcessDraftTransaction_WithZeroBalance_ShouldNotDraftAndIncrementStatement()
        {
            //Given
            var originId = Guid.NewGuid();
            var draft = 15;
            var bankAccount = new BankAccount(Guid.NewGuid());
            var draftTransaction = new FinancialTransaction(draft, originId)
            {
                Type = FinancialTransactionType.BankDraft
            };
            var originalBalance = bankAccount.GetBalance();

            //When
            bankAccount.ProcessTransaction(draftTransaction);
            BankStatement bankStatement = bankAccount.GetBankStatement();

            //Then
            Assert.True(bankAccount.GetBalance() == originalBalance);
            Assert.False(bankStatement.Contains(draftTransaction));
        }

        [Theory]
        [InlineData(10, 5)]
        [InlineData(10, 7)]
        [InlineData(33, 12)]
        public void ProcessDraftTransaction_WithValueAsPartOfAccountBalance_ShouldDraftTheValueAndIncrementStatement(decimal bankDeposit, decimal bankDraft)
        {
            //Given
            var originId = Guid.NewGuid();
            var bankAccount = new BankAccount(Guid.NewGuid());
            var depositTransaction = new FinancialTransaction(bankDeposit, originId);
            var draftTransaction = new FinancialTransaction(bankDraft, originId)
            {
                Type = FinancialTransactionType.BankDraft
            };

            //When
            bankAccount.ProcessTransaction(depositTransaction);
            bankAccount.ProcessTransaction(draftTransaction);
            BankStatement bankStatement = bankAccount.GetBankStatement();

            //Then
            Assert.True(bankAccount.GetBalance() == (bankDeposit - bankDraft));
            Assert.True(bankStatement.Contains(depositTransaction));
            Assert.True(bankStatement.Contains(draftTransaction));
        }

        [Fact]
        public void ProcessDraftTransaction_WithNegativeValue_ShouldNotDraftAndIncrementStatement()
        {
            //Given
            var originId = Guid.NewGuid();
            var bankDeposit = 3;
            var bankDraft = -5;
            var bankAccount = new BankAccount(Guid.NewGuid());
            var depositTransaction = new FinancialTransaction(bankDeposit, originId);
            var draftTransaction = new FinancialTransaction(bankDraft, originId)
            {
                Type = FinancialTransactionType.BankDraft
            };
            //When
            bankAccount.ProcessTransaction(depositTransaction);
            bankAccount.ProcessTransaction(draftTransaction);
            BankStatement bankStatement = bankAccount.GetBankStatement();

            //Then
            Assert.True(bankAccount.GetBalance() == bankDeposit);
            Assert.True(bankStatement.Contains(depositTransaction));
            Assert.False(bankStatement.Contains(draftTransaction));
        }

        [Fact]
        public void ProcessDraftTransaction_MoreThanBalance_ShoudlNotDraftAndIncrementStatement()
        {
            //Given
            var originId = Guid.NewGuid();
            var bankDeposit = 3;
            var bankDraft = 15;
            var bankAccount = new BankAccount(Guid.NewGuid());
            var depositTransaction = new FinancialTransaction(bankDeposit, originId);
            var draftTransaction = new FinancialTransaction(bankDraft, originId)
            {
                Type = FinancialTransactionType.BankDraft
            };

            //When
            bankAccount.ProcessTransaction(depositTransaction);
            bankAccount.ProcessTransaction(draftTransaction);
            BankStatement bankStatement = bankAccount.GetBankStatement();

            //Then
            Assert.True(bankAccount.GetBalance() == bankDeposit);
            Assert.True(bankStatement.Contains(depositTransaction));
            Assert.False(bankStatement.Contains(draftTransaction));
        }

        [Fact]
        public void GetBalance_AfterDepositSomeValue_ShouldReturn()
        {
            //Given
            var deposit = 10;
            var bankAccount = new BankAccount(Guid.NewGuid());
            var depositTransaction = new FinancialTransaction(deposit, Guid.NewGuid());

            //When
            bankAccount.ProcessTransaction(depositTransaction);
            var totalBalance = bankAccount.GetBalance();

            //Then
            Assert.True(totalBalance == deposit);
        }

        [Fact]
        public void GetCheckingAccountBalance_AfterDepositSomeValue_ShouldRteurnBalancePlusInterestRate()
        {
            //Given
            var deposit = 10;
            BankAccount bankAccount = new CheckingAccount(Guid.NewGuid());
            var depositTransaction = new FinancialTransaction(deposit, Guid.NewGuid());

            //When
            bankAccount.ProcessTransaction(depositTransaction);
            var totalBalance = bankAccount.GetBalance();

            //Then
            Assert.True(totalBalance == ((deposit * 0.05M) + deposit));

        }

        [Fact]
        public void GetCheckingAccountBalance_WithZeroBalance_ShouldNotComputeInteresRate()
        {
            //Given
            BankAccount bankAccount = new CheckingAccount(Guid.NewGuid());

            //When
            var totalBalance = bankAccount.GetBalance();

            //Then
            Assert.True(totalBalance == 0);
        }

        [Fact]
        public void GetSavingAccountBalance_AfterDepositSomeValue_ShouldReturnBalancePlusInterestRate()
        {
            //Given
            var deposit = 10;
            BankAccount bankAccount = new SavingAccount(Guid.NewGuid());
            var depositTransaction = new FinancialTransaction(deposit, Guid.NewGuid());

            //When
            bankAccount.ProcessTransaction(depositTransaction);
            var totalBalance = bankAccount.GetBalance();

            //Then
            Assert.True(totalBalance == ((deposit * 0.10M) + deposit));
        }

    }
}