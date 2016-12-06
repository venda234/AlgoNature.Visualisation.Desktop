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

namespace AlgoNature.Visualisation.Desktop
{
    public partial class PropertiesEditFlyOut : Form
    {
        public PropertiesEditFlyOut()
        {
            InitializeComponent();
        }

        public PropertiesEditFlyOut(PropertiesEditorGrid grid)
        {
            InitializeComponent();
            PropertiesGrid = grid;
        }

        public PropertiesEditFlyOut(object objWhosePropertiesToDisplay, PropertyInfo[] propertiesToDisplay) 
            : this(new PropertiesEditorGrid(objWhosePropertiesToDisplay, propertiesToDisplay)) { }

        public PropertiesEditFlyOut(object objWhosePropertiesToDisplay) : this(objWhosePropertiesToDisplay, objWhosePropertiesToDisplay.GetType().GetProperties()) { }

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

        public new DialogResult Show() { return this.ShowDialog(); }
        public DialogResult Show(PropertiesEditorGrid grid)
        {
            PropertiesGrid = grid;
            return this.ShowDialog();
        }

        public object EditedObject
        {
            get { return PropertiesGrid.EditedObject; }
        }

        public PropertiesEditorGrid PropertiesGrid
        {
            get
            {
                return (PropertiesEditorGrid)Controls[0];
            }
            set
            {
                Controls.Clear();
                Controls.Add(value);
                Controls[0].Dock = DockStyle.Fill;
            }
        }
    }
}
