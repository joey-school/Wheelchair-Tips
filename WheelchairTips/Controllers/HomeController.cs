using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WheelchairTips.Models;
using WheelchairTips.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WheelchairTips.Services;

namespace WheelchairTips.Controllers
{
    public class HomeController : Controller
    {
        private readonly WheelchairTipsContext _context;
        private TipService _tipService;

        public HomeController(WheelchairTipsContext context)
        {
            _context = context;
            _tipService = new TipService(context, null);
        }

        public IActionResult Index() {
            TipsCategoriesViewModel2 tipsCategories = new TipsCategoriesViewModel2();

            tipsCategories.CategoriesSelectList = tipsCategories.CategoriesSelectList = _tipService.RenderCategorySelectList();
            tipsCategories.TipCards = _tipService.GetAllTipCards();

            return View(tipsCategories);
        }
    }
}
