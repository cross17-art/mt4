using Microsoft.EntityFrameworkCore;
using SwapControl.SQL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapControl.SQL.Interface
{
    public interface IPostgreSQL : IDisposable
    {
        bool isConnected();
        void SaveChanges();
        Task SaveChangesAsync();

        public DbSet<Symbol> Symbol { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<GroupSymbol> GroupSymbol { get; set; }
    }
}
