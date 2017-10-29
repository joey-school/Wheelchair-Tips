using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WheelchairTips.Models;
using WheelchairTips.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
            TipsCategoriesViewModel2 tipsCategories = new TipsCategoriesViewModel2();

            //tipsCategories.Tips = _context.Tip.ToList();

            List<TipCardViewModel> tipCards = new List<TipCardViewModel>();

            foreach (var tip in _context.Tip.Include(t => t.Category).ToList())
            {
                TipCardViewModel tipCard = new TipCardViewModel
                {
                    Id = tip.Id,
                    Title = tip.Title,
                    ContentShort = tip.Content.Substring(0, 100).Trim() + "...",
                    ImageName = tip.ImageName,
                    CategoryName = tip.Category.Name,
                    IsDisabled = tip.IsDisabled
                };

                tipCards.Add(tipCard);
            }

            tipsCategories.TipCards = tipCards;
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
