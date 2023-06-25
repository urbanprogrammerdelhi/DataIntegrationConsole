namespace DataIntegrationServiceConsole
{
    partial class DataIntegrationTool
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataIntegrationTool));
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.btnFetch = new System.Windows.Forms.Button();
            this.cmbStartHour = new System.Windows.Forms.ComboBox();
            this.cmbEndMinutes = new System.Windows.Forms.ComboBox();
            this.cmbEndHour = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.pbDataIntegrationService = new System.Windows.Forms.ProgressBar();
            this.btnSync = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.cmbStartMinutes = new System.Windows.Forms.ComboBox();
            this.lstMessage = new System.Windows.Forms.ListBox();
            this.lblMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.CustomFormat = "yyyy-MM-dd";
            this.dtpStartDate.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStartDate.Location = new System.Drawing.Point(16, 45);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(119, 27);
            this.dtpStartDate.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "Start Date ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(288, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 18);
            this.label2.TabIndex = 3;
            this.label2.Text = "End Date :";
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.CustomFormat = "yyyy-MM-dd";
            this.dtpEndDate.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEndDate.Location = new System.Drawing.Point(291, 43);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(119, 27);
            this.dtpEndDate.TabIndex = 2;
            // 
            // btnFetch
            // 
            this.btnFetch.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFetch.Location = new System.Drawing.Point(16, 92);
            this.btnFetch.Name = "btnFetch";
            this.btnFetch.Size = new System.Drawing.Size(95, 36);
            this.btnFetch.TabIndex = 4;
            this.btnFetch.Text = "Fetch";
            this.btnFetch.UseVisualStyleBackColor = true;
            this.btnFetch.Click += new System.EventHandler(this.btnFetch_Click);
            // 
            // cmbStartHour
            // 
            this.cmbStartHour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStartHour.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbStartHour.FormattingEnabled = true;
            this.cmbStartHour.Location = new System.Drawing.Point(141, 47);
            this.cmbStartHour.Name = "cmbStartHour";
            this.cmbStartHour.Size = new System.Drawing.Size(44, 26);
            this.cmbStartHour.TabIndex = 5;
            // 
            // cmbEndMinutes
            // 
            this.cmbEndMinutes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEndMinutes.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbEndMinutes.FormattingEnabled = true;
            this.cmbEndMinutes.Location = new System.Drawing.Point(477, 45);
            this.cmbEndMinutes.Name = "cmbEndMinutes";
            this.cmbEndMinutes.Size = new System.Drawing.Size(44, 26);
            this.cmbEndMinutes.TabIndex = 8;
            // 
            // cmbEndHour
            // 
            this.cmbEndHour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEndHour.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbEndHour.FormattingEnabled = true;
            this.cmbEndHour.Location = new System.Drawing.Point(415, 45);
            this.cmbEndHour.Name = "cmbEndHour";
            this.cmbEndHour.Size = new System.Drawing.Size(44, 26);
            this.cmbEndHour.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(185, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(15, 18);
            this.label3.TabIndex = 9;
            this.label3.Text = ":";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(459, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(15, 18);
            this.label4.TabIndex = 10;
            this.label4.Text = ":";
            // 
            // pbDataIntegrationService
            // 
            this.pbDataIntegrationService.Location = new System.Drawing.Point(16, 495);
            this.pbDataIntegrationService.Name = "pbDataIntegrationService";
            this.pbDataIntegrationService.Size = new System.Drawing.Size(727, 23);
            this.pbDataIntegrationService.TabIndex = 12;
            this.pbDataIntegrationService.Click += new System.EventHandler(this.pbDataIntegrationService_Click);
            // 
            // btnSync
            // 
            this.btnSync.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSync.Location = new System.Drawing.Point(16, 92);
            this.btnSync.Name = "btnSync";
            this.btnSync.Size = new System.Drawing.Size(95, 36);
            this.btnSync.TabIndex = 13;
            this.btnSync.Text = "Sync";
            this.btnSync.UseVisualStyleBackColor = true;
            this.btnSync.Click += new System.EventHandler(this.btnSync_Click);
            // 
            // btnReset
            // 
            this.btnReset.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReset.Location = new System.Drawing.Point(117, 92);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(95, 36);
            this.btnReset.TabIndex = 14;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // cmbStartMinutes
            // 
            this.cmbStartMinutes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStartMinutes.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbStartMinutes.FormattingEnabled = true;
            this.cmbStartMinutes.Location = new System.Drawing.Point(203, 46);
            this.cmbStartMinutes.Name = "cmbStartMinutes";
            this.cmbStartMinutes.Size = new System.Drawing.Size(44, 26);
            this.cmbStartMinutes.TabIndex = 6;
            // 
            // lstMessage
            // 
            this.lstMessage.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.lstMessage.Font = new System.Drawing.Font("Verdana", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstMessage.ForeColor = System.Drawing.Color.Black;
            this.lstMessage.FormattingEnabled = true;
            this.lstMessage.ItemHeight = 16;
            this.lstMessage.Location = new System.Drawing.Point(16, 134);
            this.lstMessage.Name = "lstMessage";
            this.lstMessage.Size = new System.Drawing.Size(727, 356);
            this.lstMessage.TabIndex = 15;
            this.lstMessage.SelectedIndexChanged += new System.EventHandler(this.lstMessage_SelectedIndexChanged);
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.lblMessage.Location = new System.Drawing.Point(272, 102);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(0, 18);
            this.lblMessage.TabIndex = 16;
            // 
            // DataIntegrationTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(755, 521);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.lstMessage);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnSync);
            this.Controls.Add(this.pbDataIntegrationService);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbEndMinutes);
            this.Controls.Add(this.cmbEndHour);
            this.Controls.Add(this.cmbStartMinutes);
            this.Controls.Add(this.cmbStartHour);
            this.Controls.Add(this.btnFetch);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dtpEndDate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtpStartDate);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DataIntegrationTool";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Data Integration Service ";
            this.Load += new System.EventHandler(this.DataIntegrationTool_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.Button btnFetch;
        private System.Windows.Forms.ComboBox cmbStartHour;
        private System.Windows.Forms.ComboBox cmbEndMinutes;
        private System.Windows.Forms.ComboBox cmbEndHour;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ProgressBar pbDataIntegrationService;
        private System.Windows.Forms.Button btnSync;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.ComboBox cmbStartMinutes;
        private System.Windows.Forms.ListBox lstMessage;
        private System.Windows.Forms.Label lblMessage;
    }
}