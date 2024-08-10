using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZealEducationManager.Entities;
using ZealEducationManager.Models.BatchViewModels;
using ZealEducationManager.Models.CandidatesViewModels;

namespace ZealEducationManager.Controllers
{
    [Authorize]
    public class BatchesController : Controller
    {
        private readonly ZealEducationManagerContext _context;

        public BatchesController(ZealEducationManagerContext context)
        {
            _context = context;
        }

        // GET: Batches
        [Route("batches/all-batches")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Batches.ToListAsync());
		}
		[Route("batches/batch-details")]
		public async Task<IActionResult> ListCandidate(int? id)
        {
            if (id == null)
            {
                TempData["message"] = "Cannot find any data";
                return RedirectToAction("Message", "Dashboard");
            }

            var batch = await _context.Batches
                .FirstOrDefaultAsync(m => m.BatchId == id);
            if (batch == null)
            {
                TempData["message"] = "Cannot find any data";
                return RedirectToAction("Message", "Dashboard");
            }
            var listCandidate = await _context.Candidates.Where(c => c.BatchId == batch.BatchId).ToListAsync();
            ViewData["BatchCode"] = batch.BatchCode;
            return View(listCandidate);

        }
        // GET: Batches/Create
        [Route("batches/add-batch")]
        public async Task<IActionResult> Create()
        {
            return View();
        }


        // POST: Batches/Create
        [Route("batches/add-batch")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddBatchViewModel batch)
        {
            if (ModelState.IsValid)
            {
                var existBatchCode = _context.Batches.FirstOrDefault(c => c.BatchCode == batch.BatchCode);
                if (existBatchCode != null)
                {
                    ModelState.AddModelError("BatchCode", "Batch Code has already existed"); 
                    return View(batch);
                }
                if (batch.StartDate >= batch.EndDate)
                {
                    ModelState.AddModelError("EndDate", "The start date must be earlier than the end date");
                    return View(batch);
                }
                var newBatch = new Batch { 
                    BatchCode = batch.BatchCode,
                    StartDate = batch.StartDate,
                    EndDate = batch.EndDate
                };
                _context.Add(newBatch);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(batch);
        }

        // GET: Batches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["message"] = "Cannot find any data";
                return RedirectToAction("Message", "Dashboard");
            }

            var batch = await _context.Batches.FindAsync(id);
            if (batch == null)
            {
                TempData["message"] = "Cannot find any data";
                return RedirectToAction("Message", "Dashboard");
            }
            AddBatchViewModel modelView = new AddBatchViewModel
            {
                BatchId = batch.BatchId,
                BatchCode = batch.BatchCode,
                StartDate = batch.StartDate,
                EndDate = batch.EndDate
            };
            return View(modelView);
        }

        // POST: Batches/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BatchId,BatchCode,StartDate,EndDate")] AddBatchViewModel batch)
        {
            if (id != batch.BatchId)
            {
                TempData["message"] = "Cannot find any data";
                return RedirectToAction("Message", "Dashboard");
            }

            if (ModelState.IsValid)
            {
                try
                {
                   
                    var existBatchCode = _context.Batches
                        .Where(c => c.BatchCode == batch.BatchCode && c.BatchId != batch.BatchId).FirstOrDefault();
                    if (existBatchCode != null)
                    {
                        ModelState.AddModelError("BatchCode", "Batch Code has already existed");
                        return View(batch);
                    }
                    if (batch.StartDate >= batch.EndDate)
                    {
                        ModelState.AddModelError("EndDate", "The start date must be earlier than the end date");
                        return View(batch);
                    }
                    var updatedBatch = _context.Batches
                        .Where(c => c.BatchId == batch.BatchId).FirstOrDefault();
                    if (updatedBatch != null)
                    {
                        updatedBatch.BatchCode = batch.BatchCode;
                        updatedBatch.StartDate = batch.StartDate;
                        updatedBatch.EndDate = batch.EndDate;
                        _context.Update(updatedBatch);
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BatchExists(batch.BatchId))
                    {
                        TempData["message"] = "Cannot find any data";
                        return RedirectToAction("Message", "Dashboard");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(batch);
        }

        // GET: Batches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var batch = await _context.Batches
                .FirstOrDefaultAsync(m => m.BatchId == id);
            if (batch == null)
            {
                TempData["message"] = "Cannot find any data";
                return RedirectToAction("Message", "Dashboard");
            }

            return View(batch);
        }

        // POST: Batches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        { try
            {
				var batch = await _context.Batches.FindAsync(id);
				if (batch != null)
				{
					_context.Batches.Remove(batch);
				}

				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			} catch
            {
                TempData["message"] = "Cannot Delete this Record";
                return RedirectToAction("Message","Dashboard");
            }

            
        }

        private bool BatchExists(int id)
        {
            return _context.Batches.Any(e => e.BatchId == id);
        }
    }
}
