using SwapControl.SQL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapControl.SQL.Interface
{
    public interface ISQLWrapperPostgreSQL
    {

        public List<Symbol> GetSymbols();
        public List<Group> GetGroups();
        public List<GroupSymbol> GetGroupSymbol();

        public GroupSymbol GetGroupSymbolById(int? symbolId, int? groupId);
        public int GetGroupIdByName(string groupName);
        public Symbol GetSymbolByName(string symbolName);

        public GroupSymbol GetGroupSymbolByName(string symbolName, string groupName);
        public Task UpdateGroup_SymbolAsync(GroupSymbol groupSymbol,double swap_long,double swap_short);
        public Task UpdateSymbolAsync(Symbol searchSymbol,double swap_long, double swap_short);
        public Task DeleteSymbolAsync(Symbol symbol);
        public Task DeleteGroupAsync(Group group);
        public Task AddSymbolAsync(Symbol symbol);
        public Task AddGroupAsync(Group group);
        public Task AddGroup_SymbolAsync(string groupName, string symbolName, double sw_long, double sw_short);
        public Task SaveChanges();
        public Task DeleteSymbolAsync(int symbolId);
    }
}
