using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WheelchairTips.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WheelchairTips.Controllers
{
    public class ManageTipsController : Controller
    {
        private readonly WheelchairTipsContext _context;

        public ManageTipsController(WheelchairTipsContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            IEnumerable<Tip> model = _context.Tip.Include(t => t.ApplicationUser).ToList();

            return View(model);
        }

        public IActionResult Edit(int? id)
        {
            Tip model;

            if (id == null)
            {
                return NotFound();
            }

            model = _context.Tip
                .Single(m => m.Id == id);

            if (model == null)
            {
                return NotFound();
            }

            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", model.CategoryId);

            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tip = await _context.Tip.SingleOrDefaultAsync(s => s.Id == id);

            if (await TryUpdateModelAsync<Tip>(tip, "", t => t.Title, t => t.Content, t => t.CategoryId))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }

            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Id", tip.CategoryId);
            return View(tip);
        }

        private bool TipExists(int id)
        {
            return _context.Tip.Any(e => e.Id == id);
        }
    }
}