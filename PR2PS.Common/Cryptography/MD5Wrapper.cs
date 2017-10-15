using PR2PS.Common.Constants;
using System;
using System.Security.Cryptography;
using System.Text;

namespace PR2PS.Common.Cryptography
{
    public class MD5Wrapper : IDisposable
    {
        private MD5 md5;

        public MD5Wrapper()
        {
            this.md5 = MD5.Create();
        }

        public void Dispose()
        {
            if (this.md5 != null)
            {
                this.md5.Dispose();
            }
        }

        public String GetHashedString(String input)
        {
            return GetHashedString(Encoding.UTF8.GetBytes(input));
        }

        public String GetHashedString(Byte[] input)
        {
            Byte[] hash = this.md5.ComputeHash(input);
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sBuilder.Append(hash[i].ToString(StringFormat.HEX_LOWERCASE));
            }

            return sBuilder.ToString();
        }

        public Byte[] GetHashedArray(String input)
        {
            return GetHashedArray(Encoding.UTF8.GetBytes(input));
        }

        public Byte[] GetHashedArray(Byte[] input)
        {
            return this.md5.ComputeHash(input);
        }
    }
}
