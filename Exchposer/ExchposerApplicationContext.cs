using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Exchange.WebServices.Data;
using System.Text.RegularExpressions;
using System.IO;
using System.Drawing;
using Microsoft.Win32;

namespace Exchposer
{
    public class ExchposerApplicationContext : ApplicationContext, IExchposer
    {
        RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        private const int maxLogFileSize = 1024 * 50;
        private const int maxLogFileCount = 3;

        private const int exchangeReconnectTimeout = 10;

        static AppSettings appSettings = null;
        private FileString syncId = null;

        LogWriter logWriter = null;
        MailServer mailServer = null;
        ExchangeServer exchangeServer = null;

        //Icon AppIconNormal = new Icon("AppIconNormal.ico");
        //Icon AppIconBusy = new Icon("AppIconBusy.ico");

        public bool Initialized { get { return notifyIcon != null; } }

        NotifyIcon notifyIcon = null;
        ContextMenuStrip notifyIconMenu = null;
        OptionsForm optionsForm = null;
        LogViewForm logViewForm = null;

        public bool AutoRun
        {
            get
            {
                return (rkApp.GetValue(AppSettings.AppName) != null);
            }
            set
            {
                if (AutoRun != value)
                {
                    if (value)
                        rkApp.SetValue(AppSettings.AppName, Application.ExecutablePath.ToString());
                    else
                        rkApp.DeleteValue(AppSettings.AppName, false);
                }
            }
        }

        delegate void SetLogCallback(int level, string message);

        private void CreateAppDefaultFolder(string folderName)
        {
            if (System.IO.Path.GetFullPath(folderName).Equals(System.IO.Path.GetFullPath(AppSettings.AppDefaultFolder)))
                Directory.CreateDirectory(folderName);
        }

        protected void Log(int level, string message)
        {
            int managedThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
            message = "[thr" + managedThreadId + "] " + message;

            if (notifyIconMenu.InvokeRequired)
            {
                SetLogCallback d = new SetLogCallback(Log);
                notifyIconMenu.Invoke(d, new object[] { level, message });
                return;
            }

            if (logWriter != null)
            {
                string logText = logWriter.Log(level, message);
                if ((logViewForm != null) && (logText != ""))
                    logViewForm.AppendLog(logText + Environment.NewLine);
            }
        }

        void ProcessExchangeMessage(EmailMessage msg, bool offlineSync)
        {
            if ((notifyIcon == null) || (exchangeServer == null) || (mailServer == null))
                return;

            notifyIcon.Icon = Properties.Resources.AppIconBusy;

            exchangeServer.LoadMessage(msg);

            Log(11, String.Format("Exchange message processing. Time: {0}, Subject: {1}", msg.DateTimeReceived.ToString(), msg.Subject));

            string fromAddress = Regex.Match(msg.Sender.ToString(), "[a-zA-Z0-9_.+-]+@33[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+", RegexOptions.None).ToString();
            mailServer.Send(fromAddress, appSettings.MailToAddress, appSettings.MailFolderName, msg.MimeContent.ToString());

            if (!offlineSync)
            {
                syncId.Value = msg.DateTimeReceived.ToString();
                appSettings.Save();
            }

            notifyIcon.Icon = Properties.Resources.AppIconNormal;
        }

        void SyncExchangeMessages(DateTime fromTime, DateTime toTime, bool offlineSync)
        {
            try
            {
                //var findResults = exchangeServer.GetMessages(Convert.ToDateTime(SyncId).AddSeconds(1), DateTime.MaxValue);

                mailServer.Open();
                exchangeServer.ProcessMessages(fromTime, toTime, (msg) =>
                {
                    ProcessExchangeMessage(msg, offlineSync);
                    Application.DoEvents();
                });
                mailServer.Close();
            }
            catch (Exception ex)
            {
                Log(1, String.Format("Initial messages synchronization error: {0}", ex.Message));
            }
        }


