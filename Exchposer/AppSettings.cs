using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Exchposer
{
    public enum MailServerTypes { IMAP = 1, SMTP = 2, SMTPMX = 3 };

    public class AppSettings : XmlSettings<AppSettings>
    {
        public static string AppName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        public static string AppDefaultFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppName);
        public const string SettingsPassword = "ExCh2811";

        public string ExchangeUrl = "";
        public string ExchangeDomain = "";
        public string ExchangeUserName = "";
        public string ExchangePasswordCrypted = "";
        public int ExchangeSubscriptionLifetime = 30;
        public string ExchangePassword
        {
            get { return Crypto.Decrypt(ExchangePasswordCrypted, SettingsPassword); }
            set { ExchangePasswordCrypted = Crypto.Encrypt(value, SettingsPassword); }
        }

        public string MailServerName = "";
        public int MailServerPort = 0;
        public MailServerTypes MailServerType = MailServerTypes.IMAP;
        public string MailUserName = "";
        public string MailPasswordCrypted = "";
        public string MailToAddress = "";
        public string MailFolderName = "";
        public string MailPassword
        {
            get { return Crypto.Decrypt(MailPasswordCrypted, SettingsPassword); }
            set { MailPasswordCrypted = Crypto.Encrypt(value, SettingsPassword); }
        }

        public bool SyncEnabled = false;
        public int MaxDaysToSync = 7;

        public string LogFileName = Path.Combine(AppDefaultFolder, "log.txt");
        public int LogLevel = 11;
        public bool LogClearOnStartup = true;

        public void Copy(AppSettings appSettings)
        {
            ExchangeUrl = appSettings.ExchangeUrl;
            ExchangeDomain = appSettings.ExchangeDomain;
            ExchangeUserName = appSettings.ExchangeUserName;
            ExchangePasswordCrypted = appSettings.ExchangePasswordCrypted;
            ExchangeSubscriptionLifetime = appSettings.ExchangeSubscriptionLifetime;
            MailServerName = appSettings.MailServerName;
            MailServerPort = appSettings.MailServerPort;
            MailServerType = appSettings.MailServerType;
            MailUserName = appSettings.MailUserName;
            MailPasswordCrypted = appSettings.MailPasswordCrypted;
            MailToAddress = appSettings.MailToAddress;
            MailFolderName = appSettings.MailFolderName;
            MailPassword = appSettings.MailPassword;
            SyncEnabled = appSettings.SyncEnabled;
            MaxDaysToSync = appSettings.MaxDaysToSync;
            LogFileName = appSettings.LogFileName;
            LogLevel = appSettings.LogLevel;
            LogClearOnStartup = appSettings.LogClearOnStartup;
        }

        /*
        public AppSettings(AppSettings appSettings) : base(appSettings)
        {
            Copy(appSettings);
        }
        */
    }
}
