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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitViewPanel = new System.Windows.Forms.Panel();
            this.mainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.propertiesSplitContainer = new System.Windows.Forms.SplitContainer();
            this.propertiesDataGridView = new System.Windows.Forms.DataGridView();
            this.propertiesDataGridViewPropertyColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.propertiesDataGridViewValueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.iGrowablePropertiesDataGridView = new System.Windows.Forms.DataGridView();
            this.iGrowablePropertiesDataGridViewPropertyColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.iGrowablePropertiesDataGridViewValueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.controlsComboBox = new System.Windows.Forms.ComboBox();
            this.exportButton = new System.Windows.Forms.Button();
            this.panelGrowableContrtols = new System.Windows.Forms.Panel();
            this.dieButton = new System.Windows.Forms.Button();
            this.stopGrowingButton = new System.Windows.Forms.Button();
            this.startGrowingButton = new System.Windows.Forms.Button();
            this.resetButton = new System.Windows.Forms.Button();
            this.exportImageDialog = new System.Windows.Forms.SaveFileDialog();
            this.splitViewPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).BeginInit();
            this.mainSplitContainer.Panel1.SuspendLayout();
            this.mainSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.propertiesSplitContainer)).BeginInit();
            this.propertiesSplitContainer.Panel1.SuspendLayout();
            this.propertiesSplitContainer.Panel2.SuspendLayout();
            this.propertiesSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.propertiesDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iGrowablePropertiesDataGridView)).BeginInit();
            this.panelGrowableContrtols.SuspendLayout();
            this.SuspendLayout();
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
            // 
            // propertiesSplitContainer
            // 
            resources.ApplyResources(this.propertiesSplitContainer, "propertiesSplitContainer");
            this.propertiesSplitContainer.Name = "propertiesSplitContainer";
            // 
            // propertiesSplitContainer.Panel1
            // 
            this.propertiesSplitContainer.Panel1.Controls.Add(this.propertiesDataGridView);
            // 
            // propertiesSplitContainer.Panel2
            // 
            resources.ApplyResources(this.propertiesSplitContainer.Panel2, "propertiesSplitContainer.Panel2");
            this.propertiesSplitContainer.Panel2.Controls.Add(this.iGrowablePropertiesDataGridView);
            // 
            // propertiesDataGridView
            // 
            this.propertiesDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.propertiesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.propertiesDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.propertiesDataGridViewPropertyColumn,
            this.propertiesDataGridViewValueColumn});
            resources.ApplyResources(this.propertiesDataGridView, "propertiesDataGridView");
            this.propertiesDataGridView.Name = "propertiesDataGridView";
            this.propertiesDataGridView.RowHeadersVisible = false;
            // 
            // propertiesDataGridViewPropertyColumn
            // 
            this.propertiesDataGridViewPropertyColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.propertiesDataGridViewPropertyColumn.DefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.propertiesDataGridViewPropertyColumn, "propertiesDataGridViewPropertyColumn");
            this.propertiesDataGridViewPropertyColumn.Name = "propertiesDataGridViewPropertyColumn";
            this.propertiesDataGridViewPropertyColumn.ReadOnly = true;
            this.propertiesDataGridViewPropertyColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // propertiesDataGridViewValueColumn
            // 
            this.propertiesDataGridViewValueColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.propertiesDataGridViewValueColumn.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.propertiesDataGridViewValueColumn, "propertiesDataGridViewValueColumn");
            this.propertiesDataGridViewValueColumn.Name = "propertiesDataGridViewValueColumn";
            // 
            // iGrowablePropertiesDataGridView
            // 
            this.iGrowablePropertiesDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.iGrowablePropertiesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.iGrowablePropertiesDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.iGrowablePropertiesDataGridViewPropertyColumn,
            this.iGrowablePropertiesDataGridViewValueColumn});
            resources.ApplyResources(this.iGrowablePropertiesDataGridView, "iGrowablePropertiesDataGridView");
            this.iGrowablePropertiesDataGridView.Name = "iGrowablePropertiesDataGridView";
            this.iGrowablePropertiesDataGridView.RowHeadersVisible = false;
            this.iGrowablePropertiesDataGridView.Paint += new System.Windows.Forms.PaintEventHandler(this.iGrowablePropertiesDataGridView_Paint);
            // 
            // iGrowablePropertiesDataGridViewPropertyColumn
            // 
            resources.ApplyResources(this.iGrowablePropertiesDataGridViewPropertyColumn, "iGrowablePropertiesDataGridViewPropertyColumn");
            this.iGrowablePropertiesDataGridViewPropertyColumn.Name = "iGrowablePropertiesDataGridViewPropertyColumn";
            this.iGrowablePropertiesDataGridViewPropertyColumn.ReadOnly = true;
            // 
            // iGrowablePropertiesDataGridViewValueColumn
            // 
            resources.ApplyResources(this.iGrowablePropertiesDataGridViewValueColumn, "iGrowablePropertiesDataGridViewValueColumn");
            this.iGrowablePropertiesDataGridViewValueColumn.Name = "iGrowablePropertiesDataGridViewValueColumn";
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
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "mainForm";
            this.Resize += new System.EventHandler(this.mainForm_Resize);
            this.splitViewPanel.ResumeLayout(false);
            this.mainSplitContainer.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).EndInit();
            this.mainSplitContainer.ResumeLayout(false);
            this.propertiesSplitContainer.Panel1.ResumeLayout(false);
            this.propertiesSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.propertiesSplitContainer)).EndInit();
            this.propertiesSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.propertiesDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iGrowablePropertiesDataGridView)).EndInit();
            this.panelGrowableContrtols.ResumeLayout(false);
            this.panelGrowableContrtols.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        

        #endregion
        private System.Windows.Forms.SplitContainer mainSplitContainer;
        private System.Windows.Forms.SplitContainer propertiesSplitContainer;
        private System.Windows.Forms.ComboBox controlsComboBox;
        private System.Windows.Forms.DataGridView propertiesDataGridView;
        private System.Windows.Forms.Button exportButton;
        private System.Windows.Forms.DataGridView iGrowablePropertiesDataGridView;
        private System.Windows.Forms.Panel panelGrowableContrtols;
        private System.Windows.Forms.Button dieButton;
        private System.Windows.Forms.Button stopGrowingButton;
        private System.Windows.Forms.Button startGrowingButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn iGrowablePropertiesDataGridViewPropertyColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn iGrowablePropertiesDataGridViewValueColumn;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn propertiesDataGridViewPropertyColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn propertiesDataGridViewValueColumn;
        private System.Windows.Forms.Panel splitViewPanel;
        private System.Windows.Forms.SaveFileDialog exportImageDialog;
    }
}

