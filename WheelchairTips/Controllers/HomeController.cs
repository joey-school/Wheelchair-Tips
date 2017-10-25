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

        public IActionResult Index() {
            TipsCategoriesViewModel tipsCategories = new TipsCategoriesViewModel();

            tipsCategories.Tips = _context.Tip.ToList();
            tipsCategories.CategoriesSelectList = new List<SelectListItem>();
            tipsCategories.CategoriesSelectList.Add(new SelectListItem { Value = "", Text = "All" });

            foreach (var category in _context.Category) {
                tipsCategories.CategoriesSelectList.Add(new SelectListItem { Value = category.Id.ToString(), Text = category.Name });
            }

            return View(tipsCategories);
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
