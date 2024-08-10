using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZealEducationManager.Entities;
using ZealEducationManager.Models.Faculties;

namespace ZealEducationManager.Controllers
{
    [Authorize]
    public class FacultiesController : Controller
    {
        private readonly ZealEducationManagerContext _context;

        public FacultiesController(ZealEducationManagerContext context)
        {
            _context = context;
        }

        // GET: Faculties
        public async Task<IActionResult> Index()
        {
            return View(await _context.Faculties.ToListAsync());
        }

        // GET: Faculties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                TempData["message"] = "Cannot find any data";
                return RedirectToAction("Message", "Dashboard");
            }

            var faculty = await _context.Faculties.Include(f => f.FacultyBatches).ThenInclude(fb => fb.Batch)
                .FirstOrDefaultAsync(m => m.FacultyId == id);
            if (faculty == null)
            {
                TempData["message"] = "Cannot find any data";
                return RedirectToAction("Message", "Dashboard");
            }

            return View(faculty);
        }

        // GET: Faculties/Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FacultyId,FirstName,LastName,ContactInfo,FacultyCode")] Faculty faculty)
        {
            if (ModelState.IsValid)
            {
                _context.Add(faculty);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(faculty);
        }

        // GET: Faculties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["message"] = "Cannot find any data";
                return RedirectToAction("Message", "Dashboard");
            }

            var faculty = await _context.Faculties.FindAsync(id);
            if (faculty == null)
            {
                TempData["message"] = "Cannot find any data";
                return RedirectToAction("Message", "Dashboard");
            }
            return View(faculty);
        }

        // POST: Faculties/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FacultyId,FirstName,LastName,ContactInfo,FacultyCode")] Faculty faculty)
        {
            if (id != faculty.FacultyId)
            {
                TempData["message"] = "Cannot find any data";
                return RedirectToAction("Message", "Dashboard");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(faculty);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FacultyExists(faculty.FacultyId))
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
            return View(faculty);
        }

        // GET: Faculties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["message"] = "Cannot find any data";
                return RedirectToAction("Message", "Dashboard");
            }

            var faculty = await _context.Faculties
                .FirstOrDefaultAsync(m => m.FacultyId == id);
            if (faculty == null)
            {
                TempData["message"] = "Cannot find any data";
                return RedirectToAction("Message", "Dashboard");
            }

            return View(faculty);
        }

        // POST: Faculties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try 
            { 
                var faculty = await _context.Faculties.FindAsync(id);
                if (faculty != null)
                {
                    _context.Faculties.Remove(faculty);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["message"] = "Cannot Delete this Record";
                return RedirectToAction("Message", "Dashboard");
            }
}

        private bool FacultyExists(int id)
        {
            return _context.Faculties.Any(e => e.FacultyId == id);
        }

        //Add Batch for Teacher
        public async Task<IActionResult> AssignBatches(int id)
        {
            if (id == null)
            {
                TempData["message"] = "Cannot find any data";
                return RedirectToAction("Message", "Dashboard");
            }
            var faculty = await _context.Faculties.FindAsync(id);
            if (faculty == null)
            {
                return NotFound();
            }

            var viewModel = new AssignFacultyViewModel
            {
                FacultyID = id,
                Batches = _context.Batches
                    .Where(b => !_context.FacultyBatches.Any(fb => fb.BatchId == b.BatchId && fb.FacultyId == id))
                    .Select(b => new SelectListItem
                    {
                        Value = b.BatchId.ToString(),
                        Text = b.BatchCode
                    }).ToList()
            };

            return View(viewModel);
        }

        // POST: Faculty/AssignBatches
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignBatches(AssignFacultyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var faculty = await _context.Faculties
                .Include(f => f.FacultyBatches)
                .FirstOrDefaultAsync(f => f.FacultyId == model.FacultyID);

            if (faculty == null)
            {
                return NotFound();
            }

            // Add new assignment
            if (model.SelectedBatch.HasValue)
            {
                int batchId = model.SelectedBatch.Value;
                _context.FacultyBatches.Add(new FacultyBatch
                {
                    FacultyId = model.FacultyID,
                    BatchId = batchId
                });

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Details), new { id = model.FacultyID });
        }

    }
}
