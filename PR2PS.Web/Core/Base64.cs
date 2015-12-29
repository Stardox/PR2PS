using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR2PS.Web.Core
{
    /// <summary>
    /// Class containing Base64 methods.
    /// </summary>
    public static class Base64
    {
        public static String Encode(String data)
        {
            Byte[] byteArray = Encoding.UTF8.GetBytes(data);
            return EncodeByteArray(byteArray);
        }

        public static String EncodeByteArray(Byte[] data)
        {
            return Convert.ToBase64String(data);
        }

        public static String Decode(String data)
        {
            Byte[] byteArray = DecodeToByteArray(data);
            return Encoding.UTF8.GetString(byteArray);
        }

        public static Byte[] DecodeToByteArray(String data)
        {
            return Convert.FromBase64String(data);
        }
    }
}
