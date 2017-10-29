using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WheelchairTips.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using System.IO;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using WheelchairTips.Authorization;
using WheelchairTips.Services;

namespace WheelchairTips.Controllers
{
    // Only users with role 'Admin' and 'Member' have access to this controller. Otherwise show warning.
    [Authorize(Roles = "Admin, Member")]
    public class ManageTipsController : Controller
    {
        private readonly WheelchairTipsContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthorizationService _authorizationService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private TipService _tipService;

        public ManageTipsController(WheelchairTipsContext context, UserManager<ApplicationUser> userManager, IAuthorizationService authorizationService, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _authorizationService = authorizationService;
            _hostingEnvironment = hostingEnvironment;
            _tipService = new TipService(context, hostingEnvironment);
        }

        public IActionResult Index()
        {
            IEnumerable<Tip> tips = null;

            // When admin is online -> show all tips.
            if (User.IsInRole("Admin"))
            {
                tips = _tipService.GetAllTipsIncludeUsers();
            }

            // When member is online -> show only his tips.
            else if (User.IsInRole("Member"))
            {
                tips = _tipService.GetAllTipsFromMember(_userManager.GetUserId(HttpContext.User));
            }

            return View(tips);
        }

        public IActionResult Create()
        {
            // Create category list in viewbag to be used in view.
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Title, Content, CategoryId")] Tip tip)
        {
            // Check if input is valid.
            if (ModelState.IsValid)
            {
                var imageName = _tipService.HandleImageUpload(HttpContext.Request.Form.Files);
                var userId = _userManager.GetUserId(HttpContext.User);

                _tipService.CreateTip(tip, imageName, userId);

                return RedirectToAction("Index");
            }

            // Input is invalid..

            // Create category list in viewbag to be used in view.
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Id", tip.CategoryId);

            return View(tip);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            // We dont have an id -> show 404.
            if (id == null)
            {
                return NotFound();
            }

            // Get tip with specified id.
            var tip = _tipService.GetTipById(id);

            // Tip doesn't exist -> show 404
            if (tip == null)
            {
                return NotFound();
            }

            // Check if user is authorized to edit tip.
            var isAuthorized = await _authorizationService.AuthorizeAsync(User, tip, TipOperations.Update);

            // When user is not authorized show warning.
            if (!isAuthorized)
            {
                return new ChallengeResult();
            }

            // Create category list in viewbag to be used in view.
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", tip.CategoryId);

            return View(tip);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            // We dont have an id -> show 404.
            if (id == null)
            {
                return NotFound();
            }

            // Get tip with specified id.
            var tip = _tipService.GetTipById(id);

            // Tip doesn't exist -> show 404.
            if (tip == null)
            {
                return NotFound();
            }

            // Check if user is authorized to edit tip.
            var isAuthorized = await _authorizationService.AuthorizeAsync(User, tip, TipOperations.Update);

            // When user is not authorized show warning.
            if (!isAuthorized)
            {
                return new ChallengeResult();
            }

            // Init updating of tip with new values.
            if (await TryUpdateModelAsync<Tip>(tip, "", t => t.Title, t => t.Content, t => t.ImageName, t => t.CategoryId))
            {
                try
                {
                    var imageName = _tipService.HandleImageUpload(HttpContext.Request.Form.Files);

                    // When there is no image name there isn't one uploaded -> so use existing image name.
                    if (String.IsNullOrEmpty(imageName))
                    {
                        imageName = tip.ImageName;
                    }

                    tip.ImageName = imageName;

                    // All input is valid -> save model to database.
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException)
                {
                    // Input is invalid, we will return to edit view with warnings.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            // Create category list in viewbag to be used in view.
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", tip.CategoryId);

            return View(tip);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTip(int? id)
        {
            // We dont have an id -> show 404.
            if (id == null)
            {
                return NotFound();
            }

            // Get tip with specified id.
            var tip = _tipService.GetTipById(id);

            // Tip doesn't exist -> show 404.
            if (tip == null)
            {
                return NotFound();
            }

            // Check if user is authorized to delete tip.
            var isAuthorized = await _authorizationService.AuthorizeAsync(User, tip, TipOperations.Delete);

            // When user is not authorized show warning.
            if (!isAuthorized)
            {
                return new ChallengeResult();
            }

            // Delete tip from database.
            _tipService.DeleteTip(tip);

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Disable(int? id)
        {
            // We dont have an id -> show 404.
            if (id == null)
            {
                return NotFound();
            }

            // Get tip with specified id.
            var tip = _tipService.GetTipById(id);

            // Tip doesn't exist -> show 404.
            if (tip == null)
            {
                return NotFound();
            }

            // Disable tip.
            _tipService.DisableTip(tip);

            return RedirectToAction("Index");
        }
    }
}