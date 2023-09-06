using Microsoft.Extensions.Options;
using SwapControl.Structure;
using SwapControl.Structure.Configs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SwapControl.Structure.Configs.Products
{
    public class SQLSettings : Config
    {
        private string server { get; set; }
        private string port { get; set; }
        private string user { get; set; }
        private string password { get; set; }
        private string dataBase { get; set; }
        private string sqlQuery { get; set; }
        public string connectionString { get; set; }

        public string filePath { get; set; }

        public XmlNode node { get; set; }
        public SQLSettings() { }
        public void InitFields(XmlElement node)
        {
            user = node["User"].InnerText;
            server = node["Address"].InnerText;
            port = node["Port"].InnerText;
            dataBase = node["Database"].InnerText;
            sqlQuery = node["SQLQuery"].InnerText;

            XmlNode passwordNode = node["Password"];
            XmlAttribute securedNode = node["Password"].GetAttributeNode("Secured");

            if (securedNode.Value == "False")
            {
                password = passwordNode.InnerText;

                securedNode.Value = "True";
                string protectPass = DataProtector.ProtectData(passwordNode.InnerText);
                passwordNode.InnerText = protectPass;
                //element["Password"].GetAttribute("Secured").Value = "True";
            }
            else
            {
                string unProtectPass = DataProtector.UnprotectData(passwordNode.InnerText);
                password = unProtectPass;
            }
            //postgres
        }
        public string GetConnectionString() { return $"host={server};port={port};database={dataBase};username={user};password={password}"; }

        public string GetConfigName()
        {
            return "DataBase";
        }
    }
}
