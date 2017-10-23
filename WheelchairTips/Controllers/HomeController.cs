using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WheelchairTips.Models;
using WheelchairTips.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WheelchairTips.Controllers
{
    public class HomeController : Controller
    {
        private readonly WheelchairTipsContext _context;

        public HomeController(WheelchairTipsContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            TipsCategoriesViewModel tipsCategories = new TipsCategoriesViewModel();
            tipsCategories.Tips = _context.Tip.ToList();

           tipsCategories.Categories = new List<SelectListItem>();

           tipsCategories.Categories.Add(new SelectListItem { Value = "", Text = "All" });

            foreach (var category in _context.Category)
            {
                tipsCategories.Categories.Add(new SelectListItem { Value = category.Id.ToString(), Text = category.Name });
            }

            

            /*tipsCategories.Categories = .ToList();

            tipsCategories.Categories.Add(new Category(0, "All", );
            tipsCategories.Categories = _context.Category.ToList();*/
            return View(tipsCategories);
        }

        [HttpPost]
        public IActionResult Index(TipsCategoriesViewModel model)
        {
            if (ModelState.IsValid)
            {
                var msg = model.CategoryId;
                return RedirectToAction("Index", "Tips", new { categoryId = msg });
            }

            // If we got this far, something failed; redisplay form.
            return View(model);
        }

        public IActionResult Joey(string message)
        {
            ViewData["foo"] = message;

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
