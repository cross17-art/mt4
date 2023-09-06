using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using SwapControl.SQL.Entities;
using SwapControl.SQL.Interface;
using SwapControl.Structure;
using SwapControl.Structure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SwapControl.SQL
{
    public class SQLWrapper : ISQLWrapperPostgreSQL
    {
        private readonly IPostgreSQL _appDbContext;
        public int added { get; set; }
        public int updated { get; set; }
        public int deleted { get; set; }

        public SQLWrapper(IPostgreSQL dbContext)
        {
            _appDbContext = dbContext;
            
            bool isConnected = _appDbContext.isConnected();
            if (!isConnected)
                Logging.Finish("Failed to connect to the database", LogLevel.Error);

            Logging.Log("Successfully connected to the database", LogLevel.Info);
        }

        public List<Symbol> GetSymbols()
        {
            return _appDbContext.Symbol.ToList();
        }

        public List<Group> GetGroups()
        {
            return _appDbContext.Group.ToList();
        }

        public List<GroupSymbol> GetGroupSymbol()
        {
            return _appDbContext.GroupSymbol.ToList();
        }
        public GroupSymbol GetGroupSymbolById(int? symbolId,int? groupId)
        {
            return _appDbContext.GroupSymbol.SingleOrDefault(el => el.symbolId == symbolId && el.groupId == groupId);
        }

        public int GetGroupIdByName(string groupName)
        {
            return _appDbContext.Group.SingleOrDefault(group => group.GroupName == groupName).Id;
        }
        public Symbol GetSymbolByName(string symbolName)
        {
            return _appDbContext.Symbol.SingleOrDefault(symbol => symbol.SymbolName == symbolName);
        }
        public GroupSymbol GetGroupSymbolByName(string symbolName,string groupName)
        {
            return _appDbContext.GroupSymbol.FirstOrDefault( element => element.symbol.SymbolName == symbolName && element.group.GroupName == groupName);
        }
        public async Task UpdateSymbolAsync(Symbol searchSymbol,double swap_long, double swap_short)
        {
            try
            {
                searchSymbol.sw_long = swap_long;
                searchSymbol.sw_short = swap_short;
                Logging.Log($"SymbolName {searchSymbol.SymbolName} data in the Symbol table has been updated  | new=> {searchSymbol.sw_long + "," + searchSymbol.sw_short} |", LogLevel.Trace);
            }
            catch(Exception ex)
            {
                Logging.Finish(ex.Message, LogLevel.Error);
            }

        }

        public async Task DeleteSymbolAsync(Symbol symbol)
        {
            try
            {
                _appDbContext.Symbol.Remove(symbol);
                this.deleted++;
                Logging.Log($"Remove symbol {symbol.SymbolName} from the database", LogLevel.Trace);
            }
            catch(Exception ex)
            {
                Logging.Finish(ex.Message, LogLevel.Error);
            }
        }

        public async Task DeleteGroupAsync(Group group)
        {
            try
            {
                _appDbContext.Group.Remove(group);
                this.deleted++;
                Logging.Log($"Remove group {group.GroupName} from the database", LogLevel.Trace);
            }
            catch (Exception ex)
            {
                Logging.Finish(ex.Message, LogLevel.Error);
            }
        }

        public async Task AddSymbolAsync(Symbol symbol)
        {
            try
            {
                _appDbContext.Symbol.Add(symbol);
                Logging.Log($"Successfully add symbol {symbol.SymbolName} to the database", LogLevel.Trace);
            }
            catch (Exception ex)
            {
                Logging.Finish(ex.Message, LogLevel.Error);
            }
        }

        public async Task AddGroupAsync(Group group)
        {
            try
            {
                _appDbContext.Group.Add(group);
                Logging.Log($"Successfully add group {group.GroupName}", LogLevel.Trace);
            }
            catch (Exception ex)
            {
                Logging.Finish(ex.Message, LogLevel.Error);
            }
        }

        public async Task AddGroup_SymbolAsync(string groupName, string symbolName,double sw_long,double sw_short)
        {
            try
            {
                var groupFound = _appDbContext.Group
                  .SingleOrDefault(group => group.GroupName == groupName);
                var symbolFound = _appDbContext.Symbol
                 .SingleOrDefault(symbol => symbol.SymbolName == symbolName);

                if(symbolFound!=null && groupFound != null)
                {
                    GroupSymbol groupSymbol = new GroupSymbol
                    {
                        groupId = groupFound.Id,
                        symbolId = symbolFound.Id,
                        sw_long = sw_long,
                        sw_short = sw_short
                    };

                    _appDbContext.GroupSymbol.Add(groupSymbol);
                    Logging.Log($"Successfully added to the GroupSymbol table - group {groupFound.GroupName} and symbol {symbolName}", LogLevel.Trace);
                }
                else
                {
                    string notFindSymbol = symbolFound == null ? symbolName : "";
                    string notFindGgroup = groupFound == null ? groupName : "";
                    if(notFindSymbol!="")
                        Logging.Log($"The symbol cannot be found in the database {notFindSymbol}.The group containing the symbol is {groupName}", LogLevel.Trace);
                    if(notFindGgroup!="")
                        Logging.Log($"The group cannot be found in the database {notFindGgroup}", LogLevel.Trace);

                }
                this.SaveChanges();

            }
            catch (Exception ex)
            {
                Logging.Finish(ex.Message, LogLevel.Error);
            }
        }
        public async Task UpdateGroup_SymbolAsync(GroupSymbol groupSymbol,double swap_long,double swap_short)
        {
            try
            {
                groupSymbol.sw_long = swap_long;
                groupSymbol.sw_short = swap_short;
                Logging.Log($"Updated the fields in the table Group_Symbol for group {groupSymbol.group.GroupName} and symbol {groupSymbol.symbol.SymbolName}", LogLevel.Trace);

                //_appDbContext.GroupSymbol.Add(groupSymbol);
            }
            catch (Exception ex)
            {
                Logging.Finish(ex.Message, LogLevel.Error);
            }
        }
        public async Task SaveChanges()
        {
            try
            {
                _appDbContext.SaveChanges();
                
            }
            catch (Exception ex) {
                Logging.Finish(ex.Message, LogLevel.Error);
            }
        }

        public async Task DeleteSymbolAsync(int symbolId)
        {
            var symbolToDelete = await _appDbContext.Symbol.FindAsync(symbolId);

            if (symbolToDelete != null)
            {
                _appDbContext.Symbol.Remove(symbolToDelete);
                await _appDbContext.SaveChangesAsync();
            }
        }

        public static bool AreSymbolsEqual(Symbol symbol1, Symbol symbol2)
        {
            return symbol1.sw_long == symbol2.sw_long &&
                   symbol1.sw_short == symbol2.sw_short;
        }


    }
}
