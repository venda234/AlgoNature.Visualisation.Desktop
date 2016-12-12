﻿namespace AlgoNature.Visualisation.Desktop
{
    partial class PropertiesEditorGrid
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.propertiesDataGridViewPropertyColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.propertiesDataGridViewValueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.propertyIndexColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // propertiesDataGridViewPropertyColumn
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.propertiesDataGridViewPropertyColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.propertiesDataGridViewPropertyColumn.HeaderText = "propertyColumnHeader";
            this.propertiesDataGridViewPropertyColumn.Name = "propertiesDataGridViewPropertyColumn";
            this.propertiesDataGridViewPropertyColumn.ReadOnly = true;
            this.propertiesDataGridViewPropertyColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // propertiesDataGridViewValueColumn
            // 
            this.propertiesDataGridViewValueColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.propertiesDataGridViewValueColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.propertiesDataGridViewValueColumn.HeaderText = "valueColumnHeader";
            this.propertiesDataGridViewValueColumn.Name = "propertiesDataGridViewValueColumn";
            // 
            // propertyIndexColumn
            // 
            this.propertyIndexColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.propertyIndexColumn.HeaderText = "index";
            this.propertyIndexColumn.MinimumWidth = 2;
            this.propertyIndexColumn.Name = "propertyIndexColumn";
            this.propertyIndexColumn.ReadOnly = true;
            this.propertyIndexColumn.Visible = false;
            this.propertyIndexColumn.Width = 2;
            // 
            // PropertiesEditorGrid
            // 
            this.AllowUserToAddRows = false;
            this.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.propertiesDataGridViewPropertyColumn,
            this.propertiesDataGridViewValueColumn,
            this.propertyIndexColumn});
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MultiSelect = false;
            this.Name = "propertiesDataGridView";
            this.RowHeadersVisible = false;
            this.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Size = new System.Drawing.Size(216, 409);
            this.ColumnSortModeChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.PropertiesEditorGrid_ColumnSortModeChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.PropertiesEditorGrid_Paint);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridViewTextBoxColumn propertiesDataGridViewPropertyColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn propertiesDataGridViewValueColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn propertyIndexColumn;
    }
}
