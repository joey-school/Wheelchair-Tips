using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WheelchairTips.Models;
using WheelchairTips.Models.ViewModels;

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
            tipsCategories.Categories = _context.Category.ToList();
            return View(tipsCategories);
        }

        public IActionResult Joey()
        {
           

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
