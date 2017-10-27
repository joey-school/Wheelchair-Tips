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

namespace WheelchairTips.Controllers
{
    // Only users with role 'Admin' and 'Member' have access to this controller. Otherwise show warning.
    [Authorize(Roles = "Admin, Member")]
    public class MemberTipsController : Controller
    {
        private readonly WheelchairTipsContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthorizationService _authorizationService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public MemberTipsController(WheelchairTipsContext context, UserManager<ApplicationUser> userManager, IAuthorizationService authorizationService, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _authorizationService = authorizationService;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Tip> tips = null;

            // When admin is online -> show all tips.
            if (User.IsInRole("Admin"))
            {
                tips = _context.Tip
                .Include(t => t.ApplicationUser)
                .ToList();
            }

            // When member is online -> show only his tips.
            else if (User.IsInRole("Member"))
            {
                tips = _context.Tip
                .Include(t => t.ApplicationUser)
                .Where(t => t.ApplicationUserId == _userManager.GetUserId(HttpContext.User))
                .ToList();
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
                // Handle image upload if one is uploaded.
                tip.ImageName = HandleImageUpload();

                // Save tip with active user id.
                tip.ApplicationUserId = _userManager.GetUserId(HttpContext.User);
                _context.Add(tip);
                _context.SaveChanges();

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
            var tip = _context.Tip
                .SingleOrDefault(t => t.Id == id);

            // Tip doesn't exist -> show 404
            if (tip == null)
            {
                return NotFound();
            }

            // Check if user is authorized to edit tip.
            var isAuthorized = await _authorizationService.AuthorizeAsync( User, tip, TipOperations.Update);

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
            var tip = _context.Tip
                .Single(s => s.Id == id);

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
                    // Handle image upload if one is uploaded.
                    var imageName = HandleImageUpload();
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
        public async Task <IActionResult> DeleteTip(int id)
        {
            var tip = _context.Tip
                .Single(m => m.Id == id);

            // Check if user is authorized to delete tip.
            var isAuthorized = await _authorizationService.AuthorizeAsync(User, tip, TipOperations.Delete);

            // When user is not authorized show warning.
            if (!isAuthorized)
            {
                return new ChallengeResult();
            }

            _context.Tip.Remove(tip);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        
        // Logic to handle uploaded images.
        private string HandleImageUpload ()
        {
            var files = HttpContext.Request.Form.Files;

            foreach (var Image in files)
            {
                if (Image != null && Image.Length > 0)
                {
                    var file = Image;

                    // This is the path we will save our images.
                    var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads\\img");

                    if (file.Length > 0)
                    {
                        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                        // Save our image to path and return name of image.
                        using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                            return file.FileName;
                        }
                    }
                }
            }

            return null;
        }
    }
}