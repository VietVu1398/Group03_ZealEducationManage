using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZealEducationManager.Entities;
using ZealEducationManager.Models.ExamsViewModel;
using ZealEducationManager.Models.PaymentViewModels;

namespace ZealEducationManager.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly ZealEducationManagerContext _context;

        public PaymentsController(ZealEducationManagerContext context)
        {
            _context = context;
        }

        // GET: Payments
        public async Task<IActionResult> Index()
        {
            var zealEducationManagerContext = _context.Payments.Include(p => p.Candidate);
            return View(await zealEducationManagerContext.ToListAsync());
        }

        // GET: Payments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments
                .Include(p => p.Candidate)
                .FirstOrDefaultAsync(m => m.PaymentId == id);
            if (payment == null)
            {
                return NotFound();
            }
            return View(payment);
        }

        // GET: Payments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Payments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( AddPaymentModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var existCandidate = await _context.Candidates.FindAsync(viewModel.CandidateId);
                if (existCandidate == null)
                {
                    ModelState.AddModelError("CandidateId", "Candidate Not Found");
                    return View(viewModel);
                }
                var payment = new Payment
                {
                    PaymentId = viewModel.PaymentId,
                    CandidateId = viewModel.CandidateId,
                    Amount = viewModel.Amount,
                    PaymentDate = viewModel.PaymentDate
                };
                _context.Add(payment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        public async Task<IActionResult> MonthlyReport(int year, int month)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            // Convert DateOnly to DateTime for querying
            var payments = await _context.Payments
                .ToListAsync();
            List<Payment> paymentlist = new List<Payment>();
            foreach (var pm in payments)
            {
                if (ConvertToDateTime(pm.PaymentDate) >= startDate && ConvertToDateTime(pm.PaymentDate) <= endDate)
                {
                    paymentlist.Add(pm);
                }
            }

            return View(paymentlist);
        }

        private DateTime ConvertToDateTime(DateOnly dateOnly)
        {
            return dateOnly.ToDateTime(TimeOnly.MinValue);
        }

        // GET: Payments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            ViewData["CandidateId"] = new SelectList(_context.Candidates, "CandidateId", "CandidateId", payment.CandidateId);
            return View(payment);
        }

        // POST: Payments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PaymentId,CandidateId,Amount,PaymentDate")] Payment payment)
        {
            if (id != payment.PaymentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentExists(payment.PaymentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CandidateId"] = new SelectList(_context.Candidates, "CandidateId", "CandidateId", payment.CandidateId);
            return View(payment);
        }

        // GET: Payments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments
                .Include(p => p.Candidate)
                .FirstOrDefaultAsync(m => m.PaymentId == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment != null)
            {
                _context.Payments.Remove(payment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentExists(int id)
        {
            return _context.Payments.Any(e => e.PaymentId == id);
        }
    }
}
