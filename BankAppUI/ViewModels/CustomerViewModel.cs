using BankAppUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankAppUI.ViewModels
{
    public class CustomerViewModel
    {
        public CustomerViewModel()
        {
            CurrentCustomer = new();
            Accounts = new();
            Loans = new();
        }
        public Customer CurrentCustomer { get; set; }

        public List<Account> Accounts { get; set; }

        public List<Loan> Loans { get; set; }
    }
}