        public bool AppInit()
        {
            AppStop();
            try
            {
                AppSettings.Load(ref appSettings);
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Load settings error: {0}", ex.Message));
                return false;
            }

            try
            {
                CreateAppDefaultFolder(Path.GetDirectoryName(appSettings.LogFileName));
                logWriter = new LogWriter(true, appSettings.LogLevel, appSettings.LogFileName, maxLogFileSize, maxLogFileCount);
                if (appSettings.LogClearOnStartup)
                    logWriter.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Open/create log file error: {0}", ex.Message));
                return false;
            }

            return true;
        }

        public void AppStop()
        {
            if (logWriter != null)
                logWriter.Close();
            logWriter = null;
        }

        public bool SyncInit(DateTime syncFromTime, DateTime syncToTime, bool offlineSync)
        {

            CertificateCallback.AcceptInvalidCertificate = appSettings.ExchangeAcceptInvalidCertificate;
            exchangeServer = new ExchangeServer(appSettings.ExchangeUserName, appSettings.ExchangePassword,
                appSettings.ExchangeDomain, appSettings.ExchangeUrl, exchangeReconnectTimeout, (l, m) => { Log(l, m); });

            try
            {
                exchangeServer.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Exchange server connection error: {0}", ex.Message));
                return false;
            }

            switch (appSettings.MailServerType)
            {
                case MailServerTypes.IMAP:
                    mailServer = new ImapServer(appSettings.MailServerName, appSettings.MailServerPort,
                        appSettings.MailUserName, appSettings.MailPassword,
                        (l, m) => { Log(l, m); });
                    break;

                case MailServerTypes.SMTP:
                    mailServer = new SmtpServer(appSettings.MailServerName, appSettings.MailServerPort,
                        appSettings.MailUserName, appSettings.MailPassword,
                        (l, m) => { Log(l, m); });
                    break;

                case MailServerTypes.SMTPMX:
                    mailServer = new SmtpMxServer(appSettings.MailToAddress, (l, m) => { Log(l, m); });
                    break;

                default:
                    throw new InvalidOperationException("Invalid mail server type");
            }

            if (syncToTime > syncFromTime)
                SyncExchangeMessages(syncFromTime, syncToTime, offlineSync);

            return true;
        }

        public void SyncStart()
        {
            SyncStop();

            DateTime currentTime = DateTime.Now;
            DateTime syncFromTime = DateTime.MinValue;
            DateTime syncToTime = DateTime.MaxValue;
            try
            {
                syncFromTime = (syncId.Value != "" ? Convert.ToDateTime(syncId.Value).AddSeconds(1) : DateTime.MinValue);
            }
            catch
            {
                MessageBox.Show(String.Format("Error converting sincId \"{0}\" to time. Will using current time", syncId));
                syncFromTime = currentTime;
            }

            //if ((currentTime.Date - syncFromTime.Date).TotalDays > appSettings.MaxDaysToSync)
            if ((currentTime - syncFromTime).TotalDays > appSettings.MaxDaysToSync)
                syncFromTime = currentTime.AddDays(-appSettings.MaxDaysToSync + 1).Date;

            if (!SyncInit(syncFromTime, syncToTime, false))
                return;

            exchangeServer.StartStreamingNotifications(msg =>
            {
                try
                {
                    mailServer.Open();
                    ProcessExchangeMessage(msg, false);
                    mailServer.Close();
                }
                catch (Exception ex)
                {
                    Log(1, String.Format("Messages processing error: {0}", ex.Message));
                }
            }, appSettings.ExchangeSubscriptionLifetime);
        }

        public void SyncStop()
        {
            if (exchangeServer != null)
                exchangeServer.Close();
            exchangeServer = null;

            if (mailServer != null)
                mailServer.Close();
            mailServer = null;
        }

