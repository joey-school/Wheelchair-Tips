﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WheelchairTips.Models;

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
            return View(_context.Tip.ToList());
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
