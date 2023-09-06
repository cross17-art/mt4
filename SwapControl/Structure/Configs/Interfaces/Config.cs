using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SwapControl.Structure.Configs.Interfaces
{
    public interface Config
    {
        //void InitNodes(string filePath);
        void InitFields(XmlElement node);
        string GetConfigName();
        string GetConnectionString();
        //bool CheckAllTagsFilled(XmlNode node);

    }
}
