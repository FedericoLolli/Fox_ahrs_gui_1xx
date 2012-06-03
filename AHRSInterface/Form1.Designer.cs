
namespace AHRSInterface
{
    partial class AHRSInterface
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AHRSInterface));
            this.statusBox = new System.Windows.Forms.RichTextBox();
            this.SynchButton = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dialogsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.opend3dcube = new System.Windows.Forms.ToolStripMenuItem();
            this.logDataToolstripItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.serialPortCOMBox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.baudSelectBox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.serialConnectButton = new System.Windows.Forms.ToolStripButton();
            this.serialDisconnectButton = new System.Windows.Forms.ToolStripButton();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusBox
            // 
            this.statusBox.Location = new System.Drawing.Point(12, 373);
            this.statusBox.Name = "statusBox";
            this.statusBox.ReadOnly = true;
            this.statusBox.Size = new System.Drawing.Size(628, 114);
            this.statusBox.TabIndex = 0;
            this.statusBox.Text = "";
            this.statusBox.TextChanged += new System.EventHandler(this.statusBox_TextChanged);
            // 
            // SynchButton
            // 
            this.SynchButton.Location = new System.Drawing.Point(565, 344);
            this.SynchButton.Name = "SynchButton";
            this.SynchButton.Size = new System.Drawing.Size(75, 23);
            this.SynchButton.TabIndex = 1;
            this.SynchButton.Text = "Synch";
            this.SynchButton.UseVisualStyleBackColor = true;
            this.SynchButton.Click += new System.EventHandler(this.SynchButton_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.dialogsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(652, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // dialogsToolStripMenuItem
            // 
            this.dialogsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configToolStripMenuItem,
            this.opend3dcube,
            this.logDataToolstripItem});
            this.dialogsToolStripMenuItem.Name = "dialogsToolStripMenuItem";
            this.dialogsToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.dialogsToolStripMenuItem.Text = "Dialogs";
            // 
            // configToolStripMenuItem
            // 
            this.configToolStripMenuItem.Enabled = false;
            this.configToolStripMenuItem.Name = "configToolStripMenuItem";
            this.configToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.configToolStripMenuItem.Text = "Config";
            this.configToolStripMenuItem.Click += new System.EventHandler(this.configToolStripMenuItem_Click);
            // 
            // opend3dcube
            // 
            this.opend3dcube.Name = "opend3dcube";
            this.opend3dcube.Size = new System.Drawing.Size(152, 22);
            this.opend3dcube.Text = "3D cube";
            this.opend3dcube.Click += new System.EventHandler(this.opend3dcube_Click);
            // 
            // logDataToolstripItem
            // 
            this.logDataToolstripItem.Enabled = false;
            this.logDataToolstripItem.Name = "logDataToolstripItem";
            this.logDataToolstripItem.Size = new System.Drawing.Size(152, 22);
            this.logDataToolstripItem.Text = "Log";
            this.logDataToolstripItem.Click += new System.EventHandler(this.logDataToolstripItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.serialPortCOMBox,
            this.toolStripSeparator1,
            this.toolStripLabel2,
            this.baudSelectBox,
            this.toolStripSeparator2,
            this.serialConnectButton,
            this.serialDisconnectButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(652, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(32, 22);
            this.toolStripLabel1.Text = "Port:";
            // 
            // serialPortCOMBox
            // 
            this.serialPortCOMBox.Name = "serialPortCOMBox";
            this.serialPortCOMBox.Size = new System.Drawing.Size(121, 25);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(37, 22);
            this.toolStripLabel2.Text = "Baud:";
            // 
            // baudSelectBox
            // 
            this.baudSelectBox.Name = "baudSelectBox";
            this.baudSelectBox.Size = new System.Drawing.Size(121, 25);
            this.baudSelectBox.Click += new System.EventHandler(this.baudSelectBox_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // serialConnectButton
            // 
            this.serialConnectButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.serialConnectButton.Image = ((System.Drawing.Image)(resources.GetObject("serialConnectButton.Image")));
            this.serialConnectButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.serialConnectButton.Name = "serialConnectButton";
            this.serialConnectButton.Size = new System.Drawing.Size(23, 22);
            this.serialConnectButton.Text = "connectButton";
            this.serialConnectButton.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // serialDisconnectButton
            // 
            this.serialDisconnectButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.serialDisconnectButton.Enabled = false;
            this.serialDisconnectButton.Image = ((System.Drawing.Image)(resources.GetObject("serialDisconnectButton.Image")));
            this.serialDisconnectButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.serialDisconnectButton.Name = "serialDisconnectButton";
            this.serialDisconnectButton.Size = new System.Drawing.Size(23, 22);
            this.serialDisconnectButton.Text = "disconnectButton";
            this.serialDisconnectButton.Click += new System.EventHandler(this.serialDisconnectButton_Click);
            // 
            // AHRSInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(652, 499);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.SynchButton);
            this.Controls.Add(this.statusBox);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "AHRSInterface";
            this.Text = "AHRS Interface";
            this.Load += new System.EventHandler(this.AHRSInterface_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox statusBox;
        private System.Windows.Forms.Button SynchButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dialogsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem opend3dcube;
        private System.Windows.Forms.ToolStripMenuItem logDataToolstripItem;
        private System.Windows.Forms.ToolStripMenuItem configToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox serialPortCOMBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripComboBox baudSelectBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton serialConnectButton;
        private System.Windows.Forms.ToolStripButton serialDisconnectButton;
        //private DirectXCode.DirectXCube cube;
    }
}