        public void OfflineSync(DateTime syncFromTime, DateTime syncToTime)
        {
            if (exchangeServer == null)
            {
                SyncInit(syncFromTime, syncToTime, true);
                SyncStop();
            }
            else
                if (syncToTime > syncFromTime)
                    SyncExchangeMessages(syncFromTime, syncToTime, true);
        }

        public void ClearLog()
        {
            if (logWriter != null)
                logWriter.Clear();
        }

        public ExchposerApplicationContext(string appSettingsFileName)
        {
            string fileName = "";
            try
            {
                fileName = (appSettingsFileName != null ? appSettingsFileName : Path.Combine(AppSettings.AppDefaultFolder, "config.xml"));
                CreateAppDefaultFolder(Path.GetDirectoryName(fileName));
                appSettings = AppSettings.Load(fileName);
                appSettings.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Unable to read/write settings from file {0}: {1}", fileName, ex.Message));
                return;
            }

            syncId = new FileString(Path.Combine(AppSettings.AppDefaultFolder, "syncid"));

            notifyIconMenu = new ContextMenuStrip();

            notifyIconMenu.Items.Add("&Options...").Click += new EventHandler(OptionsMenuItem_Click);
            notifyIconMenu.Items.Add("&Log...").Click += new EventHandler(LogMenuItem_Click);
            notifyIconMenu.Items.Add("E&xit").Click += new EventHandler(ExitMenuItem_Click);

            notifyIcon = new NotifyIcon();
            notifyIcon.Text = AppSettings.AppName;
            notifyIcon.Icon = Properties.Resources.AppIconNormal;
            notifyIcon.ContextMenuStrip = notifyIconMenu;
            notifyIcon.DoubleClick += new EventHandler(NotifyIcon_DoubleClick);
            notifyIcon.Visible = true;

            CertificateCallback.Initialize();

            AppInit();
            if (appSettings.SyncEnabled)
                SyncStart();
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            LogMenuItem_Click(sender, e);
        }

        private void OptionsMenuItem_Click(object sender, EventArgs e)
        {
            if (optionsForm != null)
            {
                optionsForm.Activate();
                return;
            }
            if (logViewForm != null)
            {
                logViewForm.Dispose();
                logViewForm = null;
            }

            using (optionsForm = new OptionsForm(appSettings, this))
            {
                if (optionsForm.ShowDialog() == DialogResult.OK)
                {
                    appSettings.Copy(optionsForm.GetSettings());
                    appSettings.Save();

                    AppInit();
                    if (appSettings.SyncEnabled)
                        SyncStart();
                    else
                        SyncStop();
                }
            }
            optionsForm = null;
        }

        private void LogMenuItem_Click(object sender, EventArgs e)
        {
            if (logViewForm != null)
            {
                logViewForm.Activate();
                return;
            }
            if (optionsForm != null)
            {
                optionsForm.Dispose();
                optionsForm = null;
            }

            using (logViewForm = new LogViewForm())
            {
                if (logWriter != null)
                    logWriter.Load((msg) => logViewForm.AppendLog(msg));
                logViewForm.ShowDialog();
            }
            logViewForm = null;
        }

        void ExitMenuItem_Click(object sender, EventArgs e)
        {
            ExitThreadCore();
        }

        protected override void ExitThreadCore()
        {
            if (logViewForm != null)
            {
                logViewForm.Dispose();
                logViewForm = null;
            }

            if (optionsForm != null)
            {
                optionsForm.Dispose();
                optionsForm = null;
            }

            if (notifyIcon != null)
            {
                notifyIcon.Dispose();
                notifyIcon = null;
            }

            SyncStop();
            AppStop();

            base.ExitThreadCore();
        }

        /*
        private static void ExitMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void OnApplicationExit(object sender, EventArgs e)
        {
            if (notifyIcon != null)
            {
                notifyIcon.Dispose();
                notifyIcon = null;
            }
        }
        */
    }
}
