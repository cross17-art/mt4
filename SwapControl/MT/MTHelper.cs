using SwapControl.Structure.Enums;
using SwapControl.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SwapControl.MT.StructLib;

namespace SwapControl.MT
{
    public unsafe class MTHelper
    {

        #if !DLL32
            const string fileName = "MT4_management.dll";
        #else
            const string fileName = "mtmanapi.dll";
        #endif

        [DllImport(fileName, EntryPoint = "Start")]
        private static extern bool start();

        [DllImport(fileName, EntryPoint = "Stop")]
        private static extern void stop();

        [DllImport(fileName, EntryPoint = "Login")]
        private static extern int Login(int login, [MarshalAs(UnmanagedType.LPStr)] string password);

        [DllImport(fileName, EntryPoint = "ErrorDescription")]
        private static extern byte* ErrorDescription(int version);//[MarshalAs(UnmanagedType.Interface)]

        [DllImport(fileName, EntryPoint = "Connect")]
        private static extern int Connect([MarshalAs(UnmanagedType.LPStr), In()] string path);

        [DllImport(fileName, EntryPoint = "Disconect")]
        private static extern int Disconect();

        [DllImport(fileName, EntryPoint = "Release")]
        private static extern int Release();

        [DllImport(fileName, EntryPoint = "GroupsRequest")]
        private static extern void* GroupsRequest(int* total);

        [DllImport(fileName, EntryPoint = "SymbolsGetAll")]
        private static extern void* SymbolsGetAll(int* total);

        [DllImport(fileName, EntryPoint = "SymbolsRefresh")]
        private static extern int SymbolsRefresh();
        [DllImport(fileName, EntryPoint = "CfgUpdateSymbol")]
        private static extern int CfgUpdateSymbol(IntPtr conSymbol);

    
        protected MTHelper()
        {
/*            if (!start())
            {
                Logging.Finish("unable to load library into memory", LogLevel.Error);
            }
            Logging.Log("Start Program", LogLevel.Info);*/
        }

        protected bool InitLIB_MQ()
        {
            if (!start())
            {
                Logging.Finish("unable to load library into memory", LogLevel.Error);
            }
            return true;
        }

        protected void FreeLib()
        {
            stop();
        }
        protected int Disconnect_MT()
        {
            int res = Disconect();
            return res;
        }

        protected int Releas_MT()
        {
            int res = Release();
            return res;
        }
 
        protected string GetConnect(string address)
        {
            int res = Connect(address);
            return GetErrorDescription(res);
        }
        protected string LoginToServer(int login, string password)
        {
            int res = Login(login, password);
            return GetErrorDescription(res);
        }

        protected string InitSymbolsRefresh()
        {
            int res = SymbolsRefresh();
            return GetErrorDescription(res);
        }

        protected string GetSymbolsRefresh()
        {
            int res = SymbolsRefresh();
            return GetErrorDescription(res);
        }
        protected List<ConSymbol> GetListOfConSymbols()
        {
            int total = 60;
            void* ptr_conSymbolArray = SymbolsGetAll(&total);

            //создаем структуру
            List<ConSymbol> conSymbols = new List<ConSymbol>();
            //ConSymbol?[] conSymbols = new ConSymbol?[total];

            if (total == 0)
            {
                return conSymbols;
            }


            // с каждыйм шагом будем перемещаться на слеудующий объект с шагом ptr+=len
            int len = Marshal.SizeOf(typeof(ConSymbol));
            IntPtr ptr = (IntPtr)ptr_conSymbolArray;

            //заполняем массив
            for (int i = 0; i < total; i++, ptr += len)
            {
                conSymbols.Add((ConSymbol)Marshal.PtrToStructure(ptr, typeof(ConSymbol)));
            }

            return conSymbols;
        }

        protected List<ConGroup> GetListOfConGroup()
        {
            int total = 60;
            void* ptr_conSymbolArray = GroupsRequest(&total);

            //создаем структуру
            List<ConGroup> conGroup = new List<ConGroup>();
            //ConSymbol?[] conSymbols = new ConSymbol?[total];

            if (total == 0)
            {
                return conGroup;
            }


            // с каждыйм шагом будем перемещаться на слеудующий объект с шагом ptr+=len
            int len = Marshal.SizeOf(typeof(ConGroup));
            IntPtr ptr = (IntPtr)ptr_conSymbolArray;

            //заполняем массив
            for (int i = 0; i < total; i++, ptr += len)
            {
                conGroup.Add((ConGroup)Marshal.PtrToStructure(ptr, typeof(ConGroup)));
            }

            return conGroup;
        }

        protected string GetErrorDescription(int error)
        {
            byte* des = ErrorDescription(error);
            return toString(des);
        }

        static String toString(byte* s)  //    Собираем строку
        {
            byte* p = s;
            byte[] B = new byte[10000];
            int i = 0;
            while (*p != 0 && i < B.Length - 1) B[i++] = (*p++);
            return System.Text.Encoding.ASCII.GetString(B, 0, i);
        }
    }
}
