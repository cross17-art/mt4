using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SwapControl.SQL.Entities
{
    public class Symbol
    {
        [Key] 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string SymbolName { get; set; }
        
        [Required]
        public double sw_long { get; set; }
        
        [Required]
        public double sw_short { get; set; }

        public ICollection<GroupSymbol> groupSymbol { get; set; }
    }


}

