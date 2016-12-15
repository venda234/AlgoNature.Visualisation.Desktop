using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Threading;

namespace AlgoNature.Visualisation.Desktop
{
    public partial class PropertiesEditFlyOut : Form
    {
        /*public PropertiesEditFlyOut()
        {
            InitializeComponent();
        }*/

        public PropertiesEditFlyOut(PropertiesEditorGrid grid, string title) : this(grid)
        {
            this.Text = title;
        }
        public PropertiesEditFlyOut(PropertiesEditorGrid grid) : base()
        {
            InitializeComponent();
            PropertiesGrid = grid;
            PropertiesGrid.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        public PropertiesEditFlyOut(object objWhosePropertiesToDisplay, PropertyInfo[] propertiesToDisplay, string title)
            : this(new PropertiesEditorGrid(objWhosePropertiesToDisplay, propertiesToDisplay), title) { }
        public PropertiesEditFlyOut(object objWhosePropertiesToDisplay, PropertyInfo[] propertiesToDisplay) 
            : this(new PropertiesEditorGrid(objWhosePropertiesToDisplay, propertiesToDisplay)) { }

        public PropertiesEditFlyOut(object objWhosePropertiesToDisplay, string title) : this(objWhosePropertiesToDisplay, objWhosePropertiesToDisplay.GetType().GetProperties(), title) { }
        public PropertiesEditFlyOut(object objWhosePropertiesToDisplay) : this(objWhosePropertiesToDisplay, objWhosePropertiesToDisplay.GetType().GetProperties()) { }

        public PropertiesEditFlyOut(object objWhosePropertiesToDisplay, Type[] filterTypes, bool includeOnlyTypesPropsOrExcludeThemFromGeneral, string title)
            : this(objWhosePropertiesToDisplay, objWhosePropertiesToDisplay.GetType().GetProperties().FilterPropertiesBasedOnOtherTypes(filterTypes, includeOnlyTypesPropsOrExcludeThemFromGeneral), title)
        { }
        /// <summary>
        /// Constructor based on given object, filtering params based on given interfaces (either includes only given interfaces properties or excludes them from all object's properties) 
        /// </summary>
        /// <param name="objWhosePropertiesToDisplay">Object whose properties shall be displayed to be edited</param>
        /// <param name="filterTypes">Types used for filtering properties</param>
        /// <param name="includeOnlyTypesPropsOrExcludeThemFromGeneral">If <code>true</code>, will display only properties contained in <paramref name="filterTypes"/> types (if has them), 
        /// otherwise will display all properties except those contained in <paramref name="filterTypes"/> types</param>
        public PropertiesEditFlyOut(object objWhosePropertiesToDisplay, Type[] filterTypes, bool includeOnlyTypesPropsOrExcludeThemFromGeneral)
            : this(objWhosePropertiesToDisplay, objWhosePropertiesToDisplay.GetType().GetProperties().FilterPropertiesBasedOnOtherTypes(filterTypes, includeOnlyTypesPropsOrExcludeThemFromGeneral))
        { }

        /*private void showFlyout()
        {
            this.Show(Program.MainWindow);
        }*/
        /*public new DialogResult Show()
        {
            _result = DialogResult.None;
            Thread thr = new Thread(showFlyout);
            thr.Start();
            while (_result == DialogResult.None) Thread.Sleep(10);
            return _result;
        }*/

        public new void Show()
        {
            //if (PropertiesGrid.Height < gridViewPanel.Height) this.Height -= PropertiesGrid.Height - gridViewPanel.Height;
            base.Show();
        }
        public new void Show(IWin32Window owner)
        {
            //if (PropertiesGrid.Height < gridViewPanel.Height) this.Height -= PropertiesGrid.Height - gridViewPanel.Height;
            base.Show(owner);
        }
        public void Show(PropertiesEditorGrid grid)
        {
            PropertiesGrid = grid;
            PropertiesGrid.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //if (grid.Height < gridViewPanel.Height) this.Height -= grid.Height - gridViewPanel.Height;
            this.Show();
        }
        public void Show(PropertiesEditorGrid grid, IWin32Window owner)
        {
            PropertiesGrid = grid;
            PropertiesGrid.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //if (grid.Height < gridViewPanel.Height) this.Height -= grid.Height - gridViewPanel.Height;
            this.Show(owner);
        }

        public delegate void EditingFinishedEventHandler(DialogResult result, bool objectChanged, object editedObject);
        //public delegate void EditingFinishedEventHandler(DialogResult result);
        public event EditingFinishedEventHandler EditingFinished;

        private DialogResult _result;
        /*public DialogResult Result
        {
            get { return _result; }
        }*/

        public Type editedObjectType
        {
            get
            {
                return EditedObject.GetType();
            }
        }

        private object EditedObject
        {
            get { return PropertiesGrid.EditedObject; }
        }

        private bool EditedObjectChanged
        {
            get { return PropertiesGrid.AnythingChanged; }
        }

        public PropertiesEditorGrid PropertiesGrid
        {
            get
            {
                return (PropertiesEditorGrid)gridViewPanel.Controls[0];
            }
            set
            {
                gridViewPanel.Controls.Clear();
                gridViewPanel.Controls.Add(value);
                gridViewPanel.Controls[0].Dock = DockStyle.Fill;
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            _result = DialogResult.OK; 
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            _result = DialogResult.Cancel;
            this.Close();
        }

        private void PropertiesEditFlyOut_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_result == DialogResult.None) _result = DialogResult.OK;
            EditingFinished(_result, EditedObjectChanged, EditedObject);
        }

        private bool _userResizing = false;
        private void PropertiesEditFlyOut_Paint(object sender, PaintEventArgs e)
        {
            if (!_userResizing)
            {
                int gridHeight = PropertiesGrid.ColumnHeadersHeight + PropertiesGrid.RowCount * PropertiesGrid.RowTemplate.Height;
                if (gridHeight < gridViewPanel.Height) this.Height -= gridViewPanel.Height - gridHeight - 2; // Cells borders
            }
        }

        private void PropertiesEditFlyOut_ResizeBegin(object sender, EventArgs e)
        {
            _userResizing = true;
        }
    }
}
