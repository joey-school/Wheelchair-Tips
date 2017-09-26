using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WheelchairTips.Models
{
    public class Tip
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
    }
}
