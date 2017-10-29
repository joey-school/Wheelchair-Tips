using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WheelchairTips.Models;
using WheelchairTips.Models.ViewModels;

namespace WheelchairTips.Services
{
    public class TipService
    {
        private readonly WheelchairTipsContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public TipService (WheelchairTipsContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // Get tip with specified id.
        public Tip GetTipById (int? id)
        {
            return _context.Tip
                .SingleOrDefault(t => t.Id == id);
        }

        // Get all tips and eager load all related users.
        public IEnumerable<Tip> GetAllTipsIncludeUsers()
        {
            return _context.Tip
                .Include(t => t.ApplicationUser)
                .ToList();
        }

        // Get tips from member.
        public IEnumerable<Tip> GetAllTipsFromMember(string userId)
        {
            return _context.Tip
                .Where(t => t.ApplicationUserId == userId)
                .ToList();
        }

        // Get all tips formatted in a tip card.
        public List<TipCardViewModel> GetAllTipCards()
        {
            // Get all tips including categories.
            List<Tip> tips = _context.Tip
                .Include(t => t.Category)
                .ToList();

            // Return our rendered tip cards.
            return RenderTipCards(tips);
        }

        // Get all tips by category formatted in a tip card.
        public List<TipCardViewModel> GetAllTipCardsByCategory(int categoryId)
        {
            // Get all tips by category id.
            Category category = _context.Category
                .Where(c => c.Id == categoryId)
                .Include(c => c.Tips)
                .Single();

            // Return our rendered tip cards.
            return RenderTipCards(category.Tips);
        }

        // Get all tips by category and search query formatted in a tip card.
        public List<TipCardViewModel> GetAllTipCardsByCategoryAndSearchQuery(int categoryId, string searchQuery)
        {
            // Get all tips by category and search query.
            List<Tip> tips = _context.Tip
                .Include(t => t.Category)
                .Where(t => t.CategoryId == categoryId)
                .Where(t => t.Content.Contains(searchQuery))
                .ToList();

            // Return our rendered tip cards.
            return RenderTipCards(tips);
        }

        // Get all tips by search query formatted in a tip card.
        public List<TipCardViewModel> GetAllTipCardsBySearchQuery(string searchQuery)
        {
            // Get all tips by search query.
            List<Tip> tips = _context.Tip
                .Include(t => t.Category)
                .Where(t => t.Content.Contains(searchQuery))
                .ToList();

            // Return our rendered tip cards.
            return RenderTipCards(tips);
        }

        // Saves a new tip to database
        public void CreateTip (Tip tip, string imageName, string userId)
        {
            // Add image name.
            tip.ImageName = imageName;

            // Add user id.
            tip.ApplicationUserId = userId;

            _context.Add(tip);
            _context.SaveChanges();
        }

        // Delete tip from database.
        public void DeleteTip (Tip tip)
        {
            _context.Tip.Remove(tip);
            _context.SaveChanges();
        }

        // Handles uploading of an image, this will return the image name.
        public string HandleImageUpload(IFormFileCollection files)
        {
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

        // Create custom tip cards based on tips data. 
        public List<TipCardViewModel> RenderTipCards (List<Tip> tips)
        {
            List<TipCardViewModel> tipCards = new List<TipCardViewModel>();

            foreach (var tip in tips)
            {
                // Fill each tip card with a tip
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

            return tipCards;
        }

        // Creates a select list with all our categories
        public List<SelectListItem> RenderCategorySelectList ()
        {
            List <SelectListItem> categoriesSelectList = new List<SelectListItem>();

            // Add a default option.
            categoriesSelectList.Add(new SelectListItem { Value = "", Text = "All" });

            // Add each category to our select list.
            foreach (var category in _context.Category)
            {
                categoriesSelectList.Add(new SelectListItem { Value = category.Id.ToString(), Text = category.Name });
            }

            return categoriesSelectList;
        }
    }
}
