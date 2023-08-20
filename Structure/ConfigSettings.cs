using Swap_Control.Structure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Xml;
using Swap_Control.Structure.Configs.Products;
using Swap_Control.Structure.Configs.Interfaces;
using Swap_Control.Structure.Configs.Factories;
using System.Xml.Linq;

namespace Swap_Control.Structure
{
    internal class ConfigSettings
    {
        public readonly string filePath;
        public List<Config> mtSettings = new List<Config>();
        public List<Config> sqlSettings = new List<Config>();

        private XmlDocument document = new XmlDocument();

        public ConfigSettings(string filePath)
        {
            this.filePath = filePath;
            try
            {
                document.Load(filePath);
                XmlNode parentNode = document.SelectSingleNode("/Config/Common");
                bool result = CheckAllTagsFilled(parentNode);
                
                if (!result)
                {
                    Logging.Finish("Empty fields at config file", LogLevel.Error);
                }

            }
            catch (Exception ex)
            {
                Logging.Finish(ex.Message, LogLevel.Error);
            }
        }

        public List<Config> initConfigSettings(string nodePath,ConfigFactory factory, List<Config> settings)
        {
            try
            {
               
                XmlNode mtNodes = this.document.SelectSingleNode(nodePath);
                foreach (XmlElement node in mtNodes)
                {
                    if(node.GetType() == typeof(XmlElement))
                    {
                        Config mtConfigs = factory.CreateConfig();

                        mtConfigs.InitFields(node);
                        settings.Add(mtConfigs);
                        document.Save(filePath);
                    }
                }
                Logging.Log($"init {nodePath} configs", LogLevel.Info);
                return settings;
            }
            catch (Exception ex)
            {
                Logging.Finish(ex.Message, LogLevel.Error);
            }
            return settings;

        }

        public static bool CheckAllTagsFilled(XmlNode node)
        {
            try
            {
                if (node.NodeType == XmlNodeType.Element)
                {
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        if (childNode.NodeType == XmlNodeType.Element && string.IsNullOrWhiteSpace(childNode.InnerText))
                        {
                            return false;
                        }

                        if (!CheckAllTagsFilled(childNode))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Logging.Finish(ex.Message, LogLevel.Error);
                return false;
            }

        }
    }
}
