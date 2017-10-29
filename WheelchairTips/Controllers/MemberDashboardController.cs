using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WheelchairTips.Controllers
{
    public class MemberDashboardController : Controller
    {
        public IActionResult Index()
        {
            // For now redirect to 'manage tips'.
            return RedirectToAction("Index", "ManageTips");
        }
    }
}