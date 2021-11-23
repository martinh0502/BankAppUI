using BankAppUI.Models;
using BankAppUI.ModelsIdentity;
using BankAppUI.Repositories;
using BankAppUI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankAppUI.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerRepo _customerRepo;
        private readonly IAccountRepository _accountRepository;
        private readonly ILoanRepository _loanRepository;
        private readonly ITransactionRepository _transactionRepository;
        private UserManager<ApplicationUser> _usermanager;

        public CustomerController(ICustomerRepo customerRepo, IAccountRepository accountRepository, ILoanRepository loanRepository, UserManager<ApplicationUser> usermanager, ITransactionRepository transactionRepository)
        {
            _customerRepo = customerRepo;
            _accountRepository = accountRepository;
            _loanRepository = loanRepository;
            _usermanager = usermanager;
            _transactionRepository = transactionRepository;
        }

        [Authorize(Roles = "NormalUser")]
        public async Task<IActionResult> Main()
        {
            var user = await _usermanager.GetUserAsync(User);

            CustomerViewModel model = new();

            model.CurrentCustomer = await _customerRepo.GetCustomer(user.CustomerId);
            model.Accounts = await _accountRepository.GetAccounts(user.CustomerId);
            foreach (var acc in model.Accounts)
            {
                model.Loans.AddRange(await _loanRepository.GetLoans(acc.AccountId));
            }

            return View(model);
        }

        public async Task<IActionResult> Transfer()
        {
            var user = await _usermanager.GetUserAsync(User);
            var list = await _accountRepository.GetAccounts(user.CustomerId);

            IEnumerable<SelectListItem> items = list.Select(c => new SelectListItem
            {
                Value = Convert.ToString(c.AccountId),
                Text = Convert.ToString(c.AccountId)

            });

            TransferViewModel model = new();

            model.items = items;

            return View(model);
        }

        [HttpPost]
        public IActionResult Transfer(TransferViewModel model)
        {
            try
            {
                _transactionRepository.Transfer(model.accountId, model.targetAccount, model.amount);
            }
            catch(Exception ex)
            {
                ViewBag.Message = "Gick inte att genomföra transaktion";
                return View(model);
            }

            return RedirectToAction("Main", "Customer");
        }

        public async Task<IActionResult> ViewTransactions(int id)
        {
            try
            {
                List<Transaction> list = await _transactionRepository.GetTransactions(id);

                TransactionViewModel model = new();

                if(list == null)
                {
                    ViewBag.Message = "Inget här";
                    return View(new List<Transaction>());
                }

                model.list = list;
                model.accountId = id;

                return View(model);


            }
            catch(Exception ex)
            {
                return RedirectToAction("Main", "Customer");
            }
        }


        public async Task<IActionResult> CreateAccount()
        {
            Account account = new();

            var user = await _usermanager.GetUserAsync(User);

            _accountRepository.PostAccount(account, user.CustomerId);

            return RedirectToAction("Main", "Customer");
        }

        public IActionResult CreateLoan(int id)
        {
            Loan loan = new();

            loan.AccountId = id;

            return View(loan);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLoan(Loan loan)
        {
            try
            {
                loan = await _loanRepository.PostLoan(loan);

                if(loan == null)
                {
                    ViewBag.Message = "Kunde inte skapa lån";
                    return View();
                }

                return RedirectToAction("Main", "Customer");


            }
            catch (Exception ex)
            {
                return RedirectToAction("Main", "Customer");
            }
        }

    }
}
