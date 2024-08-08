using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZealEducationManager.Entities;
using ZealEducationManager.Models.CandidatesViewModels;
using ZealEducationManager.Models.ExamsViewModel;

namespace ZealEducationManager.Controllers
{
    [Authorize]
    public class ExamsController : Controller
    {
        private readonly ZealEducationManagerContext _context;

        public ExamsController(ZealEducationManagerContext context)
        {
            _context = context;
        }

        // GET: Exams
        public async Task<IActionResult> Index()
        {
            var zealEducationManagerContext = _context.Exams.Include(e => e.Batch).Include(e => e.Course);
            return View(await zealEducationManagerContext.ToListAsync());
        }

        // GET: Exams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exam = await _context.Exams
                .Include(e => e.Batch)
                .Include(e => e.Course)
                .FirstOrDefaultAsync(m => m.ExamId == id);
            if (exam == null)
            {
                return NotFound();
            }

            return View(exam);
        }

		public async Task<IActionResult> ListCandidate(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var exam = await _context.Exams
				.FirstOrDefaultAsync(m => m.ExamId == id);
			if (exam == null)
			{
				return NotFound();
			}
			var listCandidate = await _context.ExamResults.Include(e => e.Candidate)
                .Where(c => c.ExamId == exam.ExamId).ToListAsync();
            ViewData["ExamId"] = exam.ExamId;
            return View(listCandidate);
		}

		// GET: Exams/Create
		public IActionResult Create()
        {
            var viewModel = new AddExamModel
            {
                CourseCode = _context.Courses.Select(c => new SelectListItem
                {
                    Value = c.CourseId.ToString(),
                    Text = c.CourseCode
                }),
				BatchCode = _context.Batches.Select(b => new SelectListItem
				{
					Value = b.BatchId.ToString(),
					Text = b.BatchCode
				})
			};
			return View(viewModel);
        }

        // POST: Exams/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddExamModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var exam = new Exam
                {
                    ExamId = viewModel.ExamId,
                    ExamDate = viewModel.ExamDate,
                    CourseId = viewModel.CourseId,
                    BatchId = viewModel.BatchId,
                };
                _context.Add(exam);
                await _context.SaveChangesAsync();
                // Get all candidates in the batch
                var candidates = await _context.Candidates
                    .Where(c => c.BatchId == exam.BatchId)
                    .ToListAsync();

                // Create ExamResult records for each candidate
                foreach (var candidate in candidates)
                {
                    var examResult = new ExamResult
                    {
                        ExamId = exam.ExamId,
                        CandidateId = candidate.CandidateId
                        // MarkObtained is left null as per requirement
                    };
                    _context.ExamResults.Add(examResult);
                }

                // Save all changes to the database
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
			return View(viewModel);
        }

        //Enter Mark For Candidate
        public async Task<IActionResult> EnterMarks(int id)
        {
            var examresult = await _context.ExamResults.Where(e => e.ExamId == id).Select(er => new EnterMarksModel
            {
                ResultId = er.ResultId,
                ExamId = id,
                CandidateId = er.CandidateId,
                MarksObtained = er.MarksObtained,
                Candidate = er.Candidate,
                Exam = er.Exam
            })
            .ToListAsync();

            return View(examresult);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveMarks(List<EnterMarksModel> viewModel )
        {
            if (ModelState.IsValid)
            {
                foreach (var result in viewModel)
                {
                    var eachResult = await _context.ExamResults
                        .FirstOrDefaultAsync(er => er.ExamId == result.ExamId && er.CandidateId == result.CandidateId);

                    if (eachResult == null)
                    {
                        eachResult = new ExamResult
                        {
                            ExamId = result.ExamId,
                            CandidateId = result.CandidateId,
                            MarksObtained = result.MarksObtained
                        };
                        _context.ExamResults.Add(eachResult);
                    }
                    else
                    {
                        eachResult.MarksObtained = result.MarksObtained;
                        _context.ExamResults.Update(eachResult);
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Index"); // Redirect to a relevant action
            }
            return View("EnterMarks", viewModel);
        }

        // GET: Exams/Edit/5
        [Route("exam/edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var exam = await _context.Exams.FindAsync(id);
            if (exam == null)
            {
                return NotFound();
            }

            var viewModel = new EditExamModel
            {
                ExamId = exam.ExamId,
                ExamDate = exam.ExamDate,
                CourseId = exam.CourseId,
                BatchId = exam.BatchId,
                CourseCode = _context.Courses.Select(c => new SelectListItem
                {
                    Value = c.CourseId.ToString(),
                    Text = c.CourseCode
                }),
                BatchCode = _context.Batches.Select(b => new SelectListItem
                {
                    Value = b.BatchId.ToString(),
                    Text = b.BatchCode
                })
            };
            return View(viewModel);
        }

        // POST: Exams/Edit/5
        [Route("exam/edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditExamModel viewModel)
        {
            if (id != viewModel.ExamId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var exam = await _context.Exams.FindAsync(viewModel.ExamId);
                    exam.ExamDate = viewModel.ExamDate;
                    exam.BatchId = viewModel.BatchId;
                    exam.CourseId = viewModel.CourseId;
                    _context.Update(exam);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExamExists(viewModel.ExamId))
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
            return View(viewModel);
        }

        // GET: Exams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exam = await _context.Exams
                .Include(e => e.Batch)
                .Include(e => e.Course)
                .FirstOrDefaultAsync(m => m.ExamId == id);
            if (exam == null)
            {
                return NotFound();
            }

            return View(exam);
        }

        // POST: Exams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exam = await _context.Exams.FindAsync(id);
            if (exam != null)
            {
                _context.Exams.Remove(exam);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExamExists(int id)
        {
            return _context.Exams.Any(e => e.ExamId == id);
        }
    }
}
