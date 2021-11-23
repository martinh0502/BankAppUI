using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankAppUI.ViewModels
{
    public class TransferViewModel
    {
        public int accountId { get; set; }

        public int targetAccount { get; set; }

        public int amount { get; set; }

        public IEnumerable<SelectListItem> items { get; set; }
    }
}
