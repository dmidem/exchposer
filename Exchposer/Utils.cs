using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Security.Cryptography;
//using System.Runtime.Serialization;
//using System.Runtime.Serialization.Formatters.Binary;

namespace Exchposer
{
    public static class Crypto
    {
        private const int keySize = 256;
        private const string initVector = "C#$7gMNVFH^&%6hZ";

        public static string Encrypt(string plainText, string password)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            string result;

            using (ICryptoTransform encryptor = (new RijndaelManaged() { Mode = CipherMode.CBC }).
                CreateEncryptor((new PasswordDeriveBytes(password, null)).GetBytes(keySize / 8), Encoding.UTF8.GetBytes(initVector)))
            using (MemoryStream memoryStream = new MemoryStream())
            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
            {
                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                cryptoStream.FlushFinalBlock();
                result = Convert.ToBase64String(memoryStream.ToArray());
            }

            return result;
        }

        public static string Decrypt(string encryptedText, string password)
        {
            byte[] encryptedTextBytes = Convert.FromBase64String(encryptedText);
            byte[] plainTextBytes = new byte[encryptedTextBytes.Length];
            string result;

            using (ICryptoTransform decryptor = (new RijndaelManaged() { Mode = CipherMode.CBC }).
                CreateDecryptor((new PasswordDeriveBytes(password, null)).GetBytes(keySize / 8), Encoding.UTF8.GetBytes(initVector)))
            using (MemoryStream memoryStream = new MemoryStream(encryptedTextBytes))
            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
            {
                int plainTextByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                result = Encoding.UTF8.GetString(plainTextBytes, 0, plainTextByteCount);
            }

            return result;
        }
    }

    //[Serializable()]
    public class XmlSettings<T> where T : XmlSettings<T>, new()
    {
        private string fileName = null;

        public XmlSettings()
        {
        }

        public XmlSettings(XmlSettings<T> xmlSettings)
        {
            fileName = xmlSettings.fileName;
        }

        public void Save()
        {
            Save(fileName);
        }

        public void Save(string fileName)
        {
            //Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            using (TextWriter writer = new StreamWriter(fileName))
            {
                (new XmlSerializer(typeof(T))).Serialize(writer, this);
            }
        }

        public static T Load(string fileName)
        {
            T t;

            if ((fileName != null) && File.Exists(fileName))
                using (TextReader reader = new StreamReader(fileName))
                {
                    t = (T)(new XmlSerializer(typeof(T))).Deserialize(reader);
                }
            else
                t = new T();

            t.fileName = fileName;
            return t;
        }

        public static void Load(ref T t, string fileName = null)
        {
            if ((t != null) && (fileName == null))
                fileName = t.fileName;
            t = Load(fileName);
        }

        /*
        public T Clone()
        {
            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new MemoryStream())
            {
                formatter.Serialize(stream, this);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
        */
    }

    public class FileString
    {
        private string fileName = null;
        private string value = null;

        public string Value
        {
            get { return Load(); }
            set { Save(value); }
        }

        public FileString(string fileName)
        {
            this.fileName = fileName;
        }

        public void Save(string value)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            using (TextWriter writer = new StreamWriter(fileName))
            {
                writer.Write(value);
            }
            this.value = value;
        }

        public string Load()
        {
            if ((fileName != null) && File.Exists(fileName))
                using (TextReader reader = new StreamReader(fileName))
                {
                    value = reader.ReadToEnd();
                }
            else
                value = null;

            return value;
        }
    }
}
