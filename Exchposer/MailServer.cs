﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections;
using System.ComponentModel;
using System.Net.Sockets;
using System.IO;
using System.Net.Security;

namespace Exchposer
{
    public class Mx
    {
        public Mx()
        {
        }

        [DllImport("dnsapi", EntryPoint = "DnsQuery_W", CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
        private static extern int DnsQuery([MarshalAs(UnmanagedType.VBByRefStr)]ref string pszName, QueryTypes wType, QueryOptions options, int aipServers, ref IntPtr ppQueryResults, int pReserved);

        [DllImport("dnsapi", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern void DnsRecordListFree(IntPtr pRecordList, int FreeType);

        public static string[] GetMXRecords(string domain)
        {

            IntPtr ptr1 = IntPtr.Zero;
            IntPtr ptr2 = IntPtr.Zero;
            MXRecord recMx;
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                throw new NotSupportedException();
            }
            ArrayList list1 = new ArrayList();
            int num1 = Mx.DnsQuery(ref domain, QueryTypes.DNS_TYPE_MX, QueryOptions.DNS_QUERY_BYPASS_CACHE, 0, ref ptr1, 0);
            if (num1 != 0)
            {
                throw new Win32Exception(num1);
            }
            for (ptr2 = ptr1; !ptr2.Equals(IntPtr.Zero); ptr2 = recMx.pNext)
            {
                recMx = (MXRecord)Marshal.PtrToStructure(ptr2, typeof(MXRecord));
                if (recMx.wType == 15)
                {
                    string text1 = Marshal.PtrToStringAuto(recMx.pNameExchange);
                    list1.Add(text1);
                }
            }
            Mx.DnsRecordListFree(ptr2, 0);
            return (string[])list1.ToArray(typeof(string));
        }

        private enum QueryOptions
        {
            DNS_QUERY_ACCEPT_TRUNCATED_RESPONSE = 1,
            DNS_QUERY_BYPASS_CACHE = 8,
            DNS_QUERY_DONT_RESET_TTL_VALUES = 0x100000,
            DNS_QUERY_NO_HOSTS_FILE = 0x40,
            DNS_QUERY_NO_LOCAL_NAME = 0x20,
            DNS_QUERY_NO_NETBT = 0x80,
            DNS_QUERY_NO_RECURSION = 4,
            DNS_QUERY_NO_WIRE_QUERY = 0x10,
            DNS_QUERY_RESERVED = -16777216,
            DNS_QUERY_RETURN_MESSAGE = 0x200,
            DNS_QUERY_STANDARD = 0,
            DNS_QUERY_TREAT_AS_FQDN = 0x1000,
            DNS_QUERY_USE_TCP_ONLY = 2,
            DNS_QUERY_WIRE_ONLY = 0x100
        }

        private enum QueryTypes
        {
            DNS_TYPE_MX = 15
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MXRecord
        {
            public IntPtr pNext;
            public string pName;
            public short wType;
            public short wDataLength;
            public int flags;
            public int dwTtl;
            public int dwReserved;
            public IntPtr pNameExchange;
            public short wPreference;
            public short Pad;
        }
    }


    public abstract class MailServer
    {
        protected void TcpClientTimeoutConnect(TcpClient tc, string server, int port, int timemout)
        {
            IAsyncResult ar = tc.BeginConnect(server, port, null, null);
            System.Threading.WaitHandle wh = ar.AsyncWaitHandle;
            try
            {
                if (!ar.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(timemout * 0.001), false))
                {
                    tc.Close();
                    throw new TimeoutException();
                }
                tc.EndConnect(ar);
            }
            finally
            {
                wh.Close();
            }
        }

        protected const int connectTimeout = 5000;
        protected const int readTimeout = 5000;
        protected const int writeTimeout = 5000;

        protected string server = null;
        protected int port = 0;
        protected TcpClient client = null;
        protected NetworkStream stream = null;
        protected StreamReader reader = null;
        protected StreamWriter writer = null;

        private readonly Action<int, string> logger;

        protected void Log(int level, string message)
        {
            if (logger != null)
                logger(level, message);
        }

        public MailServer(Action<int, string> logger = null)
        {
            this.logger = logger;
        }

        virtual public void Open()
        {
            try
            {
                Close();

                client = new TcpClient();
                TcpClientTimeoutConnect(client, server, port, connectTimeout);
                stream = client.GetStream();

                Log(22, String.Format("Mail server connection opened"));
            }
            catch
            {
                if (stream != null)
                    stream.Dispose();
                stream = null;

                if (client != null)
                    client.Close();
                client = null;

                throw;
            }
        }

        virtual public void Close()
        {
            if (stream != null)
                stream.Dispose();
            stream = null;

            if (client != null)
            {
                client.Close();
                Log(22, String.Format("Mail server connection closed"));
            }
            client = null;
        }

        abstract public void Send(string fromAddress, string toAddress, string folderName, string messageData);
    }


    public class ImapServer : MailServer
    {
        private readonly string userName;
        private readonly string password;

        private SslStream sslStream = null;

        public ImapServer(string server, int port, string userName, string password, Action<int, string> logger = null)
            : base(logger)
        {
            this.server = server;
            this.port = port;
            this.userName = userName;
            this.password = password;
        }

        override public void Open()
        {
            try
            {
                Close();

                base.Open();

                sslStream = new SslStream(stream);
                sslStream.AuthenticateAsClient(server);

                reader = new StreamReader(sslStream);
                writer = new StreamWriter(sslStream) { AutoFlush = true };

                reader.BaseStream.ReadTimeout = readTimeout;
                writer.BaseStream.ReadTimeout = writeTimeout;

                string serverResponse;

                serverResponse = reader.ReadLine();
                if (!serverResponse.StartsWith("* OK"))
                    throw new InvalidOperationException("IMAP server respond to connection request: " + serverResponse);

                writer.WriteLine(". LOGIN " + userName + " " + password);
                do
                    serverResponse = reader.ReadLine();
                while (serverResponse.StartsWith("*"));
                if (!serverResponse.StartsWith(". OK"))
                    throw new InvalidOperationException("IMAP server respond to LOGIN request: " + serverResponse);

                Log(22, String.Format("IMAP server opened"));
            }
            catch (Exception ex)
            {
                Log(1, String.Format("IMAP server open error: {0}", ex.Message));

                if (writer != null)
                    writer.Dispose();
                writer = null;

                if (reader != null)
                    reader.Dispose();
                reader = null;

                if (sslStream != null)
                    sslStream.Dispose();
                sslStream = null;

                base.Close();

                throw;
            }
        }

        override public void Close()
        {
            try
            {
                if (writer != null)
                {
                    string serverResponse;

                    writer.WriteLine(". LOGOUT");
                    do
                        serverResponse = reader.ReadLine();
                    while (serverResponse.StartsWith("*"));
                    if (!serverResponse.StartsWith(". OK"))
                        throw new InvalidOperationException("IMAP server respond to LOGOUT request: " + serverResponse);

                    Log(22, String.Format("IMAP server closed"));
                }
            }
            catch (Exception ex)
            {
                Log(1, String.Format("IMAP server close error: {0}", ex.Message));
            }
            finally
            {
                if (writer != null)
                    writer.Dispose();
                writer = null;

                if (reader != null)
                    reader.Dispose();
                reader = null;

                if (sslStream != null)
                    sslStream.Dispose();
                sslStream = null;

                base.Close();
            }
        }

        override public void Send(string fromAddress, string toAddress, string folderName, string messageData)
        {
            Send(folderName, messageData);
        }

        public void Send(string folderName, string messageData)
        {
            string serverResponse;

            try
            {
                writer.WriteLine(". APPEND \"" + folderName + "\" () {" + writer.Encoding.GetByteCount(messageData) + "}");
                serverResponse = reader.ReadLine();
                if (!serverResponse.StartsWith("+"))
                    throw new InvalidOperationException("IMAP server respond to APPEND request: " + serverResponse);

                writer.WriteLine(messageData);
                do
                    serverResponse = reader.ReadLine();
                while (serverResponse.StartsWith("*"));
                if (!serverResponse.StartsWith(". OK"))
                    throw new InvalidOperationException("IMAP server respond to message data: " + serverResponse);

                Log(12, String.Format("IMAP message appended (to folded: {0})", folderName));
            }
            catch (Exception ex)
            {
                Log(1, String.Format("IMAP append error: {0}", ex.Message));
            }
        }
    }


    public class SmtpServer : MailServer
    {
        private readonly string userName;
        private readonly string password;

        private StreamReader clearTextReader = null;
        private StreamWriter clearTextWriter = null;
        private SslStream sslStream = null;

        public SmtpServer(string server, int port, string userName, string password, Action<int, string> logger = null)
            : base(logger)
        {
            this.server = server;
            this.port = port;
            this.userName = userName;
            this.password = password;
        }

        override public void Open()
        {
            try
            {
                Close();

                base.Open();

                clearTextReader = new StreamReader(stream);
                clearTextWriter = new StreamWriter(stream) { AutoFlush = true };

                clearTextReader.BaseStream.ReadTimeout = readTimeout;
                clearTextWriter.BaseStream.WriteTimeout = writeTimeout;

                string serverResponse;

                serverResponse = clearTextReader.ReadLine();
                if (!serverResponse.StartsWith("220"))
                    throw new InvalidOperationException("SMTP server respond to connection request: " + serverResponse);

                clearTextWriter.WriteLine("HELO");
                serverResponse = clearTextReader.ReadLine();
                if (!serverResponse.StartsWith("250"))
                    throw new InvalidOperationException("SMTP server respond to HELO request: " + serverResponse);

                clearTextWriter.WriteLine("STARTTLS");
                serverResponse = clearTextReader.ReadLine();
                if (!serverResponse.StartsWith("220"))
                    throw new InvalidOperationException("SMTP server respond to STARTTLS request: " + serverResponse);

                sslStream = new SslStream(stream);
                sslStream.AuthenticateAsClient(server);

                reader = new StreamReader(sslStream);
                writer = new StreamWriter(sslStream) { AutoFlush = true };

                reader.BaseStream.ReadTimeout = readTimeout;
                writer.BaseStream.ReadTimeout = writeTimeout;

                writer.WriteLine("EHLO " + server);
                serverResponse = reader.ReadLine();
                if (!serverResponse.StartsWith("250"))
                    throw new InvalidOperationException("SMTP server respond to EHLO request: " + serverResponse);
                while (reader.Peek() > -1)
                    reader.ReadLine();

                var authString = Convert.ToBase64String(Encoding.ASCII.GetBytes("\0" + userName + "\0" + password));
                writer.WriteLine("AUTH PLAIN " + authString);
                serverResponse = reader.ReadLine();
                if (!serverResponse.StartsWith("235"))
                    throw new InvalidOperationException("SMTP server respond to AUTH PLAIN request: " + serverResponse);

                Log(22, String.Format("SMTP Mx server opened"));
            }
            catch (Exception ex)
            {
                Log(1, String.Format("SMTP Mx server open error: {0}", ex.Message));

                if (writer != null)
                    writer.Dispose();
                writer = null;

                if (reader != null)
                    reader.Dispose();
                reader = null;

                if (sslStream != null)
                    sslStream.Dispose();
                sslStream = null;

                if (clearTextWriter != null)
                    clearTextWriter.Dispose();
                clearTextWriter = null;

                if (clearTextReader != null)
                    clearTextReader.Dispose();
                clearTextReader = null;

                base.Close();

                throw;
            }
        }

        override public void Close()
        {
            try
            {
                if (writer != null)
                {
                    string serverResponse;

                    writer.WriteLine("QUIT");
                    serverResponse = reader.ReadLine();
                    if (!serverResponse.StartsWith("221"))
                        throw new InvalidOperationException("SMTP server respond to QUIT request: " + serverResponse);

                    Log(22, String.Format("SMTP Mx server closed"));
                }
            }
            catch (Exception ex)
            {
                Log(1, String.Format("SMTP Mx server close error: {0}", ex.Message));
            }
            finally
            {
                if (writer != null)
                    writer.Dispose();
                writer = null;

                if (reader != null)
                    reader.Dispose();
                reader = null;

                if (sslStream != null)
                    sslStream.Dispose();
                sslStream = null;

                if (clearTextWriter != null)
                    clearTextWriter.Dispose();
                clearTextWriter = null;

                if (clearTextReader != null)
                    clearTextReader.Dispose();
                clearTextReader = null;

                base.Close();
            }
        }

        override public void Send(string fromAddress, string toAddress, string folderName, string messageData)
        {
            Send(fromAddress, toAddress, messageData);
        }

        public void Send(string fromAddress, string toAddress, string messageData)
        {
            string serverResponse;

            try
            {
                writer.WriteLine("MAIL FROM: <" + fromAddress + ">");
                serverResponse = reader.ReadLine();
                if (!serverResponse.StartsWith("250"))
                    throw new InvalidOperationException("SMTP server respond to MAIL FROM request: " + serverResponse);

                writer.WriteLine("RCPT TO: <" + toAddress + ">");
                serverResponse = reader.ReadLine();
                if (!serverResponse.StartsWith("250"))
                    throw new InvalidOperationException("SMTP server respond to RCPT TO request: " + serverResponse);

                writer.WriteLine("DATA");
                serverResponse = reader.ReadLine();
                if (!serverResponse.StartsWith("354"))
                    throw new InvalidOperationException("SMTP server respond to DATA request: " + serverResponse);

                writer.WriteLine(messageData);
                writer.WriteLine(".");
                serverResponse = reader.ReadLine();
                if (!serverResponse.StartsWith("250"))
                    throw new InvalidOperationException("SMTP server respond to end data request: " + serverResponse);

                Log(12, String.Format("SMTP Mx message sent (from address: {0} to address: {1})", fromAddress, toAddress));
            }
            catch (Exception ex)
            {
                Log(1, String.Format("SMTP Mx send error: {0}", ex.Message));
            }
        }
    }

    
    public class SmtpMxServer : MailServer
    {
        private readonly string toAddress;

        public SmtpMxServer(string toAddress, Action<int, string> logger = null)
            : base(logger)
        {
            this.toAddress = toAddress;

            int i = toAddress.IndexOf('@');
            if ((i <= 0) || (i >= toAddress.Length - 1))
                throw new InvalidOperationException("Bad recipient address");
            var mxs = Mx.GetMXRecords(toAddress.Substring(i + 1));
            if (mxs.Length < 1)
                throw new InvalidOperationException("MX record for recipient address is not found");

            server = mxs[0];
            port = 25;
        }

        override public void Open()
        {
            try
            {
                Close();

                base.Open();

                reader = new StreamReader(stream);
                writer = new StreamWriter(stream) { AutoFlush = true };

                reader.BaseStream.ReadTimeout = readTimeout;
                writer.BaseStream.ReadTimeout = writeTimeout;

                string serverResponse;

                serverResponse = reader.ReadLine();
                if (!serverResponse.StartsWith("220"))
                    throw new InvalidOperationException("SMTP server respond to connection request: " + serverResponse);

                writer.WriteLine("HELO");
                serverResponse = reader.ReadLine();
                if (!serverResponse.StartsWith("250"))
                    throw new InvalidOperationException("SMTP server respond to HELO request: " + serverResponse);

                Log(22, String.Format("SMTP server opened"));
            }
            catch (Exception ex)
            {
                Log(1, String.Format("SMTP server open error: {0}", ex.Message));

                if (writer != null)
                    writer.Dispose();
                writer = null;

                if (reader != null)
                    reader.Dispose();
                reader = null;

                base.Close();

                throw;
            }
        }

        override public void Close()
        {
            try
            {
                if (writer != null)
                {

                    string serverResponse;

                    writer.WriteLine("QUIT");
                    serverResponse = reader.ReadLine();
                    if (!serverResponse.StartsWith("221"))
                        throw new InvalidOperationException("SMTP server respond to QUIT request: " + serverResponse);

                    Log(22, String.Format("SMTP server closed"));
                }
            }
            catch (Exception ex)
            {
                Log(1, String.Format("SMTP server close error: {0}", ex.Message));
            }
            finally
            {
                if (writer != null)
                    writer.Dispose();
                writer = null;

                if (reader != null)
                    reader.Dispose();
                reader = null;

                base.Close();
            }
        }

        override public void Send(string fromAddress, string toAddress, string folderName, string messageData)
        {
            Send(fromAddress, messageData);
        }

        public void Send(string fromAddress, string messageData)
        {
            string serverResponse;

            try
            {
                writer.WriteLine("MAIL FROM: <" + fromAddress + ">");
                serverResponse = reader.ReadLine();
                if (!serverResponse.StartsWith("250"))
                    throw new InvalidOperationException("SMTP server respond to MAIL FROM request: " + serverResponse);

                writer.WriteLine("RCPT TO: <" + toAddress + ">");
                serverResponse = reader.ReadLine();
                if (!serverResponse.StartsWith("250"))
                    throw new InvalidOperationException("SMTP server respond to RCPT TO request: " + serverResponse);

                writer.WriteLine("DATA");
                serverResponse = reader.ReadLine();
                if (!serverResponse.StartsWith("354"))
                    throw new InvalidOperationException("SMTP server respond to DATA request: " + serverResponse);

                writer.WriteLine(messageData);
                writer.WriteLine(".");
                serverResponse = reader.ReadLine();
                if (!serverResponse.StartsWith("250"))
                    throw new InvalidOperationException("SMTP server respond to end data request: " + serverResponse);

                Log(12, String.Format("SMTP message sent (from address: {0})", fromAddress));
            }
            catch (Exception ex)
            {
                Log(1, String.Format("SMTP send error: {0}", ex.Message));
            }
        }
    }
}
