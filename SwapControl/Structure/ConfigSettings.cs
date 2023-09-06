using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Xml;
using SwapControl.Structure.Configs.Products;
using SwapControl.Structure.Configs.Factories;
using System.Xml.Linq;
using SwapControl.Structure.Enums;
using SwapControl.Structure.Configs.Interfaces;
using System.Reflection.Metadata;

namespace SwapControl.Structure
{
    internal class ConfigSettings
    {
        private XmlDocument document = new XmlDocument();
        public readonly string filePath;
        public List<Config> mtSettings = new List<Config>();
        public List<Config> sqlSettings = new List<Config>();

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


        public List<Config> initConfigSettings(string nodePath, ConfigFactory factory, List<Config> settings)
        {
            try
            {

                XmlNode mtNodes = document.SelectSingleNode(nodePath);
                foreach (XmlElement node in mtNodes)
                {
                    if (node.GetType() == typeof(XmlElement))
                    {
                        Config configs = factory.CreateConfig();

                        configs.InitFields(node);
                        settings.Add(configs); // вообще в этом месте можно сохраняться все в переменные класса и работать через него. Но пока  мне это не критично
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
