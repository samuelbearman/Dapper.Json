using System;
using System.Collections.Generic;
using System.Text;

namespace Dapper.Json.Tests
{
    public class Account
    {
        public Account(bool testData)
        {
            AccountId = 54;
            CreationDate = new DateTime(2020, 2, 3);
            AccountDetails = new List<AccountDetail>()
            {
                new AccountDetail()
                {
                    AccountDetailId = 678,
                    Amount = 100m
                },
                new AccountDetail()
                {
                    AccountDetailId = 621,
                    Amount = 1246m
                },
                new AccountDetail()
                {
                    AccountDetailId = 645,
                    Amount = 16789m
                }
            };
            AccountType = new AccountType()
            {
                AccountTypeId = 44,
                Description = "This is a proper account type"
            };
        }

        public Account()
        {
            AccountDetails = new List<AccountDetail>();
            AccountType = new AccountType();
        }

        public int AccountId { get; set; }
        public DateTime CreationDate { get; set; }

        public int AccountTypeId { get; set; }
        public AccountType AccountType { get; set; }
        public List<AccountDetail> AccountDetails { get; set; }
    }

    public class AccountDetail
    {
        public int AccountDetailId { get; set; }
        public decimal Amount { get; set; }

    }

    public class AccountType
    {
        public int AccountTypeId { get; set; }
        public int AccountId { get; set; }
        public string Description { get; set; }

    }
}
