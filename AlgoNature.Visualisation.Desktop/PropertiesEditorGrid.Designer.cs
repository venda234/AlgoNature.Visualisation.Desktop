namespace AlgoNature.Visualisation.Desktop
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.propertiesDataGridView = new System.Windows.Forms.DataGridView();
            this.propertiesDataGridViewPropertyColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.propertiesDataGridViewValueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.propertiesDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // propertiesDataGridView
            // 
            this.propertiesDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.propertiesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.propertiesDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.propertiesDataGridViewPropertyColumn,
            this.propertiesDataGridViewValueColumn});
            this.propertiesDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertiesDataGridView.Location = new System.Drawing.Point(0, 0);
            this.propertiesDataGridView.Name = "propertiesDataGridView";
            this.propertiesDataGridView.RowHeadersVisible = false;
            this.propertiesDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.propertiesDataGridView.Size = new System.Drawing.Size(216, 409);
            this.propertiesDataGridView.TabIndex = 0;
            this.propertiesDataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.propertiesDataGridView_CellClick);
            this.propertiesDataGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.propertiesDataGridView_CellEndEdit);
            // 
            // propertiesDataGridViewPropertyColumn
            // 
            this.propertiesDataGridViewPropertyColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.propertiesDataGridViewPropertyColumn.DefaultCellStyle = dataGridViewCellStyle5;
            this.propertiesDataGridViewPropertyColumn.HeaderText = "propertyColumnHeader";
            this.propertiesDataGridViewPropertyColumn.Name = "propertiesDataGridViewPropertyColumn";
            this.propertiesDataGridViewPropertyColumn.ReadOnly = true;
            this.propertiesDataGridViewPropertyColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // propertiesDataGridViewValueColumn
            // 
            this.propertiesDataGridViewValueColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.propertiesDataGridViewValueColumn.DefaultCellStyle = dataGridViewCellStyle6;
            this.propertiesDataGridViewValueColumn.HeaderText = "valueColumnHeader";
            this.propertiesDataGridViewValueColumn.Name = "propertiesDataGridViewValueColumn";
            // 
            // PropertiesEditorGrid
            // 
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            ((System.ComponentModel.ISupportInitialize)(this.propertiesDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView propertiesDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn propertiesDataGridViewPropertyColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn propertiesDataGridViewValueColumn;
    }
}
