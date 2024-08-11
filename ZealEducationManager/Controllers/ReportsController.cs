using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using ZealEducationManager.Entities;
using ZealEducationManager.Models.CandidatesViewModels;
using ZealEducationManager.Models.ExamsViewModel;

namespace ZealEducationManager.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly ZealEducationManagerContext _context;

        public ReportsController(ZealEducationManagerContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> BatchesAboutToEnd()
        {
            DateTime currentDate = DateTime.Now;
            DateOnly current = DateOnly.FromDateTime(currentDate);
            DateOnly upcomingEndDate = DateOnly.FromDateTime(currentDate.AddDays(15)); 

            var aboutToEndBatches = await _context.Batches
                .Where(b => b.EndDate >= current && b.EndDate <= upcomingEndDate)
                .ToListAsync();

            return View(aboutToEndBatches);
        }
        public async Task<IActionResult> RecentlyStartedBatches()
        {
            DateTime currentDate = DateTime.Now;
            DateOnly current = DateOnly.FromDateTime(currentDate);
            DateOnly recentStartDate = DateOnly.FromDateTime(currentDate.AddDays(-15)); // Adjust this as per your requirement

            var recentlyStartedBatches = await _context.Batches
                .Where(b => b.StartDate >= recentStartDate && b.StartDate <= current)
                .ToListAsync();

            return View(recentlyStartedBatches);
        }

        public async Task<IActionResult> ListBatches()
        {
            return View(await _context.Batches.ToListAsync());
        }

        public async Task<IActionResult> ListBatchExams(int? id)
        {
            var exams = await _context.Exams.Include(e => e.Course).Include(e => e.Batch)
                .Where(e => e.BatchId == id).ToListAsync();
            return View(exams);
        }


        public async Task<IActionResult> MonthYearCandidates(int? month, int? year)
        {
            if (!month.HasValue || !year.HasValue)
            {
                // Default to the current month and year if not provided
                month = DateTime.Now.Month;
                year = DateTime.Now.Year;
            }
            if (month < 1 || month > 12 || year <1)
            {
                TempData["message"] = "Invalid Month/Year";
                return RedirectToAction("Message", "Dashboard");
            }


            DateTime startDate = new DateTime(year.Value, month.Value, 1);
            DateTime endDate = startDate.AddMonths(1);

            var candidates = await _context.Candidates.Include(c => c.Batch).ToListAsync();
            List<Candidate> listCandidate = new List<Candidate>();
            foreach (var can in candidates)
            {
                if (can.DateOfJoining.ToDateTime(TimeOnly.MinValue) >= startDate &&
                            can.DateOfJoining.ToDateTime(TimeOnly.MinValue) < endDate)
                {
                    listCandidate.Add(can);
                }
            }
            ViewBag.Month = month;
            ViewBag.Year = year;
            if (listCandidate == null)
            {
                return RedirectToAction("Index");
            }

            return View(listCandidate);
        }

        // Report showing details of  Outstanding Fee by the Candidates.

        public async Task<IActionResult> OutstandingFeeCandidates()
        {
            var candidates = await _context.Candidates.Include(c => c.Batch)
                .Where(c => c.OutstandingFee!= 0  || c.OutstandingFee == null ).ToListAsync();
            if (candidates == null)
            {
                TempData["message"] = "No Candidate has Outstanding Fee";
                return RedirectToAction("Message", "Dashboard");
            }
            return View(candidates);

        }


    }
}
