using BankAppUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankAppUI.ViewModels
{
    public class TransactionViewModel
    {
        public int accountId { get; set; }

        public List<Transaction> list { get; set; }
    }
}
