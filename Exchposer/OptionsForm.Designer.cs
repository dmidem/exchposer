namespace Exchposer
{
    partial class OptionsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
            this.lblBevel = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpExchange = new System.Windows.Forms.GroupBox();
            this.lblExchangeSubscriptionLifetime = new System.Windows.Forms.Label();
            this.updExchangeSubscriptionLifetime = new System.Windows.Forms.NumericUpDown();
            this.txtExchangeDomain = new System.Windows.Forms.TextBox();
            this.lblExchangeDomain = new System.Windows.Forms.Label();
            this.txtExchangePassword = new System.Windows.Forms.TextBox();
            this.lblExchangePassword = new System.Windows.Forms.Label();
            this.txtExchangeUserName = new System.Windows.Forms.TextBox();
            this.lblExchangeUserName = new System.Windows.Forms.Label();
            this.lblExchangeUrl = new System.Windows.Forms.Label();
            this.txtExchangeUrl = new System.Windows.Forms.TextBox();
            this.grpMail = new System.Windows.Forms.GroupBox();
            this.lblMailServerType = new System.Windows.Forms.Label();
            this.cboMailServerType = new System.Windows.Forms.ComboBox();
            this.txtMailServerPort = new System.Windows.Forms.TextBox();
            this.lblMailServerPort = new System.Windows.Forms.Label();
            this.txtMailFolderName = new System.Windows.Forms.TextBox();
            this.lblMailFolderName = new System.Windows.Forms.Label();
            this.txtMailToAddress = new System.Windows.Forms.TextBox();
            this.lblMailToAddress = new System.Windows.Forms.Label();
            this.txtMailPassword = new System.Windows.Forms.TextBox();
            this.lblMailPassword = new System.Windows.Forms.Label();
            this.txtMailUserName = new System.Windows.Forms.TextBox();
            this.lblMailUserName = new System.Windows.Forms.Label();
            this.txtMailServerName = new System.Windows.Forms.TextBox();
            this.lblMailServerName = new System.Windows.Forms.Label();
            this.grpLog = new System.Windows.Forms.GroupBox();
            this.chkLogClearOnStartup = new System.Windows.Forms.CheckBox();
            this.updLogLevel = new System.Windows.Forms.NumericUpDown();
            this.lblLogLevel = new System.Windows.Forms.Label();
            this.btnLogFileName = new System.Windows.Forms.Button();
            this.txtLogFileName = new System.Windows.Forms.TextBox();
            this.lblLogFileName = new System.Windows.Forms.Label();
            this.dlgFileOpen = new System.Windows.Forms.OpenFileDialog();
            this.grpOnlineSync = new System.Windows.Forms.GroupBox();
            this.chkSyncEnabled = new System.Windows.Forms.CheckBox();
            this.lblMaxDaysToSync = new System.Windows.Forms.Label();
            this.updMaxDaysToSync = new System.Windows.Forms.NumericUpDown();
            this.grpOfflineSync = new System.Windows.Forms.GroupBox();
            this.lblOfflineSyncFromTo = new System.Windows.Forms.Label();
            this.btnOfflineSyncNow = new System.Windows.Forms.Button();
            this.dtpOffilneSyncTo = new System.Windows.Forms.DateTimePicker();
            this.dtpOffilneSyncFrom = new System.Windows.Forms.DateTimePicker();
            this.chkAutoRun = new System.Windows.Forms.CheckBox();
            this.chkExchangeAcceptInvalidCertificate = new System.Windows.Forms.CheckBox();
            this.grpExchange.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updExchangeSubscriptionLifetime)).BeginInit();
            this.grpMail.SuspendLayout();
            this.grpLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updLogLevel)).BeginInit();
            this.grpOnlineSync.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updMaxDaysToSync)).BeginInit();
            this.grpOfflineSync.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblBevel
            // 
            this.lblBevel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblBevel.Location = new System.Drawing.Point(10, 464);
            this.lblBevel.Margin = new System.Windows.Forms.Padding(0);
            this.lblBevel.Name = "lblBevel";
            this.lblBevel.Size = new System.Drawing.Size(681, 3);
            this.lblBevel.TabIndex = 6;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Font = new System.Drawing.Font("Tahoma", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnOk.Location = new System.Drawing.Point(486, 482);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(100, 28);
            this.btnOk.TabIndex = 7;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnCancel.Location = new System.Drawing.Point(592, 482);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 28);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // grpExchange
            // 
            this.grpExchange.Controls.Add(this.chkExchangeAcceptInvalidCertificate);
            this.grpExchange.Controls.Add(this.lblExchangeSubscriptionLifetime);
            this.grpExchange.Controls.Add(this.updExchangeSubscriptionLifetime);
            this.grpExchange.Controls.Add(this.txtExchangeDomain);
            this.grpExchange.Controls.Add(this.lblExchangeDomain);
            this.grpExchange.Controls.Add(this.txtExchangePassword);
            this.grpExchange.Controls.Add(this.lblExchangePassword);
            this.grpExchange.Controls.Add(this.txtExchangeUserName);
            this.grpExchange.Controls.Add(this.lblExchangeUserName);
            this.grpExchange.Controls.Add(this.lblExchangeUrl);
            this.grpExchange.Controls.Add(this.txtExchangeUrl);
            this.grpExchange.Location = new System.Drawing.Point(10, 12);
            this.grpExchange.Name = "grpExchange";
            this.grpExchange.Size = new System.Drawing.Size(338, 224);
            this.grpExchange.TabIndex = 0;
            this.grpExchange.TabStop = false;
            this.grpExchange.Text = "Exchange server";
            // 
            // lblExchangeSubscriptionLifetime
            // 
            this.lblExchangeSubscriptionLifetime.AutoSize = true;
            this.lblExchangeSubscriptionLifetime.Location = new System.Drawing.Point(113, 145);
            this.lblExchangeSubscriptionLifetime.Name = "lblExchangeSubscriptionLifetime";
            this.lblExchangeSubscriptionLifetime.Size = new System.Drawing.Size(189, 17);
            this.lblExchangeSubscriptionLifetime.TabIndex = 9;
            this.lblExchangeSubscriptionLifetime.Text = "subscription lifetime, minutes";
            // 
            // updExchangeSubscriptionLifetime
            // 
            this.updExchangeSubscriptionLifetime.Location = new System.Drawing.Point(10, 142);
            this.updExchangeSubscriptionLifetime.Name = "updExchangeSubscriptionLifetime";
            this.updExchangeSubscriptionLifetime.Size = new System.Drawing.Size(100, 22);
            this.updExchangeSubscriptionLifetime.TabIndex = 8;
            // 
            // txtExchangeDomain
            // 
            this.txtExchangeDomain.Location = new System.Drawing.Point(10, 92);
            this.txtExchangeDomain.Name = "txtExchangeDomain";
            this.txtExchangeDomain.Size = new System.Drawing.Size(100, 22);
            this.txtExchangeDomain.TabIndex = 3;
            this.txtExchangeDomain.WordWrap = false;
            // 
            // lblExchangeDomain
            // 
            this.lblExchangeDomain.AutoSize = true;
            this.lblExchangeDomain.Location = new System.Drawing.Point(7, 72);
            this.lblExchangeDomain.Name = "lblExchangeDomain";
            this.lblExchangeDomain.Size = new System.Drawing.Size(56, 17);
            this.lblExchangeDomain.TabIndex = 2;
            this.lblExchangeDomain.Text = "Domain";
            // 
            // txtExchangePassword
            // 
            this.txtExchangePassword.Location = new System.Drawing.Point(222, 92);
            this.txtExchangePassword.Name = "txtExchangePassword";
            this.txtExchangePassword.PasswordChar = '*';
            this.txtExchangePassword.Size = new System.Drawing.Size(100, 22);
            this.txtExchangePassword.TabIndex = 7;
            this.txtExchangePassword.WordWrap = false;
            // 
            // lblExchangePassword
            // 
            this.lblExchangePassword.AutoSize = true;
            this.lblExchangePassword.Location = new System.Drawing.Point(219, 72);
            this.lblExchangePassword.Name = "lblExchangePassword";
            this.lblExchangePassword.Size = new System.Drawing.Size(69, 17);
            this.lblExchangePassword.TabIndex = 6;
            this.lblExchangePassword.Text = "Password";
            // 
            // txtExchangeUserName
            // 
            this.txtExchangeUserName.Location = new System.Drawing.Point(116, 92);
            this.txtExchangeUserName.Name = "txtExchangeUserName";
            this.txtExchangeUserName.Size = new System.Drawing.Size(100, 22);
            this.txtExchangeUserName.TabIndex = 5;
            this.txtExchangeUserName.WordWrap = false;
            // 
            // lblExchangeUserName
            // 
            this.lblExchangeUserName.AutoSize = true;
            this.lblExchangeUserName.Location = new System.Drawing.Point(113, 72);
            this.lblExchangeUserName.Name = "lblExchangeUserName";
            this.lblExchangeUserName.Size = new System.Drawing.Size(77, 17);
            this.lblExchangeUserName.TabIndex = 4;
            this.lblExchangeUserName.Text = "User name";
            // 
            // lblExchangeUrl
            // 
            this.lblExchangeUrl.AutoSize = true;
            this.lblExchangeUrl.Location = new System.Drawing.Point(7, 22);
            this.lblExchangeUrl.Name = "lblExchangeUrl";
            this.lblExchangeUrl.Size = new System.Drawing.Size(70, 17);
            this.lblExchangeUrl.TabIndex = 0;
            this.lblExchangeUrl.Text = "Server url";
            // 
            // txtExchangeUrl
            // 
            this.txtExchangeUrl.Location = new System.Drawing.Point(10, 42);
            this.txtExchangeUrl.Name = "txtExchangeUrl";
            this.txtExchangeUrl.Size = new System.Drawing.Size(312, 22);
            this.txtExchangeUrl.TabIndex = 1;
            this.txtExchangeUrl.WordWrap = false;
            // 
            // grpMail
            // 
            this.grpMail.Controls.Add(this.lblMailServerType);
            this.grpMail.Controls.Add(this.cboMailServerType);
            this.grpMail.Controls.Add(this.txtMailServerPort);
            this.grpMail.Controls.Add(this.lblMailServerPort);
            this.grpMail.Controls.Add(this.txtMailFolderName);
            this.grpMail.Controls.Add(this.lblMailFolderName);
            this.grpMail.Controls.Add(this.txtMailToAddress);
            this.grpMail.Controls.Add(this.lblMailToAddress);
            this.grpMail.Controls.Add(this.txtMailPassword);
            this.grpMail.Controls.Add(this.lblMailPassword);
            this.grpMail.Controls.Add(this.txtMailUserName);
            this.grpMail.Controls.Add(this.lblMailUserName);
            this.grpMail.Controls.Add(this.txtMailServerName);
            this.grpMail.Controls.Add(this.lblMailServerName);
            this.grpMail.Location = new System.Drawing.Point(353, 12);
            this.grpMail.Name = "grpMail";
            this.grpMail.Size = new System.Drawing.Size(338, 224);
            this.grpMail.TabIndex = 1;
            this.grpMail.TabStop = false;
            this.grpMail.Text = "Mail server";
            // 
            // lblMailServerType
            // 
            this.lblMailServerType.AutoSize = true;
            this.lblMailServerType.Location = new System.Drawing.Point(163, 72);
            this.lblMailServerType.Name = "lblMailServerType";
            this.lblMailServerType.Size = new System.Drawing.Size(81, 17);
            this.lblMailServerType.TabIndex = 4;
            this.lblMailServerType.Text = "Server type";
            // 
            // cboMailServerType
            // 
            this.cboMailServerType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMailServerType.FormattingEnabled = true;
            this.cboMailServerType.ItemHeight = 16;
            this.cboMailServerType.Items.AddRange(new object[] {
            "IMAP",
            "SMTP",
            "SMTPMX"});
            this.cboMailServerType.Location = new System.Drawing.Point(169, 92);
            this.cboMailServerType.Name = "cboMailServerType";
            this.cboMailServerType.Size = new System.Drawing.Size(153, 24);
            this.cboMailServerType.TabIndex = 5;
            this.cboMailServerType.SelectedIndexChanged += new System.EventHandler(this.cboMailServerType_SelectedIndexChanged);
            // 
            // txtMailServerPort
            // 
            this.txtMailServerPort.Location = new System.Drawing.Point(10, 92);
            this.txtMailServerPort.Name = "txtMailServerPort";
            this.txtMailServerPort.Size = new System.Drawing.Size(153, 22);
            this.txtMailServerPort.TabIndex = 3;
            this.txtMailServerPort.WordWrap = false;
            // 
            // lblMailServerPort
            // 
            this.lblMailServerPort.AutoSize = true;
            this.lblMailServerPort.Location = new System.Drawing.Point(7, 72);
            this.lblMailServerPort.Name = "lblMailServerPort";
            this.lblMailServerPort.Size = new System.Drawing.Size(34, 17);
            this.lblMailServerPort.TabIndex = 2;
            this.lblMailServerPort.Text = "Port";
            // 
            // txtMailFolderName
            // 
            this.txtMailFolderName.Location = new System.Drawing.Point(169, 192);
            this.txtMailFolderName.Name = "txtMailFolderName";
            this.txtMailFolderName.Size = new System.Drawing.Size(153, 22);
            this.txtMailFolderName.TabIndex = 13;
            this.txtMailFolderName.WordWrap = false;
            // 
            // lblMailFolderName
            // 
            this.lblMailFolderName.AutoSize = true;
            this.lblMailFolderName.Location = new System.Drawing.Point(166, 172);
            this.lblMailFolderName.Name = "lblMailFolderName";
            this.lblMailFolderName.Size = new System.Drawing.Size(112, 17);
            this.lblMailFolderName.TabIndex = 12;
            this.lblMailFolderName.Text = "Mail folder name";
            // 
            // txtMailToAddress
            // 
            this.txtMailToAddress.Location = new System.Drawing.Point(10, 192);
            this.txtMailToAddress.Name = "txtMailToAddress";
            this.txtMailToAddress.Size = new System.Drawing.Size(153, 22);
            this.txtMailToAddress.TabIndex = 11;
            this.txtMailToAddress.WordWrap = false;
            // 
            // lblMailToAddress
            // 
            this.lblMailToAddress.AutoSize = true;
            this.lblMailToAddress.Location = new System.Drawing.Point(7, 172);
            this.lblMailToAddress.Name = "lblMailToAddress";
            this.lblMailToAddress.Size = new System.Drawing.Size(104, 17);
            this.lblMailToAddress.TabIndex = 10;
            this.lblMailToAddress.Text = "Mail to address";
            // 
            // txtMailPassword
            // 
            this.txtMailPassword.Location = new System.Drawing.Point(169, 142);
            this.txtMailPassword.Name = "txtMailPassword";
            this.txtMailPassword.PasswordChar = '*';
            this.txtMailPassword.Size = new System.Drawing.Size(153, 22);
            this.txtMailPassword.TabIndex = 9;
            this.txtMailPassword.WordWrap = false;
            // 
            // lblMailPassword
            // 
            this.lblMailPassword.AutoSize = true;
            this.lblMailPassword.Location = new System.Drawing.Point(166, 122);
            this.lblMailPassword.Name = "lblMailPassword";
            this.lblMailPassword.Size = new System.Drawing.Size(69, 17);
            this.lblMailPassword.TabIndex = 8;
            this.lblMailPassword.Text = "Password";
            // 
            // txtMailUserName
            // 
            this.txtMailUserName.Location = new System.Drawing.Point(10, 142);
            this.txtMailUserName.Name = "txtMailUserName";
            this.txtMailUserName.Size = new System.Drawing.Size(153, 22);
            this.txtMailUserName.TabIndex = 7;
            this.txtMailUserName.WordWrap = false;
            // 
            // lblMailUserName
            // 
            this.lblMailUserName.AutoSize = true;
            this.lblMailUserName.Location = new System.Drawing.Point(7, 122);
            this.lblMailUserName.Name = "lblMailUserName";
            this.lblMailUserName.Size = new System.Drawing.Size(77, 17);
            this.lblMailUserName.TabIndex = 6;
            this.lblMailUserName.Text = "User name";
            // 
            // txtMailServerName
            // 
            this.txtMailServerName.Location = new System.Drawing.Point(10, 42);
            this.txtMailServerName.Name = "txtMailServerName";
            this.txtMailServerName.Size = new System.Drawing.Size(312, 22);
            this.txtMailServerName.TabIndex = 1;
            this.txtMailServerName.WordWrap = false;
            // 
            // lblMailServerName
            // 
            this.lblMailServerName.AutoSize = true;
            this.lblMailServerName.Location = new System.Drawing.Point(7, 22);
            this.lblMailServerName.Name = "lblMailServerName";
            this.lblMailServerName.Size = new System.Drawing.Size(89, 17);
            this.lblMailServerName.TabIndex = 0;
            this.lblMailServerName.Text = "Server name";
            // 
            // grpLog
            // 
            this.grpLog.Controls.Add(this.chkLogClearOnStartup);
            this.grpLog.Controls.Add(this.updLogLevel);
            this.grpLog.Controls.Add(this.lblLogLevel);
            this.grpLog.Controls.Add(this.btnLogFileName);
            this.grpLog.Controls.Add(this.txtLogFileName);
            this.grpLog.Controls.Add(this.lblLogFileName);
            this.grpLog.Location = new System.Drawing.Point(10, 368);
            this.grpLog.Name = "grpLog";
            this.grpLog.Size = new System.Drawing.Size(681, 84);
            this.grpLog.TabIndex = 5;
            this.grpLog.TabStop = false;
            this.grpLog.Text = "Log";
            // 
            // chkLogClearOnStartup
            // 
            this.chkLogClearOnStartup.AutoSize = true;
            this.chkLogClearOnStartup.Location = new System.Drawing.Point(509, 46);
            this.chkLogClearOnStartup.Name = "chkLogClearOnStartup";
            this.chkLogClearOnStartup.Size = new System.Drawing.Size(152, 21);
            this.chkLogClearOnStartup.TabIndex = 5;
            this.chkLogClearOnStartup.Text = "log clear on startup";
            this.chkLogClearOnStartup.UseVisualStyleBackColor = true;
            // 
            // updLogLevel
            // 
            this.updLogLevel.Location = new System.Drawing.Point(396, 45);
            this.updLogLevel.Name = "updLogLevel";
            this.updLogLevel.Size = new System.Drawing.Size(100, 22);
            this.updLogLevel.TabIndex = 4;
            // 
            // lblLogLevel
            // 
            this.lblLogLevel.AutoSize = true;
            this.lblLogLevel.Location = new System.Drawing.Point(393, 25);
            this.lblLogLevel.Name = "lblLogLevel";
            this.lblLogLevel.Size = new System.Drawing.Size(65, 17);
            this.lblLogLevel.TabIndex = 3;
            this.lblLogLevel.Text = "Log level";
            // 
            // btnLogFileName
            // 
            this.btnLogFileName.Font = new System.Drawing.Font("Tahoma", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnLogFileName.Location = new System.Drawing.Point(354, 41);
            this.btnLogFileName.Margin = new System.Windows.Forms.Padding(0);
            this.btnLogFileName.Name = "btnLogFileName";
            this.btnLogFileName.Size = new System.Drawing.Size(30, 26);
            this.btnLogFileName.TabIndex = 2;
            this.btnLogFileName.Text = "...";
            this.btnLogFileName.UseVisualStyleBackColor = true;
            this.btnLogFileName.Click += new System.EventHandler(this.btnLogFileName_Click);
            // 
            // txtLogFileName
            // 
            this.txtLogFileName.Location = new System.Drawing.Point(10, 43);
            this.txtLogFileName.Name = "txtLogFileName";
            this.txtLogFileName.Size = new System.Drawing.Size(341, 22);
            this.txtLogFileName.TabIndex = 1;
            // 
            // lblLogFileName
            // 
            this.lblLogFileName.AutoSize = true;
            this.lblLogFileName.Location = new System.Drawing.Point(7, 23);
            this.lblLogFileName.Name = "lblLogFileName";
            this.lblLogFileName.Size = new System.Drawing.Size(93, 17);
            this.lblLogFileName.TabIndex = 0;
            this.lblLogFileName.Text = "Log file name";
            // 
            // grpOnlineSync
            // 
            this.grpOnlineSync.Controls.Add(this.chkSyncEnabled);
            this.grpOnlineSync.Controls.Add(this.lblMaxDaysToSync);
            this.grpOnlineSync.Controls.Add(this.updMaxDaysToSync);
            this.grpOnlineSync.Location = new System.Drawing.Point(10, 242);
            this.grpOnlineSync.Name = "grpOnlineSync";
            this.grpOnlineSync.Size = new System.Drawing.Size(338, 54);
            this.grpOnlineSync.TabIndex = 2;
            this.grpOnlineSync.TabStop = false;
            this.grpOnlineSync.Text = "Online synchronization";
            // 
            // chkSyncEnabled
            // 
            this.chkSyncEnabled.AutoSize = true;
            this.chkSyncEnabled.Location = new System.Drawing.Point(10, 22);
            this.chkSyncEnabled.Name = "chkSyncEnabled";
            this.chkSyncEnabled.Size = new System.Drawing.Size(82, 21);
            this.chkSyncEnabled.TabIndex = 0;
            this.chkSyncEnabled.Text = "Enabled";
            this.chkSyncEnabled.UseVisualStyleBackColor = true;
            this.chkSyncEnabled.CheckedChanged += new System.EventHandler(this.chkSyncEnabled_CheckedChanged);
            // 
            // lblMaxDaysToSync
            // 
            this.lblMaxDaysToSync.AutoSize = true;
            this.lblMaxDaysToSync.Location = new System.Drawing.Point(156, 23);
            this.lblMaxDaysToSync.Name = "lblMaxDaysToSync";
            this.lblMaxDaysToSync.Size = new System.Drawing.Size(168, 17);
            this.lblMaxDaysToSync.TabIndex = 2;
            this.lblMaxDaysToSync.Text = "max days to sync on start";
            // 
            // updMaxDaysToSync
            // 
            this.updMaxDaysToSync.Location = new System.Drawing.Point(98, 21);
            this.updMaxDaysToSync.Name = "updMaxDaysToSync";
            this.updMaxDaysToSync.Size = new System.Drawing.Size(57, 22);
            this.updMaxDaysToSync.TabIndex = 1;
            // 
            // grpOfflineSync
            // 
            this.grpOfflineSync.Controls.Add(this.lblOfflineSyncFromTo);
            this.grpOfflineSync.Controls.Add(this.btnOfflineSyncNow);
            this.grpOfflineSync.Controls.Add(this.dtpOffilneSyncTo);
            this.grpOfflineSync.Controls.Add(this.dtpOffilneSyncFrom);
            this.grpOfflineSync.Location = new System.Drawing.Point(10, 302);
            this.grpOfflineSync.Name = "grpOfflineSync";
            this.grpOfflineSync.Size = new System.Drawing.Size(338, 60);
            this.grpOfflineSync.TabIndex = 3;
            this.grpOfflineSync.TabStop = false;
            this.grpOfflineSync.Text = "Offline synchronization";
            // 
            // lblOfflineSyncFromTo
            // 
            this.lblOfflineSyncFromTo.AutoSize = true;
            this.lblOfflineSyncFromTo.Font = new System.Drawing.Font("Courier New", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblOfflineSyncFromTo.Location = new System.Drawing.Point(115, 25);
            this.lblOfflineSyncFromTo.Margin = new System.Windows.Forms.Padding(0);
            this.lblOfflineSyncFromTo.Name = "lblOfflineSyncFromTo";
            this.lblOfflineSyncFromTo.Size = new System.Drawing.Size(19, 20);
            this.lblOfflineSyncFromTo.TabIndex = 1;
            this.lblOfflineSyncFromTo.Text = "-";
            this.lblOfflineSyncFromTo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnOfflineSyncNow
            // 
            this.btnOfflineSyncNow.Location = new System.Drawing.Point(243, 22);
            this.btnOfflineSyncNow.Name = "btnOfflineSyncNow";
            this.btnOfflineSyncNow.Size = new System.Drawing.Size(82, 26);
            this.btnOfflineSyncNow.TabIndex = 3;
            this.btnOfflineSyncNow.Text = "Sync now";
            this.btnOfflineSyncNow.UseVisualStyleBackColor = true;
            this.btnOfflineSyncNow.Click += new System.EventHandler(this.btnOfflineSyncNow_Click);
            // 
            // dtpOffilneSyncTo
            // 
            this.dtpOffilneSyncTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpOffilneSyncTo.Location = new System.Drawing.Point(137, 24);
            this.dtpOffilneSyncTo.Name = "dtpOffilneSyncTo";
            this.dtpOffilneSyncTo.Size = new System.Drawing.Size(100, 22);
            this.dtpOffilneSyncTo.TabIndex = 2;
            // 
            // dtpOffilneSyncFrom
            // 
            this.dtpOffilneSyncFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpOffilneSyncFrom.Location = new System.Drawing.Point(13, 24);
            this.dtpOffilneSyncFrom.Name = "dtpOffilneSyncFrom";
            this.dtpOffilneSyncFrom.Size = new System.Drawing.Size(100, 22);
            this.dtpOffilneSyncFrom.TabIndex = 0;
            this.dtpOffilneSyncFrom.Value = new System.DateTime(2013, 11, 28, 13, 39, 39, 0);
            // 
            // chkAutoRun
            // 
            this.chkAutoRun.AutoSize = true;
            this.chkAutoRun.Location = new System.Drawing.Point(363, 261);
            this.chkAutoRun.Name = "chkAutoRun";
            this.chkAutoRun.Size = new System.Drawing.Size(180, 21);
            this.chkAutoRun.TabIndex = 4;
            this.chkAutoRun.Text = "Run at Windows startup";
            this.chkAutoRun.UseVisualStyleBackColor = true;
            // 
            // chkExchangeAcceptInvalidCertificate
            // 
            this.chkExchangeAcceptInvalidCertificate.AutoSize = true;
            this.chkExchangeAcceptInvalidCertificate.Location = new System.Drawing.Point(10, 182);
            this.chkExchangeAcceptInvalidCertificate.Name = "chkExchangeAcceptInvalidCertificate";
            this.chkExchangeAcceptInvalidCertificate.Size = new System.Drawing.Size(219, 21);
            this.chkExchangeAcceptInvalidCertificate.TabIndex = 10;
            this.chkExchangeAcceptInvalidCertificate.Text = "Accept invalid SSL certificates";
            this.chkExchangeAcceptInvalidCertificate.UseVisualStyleBackColor = true;
            // 
            // OptionsForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(704, 521);
            this.Controls.Add(this.chkAutoRun);
            this.Controls.Add(this.grpOfflineSync);
            this.Controls.Add(this.grpOnlineSync);
            this.Controls.Add(this.grpLog);
            this.Controls.Add(this.grpMail);
            this.Controls.Add(this.grpExchange);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lblBevel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "OptionsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Options";
            this.grpExchange.ResumeLayout(false);
            this.grpExchange.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updExchangeSubscriptionLifetime)).EndInit();
            this.grpMail.ResumeLayout(false);
            this.grpMail.PerformLayout();
            this.grpLog.ResumeLayout(false);
            this.grpLog.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updLogLevel)).EndInit();
            this.grpOnlineSync.ResumeLayout(false);
            this.grpOnlineSync.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updMaxDaysToSync)).EndInit();
            this.grpOfflineSync.ResumeLayout(false);
            this.grpOfflineSync.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblBevel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox grpExchange;
        private System.Windows.Forms.Label lblExchangeSubscriptionLifetime;
        private System.Windows.Forms.NumericUpDown updExchangeSubscriptionLifetime;
        private System.Windows.Forms.TextBox txtExchangeDomain;
        private System.Windows.Forms.Label lblExchangeDomain;
        private System.Windows.Forms.TextBox txtExchangePassword;
        private System.Windows.Forms.Label lblExchangePassword;
        private System.Windows.Forms.TextBox txtExchangeUserName;
        private System.Windows.Forms.Label lblExchangeUserName;
        private System.Windows.Forms.Label lblExchangeUrl;
        private System.Windows.Forms.TextBox txtExchangeUrl;
        private System.Windows.Forms.GroupBox grpMail;
        private System.Windows.Forms.GroupBox grpLog;
        private System.Windows.Forms.NumericUpDown updLogLevel;
        private System.Windows.Forms.Label lblLogLevel;
        private System.Windows.Forms.Button btnLogFileName;
        private System.Windows.Forms.TextBox txtLogFileName;
        private System.Windows.Forms.Label lblLogFileName;
        private System.Windows.Forms.OpenFileDialog dlgFileOpen;
        private System.Windows.Forms.CheckBox chkLogClearOnStartup;
        private System.Windows.Forms.ComboBox cboMailServerType;
        private System.Windows.Forms.TextBox txtMailServerPort;
        private System.Windows.Forms.Label lblMailServerPort;
        private System.Windows.Forms.TextBox txtMailFolderName;
        private System.Windows.Forms.Label lblMailFolderName;
        private System.Windows.Forms.TextBox txtMailToAddress;
        private System.Windows.Forms.Label lblMailToAddress;
        private System.Windows.Forms.TextBox txtMailPassword;
        private System.Windows.Forms.Label lblMailPassword;
        private System.Windows.Forms.TextBox txtMailUserName;
        private System.Windows.Forms.Label lblMailUserName;
        private System.Windows.Forms.TextBox txtMailServerName;
        private System.Windows.Forms.Label lblMailServerName;
        private System.Windows.Forms.Label lblMailServerType;
        private System.Windows.Forms.GroupBox grpOnlineSync;
        private System.Windows.Forms.Label lblMaxDaysToSync;
        private System.Windows.Forms.NumericUpDown updMaxDaysToSync;
        private System.Windows.Forms.CheckBox chkSyncEnabled;
        private System.Windows.Forms.GroupBox grpOfflineSync;
        private System.Windows.Forms.Label lblOfflineSyncFromTo;
        private System.Windows.Forms.Button btnOfflineSyncNow;
        private System.Windows.Forms.DateTimePicker dtpOffilneSyncTo;
        private System.Windows.Forms.DateTimePicker dtpOffilneSyncFrom;
        private System.Windows.Forms.CheckBox chkAutoRun;
        private System.Windows.Forms.CheckBox chkExchangeAcceptInvalidCertificate;
    }
}