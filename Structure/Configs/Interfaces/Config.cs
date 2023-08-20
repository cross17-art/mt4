using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Swap_Control.Structure.Configs.Interfaces
{
    internal interface Config
    {
        //void InitNodes(string filePath);
        void InitFields(XmlElement node);
        string GetConfigName();
        //bool CheckAllTagsFilled(XmlNode node);

    }
}
