using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NetBankAppV1.Data;
using NetBankAppV1.Models;

namespace NetBankAppV1.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly BankDbContext _context;

        public TransactionsController(BankDbContext context)
        {
            _context = context;
        }

        // GET: Transactions
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //var account = _context.Accounts.FirstOrDefault(m => m.AccountNumber == id);
            ViewBagInfo(id);
            var transList = await _context.Transcations.Where(m => m.account.AccountNumber == id).ToListAsync();
            IEnumerable<Transaction> result = transList.OrderByDescending(m => m.date).Take(10).ToList();
            return View(result);
        }

        // GET: Transactions
        public async Task<IActionResult> TransactionDateRange(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var transList = await _context.Transcations.Where(m => m.account.AccountNumber == id).ToListAsync();
            IEnumerable<Transaction> result = transList.OrderByDescending(m => m.date).ToList();
            ViewBagInfo(id);
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TransactionDateRange(int id, DateTime from, DateTime to)
        {
            to=to.AddDays(1);
            var transList = await _context.Transcations.Where(m => m.account.AccountNumber == id && m.date >= from && m.date<= to).ToListAsync();
            IEnumerable<Transaction> result = transList.OrderByDescending(m => m.date).ToList();
            ViewBagInfo(id);
            return View(result);
        }

        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transcations
                .FirstOrDefaultAsync(m => m.TranId == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: Transactions/Create
        public IActionResult Deposit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBagInfo(id);
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deposit(int id, [Bind("TranId,amount,date,TranscationType, account")] Transaction transaction)
        {
            var account = _context.Accounts.FirstOrDefault(m => m.AccountNumber == id);
            
            //account.Balance += transaction.amount;
            transaction.account = account;
            transaction.date = DateTime.Now;
            transaction.TranscationType = "Deposit";
            if (ModelState.IsValid&& account.Deposit(transaction.amount) && account.IsActive)
            {

                _context.Add(transaction);
                _context.Update(account);
                await _context.SaveChangesAsync();

                return RedirectToAction("../Accounts/Index");
            }
            ViewBagInfo(id);
            ViewBag.ErrorMessage = "Invalid operation! Check the amount or your account status.";
            return View(transaction);

        }


        // GET: Transactions/Create
        public IActionResult Transfer(int? id)
        {   
            if (id == null)
            {
                return NotFound();
            }
            ViewBagInfo(id);
            ViewBag.AccountList =  _context.Accounts.Where(m => m.customer.CustomerUser.UserName == HttpContext.User.Identity.Name && m.AccountNumber!=id).ToList();
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Transfer(int id, [Bind("TranId,amount,date,TranscationType, account")] Transaction transaction)
        {
            //var TOAccount = _context.Accounts.FindAsync(transaction.account.AccountNumber);
            var ToAccountNumber= transaction.account.AccountNumber;
            var account = _context.Accounts.FirstOrDefault(m => m.AccountNumber == id);
            if (ModelState.IsValid)
            {
                await Withdraw(id, transaction);

                //await _context.SaveChangesAsync();
                if (transaction.TranId != 0)
                {
                    Transaction depositTrans = new Transaction
                    {
                        amount = transaction.amount,
                        date = DateTime.Now,
                        TranscationType = "Deposit",
                        account = transaction.account
                    };


                    await Deposit(ToAccountNumber, depositTrans);
                    return RedirectToAction("../Accounts/Index");
                }

            }
            ViewBagInfo(id);
            ViewBag.ErrorMessage = "Invalid operation! Check the amount or your account status.";
            ViewBag.AccountList = _context.Accounts.Where(m => m.customer.CustomerUser.UserName == HttpContext.User.Identity.Name && m.AccountNumber != id).ToList();
            //return RedirectToAction("../Accounts/Index");
            return View(transaction);
        }

        // GET: Transactions/Create
        public IActionResult PayLoan(int? id)
        {
            var account = _context.Accounts.FirstOrDefault(m => m.AccountNumber == id);
            if (id == null || !account.GetType().Name.Equals("Loan"))
            {
                return NotFound("Error! You have to select a Loan account to pay loan or your account number is not valid!");
            }
            ViewBagInfo(id);
            ViewBag.AccountList = _context.Accounts.Where(m => m.customer.CustomerUser.UserName == HttpContext.User.Identity.Name && m.AccountNumber != id && !m.GetType().Name.Equals("Loan")).ToList();
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PayLoan(int id, [Bind("TranId,amount,date,TranscationType, account")] Transaction transaction)
        {
            var account = _context.Accounts.FirstOrDefault(m => m.AccountNumber == id);
            if (ModelState.IsValid)
            {
                var FromAccountNumber = transaction.account.AccountNumber;
                transaction.account.AccountNumber = id;
                await Transfer(FromAccountNumber, transaction);

                return RedirectToAction("../Accounts/Index");
            }
            ViewBagInfo(id);
            ViewBag.ErrorMessage = "Invalid operation! Check the amount or your account status.";
            return View(transaction);
        }


        // GET: Transactions/Edit/5
        public IActionResult Withdraw(int? id)
        {
            var account = _context.Accounts.FirstOrDefault(m => m.AccountNumber == id);
            if (account.GetType().Name.Equals("BusinessAccount"))
            {
                ViewBag.OverDraft=((BusinessAccount)account).OverdraftAmount;
            }
            if (account.GetType().Name.Equals("TermDeposit"))
            {
                ViewBag.TermDepositMsg = "If you withdraw early, you will have 10% penalty on your balance. Else you will making interest. Are you sure you want to withdraw $" +
                    account.Balance + "?";
            }
            if (id == null)
            {
                return NotFound();
            }
            ViewBagInfo(id);
          
            return View();
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Withdraw(int id, [Bind("TranId,amount,date,TranscationType, account")] Transaction transaction)
        {

            var account = _context.Accounts.FirstOrDefault(m => m.AccountNumber == id);
            double transAmount = 0;
            //account.Balance += transaction.amount;
            transaction.account = account;
            transaction.date = DateTime.Now;
            transaction.TranscationType = "Withdraw";
            if (account.GetType().Name.Equals("TermDeposit")&& DateTime.Now >= ((TermDeposit)account).TermEndedDate)
            {       
                ((TermDeposit)account).termEnded = true;
            }
            if(account.GetType().Name.Equals("TermDeposit"))
            {
                transAmount = ((TermDeposit)account).Earning();
            }

            if (ModelState.IsValid&& account.Withdraw(transaction.amount) && account.IsActive)
            {
                //account.Balance -= transaction.amount;
                //transaction.account = account;
                if (transAmount != 0)
                {
                    transaction.amount = transAmount;
                }
                _context.Add(transaction);
                _context.Update(account);
                //if (account.Balance == 0 && account.GetType().Name.Equals("BusinessAccount") && ((BusinessAccount)account).OverdraftAmount < 100 && ((BusinessAccount)account).AllowLoan == true)
                //{
                //    //creating new loan account
                //    Loan newLoan = new Loan();
                //    newLoan.Balance = 100 - ((BusinessAccount)account).OverdraftAmount;
                //    ((BusinessAccount)account).AllowLoan = false;

                //    Transaction newTrans = new Transaction();

                //    AccountsController accountsController = new AccountsController(_context);

                //    newLoan.User = transaction.account.User;
                //    newLoan.IsActive = true;

                //    await accountsController.CreateLoan(newLoan);


                //    newTrans.account = newLoan;
                //    newTrans.date = DateTime.Now;
                //    newTrans.TranscationType = "Deposite";

                //    _context.Add(newLoan);
                //    _context.Add(newTrans);
                //    await _context.SaveChangesAsync();
                //    //account.Add(newLoan);
                //    //newLoan.customer.account = account;
                //}

                await _context.SaveChangesAsync();

                return RedirectToAction("../Accounts/Index");
            }
            ViewBagInfo(id);
            ViewBag.ErrorMessage = "Invalid operation! Check the amount or your account status.";
            return View(transaction);

        }

        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transcations
                .FirstOrDefaultAsync(m => m.TranId == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transcations.FindAsync(id);
            _context.Transcations.Remove(transaction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(int id)
        {
            return _context.Transcations.Any(e => e.TranId == id);
        }

        public void ViewBagInfo(int? id)
        {
            if (id != null)
            {
                var account = _context.Accounts.FirstOrDefault(m => m.AccountNumber == id);
                ViewBag.Accounts = id;
                ViewBag.Balance = account.Balance;
            }
        }
    }
}
