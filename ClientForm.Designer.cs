namespace The_Guild_2_Network_Helper
{
    partial class clientForm
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
        public void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.CheckBox autoRestartCheckBox;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(clientForm));
            this.clientInfoBox = new System.Windows.Forms.RichTextBox();
            this.hostIPLabel = new System.Windows.Forms.Label();
            this.hostPortBox = new System.Windows.Forms.NumericUpDown();
            this.hostIPBox = new System.Windows.Forms.TextBox();
            this.hostPortLabel = new System.Windows.Forms.Label();
            this.clientToolTips = new System.Windows.Forms.ToolTip(this.components);
            this.usernameBox = new System.Windows.Forms.TextBox();
            this.clientStartStopButton = new System.Windows.Forms.Button();
            this.browseDirectoryButton = new System.Windows.Forms.Button();
            this.saveTransferBar = new System.Windows.Forms.ProgressBar();
            this.clientPortBox = new System.Windows.Forms.NumericUpDown();
            this.saveFolderLabel = new System.Windows.Forms.Label();
            this.directoryPathBox = new System.Windows.Forms.TextBox();
            this.folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.usernameLabel = new System.Windows.Forms.Label();
            this.clientPortLabel = new System.Windows.Forms.Label();
            autoRestartCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.hostPortBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clientPortBox)).BeginInit();
            this.SuspendLayout();
            // 
            // clientInfoBox
            // 
            this.clientInfoBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clientInfoBox.Location = new System.Drawing.Point(8, 8);
            this.clientInfoBox.Name = "clientInfoBox";
            this.clientInfoBox.ReadOnly = true;
            this.clientInfoBox.Size = new System.Drawing.Size(239, 168);
            this.clientInfoBox.TabIndex = 0;
            this.clientInfoBox.TabStop = false;
            this.clientInfoBox.Text = "Fill out the settings below and press Start when the host is ready.";
            // 
            // hostIPLabel
            // 
            this.hostIPLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.hostIPLabel.AutoSize = true;
            this.hostIPLabel.Location = new System.Drawing.Point(25, 202);
            this.hostIPLabel.Name = "hostIPLabel";
            this.hostIPLabel.Size = new System.Drawing.Size(45, 13);
            this.hostIPLabel.TabIndex = 0;
            this.hostIPLabel.Text = "Host IP:";
            // 
            // hostPortBox
            // 
            this.hostPortBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hostPortBox.Location = new System.Drawing.Point(71, 220);
            this.hostPortBox.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.hostPortBox.Name = "hostPortBox";
            this.hostPortBox.Size = new System.Drawing.Size(176, 20);
            this.hostPortBox.TabIndex = 2;
            this.clientToolTips.SetToolTip(this.hostPortBox, "The Host\'s port");
            // 
            // hostIPBox
            // 
            this.hostIPBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hostIPBox.Location = new System.Drawing.Point(71, 199);
            this.hostIPBox.Name = "hostIPBox";
            this.hostIPBox.Size = new System.Drawing.Size(176, 20);
            this.hostIPBox.TabIndex = 1;
            this.clientToolTips.SetToolTip(this.hostIPBox, "The Host\'s IP address");
            // 
            // hostPortLabel
            // 
            this.hostPortLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.hostPortLabel.AutoSize = true;
            this.hostPortLabel.Location = new System.Drawing.Point(16, 223);
            this.hostPortLabel.Name = "hostPortLabel";
            this.hostPortLabel.Size = new System.Drawing.Size(54, 13);
            this.hostPortLabel.TabIndex = 0;
            this.hostPortLabel.Text = "Host Port:";
            // 
            // clientToolTips
            // 
            this.clientToolTips.AutomaticDelay = 0;
            this.clientToolTips.AutoPopDelay = 30000;
            this.clientToolTips.InitialDelay = 1;
            this.clientToolTips.ReshowDelay = 0;
            this.clientToolTips.ShowAlways = true;
            this.clientToolTips.Tag = "";
            this.clientToolTips.UseAnimation = false;
            this.clientToolTips.UseFading = false;
            // 
            // usernameBox
            // 
            this.usernameBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.usernameBox.Location = new System.Drawing.Point(71, 283);
            this.usernameBox.Name = "usernameBox";
            this.usernameBox.Size = new System.Drawing.Size(176, 20);
            this.usernameBox.TabIndex = 5;
            this.clientToolTips.SetToolTip(this.usernameBox, "Choose any username. This is used so that the host knows who\'s trying to connect " +
        "with them.");
            // 
            // clientStartStopButton
            // 
            this.clientStartStopButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.clientStartStopButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.clientStartStopButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clientStartStopButton.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.clientStartStopButton.Location = new System.Drawing.Point(142, 307);
            this.clientStartStopButton.Name = "clientStartStopButton";
            this.clientStartStopButton.Size = new System.Drawing.Size(105, 31);
            this.clientStartStopButton.TabIndex = 6;
            this.clientStartStopButton.Text = "Connect";
            this.clientToolTips.SetToolTip(this.clientStartStopButton, "Connect to host");
            this.clientStartStopButton.UseVisualStyleBackColor = false;
            this.clientStartStopButton.Click += new System.EventHandler(this.clientStartStopButton_Click);
            // 
            // autoRestartCheckBox
            // 
            autoRestartCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            autoRestartCheckBox.AutoSize = true;
            autoRestartCheckBox.Enabled = false;
            autoRestartCheckBox.Location = new System.Drawing.Point(6, 306);
            autoRestartCheckBox.Name = "autoRestartCheckBox";
            autoRestartCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            autoRestartCheckBox.Size = new System.Drawing.Size(80, 17);
            autoRestartCheckBox.TabIndex = 0;
            autoRestartCheckBox.TabStop = false;
            autoRestartCheckBox.Text = ":Autorestart";
            this.clientToolTips.SetToolTip(autoRestartCheckBox, "Automatically restarts the game when the save transfer completes. If unchecked, y" +
        "ou must manually restart the game.");
            autoRestartCheckBox.UseVisualStyleBackColor = true;
            autoRestartCheckBox.Visible = false;
            // 
            // browseDirectoryButton
            // 
            this.browseDirectoryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.browseDirectoryButton.Image = global::The_Guild_2_Network_Helper.Properties.Resources.folder_Open_16xLG;
            this.browseDirectoryButton.Location = new System.Drawing.Point(223, 261);
            this.browseDirectoryButton.Name = "browseDirectoryButton";
            this.browseDirectoryButton.Size = new System.Drawing.Size(25, 22);
            this.browseDirectoryButton.TabIndex = 0;
            this.browseDirectoryButton.TabStop = false;
            this.clientToolTips.SetToolTip(this.browseDirectoryButton, "Don\'t change if already set! Set this path to your game\'s savegames folder. Examp" +
        "le: \"C:\\Program Files (x86)\\Steam\\steamapps\\common\\The Guild 2 Renaissance\\saveg" +
        "ames\"");
            this.browseDirectoryButton.UseVisualStyleBackColor = true;
            this.browseDirectoryButton.Click += new System.EventHandler(this.browseDirectoryButton_Click);
            // 
            // saveTransferBar
            // 
            this.saveTransferBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.saveTransferBar.Location = new System.Drawing.Point(8, 180);
            this.saveTransferBar.Name = "saveTransferBar";
            this.saveTransferBar.Size = new System.Drawing.Size(239, 15);
            this.saveTransferBar.TabIndex = 0;
            this.clientToolTips.SetToolTip(this.saveTransferBar, "Save Transfer Progress");
            // 
            // clientPortBox
            // 
            this.clientPortBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clientPortBox.Location = new System.Drawing.Point(71, 241);
            this.clientPortBox.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.clientPortBox.Name = "clientPortBox";
            this.clientPortBox.Size = new System.Drawing.Size(176, 20);
            this.clientPortBox.TabIndex = 3;
            this.clientToolTips.SetToolTip(this.clientPortBox, "Any unused port");
            this.clientPortBox.Value = new decimal(new int[] {
            25555,
            0,
            0,
            0});
            // 
            // saveFolderLabel
            // 
            this.saveFolderLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.saveFolderLabel.AutoSize = true;
            this.saveFolderLabel.Location = new System.Drawing.Point(3, 265);
            this.saveFolderLabel.Name = "saveFolderLabel";
            this.saveFolderLabel.Size = new System.Drawing.Size(67, 13);
            this.saveFolderLabel.TabIndex = 0;
            this.saveFolderLabel.Text = "Save Folder:";
            // 
            // directoryPathBox
            // 
            this.directoryPathBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.directoryPathBox.Location = new System.Drawing.Point(71, 262);
            this.directoryPathBox.Name = "directoryPathBox";
            this.directoryPathBox.Size = new System.Drawing.Size(152, 20);
            this.directoryPathBox.TabIndex = 4;
            // 
            // folderDialog
            // 
            this.folderDialog.SelectedPath = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\The Guild 2 Renaissance\\savegames";
            // 
            // usernameLabel
            // 
            this.usernameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.usernameLabel.AutoSize = true;
            this.usernameLabel.Location = new System.Drawing.Point(12, 286);
            this.usernameLabel.Name = "usernameLabel";
            this.usernameLabel.Size = new System.Drawing.Size(58, 13);
            this.usernameLabel.TabIndex = 0;
            this.usernameLabel.Text = "Username:";
            // 
            // clientPortLabel
            // 
            this.clientPortLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.clientPortLabel.AutoSize = true;
            this.clientPortLabel.Location = new System.Drawing.Point(12, 244);
            this.clientPortLabel.Name = "clientPortLabel";
            this.clientPortLabel.Size = new System.Drawing.Size(58, 13);
            this.clientPortLabel.TabIndex = 0;
            this.clientPortLabel.Text = "Client Port:";
            // 
            // clientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(254, 345);
            this.Controls.Add(this.clientPortBox);
            this.Controls.Add(this.clientPortLabel);
            this.Controls.Add(this.saveTransferBar);
            this.Controls.Add(autoRestartCheckBox);
            this.Controls.Add(this.usernameBox);
            this.Controls.Add(this.usernameLabel);
            this.Controls.Add(this.saveFolderLabel);
            this.Controls.Add(this.directoryPathBox);
            this.Controls.Add(this.browseDirectoryButton);
            this.Controls.Add(this.hostIPBox);
            this.Controls.Add(this.hostPortBox);
            this.Controls.Add(this.hostPortLabel);
            this.Controls.Add(this.hostIPLabel);
            this.Controls.Add(this.clientInfoBox);
            this.Controls.Add(this.clientStartStopButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(466, 768);
            this.MinimumSize = new System.Drawing.Size(233, 384);
            this.Name = "clientForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Client";
            ((System.ComponentModel.ISupportInitialize)(this.hostPortBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clientPortBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.RichTextBox clientInfoBox;
        private System.Windows.Forms.Label hostIPLabel;
        private System.Windows.Forms.NumericUpDown hostPortBox;
        private System.Windows.Forms.TextBox hostIPBox;
        private System.Windows.Forms.Label hostPortLabel;
        private System.Windows.Forms.ToolTip clientToolTips;
        private System.Windows.Forms.FolderBrowserDialog folderDialog;
        private  System.Windows.Forms.Button browseDirectoryButton;
        private System.Windows.Forms.TextBox directoryPathBox;
        private System.Windows.Forms.Label saveFolderLabel;
        private System.Windows.Forms.Label usernameLabel;
        private System.Windows.Forms.TextBox usernameBox;
        public System.Windows.Forms.Button clientStartStopButton;
        public System.Windows.Forms.ProgressBar saveTransferBar;
        private System.Windows.Forms.Label clientPortLabel;
        private System.Windows.Forms.NumericUpDown clientPortBox;
    }
}