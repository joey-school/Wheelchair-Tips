using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WheelchairTips.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace WheelchairTips.Models
{
    public class WheelchairTipsContext : IdentityDbContext<ApplicationUser>
    {
        public WheelchairTipsContext (DbContextOptions<WheelchairTipsContext> options)
            : base(options)
        {
        }

        public DbSet<WheelchairTips.Models.Tip> Tip { get; set; }

        //categories
        public DbSet<WheelchairTips.Models.Category> Category { get; set; }


/*        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }*/

    }
}
