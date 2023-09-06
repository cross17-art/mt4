using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapControl.SQL.Entities
{
    public class GroupSymbol
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int groupId { get; set; }
        public int symbolId { get; set; }
        [Required]
        public double sw_long { get; set; }
        [Required]
        public double sw_short { get; set; }

        public Group group { get; set; }
        public Symbol symbol { get; set; }
    }
}
