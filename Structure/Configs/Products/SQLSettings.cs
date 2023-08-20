using Swap_Control.Structure.Configs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Swap_Control.Structure.Configs.Products
{
    internal class SQLSettings : Config
    {
        private string server { get; set; }
        private string port { get; set; }
        private string user { get; set; }
        private string password { get; set; }
        private string dataBase { get; set; }
        private string sqlQuery { get; set; }

        public string filePath { get; set; }

        public XmlNode node { get; set; }
        public SQLSettings() { }
        public void InitFields(XmlElement node)
        {
            this.user = node["User"].InnerText;
            this.server = node["Address"].InnerText;
            this.port = node["Port"].InnerText;
            this.dataBase = node["Database"].InnerText;
            this.sqlQuery = node["SQLQuery"].InnerText;

            XmlNode passwordNode = node["Password"];
            XmlAttribute securedNode = node["Password"].GetAttributeNode("Secured");

            if (securedNode.Value == "False")
            {
                securedNode.Value = "True";
                string protectPass = DataProtector.ProtectData(passwordNode.InnerText);
                passwordNode.InnerText = protectPass;
                //element["Password"].GetAttribute("Secured").Value = "True";
                this.password = passwordNode.InnerText;
            }
            else
            {
                string unProtectPass = DataProtector.UnprotectData(passwordNode.InnerText);
                this.password = unProtectPass;
            }
        }

        public string GetConfigName()
        {
            return "DataBase";
        }
    }
}
