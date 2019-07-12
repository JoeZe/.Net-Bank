using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NetBankAppV1.Data;
using NetBankAppV1.Models;
using Microsoft.AspNetCore.Authorization;

namespace NetBankAppV1.Controllers
{
    [Authorize] 
    public class AccountsController : Controller
    {
        private readonly BankDbContext _context;

        public AccountsController(BankDbContext context)
        {
            _context = context;
        }

        // GET: Accounts
        public async Task<IActionResult> Index()
        {
            var user = await _context.Users.FirstOrDefaultAsync(m => m.UserName == HttpContext.User.Identity.Name);
            var customer = await _context.customers.FirstOrDefaultAsync(m => m.UserRef == user.Id);
            if (customer == null)
            {
                return RedirectToAction("../Customers/Create");
            }
            ViewBag.CustomerName = customer.FirstName + " " + customer.LastName;
            return View(await _context.Accounts.Where(m => m.customer.CustomerUser.UserName == HttpContext.User.Identity.Name &&m.IsActive).ToListAsync());
        }

        // GET: Accounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .FirstOrDefaultAsync(m => m.AccountNumber == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // GET: Accounts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountNumber,Balance,InterestRate,IsActive")] Account account)
        {
            if (ModelState.IsValid)
            {
                _context.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        // GET: Accounts/Create
        
        public IActionResult CreateCheckingAccount()
        {
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCheckingAccount([Bind("AccountNumId,User,Balance,IsActive")] CheckingAccount account)
        {
            if (ModelState.IsValid)
            {
                account.Balance = ((CheckingAccount)account).GetMonthlyInterest();
                var user = await _context.Users.FirstOrDefaultAsync(m => m.UserName == HttpContext.User.Identity.Name);
                var customer = await _context.customers.FirstOrDefaultAsync(m => m.UserRef == user.Id);
                account.customer = customer;
                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        // GET: Accounts/Create
        public IActionResult CreateBusinessAccount()
        {
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBusinessAccount([Bind("AccountNumId,User,Balance,IsActive")] BusinessAccount account)
        {
            if (ModelState.IsValid)
            {

                var user = await _context.Users.FirstOrDefaultAsync(m => m.UserName == HttpContext.User.Identity.Name);
                var customer = await _context.customers.FirstOrDefaultAsync(m => m.UserRef == user.Id);
                account.customer = customer;
                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        // GET: Accounts/Create
        public IActionResult CreateLoan()
        {
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLoan([Bind("AccountNumId,User,Balance,IsActive")] Loan account)
        {
            if (ModelState.IsValid)
            {
                account.Balance =account.Balance+ account.Balance*(((Loan) account).Interest/100);
                var user = await _context.Users.FirstOrDefaultAsync(m => m.UserName == HttpContext.User.Identity.Name);
                var customer = await _context.customers.FirstOrDefaultAsync(m => m.UserRef == user.Id);
                account.customer = customer;
                account.IsActive = true;
                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        // GET: Accounts/Create
        public IActionResult CreateTermDeposit()
        {
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTermDeposit([Bind("AccountNumId,User,Balance,IsActive")] TermDeposit account)
        {
            if (ModelState.IsValid)
            {
                account.TermEndedDate=DateTime.Today.AddDays(1);
                var user = await _context.Users.FirstOrDefaultAsync(m => m.UserName == HttpContext.User.Identity.Name);
                var customer = await _context.customers.FirstOrDefaultAsync(m => m.UserRef == user.Id);
                account.customer = customer;
                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }


        // GET: Accounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .FirstOrDefaultAsync(m => m.AccountNumber == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            //_context.Accounts.Remove(account);
            account.IsActive = false;
            _context.Update(account);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.AccountNumber == id);
        }
    }
}
