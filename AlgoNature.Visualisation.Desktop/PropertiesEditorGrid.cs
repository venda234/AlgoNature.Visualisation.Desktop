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
    public partial class PropertiesEditorGrid : DataGridView
    {
        // Boolean will be displayed other way
        private const string DIRECTLY_EDITABLE_TYPES_STR = "SByteCharDateTimeDecimalDoubleUInt16UInt32UInt64SingleString";

        public PropertiesEditorGrid(object objWhosePropertiesToDisplay, PropertyInfo[] propertiesToDisplay)
        {
            InitializeComponent();
            Properties = propertiesToDisplay;
            initializeGrid();
        }

        public PropertiesEditorGrid(object objWhosePropertiesToDisplay) : this(objWhosePropertiesToDisplay, objWhosePropertiesToDisplay.GetType().GetProperties()) { }

        /// <summary>
        /// Constructor based on given object, filtering params based on given interfaces (either includes only given interfaces properties or excludes them from all object's properties) 
        /// </summary>
        /// <param name="objWhosePropertiesToDisplay">Object whose properties shall be displayed to be edited</param>
        /// <param name="filterTypes">Types used for filtering properties</param>
        /// <param name="includeOnlyTypesPropsOrExcludeThemFromGeneral">If <code>true</code>, will display only properties contained in <paramref name="filterTypes"/> types (if has them), 
        /// otherwise will display all properties except those contained in <paramref name="filterTypes"/> types</param>
        public PropertiesEditorGrid(object objWhosePropertiesToDisplay, Type[] filterTypes, bool includeOnlyTypesPropsOrExcludeThemFromGeneral)
            : this(objWhosePropertiesToDisplay, objWhosePropertiesToDisplay.GetType().GetProperties().FilterPropertiesBasedOnOtherTypes(filterTypes, includeOnlyTypesPropsOrExcludeThemFromGeneral))
        { }

        private void initializeGrid()
        {
            if (_editedObject.GetType().IsArray) // Edited object is an array
            {
                initializeArrayObject();
            }
            else // Edited object is a regular object
            {
                propertiesDataGridView.Rows.Clear();
                propertiesDataGridView.RowCount = _properties.Length;
                for (int i = 0; i < _properties.Length; i++)
                {
                    initializeEditedObjectPropertyRow(i, _properties[i]);
                }
            }

        }

        /// <summary>
        /// Method that initializes an array
        /// </summary>
        private void initializeArrayObject()
        {
            int length = ((object[])_editedObject).Length;
            for (int i = 0; i < length; i++) ; // TODO initializeArrayRow(i, i, ref ((object[])_editedObject)[i]);
        }

        /// <summary>
        /// A method for displaying the (edited object as array)'s element
        /// </summary>
        /// <param name="rowIndex">Index of the particular DataGridView row</param>
        /// <param name="arrayIndex">Index of the array's item which is supposed to be displayed</param>
        private void initializeArrayRow(int rowIndex, int arrayIndex)
        {
            initializePropertyRowWithPropertyName(rowIndex, String.Format("[{0}]", arrayIndex));
            if (((object[])_editedObject)[arrayIndex].IsDirectlyEditableValueByGrid()) initializeDirectValueObjectOfArrayCell(rowIndex, arrayIndex);
            else initializeOtherTypeObjectOfArrayCell(rowIndex, arrayIndex);
        }

        /// <summary>
        /// Initializes a blank value row with displayed property name + adds the row if a row with the given index doesn't exist
        /// </summary>
        /// <param name="rowIndex">Index of the particular DataGridView row</param>
        /// <param name="propertyName">Name of a property to be displayed</param>
        private void initializePropertyRowWithPropertyName(int rowIndex, string propertyName)
        {
            // testing whether a row with this index exists
            bool addRow;
            do
            {
                addRow = false;
                try
                {
                    this.propertiesDataGridView[0, rowIndex].ReadOnly = true; // property name column won't be editable
                }
                catch
                {
                    addRow = true;
                    this.propertiesDataGridView.RowCount++;
                }
            } while (addRow);

            this.propertiesDataGridView[0, rowIndex].Value = propertyName;
        }

        /*
       /// <summary>
       /// A method for initializing a row by its index 
       /// </summary>
       /// <param name="rowIndex">Index of the particular row</param>
       /// <param name="arrayIndex">Index of the displayed value in an array to be displayed</param>
       /// <param name="value">Value to be initialized and displayed</param>
       private void initializeRow(int rowIndex, int arrayIndex, ref object value)
           => initializeRow(rowIndex, String.Format("[{0}]", arrayIndex), ref value);*/
        /// <summary>
        /// A method for initializing a row by its index 
        /// </summary>
        /// <param name="rowIndex">Index of the particular DataGridView row</param>
        /// <param name="valueProperty">A property value of which shall be displayed and edited</param>
        private void initializeEditedObjectPropertyRow(int rowIndex, PropertyInfo valueProperty)
        {
            string propertyName = valueProperty.Name;

            // initialize row
            initializePropertyRowWithPropertyName(rowIndex, propertyName);

            var value = valueProperty.GetValue(_editedObject);
            Type valType = value.GetType();
            //if (DIRECTLY_EDITABLE_TYPES_STR.Contains(value.GetType().Name))
            // If it is directly editable value type, just assign, the dataGridView will manage it itself
            if (value.IsDirectlyEditableValueByGrid()) // Boolean will be displayed as a checkbbox or so
            {
                initializeDirectValuePropertyCell(rowIndex, _properties[rowIndex]);
            }
            else // Displaying other object types
            {
                initializeOtherObjectTypePropertyCell(rowIndex, _properties[rowIndex]);
            }
        }

        /// <summary>
        /// A method for initializing a cell by its row index a value of which is directly editable
        /// </summary>
        /// <param name="rowIndex">Index of the particular DataGridView row</param>
        /// <param name="valueProperty">A property value of which shall be displayed and edited</param>
        private void initializeDirectValuePropertyCell(int rowIndex, PropertyInfo valueProperty)
        {
            var value = valueProperty.GetValue(_editedObject);
            this.propertiesDataGridView[1, rowIndex].Value = value;
            this.propertiesDataGridView[1, rowIndex].ValueType = value.GetType();
            // Anonymous method for changing back the value
            this.propertiesDataGridView.CellEndEdit +=
                (sender, e) =>
                {
                    if (e.RowIndex == rowIndex)
                    {
                        _properties[rowIndex].SetValue(_editedObject, this.propertiesDataGridView[1, rowIndex].Value);
                    }
                };
        }

        private void initializeDirectValueObjectOfArrayCell(int rowIndex, int arrayIndex)
        {
            object[] arr = (object[])_editedObject;
            this.propertiesDataGridView[1, rowIndex].Value = arr[arrayIndex];
            this.propertiesDataGridView[1, rowIndex].ValueType = arr[arrayIndex].GetType();

            // Anonymous method for changing back the value
            this.propertiesDataGridView.CellEndEdit +=
                (sender, e) =>
                {
                    if (e.RowIndex == rowIndex)
                    {
                        arr[arrayIndex] = this.propertiesDataGridView[1, rowIndex].Value;
                        AnythingChanged = true;
                    }
                };
        }

        private void initializeOtherObjectTypePropertyCell(int rowIndex, PropertyInfo valueProperty)
        {
            var value = valueProperty.GetValue(_editedObject);
            
            if (value is bool)
            {
                propertiesDataGridView[1, rowIndex] = new DataGridViewCheckBoxCell(false);
                ((DataGridViewCheckBoxCell)propertiesDataGridView[1, rowIndex]).Value = value;
                    
                this.propertiesDataGridView.CellEndEdit +=
                    (sender, e) =>
                    {
                        if (e.RowIndex == rowIndex)
                        {
                            _properties[rowIndex].SetValue(_editedObject, propertiesDataGridView[1, rowIndex].Value);
                            AnythingChanged = true;
                        }
                    };
            }
            else if (value is Color)
            {
                propertiesDataGridView[1, rowIndex] = new DataGridViewButtonCell();
                propertiesDataGridView[1, rowIndex].Value = value;
                ((DataGridViewButtonCell)propertiesDataGridView[1, rowIndex]).Style.BackColor = (Color)value;

                this.propertiesDataGridView.CellClick +=
                    (sender, e) =>
                    {
                        if (e.RowIndex == rowIndex)
                        {
                            ColorDialog colDialog = new ColorDialog();
                            DialogResult result = colDialog.ShowDialog();

                            if (result == DialogResult.OK)
                            {
                                _properties[rowIndex].SetValue(_editedObject, colDialog.Color);
                                propertiesDataGridView[1, rowIndex].Value = colDialog.Color;
                                ((DataGridViewButtonCell)propertiesDataGridView[1, rowIndex]).Style.BackColor = colDialog.Color;
                                AnythingChanged = true;
                            }
                            // TODO zjistit jiné výsledky dialogResult
                        }
                    };
            }
            else if (value is DateTime)
            {
                //propertiesDataGridView[1, rowIndex].ValueType = typeof(DateTime);
                propertiesDataGridView[1, rowIndex].Value = value;

                this.propertiesDataGridView.CellEndEdit +=
                    (sender, e) =>
                    {
                        if (e.RowIndex == rowIndex)
                        {
                            _properties[rowIndex].SetValue(_editedObject, DateTime.Parse(propertiesDataGridView[1, rowIndex].Value.ToString()));
                            AnythingChanged = true;
                        }
                    };
            }
            else if (value is Point)
            {
                propertiesDataGridView[1, rowIndex].Value = value;

                this.propertiesDataGridView.CellEndEdit +=
                    (sender, e) =>
                    {
                        if (e.RowIndex == rowIndex)
                        {
                            _properties[rowIndex].SetValue(_editedObject, propertiesDataGridView[1, rowIndex].Value);
                            AnythingChanged = true;
                        }
                    };
            }
            else if (value is PointF)
            {
                propertiesDataGridView[1, rowIndex].Value = value;

                this.propertiesDataGridView.CellEndEdit +=
                    (sender, e) =>
                    {
                        if (e.RowIndex == rowIndex)
                        {
                            _properties[rowIndex].SetValue(_editedObject, propertiesDataGridView[1, rowIndex].Value);
                            AnythingChanged = true;
                        }
                    };
            }
            else
            {
                Type type = value.GetType();
                if (type.IsEnum)
                {
                    propertiesDataGridView[1, rowIndex] = new DataGridViewComboBoxCell();
                    ((DataGridViewComboBoxCell)propertiesDataGridView[1, rowIndex]).Items.AddRange(type.GetEnumNames());
                    ((DataGridViewComboBoxCell)propertiesDataGridView[1, rowIndex]).Value = value.ToString();

                    this.propertiesDataGridView.CellValueChanged +=
                        (sender, e) =>
                        {
                            if (e.RowIndex == rowIndex)
                            {
                                _properties[rowIndex].SetValue(_editedObject, Enum.Parse(type, propertiesDataGridView[1, rowIndex].Value.ToString()));
                                AnythingChanged = true;
                            }
                        };
                }
                else
                {
                    propertiesDataGridView[1, rowIndex] = new DataGridViewButtonCell();
                    ((DataGridViewButtonCell)propertiesDataGridView[1, rowIndex]).Value = value;

                    this.propertiesDataGridView.CellClick +=
                        (sender, e) =>
                        {
                            if (e.RowIndex == rowIndex)
                            {
                                object prop = _properties[rowIndex].GetValue(_editedObject);

                                PropertiesEditFlyOut flyout = new PropertiesEditFlyOut(prop);
                                DialogResult result = flyout.Show();

                                if (result != DialogResult.Cancel && flyout.EditedObjectChanged)
                                {
                                    _properties[rowIndex].SetValue(_editedObject, flyout.EditedObject);
                                    AnythingChanged = true;
                                }
                            // TODO zjistit jiné výsledky dialogResult
                            }
                        };
                }
            }
        }

        private void initializeOtherTypeObjectOfArrayCell(int rowIndex, int arrayIndex)
        {
            //initializeOtherObjectTypeCell(rowIndex, String.Format("[{0}]", arrayIndex), ref value);
            // TODO
            object[] array = (object[])_editedObject;
            var value = array[arrayIndex];
            
            if (value is bool)
            {
                propertiesDataGridView[1, rowIndex] = new DataGridViewCheckBoxCell(false);
                ((DataGridViewCheckBoxCell)propertiesDataGridView[1, rowIndex]).Value = value;

                this.propertiesDataGridView.CellEndEdit +=
                    (sender, e) =>
                    {
                        if (e.RowIndex == rowIndex)
                        {
                            array[arrayIndex] = propertiesDataGridView[1, rowIndex].Value;
                            AnythingChanged = true;
                        }
                    };
            }
            else if (value is Color)
            {
                propertiesDataGridView[1, rowIndex] = new DataGridViewButtonCell();
                propertiesDataGridView[1, rowIndex].Value = value;
                ((DataGridViewButtonCell)propertiesDataGridView[1, rowIndex]).Style.BackColor = (Color)value;

                this.propertiesDataGridView.CellClick +=
                    (sender, e) =>
                    {
                        if (e.RowIndex == rowIndex)
                        {
                            ColorDialog colDialog = new ColorDialog();
                            DialogResult result = colDialog.ShowDialog();

                            if (result == DialogResult.OK)
                            {
                                array[arrayIndex] = colDialog.Color;
                                propertiesDataGridView[1, rowIndex].Value = colDialog.Color;
                                ((DataGridViewButtonCell)propertiesDataGridView[1, rowIndex]).Style.BackColor = colDialog.Color;
                                AnythingChanged = true;
                            }
                            // TODO zjistit jiné výsledky dialogResult
                        }
                    };
            }
            else if (value is DateTime)
            {
                //propertiesDataGridView[1, rowIndex].ValueType = typeof(DateTime);
                propertiesDataGridView[1, rowIndex].Value = value;

                this.propertiesDataGridView.CellEndEdit +=
                    (sender, e) =>
                    {
                        if (e.RowIndex == rowIndex)
                        {
                            array[arrayIndex] = DateTime.Parse(propertiesDataGridView[1, rowIndex].Value.ToString());
                            AnythingChanged = true;
                        }
                    };
            }
            else if (value is Point)
            {
                propertiesDataGridView[1, rowIndex].Value = value;

                this.propertiesDataGridView.CellEndEdit +=
                    (sender, e) =>
                    {
                        if (e.RowIndex == rowIndex)
                        {
                            array[arrayIndex] = propertiesDataGridView[1, rowIndex].Value;
                            AnythingChanged = true;
                        }
                    };
            }
            else if (value is PointF)
            {
                propertiesDataGridView[1, rowIndex].Value = value;

                this.propertiesDataGridView.CellEndEdit +=
                    (sender, e) =>
                    {
                        if (e.RowIndex == rowIndex)
                        {
                            array[arrayIndex] = propertiesDataGridView[1, rowIndex].Value;
                            AnythingChanged = true;
                        }
                    };
            }
            else
            {
                Type type = value.GetType();
                if (type.IsEnum)
                {
                    propertiesDataGridView[1, rowIndex] = new DataGridViewComboBoxCell();
                    ((DataGridViewComboBoxCell)propertiesDataGridView[1, rowIndex]).Items.AddRange(type.GetEnumNames());
                    ((DataGridViewComboBoxCell)propertiesDataGridView[1, rowIndex]).Value = value.ToString();

                    this.propertiesDataGridView.CellValueChanged +=
                        (sender, e) =>
                        {
                            if (e.RowIndex == rowIndex)
                            {
                                array[arrayIndex] = Enum.Parse(type, propertiesDataGridView[1, rowIndex].Value.ToString());
                                AnythingChanged = true;
                            }
                        };
                }
                else
                {
                    propertiesDataGridView[1, rowIndex] = new DataGridViewButtonCell();
                    ((DataGridViewButtonCell)propertiesDataGridView[1, rowIndex]).Value = value;

                    this.propertiesDataGridView.CellClick +=
                        (sender, e) =>
                        {
                            if (e.RowIndex == rowIndex)
                            {
                                object prop = _properties[rowIndex].GetValue(_editedObject);

                                PropertiesEditFlyOut flyout = new PropertiesEditFlyOut(prop);
                                DialogResult result = flyout.Show();

                                if (result != DialogResult.Cancel && flyout.EditedObjectChanged)
                                {
                                    array[arrayIndex] = flyout.EditedObject;
                                    AnythingChanged = true;
                                }
                                // TODO zjistit jiné výsledky dialogResult
                            }
                        };
                }
            }
        }          
        

        //private Dictionary<int, PropertiesEditFlyOut> propertiesFlyOuts = new Dictionary<int, PropertiesEditFlyOut>();
        
        public bool AnythingChanged
        {
            get;
            private set;
        }

        //private string[] shownProperties;

        private PropertyInfo[] _properties;
        public PropertyInfo[] Properties
        {
            get { return _properties; }
            set
            {
                _properties = value;
                initializeGrid();
            }
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

        private void propertiesDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                int row = e.RowIndex;
                
            }            
        }

        private void propertiesDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

        }
    }

    internal static partial class Extensions
    {
        public static bool IsDirectlyEditableValueByGrid(this object value)
        {
            Type type = value.GetType();
            return type.IsValueType && type.Name != "Boolean";
        }
    }
}
