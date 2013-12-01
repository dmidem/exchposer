using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Exchposer
{
    public partial class OptionsForm : Form
    {
        private AppSettings appSetting = new AppSettings();
        private IExchposer exchposer = null;

        public OptionsForm()
        {
            InitializeComponent();
        }

        public OptionsForm(AppSettings appSetting, IExchposer exchposer)
        {
            InitializeComponent();

            this.exchposer = exchposer;

            txtExchangeUrl.Text = appSetting.ExchangeUrl;
            txtExchangeDomain.Text = appSetting.ExchangeDomain;
            txtExchangeUserName.Text = appSetting.ExchangeUserName;
            txtExchangePassword.Text = appSetting.ExchangePassword;
            updExchangeSubscriptionLifetime.Value = appSetting.ExchangeSubscriptionLifetime;

            txtMailServerName.Text = appSetting.MailServerName;
            txtMailServerPort.Text = appSetting.MailServerPort.ToString();
            cboMailServerType.SelectedIndex = (int)appSetting.MailServerType - 1;
            txtMailUserName.Text = appSetting.MailUserName;
            txtMailPassword.Text = appSetting.MailPassword;
            txtMailToAddress.Text = appSetting.MailToAddress;
            txtMailFolderName.Text = appSetting.MailFolderName;

            chkSyncEnabled.Checked = appSetting.SyncEnabled;

            txtLogFileName.Text = appSetting.LogFileName;
            updLogLevel.Text = appSetting.LogLevel.ToString();
            chkLogClearOnStartup.Checked = appSetting.LogClearOnStartup;

            chkAutoRun.Checked = exchposer.AutoRun;

            UpdateControlsEnabling();
        }

        public AppSettings GetSettings()
        {
            return appSetting;
        }

        public void UpdateSettings()
        {
            appSetting.ExchangeUrl = txtExchangeUrl.Text;
            appSetting.ExchangeDomain = txtExchangeDomain.Text;
            appSetting.ExchangeUserName = txtExchangeUserName.Text;
            appSetting.ExchangePassword = txtExchangePassword.Text;
            appSetting.ExchangeSubscriptionLifetime = (int)updExchangeSubscriptionLifetime.Value;

            appSetting.MailServerName = txtMailServerName.Text;
            appSetting.MailServerPort = Convert.ToInt32(txtMailServerPort.Text);
            appSetting.MailServerType = (MailServerTypes)(cboMailServerType.SelectedIndex + 1);
            appSetting.MailUserName = txtMailUserName.Text;
            appSetting.MailPassword = txtMailPassword.Text;
            appSetting.MailToAddress = txtMailToAddress.Text;
            appSetting.MailFolderName = txtMailFolderName.Text;

            appSetting.SyncEnabled = chkSyncEnabled.Checked;

            appSetting.LogFileName = txtLogFileName.Text;
            appSetting.LogLevel = Convert.ToInt32(updLogLevel.Text);
            appSetting.LogClearOnStartup = chkLogClearOnStartup.Checked;

            exchposer.AutoRun = chkAutoRun.Checked;
        }


        private void UpdateControlsEnabling()
        {
            MailServerTypes mailServerType = (MailServerTypes)(cboMailServerType.SelectedIndex + 1);

            txtMailUserName.Enabled = ((mailServerType == MailServerTypes.IMAP) || (mailServerType == MailServerTypes.SMTP));
            txtMailPassword.Enabled = ((mailServerType == MailServerTypes.IMAP) || (mailServerType == MailServerTypes.SMTP));
            txtMailToAddress.Enabled = ((mailServerType == MailServerTypes.SMTP) || (mailServerType == MailServerTypes.SMTPMX));
            txtMailFolderName.Enabled = (mailServerType == MailServerTypes.IMAP);
            updMaxDaysToSync.Enabled = chkSyncEnabled.Checked;
        }

        private void cboMailServerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateControlsEnabling();
        }

        private void chkSyncEnabled_CheckedChanged(object sender, EventArgs e)
        {
            UpdateControlsEnabling();
        }

        private void btnLogFileName_Click(object sender, EventArgs e)
        {
            dlgFileOpen.InitialDirectory = Path.GetDirectoryName(txtLogFileName.Text);
            dlgFileOpen.FileName = Path.GetFileName(txtLogFileName.Text);
            if (dlgFileOpen.ShowDialog() == DialogResult.OK)
                txtLogFileName.Text = dlgFileOpen.FileName;
        }

        private void btnOfflineSyncNow_Click(object sender, EventArgs e)
        {
            DateTime syncFromTime = dtpOffilneSyncFrom.Value.Date;
            DateTime syncToTime = dtpOffilneSyncTo.Value.Date.AddDays(1).AddSeconds(-1);

            if (exchposer == null)
                return;

            if (MessageBox.Show(String.Format("Do you want to synchronize messages from time {0} to time {1}?",
                syncFromTime, syncToTime), "Offline synchronization", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                exchposer.OfflineSync(syncFromTime, syncToTime);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateSettings();
            }
            catch
            {
                MessageBox.Show("Error in values");
                DialogResult = DialogResult.None;
            }
        }

    }
}
