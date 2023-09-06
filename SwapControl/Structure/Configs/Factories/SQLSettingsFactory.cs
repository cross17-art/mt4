using SwapControl.Structure.Configs.Products;
using SwapControl.Structure.Configs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapControl.Structure.Configs.Factories
{
    public class SQLSettingsFactory : ConfigFactory
    {
        public Config CreateConfig()
        {
            return new SQLSettings();
        }
    }
}
