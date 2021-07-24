using System;
using Domain.Entities;
using Xunit;

namespace DomainTests
{
    public class BankAccountTests
    {
        [Fact]
        public void BankDeposit_AddingNewBankDeposit_ShouldIncrementBalance()
        {
            //Given
            var bankAccount = new BankAccount(Guid.NewGuid());
            bankAccount.BankDeposit(1);

            decimal deposit = 200;

            var balanceToMatch = bankAccount.Balance;

            //When
            bankAccount.BankDeposit(deposit);

            //Then
            Assert.True(bankAccount.Balance > balanceToMatch);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-3)]
        public void BankDeposit_AddingNegativeBankDeposit_ShouldNotIncrementBalance(decimal deposit)
        {
            //Given
            var bankAccount = new BankAccount(Guid.NewGuid());
            bankAccount.BankDeposit(1);

            var balanceToMatch = bankAccount.Balance;

            //When
            bankAccount.BankDeposit(deposit);

            //Then
            Assert.True(bankAccount.Balance == balanceToMatch);
        }

        [Fact]
        public void BankDraft_WithZeroBalance_ShouldNotDraft()
        {
            //Given
            var bankAccount = new BankAccount(Guid.NewGuid());

            //When
            bankAccount.BankDraft(10);

            //Then
            Assert.True(bankAccount.Balance == 0);
        }

        [Theory]
        [InlineData(10, 5)]
        [InlineData(10, 7)]
        [InlineData(33, 12)]
        public void BankDraft_APartOfAccountBalance_ShouldDraftTheValue(decimal bankDeposit, decimal bankDraft)
        {
            //Given
            var bankAccount = new BankAccount(Guid.NewGuid());
            bankAccount.BankDeposit(bankDeposit);

            //When
            bankAccount.BankDraft(bankDraft);

            //Then
            Assert.True(bankAccount.Balance == (bankDeposit - bankDraft));
        }

        [Fact]
        public void BankDradt_LessThanZero_ShouldNotDraft()
        {
            //Given
            var bankAccount = new BankAccount(Guid.NewGuid());

            bankAccount.BankDeposit(3);
            //When
            bankAccount.BankDraft(-5);

            //Then
            Assert.True(bankAccount.Balance == 3);
        }

        [Fact]
        public void BankDraft_MoreThanBalance_ShoudlNotDraft()
        {
            //Given
            var bankAccount = new BankAccount(Guid.NewGuid());

            bankAccount.BankDeposit(3);
            //When
            bankAccount.BankDraft(15);

            //Then
            Assert.True(bankAccount.Balance == 3);
        }

    }
}