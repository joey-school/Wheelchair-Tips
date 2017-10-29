    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WheelchairTips.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WheelchairTips.Models.ViewModels;
using WheelchairTips.Services;

namespace WheelchairTips.Controllers
{
    public class TipsController : Controller
    {
        private readonly WheelchairTipsContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private TipService _tipService;

        public TipsController(WheelchairTipsContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _tipService = new TipService(context, null);
        }

        public IActionResult Index(string categoryId, string searchQuery)
        {
            TipsCategoriesViewModel2 tipsCategories = new TipsCategoriesViewModel2();

            tipsCategories.CategoriesSelectList = _tipService.RenderCategorySelectList();

            // User filtered only by category.
            if (!String.IsNullOrEmpty(categoryId) && String.IsNullOrEmpty(searchQuery))
            {
                tipsCategories.TipCards = _tipService.GetAllTipCardsByCategory(Int32.Parse(categoryId));
            }

            // User filtered by category + searched for a keyword.
            else if (!String.IsNullOrEmpty(categoryId) && !String.IsNullOrEmpty(searchQuery))
            {
                tipsCategories.TipCards = _tipService.GetAllTipCardsByCategoryAndSearchQuery(Int32.Parse(categoryId), searchQuery);
            }

            // User searched only for a keyword.
            else if (String.IsNullOrEmpty(categoryId) && !String.IsNullOrEmpty(searchQuery))
            {
                tipsCategories.TipCards = _tipService.GetAllTipCardsBySearchQuery(searchQuery);
            }

            // User didn't filtered or searched.
            else
            {
                tipsCategories.TipCards = _tipService.GetAllTipCards();
            }

            return View(tipsCategories);
        }

        // GET: Tips/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tip = await _context.Tip
                .Include(t => t.Category)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (tip == null)
            {
                return NotFound();
            }

            return View(tip);
        }

        // GET: Tips/Create
        [Authorize]
        public IActionResult Create()
        {


            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name");
            return View();
        }

        // POST: Tips/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Content,CategoryId")] Tip tip)
        {
            if (ModelState.IsValid)
            {
                tip.ApplicationUserId = _userManager.GetUserId(HttpContext.User);
                _context.Add(tip);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Id", tip.CategoryId);
            return View(tip);
        }

        // GET: Tips/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tip = await _context.Tip.SingleOrDefaultAsync(m => m.Id == id);
            if (tip == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Id", tip.CategoryId);
            return View(tip);
        }

        // POST: Tips/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,Author,CategoryId")] Tip tip)
        {
            if (id != tip.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tip);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipExists(tip.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Id", tip.CategoryId);
            return View(tip);
        }

        // GET: Tips/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tip = await _context.Tip
                .Include(t => t.Category)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (tip == null)
            {
                return NotFound();
            }

            return View(tip);
        }

        // POST: Tips/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tip = await _context.Tip.SingleOrDefaultAsync(m => m.Id == id);
            _context.Tip.Remove(tip);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool TipExists(int id)
        {
            return _context.Tip.Any(e => e.Id == id);
        }
    }
}
