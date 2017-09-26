using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WheelchairTips.Models;

namespace WheelchairTips.Models
{
    public class WheelchairTipsContext : DbContext
    {
        public WheelchairTipsContext (DbContextOptions<WheelchairTipsContext> options)
            : base(options)
        {
        }

        public DbSet<WheelchairTips.Models.Tip> Tip { get; set; }
    }
}
