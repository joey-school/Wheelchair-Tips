using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WheelchairTips.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using WheelchairTips.Models;

namespace WheelchairTips.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UsersController (UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            List<UserListViewModel> model = new List<UserListViewModel>();

            model = userManager.Users.Select(u => new UserListViewModel
            {
                Id = u.Id,
                Name = u.UserName,
                Email = u.Email
            }).ToList();

            return View(model);
        }
    }
}