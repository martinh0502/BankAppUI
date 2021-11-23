using BankAppUI.Models;
using BankAppUI.Repositories;
using BankAppUI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankAppUI.Controllers
{
    public class AdminController : Controller
    {
        private readonly ICustomerRepo _customerRepo;
        private readonly IAccountRepository _accountRepository;
        private readonly ILoanRepository _loanRepository;

        public AdminController(ICustomerRepo customerRepo, IAccountRepository accountRepository, ILoanRepository loanRepository)
        {
            _customerRepo = customerRepo;
            _accountRepository = accountRepository;
            _loanRepository = loanRepository;
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Main(string searchString)
        {
            CustomerViewModel model = new();

            if (!String.IsNullOrEmpty(searchString))
            {
                model.CurrentCustomer = await _customerRepo.GetCustomer(int.Parse(searchString));
                if(model.CurrentCustomer != null)
                {
                    ViewBag.Message = string.Empty;
                    model.Accounts = await _accountRepository.GetAccounts(model.CurrentCustomer.CustomerId);
                    foreach (var acc in model.Accounts)
                    {
                        model.Loans.AddRange(await _loanRepository.GetLoans(acc.AccountId));
                    }
                }
            }

            if (model.CurrentCustomer == null)
            {
                ViewBag.Message = "Hitta inte kund";
                model = new();
            }


            return View(model);
        }

        public IActionResult CreateCustomer()
        {
            
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> CreateCustomer(Customer customer)
        {
            customer.CountryCode = "SE";
            customer.Country = "Sweden";
            customer.Telephonecountrycode = "+46";

            customer = await _customerRepo.CreateCustomer(customer);



            if(customer.CustomerId > 0)
            {
                ViewBag.Message = "Kund skapad med kundID: " + customer.CustomerId;
            }
            else
            {
                ViewBag.Message = "Något gick fel";
            }

            return View();
        }

        public IActionResult ApproveLoan(string loanId, string customerId)
        {
            _loanRepository.UpdateLoanStatus(int.Parse(loanId));

            return RedirectToAction("Main", "Admin", new { searchString = int.Parse(customerId) });
        }
        
    }
}
