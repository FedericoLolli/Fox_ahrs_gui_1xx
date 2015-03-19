namespace AHRSInterface
{
    partial class AHRSLog
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
            this.filenameBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.browseButton = new System.Windows.Forms.Button();
            this.startLoggingButton = new System.Windows.Forms.Button();
            this.stopLoggingButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // filenameBox
            // 
            this.filenameBox.Location = new System.Drawing.Point(12, 34);
            this.filenameBox.Name = "filenameBox";
            this.filenameBox.ReadOnly = true;
            this.filenameBox.Size = new System.Drawing.Size(404, 20);
            this.filenameBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Filename";
            // 
            // browseButton
            // 
            this.browseButton.Location = new System.Drawing.Point(422, 32);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(75, 23);
            this.browseButton.TabIndex = 2;
            this.browseButton.Text = "Browse";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
            // 
            // startLoggingButton
            // 
            this.startLoggingButton.Enabled = false;
            this.startLoggingButton.Location = new System.Drawing.Point(16, 74);
            this.startLoggingButton.Name = "startLoggingButton";
            this.startLoggingButton.Size = new System.Drawing.Size(75, 23);
            this.startLoggingButton.TabIndex = 3;
            this.startLoggingButton.Text = "Start";
            this.startLoggingButton.UseVisualStyleBackColor = true;
            this.startLoggingButton.Click += new System.EventHandler(this.startLoggingButton_Click);
            // 
            // stopLoggingButton
            // 
            this.stopLoggingButton.Enabled = false;
            this.stopLoggingButton.Location = new System.Drawing.Point(97, 74);
            this.stopLoggingButton.Name = "stopLoggingButton";
            this.stopLoggingButton.Size = new System.Drawing.Size(75, 23);
            this.stopLoggingButton.TabIndex = 4;
            this.stopLoggingButton.Text = "Stop";
            this.stopLoggingButton.UseVisualStyleBackColor = true;
            this.stopLoggingButton.Click += new System.EventHandler(this.stopLoggingButton_Click);
            // 
            // Log
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(511, 116);
            this.Controls.Add(this.stopLoggingButton);
            this.Controls.Add(this.startLoggingButton);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.filenameBox);
            this.Name = "Log";
            this.Text = "Log";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox filenameBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.Button startLoggingButton;
        private System.Windows.Forms.Button stopLoggingButton;
    }
}