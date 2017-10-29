using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WheelchairTips.Models.ViewModels
{
    public class TipCardViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ContentShort { get; set; }
        public string ImageName { get; set; }
        public string CategoryName { get; set; }
        public bool IsDisabled { get; set; }
    }
}
