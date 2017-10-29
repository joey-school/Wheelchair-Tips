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
                .Include(t => t.ApplicationUser)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (tip == null)
            {
                return NotFound();
            }

            return View(tip);
        }
    }
}
