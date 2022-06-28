namespace The_Guild_2_Network_Helper
{
    partial class hostForm
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Button dropClientButton;
            System.Windows.Forms.Label clientIPLabel;
            System.Windows.Forms.ProgressBar saveTransferBar;
            System.Windows.Forms.Label clientUserNameLabel;
            System.Windows.Forms.PictureBox connectionIndicator;
            System.Windows.Forms.PictureBox transferIndicator;
            System.Windows.Forms.PictureBox fileVerifiedIndicator;
            System.Windows.Forms.CheckBox autoRestartCheckBox;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(hostForm));
            this.clientsPanel = new System.Windows.Forms.Panel();
            this.portLabel = new System.Windows.Forms.Label();
            this.saveFolderLabel = new System.Windows.Forms.Label();
            this.maxClientsLabel = new System.Windows.Forms.Label();
            this.hostStartStopButton = new System.Windows.Forms.Button();
            this.upnpCheckBox = new System.Windows.Forms.CheckBox();
            this.hostToolTips = new System.Windows.Forms.ToolTip(this.components);
            this.browseDirectoryButton = new System.Windows.Forms.Button();
            this.maxClientsBox = new System.Windows.Forms.NumericUpDown();
            this.hostPortBox = new System.Windows.Forms.NumericUpDown();
            this.outOfSyncButton = new System.Windows.Forms.Button();
            this.directoryPathBox = new System.Windows.Forms.TextBox();
            this.folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            dropClientButton = new System.Windows.Forms.Button();
            clientIPLabel = new System.Windows.Forms.Label();
            saveTransferBar = new System.Windows.Forms.ProgressBar();
            clientUserNameLabel = new System.Windows.Forms.Label();
            connectionIndicator = new System.Windows.Forms.PictureBox();
            transferIndicator = new System.Windows.Forms.PictureBox();
            fileVerifiedIndicator = new System.Windows.Forms.PictureBox();
            autoRestartCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(connectionIndicator)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(transferIndicator)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(fileVerifiedIndicator)).BeginInit();
            this.clientsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxClientsBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hostPortBox)).BeginInit();
            this.SuspendLayout();
            // 
            // dropClientButton
            // 
            dropClientButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dropClientButton.BackColor = System.Drawing.Color.Red;
            dropClientButton.BackgroundImage = global::The_Guild_2_Network_Helper.Properties.Resources.Disconnect_9957;
            dropClientButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            dropClientButton.Enabled = false;
            dropClientButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            dropClientButton.Location = new System.Drawing.Point(156, 11);
            dropClientButton.Name = "dropClientButton";
            dropClientButton.Size = new System.Drawing.Size(20, 20);
            dropClientButton.TabIndex = 0;
            dropClientButton.TabStop = false;
            this.hostToolTips.SetToolTip(dropClientButton, "Drops this client from your connection.");
            dropClientButton.UseVisualStyleBackColor = false;
            dropClientButton.Visible = false;
            // 
            // clientIPLabel
            // 
            clientIPLabel.AutoSize = true;
            clientIPLabel.Enabled = false;
            clientIPLabel.Location = new System.Drawing.Point(18, 24);
            clientIPLabel.Name = "clientIPLabel";
            clientIPLabel.Size = new System.Drawing.Size(104, 13);
            clientIPLabel.TabIndex = 0;
            clientIPLabel.Text = "IP: 888.888.888.888";
            clientIPLabel.Visible = false;
            // 
            // saveTransferBar
            // 
            saveTransferBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            saveTransferBar.Enabled = false;
            saveTransferBar.Location = new System.Drawing.Point(5, 40);
            saveTransferBar.Name = "saveTransferBar";
            saveTransferBar.Size = new System.Drawing.Size(233, 15);
            saveTransferBar.TabIndex = 0;
            this.hostToolTips.SetToolTip(saveTransferBar, "Save transfer progress");
            saveTransferBar.Visible = false;
            // 
            // clientUserNameLabel
            // 
            clientUserNameLabel.AutoSize = true;
            clientUserNameLabel.Enabled = false;
            clientUserNameLabel.Location = new System.Drawing.Point(2, 8);
            clientUserNameLabel.Name = "clientUserNameLabel";
            clientUserNameLabel.Size = new System.Drawing.Size(143, 13);
            clientUserNameLabel.TabIndex = 0;
            clientUserNameLabel.Text = "Client: AgentmassAgentmass";
            clientUserNameLabel.Visible = false;
            // 
            // connectionIndicator
            // 
            connectionIndicator.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            connectionIndicator.BackColor = System.Drawing.Color.Red;
            connectionIndicator.Enabled = false;
            connectionIndicator.Location = new System.Drawing.Point(241, 40);
            connectionIndicator.Name = "connectionIndicator";
            connectionIndicator.Size = new System.Drawing.Size(15, 15);
            connectionIndicator.TabIndex = 4;
            connectionIndicator.TabStop = false;
            this.hostToolTips.SetToolTip(connectionIndicator, "File Transfer Connection established");
            connectionIndicator.Visible = false;
            // 
            // transferIndicator
            // 
            transferIndicator.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            transferIndicator.BackColor = System.Drawing.Color.Red;
            transferIndicator.Enabled = false;
            transferIndicator.Location = new System.Drawing.Point(258, 40);
            transferIndicator.Name = "transferIndicator";
            transferIndicator.Size = new System.Drawing.Size(15, 15);
            transferIndicator.TabIndex = 4;
            transferIndicator.TabStop = false;
            this.hostToolTips.SetToolTip(transferIndicator, "Save transfer started");
            transferIndicator.Visible = false;
            // 
            // fileVerifiedIndicator
            // 
            fileVerifiedIndicator.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            fileVerifiedIndicator.BackColor = System.Drawing.Color.Red;
            fileVerifiedIndicator.Enabled = false;
            fileVerifiedIndicator.Location = new System.Drawing.Point(275, 40);
            fileVerifiedIndicator.Name = "fileVerifiedIndicator";
            fileVerifiedIndicator.Size = new System.Drawing.Size(15, 15);
            fileVerifiedIndicator.TabIndex = 4;
            fileVerifiedIndicator.TabStop = false;
            this.hostToolTips.SetToolTip(fileVerifiedIndicator, "File verified");
            fileVerifiedIndicator.Visible = false;
            // 
            // clientsPanel
            // 
            this.clientsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clientsPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.clientsPanel.Controls.Add(dropClientButton);
            this.clientsPanel.Controls.Add(clientIPLabel);
            this.clientsPanel.Controls.Add(saveTransferBar);
            this.clientsPanel.Controls.Add(clientUserNameLabel);
            this.clientsPanel.Controls.Add(connectionIndicator);
            this.clientsPanel.Controls.Add(transferIndicator);
            this.clientsPanel.Controls.Add(fileVerifiedIndicator);
            this.clientsPanel.Location = new System.Drawing.Point(8, 8);
            this.clientsPanel.Name = "clientsPanel";
            this.clientsPanel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.clientsPanel.Size = new System.Drawing.Size(298, 226);
            this.clientsPanel.TabIndex = 0;
            // 
            // portLabel
            // 
            this.portLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.portLabel.AutoSize = true;
            this.portLabel.Location = new System.Drawing.Point(46, 241);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(29, 13);
            this.portLabel.TabIndex = 1;
            this.portLabel.Text = "Port:";
            // 
            // saveFolderLabel
            // 
            this.saveFolderLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.saveFolderLabel.AutoSize = true;
            this.saveFolderLabel.Location = new System.Drawing.Point(8, 262);
            this.saveFolderLabel.Name = "saveFolderLabel";
            this.saveFolderLabel.Size = new System.Drawing.Size(67, 13);
            this.saveFolderLabel.TabIndex = 3;
            this.saveFolderLabel.Text = "Save Folder:";
            // 
            // maxClientsLabel
            // 
            this.maxClientsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.maxClientsLabel.AutoSize = true;
            this.maxClientsLabel.Location = new System.Drawing.Point(11, 283);
            this.maxClientsLabel.Name = "maxClientsLabel";
            this.maxClientsLabel.Size = new System.Drawing.Size(64, 13);
            this.maxClientsLabel.TabIndex = 4;
            this.maxClientsLabel.Text = "Max Clients:";
            // 
            // hostStartStopButton
            // 
            this.hostStartStopButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.hostStartStopButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.hostStartStopButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.hostStartStopButton.Location = new System.Drawing.Point(225, 302);
            this.hostStartStopButton.Name = "hostStartStopButton";
            this.hostStartStopButton.Size = new System.Drawing.Size(80, 31);
            this.hostStartStopButton.TabIndex = 4;
            this.hostStartStopButton.Text = "Start Server";
            this.hostStartStopButton.UseVisualStyleBackColor = false;
            this.hostStartStopButton.Click += new System.EventHandler(this.hostStartStopButton_Click);
            // 
            // upnpCheckBox
            // 
            this.upnpCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.upnpCheckBox.AutoSize = true;
            this.upnpCheckBox.Location = new System.Drawing.Point(35, 302);
            this.upnpCheckBox.Name = "upnpCheckBox";
            this.upnpCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.upnpCheckBox.Size = new System.Drawing.Size(57, 17);
            this.upnpCheckBox.TabIndex = 0;
            this.upnpCheckBox.TabStop = false;
            this.upnpCheckBox.Text = ":UPnP";
            this.hostToolTips.SetToolTip(this.upnpCheckBox, "Enabling this will attempt to use UPnP to portforward. If you don\'t want to portf" +
        "orward, leave this checked. This may not work on all routers.");
            this.upnpCheckBox.UseVisualStyleBackColor = true;
            // 
            // hostToolTips
            // 
            this.hostToolTips.AutomaticDelay = 0;
            this.hostToolTips.AutoPopDelay = 30000;
            this.hostToolTips.InitialDelay = 1;
            this.hostToolTips.ReshowDelay = 0;
            this.hostToolTips.UseAnimation = false;
            this.hostToolTips.UseFading = false;
            // 
            // browseDirectoryButton
            // 
            this.browseDirectoryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.browseDirectoryButton.Image = global::The_Guild_2_Network_Helper.Properties.Resources.folder_Open_16xLG;
            this.browseDirectoryButton.Location = new System.Drawing.Point(281, 258);
            this.browseDirectoryButton.Name = "browseDirectoryButton";
            this.browseDirectoryButton.Size = new System.Drawing.Size(25, 22);
            this.browseDirectoryButton.TabIndex = 0;
            this.browseDirectoryButton.TabStop = false;
            this.hostToolTips.SetToolTip(this.browseDirectoryButton, "Don\'t change if already set! Set this path to your game\'s savegames folder. Examp" +
        "le: \"C:\\Program Files (x86)\\Steam\\steamapps\\common\\The Guild 2 Renaissance\\saveg" +
        "ames\"");
            this.browseDirectoryButton.UseVisualStyleBackColor = true;
            this.browseDirectoryButton.Click += new System.EventHandler(this.browseDirectoryButton_Click);
            // 
            // maxClientsBox
            // 
            this.maxClientsBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.maxClientsBox.Location = new System.Drawing.Point(78, 280);
            this.maxClientsBox.Maximum = new decimal(new int[] {
            11,
            0,
            0,
            0});
            this.maxClientsBox.Name = "maxClientsBox";
            this.maxClientsBox.Size = new System.Drawing.Size(227, 20);
            this.maxClientsBox.TabIndex = 3;
            this.hostToolTips.SetToolTip(this.maxClientsBox, "The number of people you will be playing with.");
            // 
            // hostPortBox
            // 
            this.hostPortBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hostPortBox.Location = new System.Drawing.Point(78, 238);
            this.hostPortBox.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.hostPortBox.Name = "hostPortBox";
            this.hostPortBox.Size = new System.Drawing.Size(227, 20);
            this.hostPortBox.TabIndex = 1;
            this.hostToolTips.SetToolTip(this.hostPortBox, "Enter any valid unused port.");
            // 
            // outOfSyncButton
            // 
            this.outOfSyncButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.outOfSyncButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.outOfSyncButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.outOfSyncButton.Location = new System.Drawing.Point(139, 302);
            this.outOfSyncButton.Name = "outOfSyncButton";
            this.outOfSyncButton.Size = new System.Drawing.Size(80, 31);
            this.outOfSyncButton.TabIndex = 0;
            this.outOfSyncButton.TabStop = false;
            this.outOfSyncButton.Text = "Out of Sync!";
            this.hostToolTips.SetToolTip(this.outOfSyncButton, "If the game goes out of sync, press this button to start the save transfer.");
            this.outOfSyncButton.UseVisualStyleBackColor = false;
            this.outOfSyncButton.Visible = false;
            this.outOfSyncButton.Click += new System.EventHandler(this.outOfSyncButton_Click);
            // 
            // autoRestartCheckBox
            // 
            autoRestartCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            autoRestartCheckBox.AutoSize = true;
            autoRestartCheckBox.Enabled = false;
            autoRestartCheckBox.Location = new System.Drawing.Point(13, 314);
            autoRestartCheckBox.Name = "autoRestartCheckBox";
            autoRestartCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            autoRestartCheckBox.Size = new System.Drawing.Size(80, 17);
            autoRestartCheckBox.TabIndex = 0;
            autoRestartCheckBox.TabStop = false;
            autoRestartCheckBox.Text = ":Autorestart";
            this.hostToolTips.SetToolTip(autoRestartCheckBox, "Automatically restarts the game when the save transfer completes. If unchecked, y" +
        "ou must manually restart the game.");
            autoRestartCheckBox.UseVisualStyleBackColor = true;
            autoRestartCheckBox.Visible = false;
            // 
            // directoryPathBox
            // 
            this.directoryPathBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.directoryPathBox.Location = new System.Drawing.Point(78, 259);
            this.directoryPathBox.Name = "directoryPathBox";
            this.directoryPathBox.Size = new System.Drawing.Size(203, 20);
            this.directoryPathBox.TabIndex = 2;
            // 
            // folderDialog
            // 
            this.folderDialog.SelectedPath = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\The Guild 2 Renaissance\\savegames";
            // 
            // hostForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(313, 339);
            this.Controls.Add(autoRestartCheckBox);
            this.Controls.Add(this.hostPortBox);
            this.Controls.Add(this.maxClientsBox);
            this.Controls.Add(this.browseDirectoryButton);
            this.Controls.Add(this.upnpCheckBox);
            this.Controls.Add(this.maxClientsLabel);
            this.Controls.Add(this.saveFolderLabel);
            this.Controls.Add(this.directoryPathBox);
            this.Controls.Add(this.portLabel);
            this.Controls.Add(this.clientsPanel);
            this.Controls.Add(this.outOfSyncButton);
            this.Controls.Add(this.hostStartStopButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(658, 765);
            this.MinimumSize = new System.Drawing.Size(329, 378);
            this.Name = "hostForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Host";
            ((System.ComponentModel.ISupportInitialize)(connectionIndicator)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(transferIndicator)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(fileVerifiedIndicator)).EndInit();
            this.clientsPanel.ResumeLayout(false);
            this.clientsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxClientsBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hostPortBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel clientsPanel;
        private System.Windows.Forms.Label portLabel;
        private System.Windows.Forms.Label saveFolderLabel;
        private System.Windows.Forms.Label maxClientsLabel;
        private System.Windows.Forms.Button hostStartStopButton;
        private System.Windows.Forms.CheckBox upnpCheckBox;
        private System.Windows.Forms.ToolTip hostToolTips;
        private System.Windows.Forms.TextBox directoryPathBox;
        private System.Windows.Forms.Button browseDirectoryButton;
        private System.Windows.Forms.NumericUpDown maxClientsBox;
        private System.Windows.Forms.NumericUpDown hostPortBox;
        public System.Windows.Forms.Button outOfSyncButton;
        private System.Windows.Forms.FolderBrowserDialog folderDialog;
    }
}

