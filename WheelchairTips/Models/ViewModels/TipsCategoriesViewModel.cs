using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WheelchairTips.Models.ViewModels
{
    public class TipsCategoriesViewModel
    {
        public List<Tip> Tips { get; set; }
        public string Category { get; set; }
        public List<Category> Categories { get; set; }
    }
}
