namespace AlgoNature.Visualisation.Desktop
{
    partial class mainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainForm));
            this.propertiesSplitContainer = new System.Windows.Forms.SplitContainer();
            this.splitViewPanel = new System.Windows.Forms.Panel();
            this.mainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.controlsComboBox = new System.Windows.Forms.ComboBox();
            this.exportButton = new System.Windows.Forms.Button();
            this.panelGrowableContrtols = new System.Windows.Forms.Panel();
            this.dieButton = new System.Windows.Forms.Button();
            this.stopGrowingButton = new System.Windows.Forms.Button();
            this.startGrowingButton = new System.Windows.Forms.Button();
            this.resetButton = new System.Windows.Forms.Button();
            this.exportImageDialog = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.propertiesSplitContainer)).BeginInit();
            this.propertiesSplitContainer.SuspendLayout();
            this.splitViewPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).BeginInit();
            this.mainSplitContainer.Panel1.SuspendLayout();
            this.mainSplitContainer.SuspendLayout();
            this.panelGrowableContrtols.SuspendLayout();
            this.SuspendLayout();
            // 
            // propertiesSplitContainer
            // 
            resources.ApplyResources(this.propertiesSplitContainer, "propertiesSplitContainer");
            this.propertiesSplitContainer.Name = "propertiesSplitContainer";
            // 
            // propertiesSplitContainer.Panel2
            // 
            resources.ApplyResources(this.propertiesSplitContainer.Panel2, "propertiesSplitContainer.Panel2");
            // 
            // splitViewPanel
            // 
            resources.ApplyResources(this.splitViewPanel, "splitViewPanel");
            this.splitViewPanel.Controls.Add(this.mainSplitContainer);
            this.splitViewPanel.Name = "splitViewPanel";
            // 
            // mainSplitContainer
            // 
            this.mainSplitContainer.BackColor = System.Drawing.Color.Silver;
            resources.ApplyResources(this.mainSplitContainer, "mainSplitContainer");
            this.mainSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.mainSplitContainer.Name = "mainSplitContainer";
            // 
            // mainSplitContainer.Panel1
            // 
            this.mainSplitContainer.Panel1.Controls.Add(this.propertiesSplitContainer);
            // 
            // mainSplitContainer.Panel2
            // 
            this.mainSplitContainer.Panel2.BackColor = System.Drawing.Color.White;
            this.mainSplitContainer.Panel2.Cursor = System.Windows.Forms.Cursors.Cross;
            this.mainSplitContainer.SplitterMoving += new System.Windows.Forms.SplitterCancelEventHandler(this.mainSplitContainer_SplitterMoving);
            this.mainSplitContainer.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.mainSplitContainer_SplitterMoved);
            // 
            // controlsComboBox
            // 
            this.controlsComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.controlsComboBox, "controlsComboBox");
            this.controlsComboBox.Name = "controlsComboBox";
            this.controlsComboBox.SelectedIndexChanged += new System.EventHandler(this.controlsComboBox_SelectedIndexChanged);
            // 
            // exportButton
            // 
            resources.ApplyResources(this.exportButton, "exportButton");
            this.exportButton.Name = "exportButton";
            this.exportButton.UseVisualStyleBackColor = true;
            this.exportButton.Click += new System.EventHandler(this.exportButton_Click);
            // 
            // panelGrowableContrtols
            // 
            this.panelGrowableContrtols.Controls.Add(this.dieButton);
            this.panelGrowableContrtols.Controls.Add(this.stopGrowingButton);
            this.panelGrowableContrtols.Controls.Add(this.startGrowingButton);
            resources.ApplyResources(this.panelGrowableContrtols, "panelGrowableContrtols");
            this.panelGrowableContrtols.Name = "panelGrowableContrtols";
            // 
            // dieButton
            // 
            resources.ApplyResources(this.dieButton, "dieButton");
            this.dieButton.Name = "dieButton";
            this.dieButton.UseVisualStyleBackColor = true;
            this.dieButton.Click += new System.EventHandler(this.dieButton_Click);
            // 
            // stopGrowingButton
            // 
            resources.ApplyResources(this.stopGrowingButton, "stopGrowingButton");
            this.stopGrowingButton.Name = "stopGrowingButton";
            this.stopGrowingButton.UseVisualStyleBackColor = true;
            this.stopGrowingButton.Click += new System.EventHandler(this.stopGrowingButton_Click);
            // 
            // startGrowingButton
            // 
            resources.ApplyResources(this.startGrowingButton, "startGrowingButton");
            this.startGrowingButton.Name = "startGrowingButton";
            this.startGrowingButton.UseVisualStyleBackColor = true;
            this.startGrowingButton.Click += new System.EventHandler(this.startGrowingButton_Click);
            // 
            // resetButton
            // 
            resources.ApplyResources(this.resetButton, "resetButton");
            this.resetButton.Name = "resetButton";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // exportImageDialog
            // 
            this.exportImageDialog.DefaultExt = "bmp";
            resources.ApplyResources(this.exportImageDialog, "exportImageDialog");
            this.exportImageDialog.InitialDirectory = "MyPictures";
            // 
            // mainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.Controls.Add(this.resetButton);
            this.Controls.Add(this.panelGrowableContrtols);
            this.Controls.Add(this.exportButton);
            this.Controls.Add(this.controlsComboBox);
            this.Controls.Add(this.splitViewPanel);
            this.DoubleBuffered = true;
            this.Name = "mainForm";
            this.ResizeBegin += new System.EventHandler(this.mainForm_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.mainForm_ResizeEnd);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.mainForm_Paint);
            this.Resize += new System.EventHandler(this.mainForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.propertiesSplitContainer)).EndInit();
            this.propertiesSplitContainer.ResumeLayout(false);
            this.splitViewPanel.ResumeLayout(false);
            this.mainSplitContainer.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).EndInit();
            this.mainSplitContainer.ResumeLayout(false);
            this.panelGrowableContrtols.ResumeLayout(false);
            this.panelGrowableContrtols.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        

        #endregion
        private System.Windows.Forms.SplitContainer mainSplitContainer;
        private System.Windows.Forms.SplitContainer propertiesSplitContainer;
        private System.Windows.Forms.ComboBox controlsComboBox;
        private System.Windows.Forms.Button exportButton;
        private System.Windows.Forms.Panel panelGrowableContrtols;
        private System.Windows.Forms.Button dieButton;
        private System.Windows.Forms.Button stopGrowingButton;
        private System.Windows.Forms.Button startGrowingButton;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.Panel splitViewPanel;
        private System.Windows.Forms.SaveFileDialog exportImageDialog;
    }
}

