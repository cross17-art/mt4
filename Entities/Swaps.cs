using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swap_Control.Entities
{
    internal class Swaps
    {
        [Key]
        public int SwapID { get; set; }
        [Required]
        public string Symbol { get; set; }
        [Required]
        public string Group { get; set; }
/*        public double Long_pos { get; set; } 
        public double Short_pos { get; set; }
        public DateTime Update { get; set; }*/

    }
}
