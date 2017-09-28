using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WheelchairTips.Models
{
    public class Tip
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
