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
    public class MemberTipsController : Controller
    {
        private readonly WheelchairTipsContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IAuthorizationService _authorizationService;

        public MemberTipsController(WheelchairTipsContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment, IAuthorizationService authorizationService)
        {
            _context = context;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
            _authorizationService = authorizationService;
        }

        public IActionResult Index()
        {
            IEnumerable<Tip> model = _context.Tip
                .Include(t => t.ApplicationUser)
                .Where(t => t.ApplicationUserId == _userManager.GetUserId(HttpContext.User))
                .ToList();

            return View(model);
        }

        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Title, Content, CategoryId")] Tip tip)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;

                foreach (var Image in files)
                {
                    if (Image != null && Image.Length > 0)
                    {
                        var file = Image;
                        var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads\\img");

                        if (file.Length > 0)
                        {
                            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                            using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                            {
                                file.CopyTo(fileStream);
                                tip.ImageName = file.FileName;
                            }
                        }
                    }
                }

                tip.ApplicationUserId = _userManager.GetUserId(HttpContext.User);
                _context.Add(tip);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Id", tip.CategoryId);
            return View(tip);
        }

        public async Task<IActionResult> Edit(int? id)
        {



            if (id == null)
            {
                return NotFound();
            }

            var tip = _context.Tip
                .Single(t => t.Id == id);

            if (tip == null)
            {
                return NotFound();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync( User, tip, TipOperations.Update);

            if (!isAuthorized)
            {
                return new ChallengeResult();
            }

            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", tip.CategoryId);

            return View(tip);






            //Tip model;

            //if (id == null)
            //{
            //    return NotFound();
            //}

            //model = _context.Tip
            //    .Single(m => m.Id == id);

            //if (model == null)
            //{
            //    return NotFound();
            //}

            //ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", model.CategoryId);

            //return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tip = _context.Tip
                .Single(s => s.Id == id);

            if (await TryUpdateModelAsync<Tip>(tip, "", t => t.Title, t => t.Content, t => t.CategoryId))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }

            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Id", tip.CategoryId);
            return View(tip);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteTip(int id)
        {
            var tip = _context.Tip
                .Single(m => m.Id == id);

            _context.Tip.Remove(tip);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        private bool TipExists(int id)
        {
            return _context.Tip.Any(e => e.Id == id);
        }
    }
}