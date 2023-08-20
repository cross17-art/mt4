using Microsoft.EntityFrameworkCore;
using Swap_Control.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Swap_Control
{
    class Context: DbContext
    {
        public Context() {
            /*Database.EnsureDeleted();
            Database.EnsureCreated();*/
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(@"host=localhost;port=5432;database=Doto;username=postgres;password=1234");
        }

        public DbSet<Swaps> Swaps {  get; set; }


        public List<Swaps> GetEntityWithSameSymbol(string name)
        {
            var entityWithSameSymbol = Swaps.Where(swap => swap.Symbol == name).ToList();
            return entityWithSameSymbol;
        }

    }
}
