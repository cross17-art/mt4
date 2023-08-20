using Swap_Control.Structure.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
namespace Swap_Control.Structure
{
    internal class Logging
    {
        private static string filePath { get; set; }
        private static string fileDirectory { get; set; }

        static Logging()
        {
            DateTime currentDate = DateTime.Now.Date;
            string formattedDate = currentDate.ToString("dd.MM.yyyy");

            filePath = Directory.GetCurrentDirectory()+"\\log\\"+formattedDate+"_log-message";
            fileDirectory = Directory.GetCurrentDirectory()+"\\log";
        }

        public static void Log(string message,LogLevel logLevel,string errorFunction = "")
        {
            try
            {
                if (!Directory.Exists(fileDirectory))
                {
                    Directory.CreateDirectory(fileDirectory);
                }

                using (StreamWriter writer = File.AppendText(filePath))
                {
                    string logMessage = errorFunction==""?$"{DateTime.Now} - {logLevel} - {message}":
                        $"{DateTime.Now} - {logLevel} - function=> {errorFunction} - {message}";
                   
                    Console.WriteLine(logMessage);
                    writer.WriteLine(logMessage);
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = File.AppendText(filePath))
                {

                    string logMessage = $"{DateTime.Now} - {LogLevel.Error} -"+$"An error occurred while logging: {ex.Message}";
                    
                    Console.WriteLine(logMessage);
                    writer.WriteLine(logMessage);
                }
                
            }
            

        }
        public static void Finish(string message, LogLevel logLevel)
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame stackFrame = stackTrace.GetFrame(1); // 0 - это GetCallingMethodName, 1 - это функция, вызвавшая GetCallingMethodName
            string errorFunction =  stackFrame.GetMethod().Name;

            Logging.Log(message,logLevel, errorFunction);
            Environment.Exit(0);
            //throw new Exception(message);
        }
        public static string GetCallingMethodName()
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame stackFrame = stackTrace.GetFrame(3); // 0 - это GetCallingMethodName, 1 - это функция, вызвавшая GetCallingMethodName
            return stackFrame.GetMethod().Name;
        }
    }
}
