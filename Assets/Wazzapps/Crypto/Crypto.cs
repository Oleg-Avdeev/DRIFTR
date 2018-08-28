using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Security.Cryptography;
using UnityEngine;

namespace Wazzapps.Crypto
{
    public static class Crypto
    {
        private static readonly SHA1 sha1Provider = new SHA1CryptoServiceProvider();
        private static readonly SHA256 sha256Provider = new SHA256Managed();
        private static readonly BinaryFormatter binaryFormatter = new BinaryFormatter();

        private static string a_key = null;
        private static string A_KEY
        {
            get
            {
                if (a_key == null)
                {
                    a_key = Resources.Load<SecurityKey>("Wazzapps/Crypto/SecurityKey").KEY;
                }
                return a_key;
            }
        }
        private static string B_KEY { get { return "3f8a2204f0ffa1e30d64b21c81576683876fe10272b7b83a44ab1f4a5860d83a"; } }
        #region HEX
        public static byte[] ConvertFromHex(string hex)
        {
            if (hex.Length % 2 == 1)
                throw new Exception("The binary key cannot have an odd number of digits");

            byte[] arr = new byte[hex.Length >> 1];

            for (int i = 0; i < hex.Length >> 1; ++i)
            {
                arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
            }

            return arr;
        }
        public static int GetHexVal(char hex)
        {
            int val = (int)hex;
            //For uppercase A-F letters:
            //return val - (val < 58 ? 48 : 55);
            //For lowercase a-f letters:
            return val - (val < 58 ? 48 : 87);
        }
        public static string ConvertToHex(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString("x2"));
            }
            return sb.ToString();
        }
        #endregion
        #region serialize

        public static byte[] SerializeToBytes<T>(T value)
        {
            var memoryStream = new MemoryStream();
            binaryFormatter.Serialize(memoryStream, value);
            return memoryStream.ToArray();
        }
        public static T DeserializeFromBytes<T>(byte[] data)
        {
            var dataStream = new MemoryStream(data);
            return (T)(binaryFormatter.Deserialize(dataStream));
        }

        #endregion
        #region SHA
        public static string SHA1(string s)
        {
            return ConvertToHex(SHA1(SerializeToBytes(s + A_KEY)));
        }
        public static string SHA256(string s)
        {
            return ConvertToHex(SHA256(SerializeToBytes(s + A_KEY)));
        }
        public static byte[] SHA1(byte[] data)
        {
            return sha1Provider.ComputeHash(data);
        }
        public static byte[] SHA256(byte[] data)
        {
            return sha256Provider.ComputeHash(data);
        }
        #endregion
        #region bytes
        public static byte[] Encrypt(byte[] message)
        {
            if ((message == null) || (message.Length == 0))
            {
                return message;
            }

            RijndaelManaged alg = new RijndaelManaged();
            alg.Key = ConvertFromHex(B_KEY);
            alg.IV = ConvertFromHex(A_KEY);

            using (var stream = new MemoryStream())
            using (var encryptor = alg.CreateEncryptor())
            using (var encrypt = new CryptoStream(stream, encryptor, CryptoStreamMode.Write))
            {
                encrypt.Write(message, 0, message.Length);
                encrypt.FlushFinalBlock();
                return stream.ToArray();
            }
        }

        public static byte[] Decrypt(byte[] message)
        {
            if ((message == null) || (message.Length == 0))
            {
                return message;
            }

            RijndaelManaged alg = new RijndaelManaged();
            alg.Key = ConvertFromHex(B_KEY);
            alg.IV = ConvertFromHex(A_KEY);

            using (var stream = new MemoryStream())
            using (var decryptor = alg.CreateDecryptor())
            using (var encrypt = new CryptoStream(stream, decryptor, CryptoStreamMode.Write))
            {
                encrypt.Write(message, 0, message.Length);
                encrypt.FlushFinalBlock();
                return stream.ToArray();
            }
        }
        #endregion
        #region fingerprint
        public static string GetFingerprint()
        {
            return CertificateSHA1Fingerprint.getCertificateSHA1Fingerprint();
        }
        #endregion
    }
}
