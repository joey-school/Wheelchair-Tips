using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WheelchairTips.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string Name { get; set; }

        public List<Tip> Tips { get; set; }
    }
}
