// See https://aka.ms/new-console-template for more information

using Swap_Control.Entities;
using Swap_Control.Structure;
using Swap_Control.Structure.Configs.Factories;
using Swap_Control.Structure.Configs.Interfaces;
using Swap_Control.Structure.Enums;
using System.Text;
using System.Xml;

namespace Swap_Control
{

    class Program
    {
        static void Main(string[] args)
        {
            Logging.Log("Start Program", LogLevel.Info);

            //      --------- START ---------
            //--------- Initializing the config ---------
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "config.xml");
            ConfigSettings configSettings = new ConfigSettings(filePath);

            ConfigFactory MTfactory = new MTSettingsFactory();
            List<Config> mtSettings = new List<Config>();
            configSettings.initConfigSettings("/Config/Common/MT4", MTfactory, mtSettings);

            ConfigFactory SQLfactory = new SQLSettingsFactory();
            List<Config> sqlSettings = new List<Config>();
            configSettings.initConfigSettings("/Config/Common/SQL", SQLfactory, sqlSettings);
            //      --------- END ---------

            /*                using (var context = new Context())
                            {
                                context.GetEntityWithSameSymbol("AUDCAD");
                                var swap = new Swaps()
                                {
                                    Symbol = "AUDCAD",
                                    Group = "Demo",
                                };
                                var swap2 = new Swaps()
                                {
                                    Symbol = "ETH",
                                    Group = "Demo",
                                };
                                var swap3 = new Swaps()
                                {
                                    Symbol = "BTC",
                                    Group = "Demo",
                                };
                                context.Swaps.Add(swap);
                                context.Swaps.Add(swap2);
                                context.Swaps.Add(swap3);
                                context.SaveChanges();
                            }*/
        }
    }
}

