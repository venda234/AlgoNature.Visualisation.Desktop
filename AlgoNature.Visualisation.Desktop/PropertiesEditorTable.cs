using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace AlgoNature.Visualisation.Desktop
{
    public partial class PropertiesEditorTable : DataGridView
    {
        public PropertiesEditorTable(object objWhosePropertiesToDisplay, PropertyInfo[] propertiesToDisplay)
        {
            InitializeComponent();
            Properties = propertiesToDisplay;

        }

        public PropertiesEditorTable(object objWhosePropertiesToDisplay) : this(objWhosePropertiesToDisplay, objWhosePropertiesToDisplay.GetType().GetProperties()) { }

        /// <summary>
        /// Constructor based on given object, filtering params based on given interfaces (either includes only given interfaces properties or excludes them from all object's properties) 
        /// </summary>
        /// <param name="objWhosePropertiesToDisplay">Object whose properties shall be displayed to be edited</param>
        /// <param name="filterTypes">Types used for filtering properties</param>
        /// <param name="includeOnlyTypesPropsOrExcludeThemFromGeneral">If <code>true</code>, will display only properties contained in <paramref name="filterTypes"/> types (if has them), 
        /// otherwise will display all properties except those contained in <paramref name="filterTypes"/> types</param>
        public PropertiesEditorTable(object objWhosePropertiesToDisplay, Type[] filterTypes, bool includeOnlyTypesPropsOrExcludeThemFromGeneral)
            : this(objWhosePropertiesToDisplay, objWhosePropertiesToDisplay.GetType().GetProperties().FilterPropertiesBasedOnOtherTypes(filterTypes, includeOnlyTypesPropsOrExcludeThemFromGeneral))
        {

        }

        

        public PropertyInfo[] Properties
        {
            get;
            private set;
        }
        private object _editedObject;
        public object EditedObject
        {
            get { return _editedObject; }
            set
            {
                _editedObject = value;
            }
        }
    }
}
