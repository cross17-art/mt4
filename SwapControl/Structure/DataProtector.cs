using SwapControl.Structure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SwapControl.Structure
{
    public class DataProtector
    {
        public static string ProtectData(string data)
        {
            try
            {
                byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                byte[] protectedBytes = ProtectedData.Protect(dataBytes, null, DataProtectionScope.CurrentUser);
                return Convert.ToBase64String(protectedBytes);
            }
            catch (Exception ex)
            {
                Logging.Finish("there is a problem with the ProtectData method", LogLevel.Error);
                return string.Empty;
            }

        }

        public static string UnprotectData(string protectedData)
        {
            try
            {
                byte[] protectedBytes = Convert.FromBase64String(protectedData);
                byte[] unprotectedBytes = ProtectedData.Unprotect(protectedBytes, null, DataProtectionScope.CurrentUser);
                return Encoding.UTF8.GetString(unprotectedBytes);
            }
            catch (Exception ex)
            {
                Logging.Finish("launch attempt from another user", LogLevel.Error);
                return string.Empty;
            }

        }
    }
}
