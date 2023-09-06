using SwapControl.MT.StructLib;
using SwapControl.Structure;
using SwapControl.Structure.Configs.Interfaces;
using SwapControl.Structure.Configs.Products;
using SwapControl.Structure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SwapControl.MT
{
    public class MTSwapsWrapper : MTHelper, IDisposable
    {
        //private MTHelper mtHelper = new MTHelper();
        private readonly List<Config> mtSettings;
        public MTSwapsWrapper(List<Config> mtSettings)
        {
            this.mtSettings = mtSettings;
        }

        public void CreateConnection()
        {
            try
            {
                string res = "";
                base.InitLIB_MQ();
                foreach (MTSettings config in mtSettings)
                {
                    res = base.GetConnect(config.address);
                    if (res != "OK")
                        Logging.Finish($"Wrong server address => {config.address}. Status =>{res}", LogLevel.Error);
                        
                    
                    res = base.LoginToServer(config.login, config.password);
                    if (res != "OK")
                        Logging.Finish($"Wrong server address => {config.address}. Status =>{res}", LogLevel.Error);

                    Logging.Log($"Successfully logged in to the server = {config.address}, status = {res}", LogLevel.Info);
                    
                    //т.к. у меня не было возможности использовать несколько серверов МТ, тут брейк. 
                    break;
                }
            }
            catch (Exception ex)
            {
                Logging.Finish($"{ex.Message}", LogLevel.Error);
            }
        }

        public void DestroyMT()
        {
            
        }

        public void SymbolsRefresh()
        {
            try
            {
               string res = base.GetSymbolsRefresh();
                if (res != "OK")
                    Logging.Finish($"Faild SymbolsRefresh. Status =>{res}", LogLevel.Error);
            }
            catch (Exception ex)
            {
                Logging.Finish($"{ex.Message}", LogLevel.Error);
            }
        }

        public List<ConSymbol> GetAllSymbols()
        {
            return base.GetListOfConSymbols();
        }

        public List<ConGroup> GetAllGroups()
        {
             return base.GetListOfConGroup();
        }

        public void InitArraySymsols()
        {

        }

        public void Dispose()
        {
            try
            {
                int str = 0;

                str = base.Disconnect_MT();
                str = base.Releas_MT();
                base.FreeLib();

            }
            catch (Exception ex)
            {
                Logging.Finish($"{ex.Message}", LogLevel.Error);
            }
        }
    }
}
