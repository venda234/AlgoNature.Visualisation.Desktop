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
            this.exportImageDialog = new System.Windows.Forms.SaveFileDialog();
            this.resetToDefaultButton = new System.Windows.Forms.Button();
            this.controlsComboBox = new System.Windows.Forms.ComboBox();
            this.resetWithoutLosingSettingsButton = new System.Windows.Forms.Button();
            this.exportButton = new System.Windows.Forms.Button();
            this.headerTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.startGrowingButton = new System.Windows.Forms.Button();
            this.dieButton = new System.Windows.Forms.Button();
            this.stopGrowingButton = new System.Windows.Forms.Button();
            this.iGrowableButtonsTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.propertiesSplitContainer)).BeginInit();
            this.propertiesSplitContainer.SuspendLayout();
            this.splitViewPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).BeginInit();
            this.mainSplitContainer.Panel1.SuspendLayout();
            this.mainSplitContainer.SuspendLayout();
            this.headerTableLayoutPanel.SuspendLayout();
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
            this.propertiesSplitContainer.SplitterMoving += new System.Windows.Forms.SplitterCancelEventHandler(this.propertiesSplitContainer_SplitterMoving);
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
            this.mainSplitContainer.Panel2.Scroll += new System.Windows.Forms.ScrollEventHandler(this.mainSplitContainer_Panel2_Scroll);
            this.mainSplitContainer.SplitterMoving += new System.Windows.Forms.SplitterCancelEventHandler(this.mainSplitContainer_SplitterMoving);
            this.mainSplitContainer.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.mainSplitContainer_SplitterMoved);
            // 
            // exportImageDialog
            // 
            this.exportImageDialog.DefaultExt = "bmp";
            resources.ApplyResources(this.exportImageDialog, "exportImageDialog");
            this.exportImageDialog.InitialDirectory = "MyPictures";
            // 
            // resetToDefaultButton
            // 
            resources.ApplyResources(this.resetToDefaultButton, "resetToDefaultButton");
            this.resetToDefaultButton.Name = "resetToDefaultButton";
            this.resetToDefaultButton.UseVisualStyleBackColor = true;
            this.resetToDefaultButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // controlsComboBox
            // 
            resources.ApplyResources(this.controlsComboBox, "controlsComboBox");
            this.controlsComboBox.FormattingEnabled = true;
            this.controlsComboBox.Name = "controlsComboBox";
            this.controlsComboBox.SelectedIndexChanged += new System.EventHandler(this.controlsComboBox_SelectedIndexChanged);
            // 
            // resetWithoutLosingSettingsButton
            // 
            resources.ApplyResources(this.resetWithoutLosingSettingsButton, "resetWithoutLosingSettingsButton");
            this.resetWithoutLosingSettingsButton.Name = "resetWithoutLosingSettingsButton";
            this.resetWithoutLosingSettingsButton.UseVisualStyleBackColor = true;
            this.resetWithoutLosingSettingsButton.Click += new System.EventHandler(this.resetWithoutLosingSettingsButton_Click);
            // 
            // exportButton
            // 
            resources.ApplyResources(this.exportButton, "exportButton");
            this.exportButton.Name = "exportButton";
            this.exportButton.UseVisualStyleBackColor = true;
            this.exportButton.Click += new System.EventHandler(this.exportButton_Click);
            // 
            // headerTableLayoutPanel
            // 
            resources.ApplyResources(this.headerTableLayoutPanel, "headerTableLayoutPanel");
            this.headerTableLayoutPanel.Controls.Add(this.startGrowingButton, 3, 0);
            this.headerTableLayoutPanel.Controls.Add(this.dieButton, 5, 0);
            this.headerTableLayoutPanel.Controls.Add(this.stopGrowingButton, 4, 0);
            this.headerTableLayoutPanel.Controls.Add(this.resetWithoutLosingSettingsButton, 2, 0);
            this.headerTableLayoutPanel.Controls.Add(this.controlsComboBox, 0, 0);
            this.headerTableLayoutPanel.Controls.Add(this.resetToDefaultButton, 1, 0);
            this.headerTableLayoutPanel.Controls.Add(this.exportButton, 6, 0);
            this.headerTableLayoutPanel.Name = "headerTableLayoutPanel";
            // 
            // startGrowingButton
            // 
            resources.ApplyResources(this.startGrowingButton, "startGrowingButton");
            this.startGrowingButton.Name = "startGrowingButton";
            this.startGrowingButton.UseVisualStyleBackColor = true;
            this.startGrowingButton.Click += new System.EventHandler(this.startGrowingButton_Click);
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
            // iGrowableButtonsTableLayoutPanel
            // 
            resources.ApplyResources(this.iGrowableButtonsTableLayoutPanel, "iGrowableButtonsTableLayoutPanel");
            this.iGrowableButtonsTableLayoutPanel.Name = "iGrowableButtonsTableLayoutPanel";
            // 
            // mainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.Controls.Add(this.iGrowableButtonsTableLayoutPanel);
            this.Controls.Add(this.headerTableLayoutPanel);
            this.Controls.Add(this.splitViewPanel);
            this.DoubleBuffered = true;
            this.Name = "mainForm";
            this.ResizeBegin += new System.EventHandler(this.mainForm_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.mainForm_ResizeEnd);
            this.Scroll += new System.Windows.Forms.ScrollEventHandler(this.mainForm_Scroll);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.mainForm_Paint);
            this.Resize += new System.EventHandler(this.mainForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.propertiesSplitContainer)).EndInit();
            this.propertiesSplitContainer.ResumeLayout(false);
            this.splitViewPanel.ResumeLayout(false);
            this.mainSplitContainer.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).EndInit();
            this.mainSplitContainer.ResumeLayout(false);
            this.headerTableLayoutPanel.ResumeLayout(false);
            this.headerTableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }



        #endregion
        private System.Windows.Forms.SplitContainer mainSplitContainer;
        private System.Windows.Forms.SplitContainer propertiesSplitContainer;
        private System.Windows.Forms.Panel splitViewPanel;
        private System.Windows.Forms.SaveFileDialog exportImageDialog;
        private System.Windows.Forms.Button resetToDefaultButton;
        private System.Windows.Forms.ComboBox controlsComboBox;
        private System.Windows.Forms.Button resetWithoutLosingSettingsButton;
        private System.Windows.Forms.Button exportButton;
        private System.Windows.Forms.TableLayoutPanel headerTableLayoutPanel;
        private System.Windows.Forms.Button startGrowingButton;
        private System.Windows.Forms.Button dieButton;
        private System.Windows.Forms.Button stopGrowingButton;
        private System.Windows.Forms.TableLayoutPanel iGrowableButtonsTableLayoutPanel;
    }
}

