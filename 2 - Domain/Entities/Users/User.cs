using System;
using System.Collections.Generic;
using Domain.Entities.Accounts;

namespace Domain.Entities.Users
{
    public class User : Entity
    {

        public User()
        {
            Address = new Address();
            BankAccounts = new List<BankAccount>();
        }
        
        public string Name { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDate { get; set; } 
        
        public string Email { get; set; }

        public Address Address { get; set; }
        
        
        public List<BankAccount> BankAccounts { get; set; }

    }
}
