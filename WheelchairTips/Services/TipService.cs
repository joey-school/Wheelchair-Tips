using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WheelchairTips.Models;

namespace WheelchairTips.Services
{
    public class TipService
    {
        private WheelchairTipsContext _context;

        public TipService (WheelchairTipsContext context)
        {
            _context = context;
        }

        // Get tip with specified id.
        public Tip GetTipById (int? id)
        {
            return _context.Tip
                .SingleOrDefault(t => t.Id == id);
        }
    }
}
