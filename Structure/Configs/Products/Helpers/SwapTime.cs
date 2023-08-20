using Swap_Control.Structure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace Swap_Control.Structure.Configs.Products.Helpers
{
    internal class SwapTime
    {
        public string dayOfWeek { get; set; }
        public List<string> cronListStr = new List<string>();

        public SwapTime()
        {
          
        }

        public void InitSwapTime(XmlNodeList nodeList,string nameNode)
        {

            foreach (XmlNode node in nodeList) 
            {
                string time = node.InnerText;
                if (time != null && IsCorrectTimeString(time))
                {
                    var (hours, minutes) = SplitTime(time);
                    string cronStr = CreateCronStr(int.Parse(hours), int.Parse(minutes), 0, nameNode);
                    cronListStr.Add(cronStr);
                }
                else
                {
                    Logging.Finish("The time tag does not match, the time field should contain the following format 10:00", LogLevel.Error);

                }
            }
        }

        public void InitSwapTime(string time)
        {
            //проверка time != null не нужна т.к. в самом начале проверяются все поля
            if (time != null && SwapTime.IsCorrectTimeString(time))
            {   
                var (hours, minutes) = SplitTime(time);
                string cronStr = CreateCronStr(int.Parse(hours), int.Parse(minutes), 0, "*");
                cronListStr.Add(cronStr);
            }
            else
            {
                Logging.Finish("The time tag does not match, the time field should contain the following format 10:00", LogLevel.Error);
            }

        }

        static public (string,string) SplitTime(string time)
        {
            return (time.Split(":")[0], time.Split(":")[1]);
        }

        static public bool IsCorrectTimeString(string input)
        {
            return Regex.IsMatch(input, @"^\d{2}\:\d{2}$");
        }

        static public string CreateCronStr(int hours,int minutes,int seconds,string dayOfWeek)
        {
            dayOfWeek = dayOfWeek.ToUpper();
            return $"{seconds} {minutes} {hours} ? * {dayOfWeek}";
        }

      

    }
}
