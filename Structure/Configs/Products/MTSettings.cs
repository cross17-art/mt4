using Swap_Control.Structure.Configs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using Swap_Control.Structure.Enums;
using Swap_Control.Structure.Configs.Products.Helpers;

namespace Swap_Control.Structure.Configs.Products
{
    internal class MTSettings : Config
    {
       private string address { get; set; }
       private string login { get; set; }
       private string password { get; set; }

       public List<SwapTime> swapTimes = new List<SwapTime>();

      // public XmlNode node {get; set; }
       

        public void InitFields(XmlElement node)
        {
            //init main fields
            this.login = node["Login"].InnerText;
            this.address = node["Address"].InnerText;

            XmlNode passwordNode = node["Password"];
            XmlAttribute securedNode = node["Password"].GetAttributeNode("Secured");

            if (securedNode.Value == "False")
            {
                securedNode.Value = "True";
                string protectPass = DataProtector.ProtectData(passwordNode.InnerText);
                passwordNode.InnerText = protectPass;
                this.password = passwordNode.InnerText;
            }
            else
            {
                string unProtectPass = DataProtector.UnprotectData(passwordNode.InnerText);
                this.password = unProtectPass;
            }



            XmlNode? swapNode = node["syncSwapTime"];
            if (swapNode !=null)
            {
                CreateSwapTimeList(swapNode);
            }

            
        }

        public void CreateSwapTimeList(XmlNode swapNode)
        {
            if (swapNode["time"] != null)
            {
                string? time = swapNode["time"].InnerText;
                SwapTime swapTime = new SwapTime();
                swapTime.InitSwapTime(time);
                swapTimes.Add(swapTime);
           
            }
            else
            {
                string[] correctNamesOfWeek = { "MON", "TUE", "WED", "THU", "FRI", "SAT", "SUN" };
                foreach (XmlElement element in swapNode)
                {
                    string nameNode = element.Name;
                    if (!correctNamesOfWeek.Contains(nameNode))
                        Logging.Finish($"Invalid name => {nameNode} of the week. Should be =>{string.Join(",",correctNamesOfWeek)}", LogLevel.Error);

                    SwapTime swapTime = new SwapTime();
                    swapTime.InitSwapTime(element.ChildNodes, nameNode);
                    swapTimes.Add(swapTime);
                }
            }

        }




        public string GetConfigName()
        {
            return "MetaTrader";
        }
    }
}
