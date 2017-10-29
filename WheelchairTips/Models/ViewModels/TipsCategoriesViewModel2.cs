using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WheelchairTips.Models.ViewModels
{
    public class TipsCategoriesViewModel2
    {
        public List<Category> Categories { get; set; }
        public List<TipCardViewModel> TipCards { get; set; }
        public string CategoryId { get; set; }
        public List<SelectListItem> CategoriesSelectList { get; set; }
    }
}
