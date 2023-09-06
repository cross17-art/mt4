using Microsoft.EntityFrameworkCore;
using SwapControl.MT;
using SwapControl.MT.StructLib;
using SwapControl.SQL;
using SwapControl.SQL.Entities;
using SwapControl.Structure.Configs.Products;
using SwapControl.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwapControl.Structure.Configs.Interfaces;
using SwapControl.Structure.Enums;
using System.Xml.Linq;
using Quartz;
using SwapControl.SQL.Interface;

namespace SwapControl.JobsScheduler
{
    internal class SyncData : IJob
    {

        public SyncData()
        {
        }

        public Task Execute(IJobExecutionContext context)
        {
            Logging.Log("--------- Start SyncData ---------", LogLevel.Info);
            JobDataMap jobDataMap = context.MergedJobDataMap;

            string connectionSQL = (string)jobDataMap["connectionSQL"];
            List<Config> mtSettings = (List<Config>)jobDataMap["mtSettings"];

            InitSyncData(connectionSQL, mtSettings);
            Logging.Log("--------- End SyncData ---------", LogLevel.Info);

            return Task.CompletedTask;
        }

        public async Task InitSyncData(string connectionSQL, List<Config> mtSettings)
        {
            try
            {
                using (IPostgreSQL appDbContext = new AppDbContext(connectionSQL))
                {
                    int identicalSymbols = 0;

                    var sqlWrapper = new SQLWrapper(appDbContext);
                    var symbolsSQL = sqlWrapper.GetSymbols();
                    var groupsSQL = sqlWrapper.GetGroups();

                    var (conSymbols, conGroups) = GetDataFromMT(mtSettings);

                    var resSymbol = await SynchronizationSymbolTabel(new List<ConSymbol>(conSymbols), symbolsSQL, sqlWrapper);
                    var resGroup = await SynchronizationGroupTable(new List<ConGroup>(conGroups), groupsSQL, sqlWrapper);

                    //

                    await SynchronizationGroup_SymbolTable(conGroups, sqlWrapper);


                    await sqlWrapper.SaveChanges();


                }
            }
            catch (Exception ex)
            {
                Logging.Finish(ex.Message, LogLevel.Error);
            }
        }

        public async Task<bool> SynchronizationSymbolTabel(List<ConSymbol> conSymbols, List<Symbol> symbolsSQL, SQLWrapper sqlWrapper) {
            bool result = true;
            try
            {
                foreach (var symbol in symbolsSQL)
                {
                    ConSymbol searchSymbol = conSymbols.Find(el => el.symbol == symbol.SymbolName);

                    if (searchSymbol.symbol == null)
                    {
                        await sqlWrapper.DeleteSymbolAsync(symbol);
                    }
                    else
                    {
                        bool symbolSame = SQLWrapper.AreSymbolsEqual(new Symbol
                        {
                            sw_long = searchSymbol.swap_long,
                            sw_short = searchSymbol.swap_short,
                            SymbolName = searchSymbol.symbol
                        }
                        , symbol);

                        if (symbolSame == true)
                            conSymbols.Remove(searchSymbol);
                    }
                }

                foreach (ConSymbol element in conSymbols)
                {

                    var symbolsFound = sqlWrapper.GetSymbolByName(element.symbol);

                    if (symbolsFound != null)
                    {
                        if (!SQLWrapper.AreSymbolsEqual(new Symbol
                                                        {
                                                            sw_long = element.swap_long,
                                                            sw_short = element.swap_short,
                                                            SymbolName = element.symbol
                                                        }, 
                                                        symbolsFound))   
                        {

                            await sqlWrapper.UpdateSymbolAsync(symbolsFound, element.swap_long, element.swap_short);
                        }
                    }
                    else
                    {
                        Symbol symbol = new Symbol
                        {
                            sw_long = element.swap_long,
                            sw_short = element.swap_short,
                            SymbolName = element.symbol
                        };
                        await sqlWrapper.AddSymbolAsync(symbol);
                    }

                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return result;
        }

        public async Task<bool> SynchronizationGroupTable(List<ConGroup> conGroups, List<Group> groupsSQL, SQLWrapper sqlWrapper) {
            bool result = true;

            try
            {
                foreach (var group in groupsSQL)
                {
                    ConGroup searchGroups = conGroups.Find(el => el.group == group.GroupName);

                    if (searchGroups.group == null)
                    {
                        await sqlWrapper.DeleteGroupAsync(group);
                    }
                    else
                    {
                        conGroups.Remove(searchGroups);
                    }
                }

                foreach (ConGroup element in conGroups)
                {
                    Group group = new Group
                    {
                        GroupName = element.group
                    };
                    await sqlWrapper.AddGroupAsync(group);
                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        public async Task<bool> SynchronizationGroup_SymbolTable(List<ConGroup> conGroups, SQLWrapper sqlWrapper)
        {
            try
            {
                foreach (var group in conGroups)
                {
                    var conGroupMarginList = group.secmargins.Where(el=> el.symbol!="").ToList();
                    foreach (var conGroupSymbols in conGroupMarginList)
                    {
                        GroupSymbol groupSymbolSearch = sqlWrapper.GetGroupSymbolByName(conGroupSymbols.symbol, group.group);
                        double swap_long = conGroupSymbols.swap_long == double.MaxValue ? 0 : conGroupSymbols.swap_long;
                        double swap_short = conGroupSymbols.swap_short == double.MaxValue ? 0 : conGroupSymbols.swap_short;

                        if (groupSymbolSearch != null)
                        {
                            
                            if(groupSymbolSearch.sw_long != swap_long || groupSymbolSearch.sw_short!= swap_short)
                            {
                                await sqlWrapper.UpdateGroup_SymbolAsync(groupSymbolSearch,swap_long,swap_short);
                            }
                        }
                        else
                        {
                            await sqlWrapper.AddGroup_SymbolAsync(group.group,
                                conGroupSymbols.symbol,
                                swap_long,
                                swap_short);
                        }
                       

                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }


        //--------- Initializing the MT LIB and Connection ---------
        public (List<ConSymbol>,List<ConGroup>) GetDataFromMT(List<Config> mtSettings)
        {

            List <ConSymbol> symbolsMT = new List<ConSymbol>();
            List <ConGroup> groupsMT = new List<ConGroup>();
            try
            {
                using (MTSwapsWrapper mtSwapsWrapper = new MTSwapsWrapper(mtSettings))
                {
                    mtSwapsWrapper.CreateConnection();
                    mtSwapsWrapper.SymbolsRefresh();
                    symbolsMT = mtSwapsWrapper.GetAllSymbols();
                    groupsMT = mtSwapsWrapper.GetAllGroups();
                }

            }
            catch (Exception ex)
            {
                Logging.Finish(ex.Message, LogLevel.Error);
                
            }
            return (symbolsMT,groupsMT);
        }
        //      --------- END ---------



    }
}
