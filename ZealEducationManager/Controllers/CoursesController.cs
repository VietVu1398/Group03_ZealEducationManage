using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZealEducationManager.Entities;

namespace ZealEducationManager.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ZealEducationManagerContext _context;

        public CoursesController(ZealEducationManagerContext context)
        {
            _context = context;
        }

        // GET: Courses
        [Route("Courses/all-Courses")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Courses.ToListAsync());
        }

        // GET: Courses/Details/5
        [Route("Courses/Course-details")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                TempData["message"] = "Cannot find any data";
                return RedirectToAction("Message", "Dashboard");
            }

            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                TempData["message"] = "Cannot find any data";
                return RedirectToAction("Message", "Dashboard");
            }

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseId,CourseCode,CourseName,CourseFee")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["message"] = "Cannot find any data";
                return RedirectToAction("Message", "Dashboard");
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                TempData["message"] = "Cannot find any data";
                return RedirectToAction("Message", "Dashboard");
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseId,CourseCode,CourseName,CourseFee")] Course course)
        {
            if (id != course.CourseId)
            {
                TempData["message"] = "Cannot find any data";
                return RedirectToAction("Message", "Dashboard");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.CourseId))
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
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["message"] = "Cannot find any data";
                return RedirectToAction("Message", "Dashboard");
            }

            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                TempData["message"] = "Cannot find any data";
                return RedirectToAction("Message", "Dashboard");
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var course = await _context.Courses.FindAsync(id);
                if (course != null)
                {
                    _context.Courses.Remove(course);
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

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }
    }
}
