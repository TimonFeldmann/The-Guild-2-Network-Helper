namespace The_Guild_2_Network_Helper
{
    partial class ChoiceMessageBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChoiceMessageBox));
            this.clientButton = new System.Windows.Forms.Button();
            this.hostButton = new System.Windows.Forms.Button();
            this.clientOrHostLabel = new System.Windows.Forms.Label();
            this.myLabel = new System.Windows.Forms.Label();
            this.clientHelpButton = new System.Windows.Forms.Button();
            this.hostHelpButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // clientButton
            // 
            this.clientButton.Location = new System.Drawing.Point(16, 36);
            this.clientButton.Name = "clientButton";
            this.clientButton.Size = new System.Drawing.Size(104, 48);
            this.clientButton.TabIndex = 0;
            this.clientButton.Text = "Client";
            this.clientButton.UseVisualStyleBackColor = true;
            this.clientButton.Click += new System.EventHandler(this.clientButton_Click);
            // 
            // hostButton
            // 
            this.hostButton.Location = new System.Drawing.Point(160, 36);
            this.hostButton.Name = "hostButton";
            this.hostButton.Size = new System.Drawing.Size(104, 48);
            this.hostButton.TabIndex = 1;
            this.hostButton.Text = "Host";
            this.hostButton.UseVisualStyleBackColor = true;
            this.hostButton.Click += new System.EventHandler(this.hostButton_Click);
            // 
            // clientOrHostLabel
            // 
            this.clientOrHostLabel.AutoSize = true;
            this.clientOrHostLabel.Location = new System.Drawing.Point(40, 16);
            this.clientOrHostLabel.Name = "clientOrHostLabel";
            this.clientOrHostLabel.Size = new System.Drawing.Size(202, 13);
            this.clientOrHostLabel.TabIndex = 1;
            this.clientOrHostLabel.Text = "Are you a client or the host for this game?";
            // 
            // myLabel
            // 
            this.myLabel.AutoSize = true;
            this.myLabel.Location = new System.Drawing.Point(3, 116);
            this.myLabel.Name = "myLabel";
            this.myLabel.Size = new System.Drawing.Size(276, 13);
            this.myLabel.TabIndex = 1;
            this.myLabel.Text = "The Guild 2: Renaissance Network Helper by Agentmass";
            // 
            // clientHelpButton
            // 
            this.clientHelpButton.Location = new System.Drawing.Point(16, 86);
            this.clientHelpButton.Name = "clientHelpButton";
            this.clientHelpButton.Size = new System.Drawing.Size(104, 23);
            this.clientHelpButton.TabIndex = 2;
            this.clientHelpButton.Text = "Show Client Help";
            this.clientHelpButton.UseVisualStyleBackColor = true;
            this.clientHelpButton.Click += new System.EventHandler(this.clientHelpButton_Click);
            // 
            // hostHelpButton
            // 
            this.hostHelpButton.Location = new System.Drawing.Point(160, 86);
            this.hostHelpButton.Name = "hostHelpButton";
            this.hostHelpButton.Size = new System.Drawing.Size(104, 23);
            this.hostHelpButton.TabIndex = 3;
            this.hostHelpButton.Text = "Show Host Help";
            this.hostHelpButton.UseVisualStyleBackColor = true;
            this.hostHelpButton.Click += new System.EventHandler(this.hostHelpButton_Click);
            // 
            // ChoiceMessageBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(281, 131);
            this.Controls.Add(this.hostHelpButton);
            this.Controls.Add(this.clientHelpButton);
            this.Controls.Add(this.myLabel);
            this.Controls.Add(this.clientOrHostLabel);
            this.Controls.Add(this.hostButton);
            this.Controls.Add(this.clientButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChoiceMessageBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Guild 2 Network Helper - Host or Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button clientButton;
        private System.Windows.Forms.Button hostButton;
        private System.Windows.Forms.Label clientOrHostLabel;
        private System.Windows.Forms.Label myLabel;
        private System.Windows.Forms.Button clientHelpButton;
        private System.Windows.Forms.Button hostHelpButton;
    }
}