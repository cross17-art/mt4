using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using SwapControl.Structure;
using SwapControl.Structure.Enums;
using SwapControl.Structure.Configs.Interfaces;
using SwapControl.Structure.Configs.Products.Helpers;

namespace SwapControl.Structure.Configs.Products
{
    public class MTSettings : Config
    {
        public string address { get; set; }
        public int login { get; set; }
        public string password { get; set; }

        public List<SwapTime> swapTimes = new List<SwapTime>();

        // public XmlNode node {get; set; }


        public void InitFields(XmlElement node)
        {
            //init main fields
            login = int.Parse(node["Login"].InnerText);
            address = node["Address"].InnerText;

            XmlNode passwordNode = node["Password"];
            XmlAttribute securedNode = node["Password"].GetAttributeNode("Secured");

            if (securedNode.Value == "False")
            {
                securedNode.Value = "True";
                string protectPass = DataProtector.ProtectData(passwordNode.InnerText);
                password = passwordNode.InnerText;
                passwordNode.InnerText = protectPass;
            }
            else
            {
                string unProtectPass = DataProtector.UnprotectData(passwordNode.InnerText);
                password = unProtectPass;
            }



            XmlNode? swapNode = node["syncSwapTime"];
            if (swapNode != null)
            {
                CreateSwapTimeList(swapNode);
            }


        }

        public void CreateSwapTimeList(XmlNode swapNode)
        {
            if (swapNode.SelectNodes("time").Count != 0)
            {
                foreach (XmlElement element in swapNode.SelectNodes("time"))
                {
                    string? time = element.InnerText;
                    SwapTime swapTime = new SwapTime();
                    swapTime.InitSwapTime(time);
                    swapTimes.Add(swapTime);
                }
            }
            else
            {
                string[] correctNamesOfWeek = { "MON", "TUE", "WED", "THU", "FRI", "SAT", "SUN" };
                foreach (XmlElement element in swapNode)
                {
                    string nameNode = element.Name;
                    if (!correctNamesOfWeek.Contains(nameNode))
                        Logging.Finish($"Invalid name => {nameNode} of the week. Should be =>{string.Join(",", correctNamesOfWeek)}", LogLevel.Error);

                    SwapTime swapTime = new SwapTime();
                    swapTime.InitSwapTime(element.ChildNodes, nameNode);
                    swapTimes.Add(swapTime);
                }
            }

        }
        
        public List<string> GetArrayOfSwapTimes()
        {
            List<string> swapTimesTotal = new List<string>();
            foreach(SwapTime day in swapTimes)
            {
                swapTimesTotal.AddRange(day.cronListStr);
            }
            return swapTimesTotal;
        }

        public string GetConfigName()
        {
            return "MetaTrader";
        }

        public string GetConnectionString()
        {
            return $"{address}";
        }
    }
}
