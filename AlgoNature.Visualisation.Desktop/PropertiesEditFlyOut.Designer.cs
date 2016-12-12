namespace AlgoNature.Visualisation.Desktop
{
    partial class PropertiesEditFlyOut
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PropertiesEditFlyOut));
            this.buttonsDockPanel = new System.Windows.Forms.Panel();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.gridViewPanel = new System.Windows.Forms.Panel();
            this.buttonsDockPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonsDockPanel
            // 
            this.buttonsDockPanel.Controls.Add(this.cancelButton);
            this.buttonsDockPanel.Controls.Add(this.okButton);
            resources.ApplyResources(this.buttonsDockPanel, "buttonsDockPanel");
            this.buttonsDockPanel.Name = "buttonsDockPanel";
            // 
            // cancelButton
            // 
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okButton
            // 
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.Name = "okButton";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // gridViewPanel
            // 
            resources.ApplyResources(this.gridViewPanel, "gridViewPanel");
            this.gridViewPanel.Name = "gridViewPanel";
            // 
            // PropertiesEditFlyOut
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridViewPanel);
            this.Controls.Add(this.buttonsDockPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "PropertiesEditFlyOut";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PropertiesEditFlyOut_FormClosed);
            this.ResizeBegin += new System.EventHandler(this.PropertiesEditFlyOut_ResizeBegin);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.PropertiesEditFlyOut_Paint);
            this.buttonsDockPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel buttonsDockPanel;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Panel gridViewPanel;
    }
}