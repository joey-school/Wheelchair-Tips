using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WheelchairTips.Models;

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

        public string HandleImageUpload(IFormFileCollection files)
        {
            //var files = HttpContext.Request.Form.Files;

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
