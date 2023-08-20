using Swap_Control.Structure.Configs.Interfaces;
using Swap_Control.Structure.Configs.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swap_Control.Structure.Configs.Factories
{
    internal class SQLSettingsFactory : ConfigFactory
    {
        public Config CreateConfig()
        {
            return new SQLSettings();
        }
    }
}
