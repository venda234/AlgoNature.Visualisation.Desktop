using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using System.Reflection;

namespace AlgoNature.Visualisation.Desktop
{
    public partial class PropertiesEditorGrid : DataGridView
    {
        // Boolean will be displayed other way
        internal const string DIRECTLY_EDITABLE_TYPES_STR = "SByteCharDateTimeDecimalDoubleUInt16UInt32UInt64SingleString";

        public PropertiesEditorGrid(object objWhosePropertiesToDisplay, PropertyInfo[] propertiesToDisplay)
        {
            InitializeComponent();
            _editedObject = objWhosePropertiesToDisplay;
            _properties = propertiesToDisplay;
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

        public delegate void PropertiesEditorGridLoadedEventHandler(object sender);
        public event PropertiesEditorGridLoadedEventHandler PropertiesEditorGridLoaded;

        private PointConverter pc;

        private void initializeGrid()
        {
            allRowsInitialized = false;
            if (_editedObject.GetType().IsArray) // Edited object is an array
            {
                initializeArrayObject();
            }
            else // Edited object is a regular object
            {
                this.Rows.Clear();
                this.RowCount = _properties.Length;
                for (int i = 0; i < _properties.Length; i++)
                {
                    try // kdyby nešla načíst vlastnost
                    {
                        initializeEditedObjectPropertyRow(i, _properties[i]);
                    }
                    catch // neřešit řádek, odebrat vlastnost ze seznamu
                    {
                        List<PropertyInfo> props = _properties.ToList();
                        props.RemoveAt(i);
                        _properties = props.ToArray();
                        i--;
                    }
                }
                this.RowCount = (_properties.Length > 0) ? _properties.Length : 1; // Again if any property was omitted
            }
            allRowsInitialized = true;
        }
        bool allRowsInitialized;

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
            if (rowIndex + 1 > this.RowCount)
            {
                this.RowCount = rowIndex + 1;
            }

            this[0, rowIndex].ReadOnly = true;
            this[0, rowIndex].ValueType = typeof(string);
            this[0, rowIndex].Value = propertyName;
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
            this[1, rowIndex].Value = value;
            this[1, rowIndex].ValueType = value.GetType();

            if ((bool)(_properties[rowIndex].SetMethod?.IsPublic))
            {
                // Anonymous method for changing back the value
                this.CellEndEdit +=
                    (sender, e) =>
                    {
                        if (e.RowIndex == rowIndex)
                        {
                            _properties[rowIndex].SetValue(_editedObject, this[1, rowIndex].Value);
                        }
                    };
            }
            else this[1, rowIndex].ReadOnly = true; // Don't allow changing the property if it is readonly
        }

        private void initializeDirectValueObjectOfArrayCell(int rowIndex, int arrayIndex)
        {
            object[] arr = (object[])_editedObject;
            this[1, rowIndex].Value = arr[arrayIndex];
            this[1, rowIndex].ValueType = arr[arrayIndex].GetType();

            // Anonymous method for changing back the value
            this.CellEndEdit +=
                (sender, e) =>
                {
                    if (e.RowIndex == rowIndex)
                    {
                        arr[arrayIndex] = this[1, rowIndex].Value;
                        AnythingChanged = true;
                    }
                };
        }

        private void initializeOtherObjectTypePropertyCell(int rowIndex, PropertyInfo valueProperty)
        {
            var value = valueProperty.GetValue(_editedObject);
            
            if (value is bool)
            {
                this[1, rowIndex] = new DataGridViewCheckBoxCell(false);
                ((DataGridViewCheckBoxCell)this[1, rowIndex]).Value = value;
                    
                this.CellContentClick +=
                    (sender, e) =>
                    {
                        if (e.RowIndex == rowIndex)
                        {
                            _properties[rowIndex].SetValue(_editedObject, this[1, rowIndex].Value);
                            AnythingChanged = true;
                        }
                    };
            }
            else if (value is Color)
            {
                this[1, rowIndex] = new DataGridViewButtonCell();
                this[1, rowIndex].Value = value;
                ((DataGridViewButtonCell)this[1, rowIndex]).FlatStyle = FlatStyle.Popup;
                ((DataGridViewButtonCell)this[1, rowIndex]).Style.BackColor = (Color)value;

                this.CellContentClick +=
                    (sender, e) =>
                    {
                        if (e.RowIndex == rowIndex)
                        {
                            ColorDialog colDialog = new ColorDialog();
                            DialogResult result = colDialog.ShowDialog();

                            if (result == DialogResult.OK)
                            {
                                _properties[rowIndex].SetValue(_editedObject, colDialog.Color);
                                this[1, rowIndex].Value = colDialog.Color;
                                ((DataGridViewButtonCell)this[1, rowIndex]).Style.BackColor = colDialog.Color;
                                AnythingChanged = true;
                            }
                            // TODO zjistit jiné výsledky dialogResult
                        }
                    };
            }
            else if (value is SolidBrush)
            {
                this[1, rowIndex] = new DataGridViewButtonCell();
                this[1, rowIndex].Value = value;
                ((DataGridViewButtonCell)this[1, rowIndex]).FlatStyle = FlatStyle.Popup;
                ((DataGridViewButtonCell)this[1, rowIndex]).Style.BackColor = ((SolidBrush)value).Color;

                this.CellContentClick +=
                    (sender, e) =>
                    {
                        if (e.RowIndex == rowIndex)
                        {
                            ColorDialog colDialog = new ColorDialog();
                            DialogResult result = colDialog.ShowDialog();

                            if (result == DialogResult.OK)
                            {
                                SolidBrush proprt = (SolidBrush)_properties[rowIndex].GetValue(_editedObject);
                                proprt.Color = colDialog.Color;
                                _properties[rowIndex].SetValue(_editedObject, proprt);
                                this[1, rowIndex].Value = colDialog.Color;
                                ((DataGridViewButtonCell)this[1, rowIndex]).Style.BackColor = colDialog.Color;
                                AnythingChanged = true;
                            }
                                // TODO zjistit jiné výsledky dialogResult
                        }
                    };
            }
            else if (value is Pen)
            {
                this[1, rowIndex] = new DataGridViewButtonCell();
                this[1, rowIndex].Value = ((Pen)value).Width;
                ((DataGridViewButtonCell)this[1, rowIndex]).FlatStyle = FlatStyle.Popup;
                ((DataGridViewButtonCell)this[1, rowIndex]).Style.BackColor = ((Pen)value).Color;

                this.CellContentClick +=
                    (sender, e) =>
                    {
                        if (e.RowIndex == rowIndex)
                        {
                            object prop = _properties[rowIndex].GetValue(_editedObject);
                            propertiesFlyOuts.Add(rowIndex, new PropertiesEditFlyOut(prop));
                            DialogResult result = propertiesFlyOuts[rowIndex].Show();

                            if (result != DialogResult.Cancel && propertiesFlyOuts[rowIndex].EditedObjectChanged)
                            {
                                _properties[rowIndex].SetValue(_editedObject, propertiesFlyOuts[rowIndex].EditedObject);
                                ((DataGridViewButtonCell)this[1, rowIndex]).Style.BackColor = ((Pen)propertiesFlyOuts[rowIndex].EditedObject).Color;
                                AnythingChanged = true;
                            }
                            // TODO zjistit jiné výsledky dialogResult
                        }
                    };
            }
            else if (value is DateTime)
            {
                //this[1, rowIndex].ValueType = typeof(DateTime);
                this[1, rowIndex].Value = value;

                this.CellEndEdit +=
                    (sender, e) =>
                    {
                        if (e.RowIndex == rowIndex)
                        {
                            _properties[rowIndex].SetValue(_editedObject, DateTime.Parse(this[1, rowIndex].Value.ToString()));
                            AnythingChanged = true;
                        }
                    };
            }
            else if (value is Point)
            {
                this[1, rowIndex].Value = value;

                this.CellEndEdit +=
                    (sender, e) =>
                    {
                        if (e.RowIndex == rowIndex)
                        {
                            _properties[rowIndex].SetValue(_editedObject, ((string)this[1, rowIndex].Value).ToPoint());
                            AnythingChanged = true;
                        }
                    };
            }
            else if (value is PointF)
            {
                this[1, rowIndex].Value = value;

                this.CellEndEdit +=
                    (sender, e) =>
                    {
                        if (e.RowIndex == rowIndex)
                        {
                            _properties[rowIndex].SetValue(_editedObject, ((string)this[1, rowIndex].Value).ToPointF());
                            AnythingChanged = true;
                        }
                    };
            }
            else
            {
                Type type = value.GetType();
                if (type.IsEnum)
                {
                    this[1, rowIndex] = new DataGridViewComboBoxCell();
                    ((DataGridViewComboBoxCell)this[1, rowIndex]).Items.AddRange(type.GetEnumNames());
                    ((DataGridViewComboBoxCell)this[1, rowIndex]).Value = value.ToString();

                    this.CellValueChanged +=
                        (sender, e) =>
                        {
                            if (e.RowIndex == rowIndex)
                            {
                                _properties[rowIndex].SetValue(_editedObject, Enum.Parse(type, this[1, rowIndex].Value.ToString()));
                                AnythingChanged = true;
                            }
                        };
                }
                else
                {
                    this[1, rowIndex] = new DataGridViewButtonCell();
                    ((DataGridViewButtonCell)this[1, rowIndex]).Value = value;

                    this.CellClick +=
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
                this[1, rowIndex] = new DataGridViewCheckBoxCell(false);
                ((DataGridViewCheckBoxCell)this[1, rowIndex]).Value = value;

                this.CellEndEdit +=
                    (sender, e) =>
                    {
                        if (e.RowIndex == rowIndex)
                        {
                            array[arrayIndex] = this[1, rowIndex].Value;
                            AnythingChanged = true;
                        }
                    };
            }
            else if (value is Color)
            {
                this[1, rowIndex] = new DataGridViewButtonCell();
                this[1, rowIndex].Value = value;
                ((DataGridViewButtonCell)this[1, rowIndex]).Style.BackColor = (Color)value;

                this.CellClick +=
                    (sender, e) =>
                    {
                        if (e.RowIndex == rowIndex)
                        {
                            ColorDialog colDialog = new ColorDialog();
                            DialogResult result = colDialog.ShowDialog();

                            if (result == DialogResult.OK)
                            {
                                array[arrayIndex] = colDialog.Color;
                                this[1, rowIndex].Value = colDialog.Color;
                                ((DataGridViewButtonCell)this[1, rowIndex]).Style.BackColor = colDialog.Color;
                                AnythingChanged = true;
                            }
                            // TODO zjistit jiné výsledky dialogResult
                        }
                    };
            }
            else if (value is DateTime)
            {
                //this[1, rowIndex].ValueType = typeof(DateTime);
                this[1, rowIndex].Value = value;

                this.CellEndEdit +=
                    (sender, e) =>
                    {
                        if (e.RowIndex == rowIndex)
                        {
                            array[arrayIndex] = DateTime.Parse(this[1, rowIndex].Value.ToString());
                            AnythingChanged = true;
                        }
                    };
            }
            else if (value is Point)
            {
                this[1, rowIndex].Value = value;

                this.CellEndEdit +=
                    (sender, e) =>
                    {
                        if (e.RowIndex == rowIndex)
                        {
                            array[arrayIndex] = this[1, rowIndex].Value;
                            AnythingChanged = true;
                        }
                    };
            }
            else if (value is PointF)
            {
                this[1, rowIndex].Value = value;

                this.CellEndEdit +=
                    (sender, e) =>
                    {
                        if (e.RowIndex == rowIndex)
                        {
                            array[arrayIndex] = this[1, rowIndex].Value;
                            AnythingChanged = true;
                        }
                    };
            }
            else
            {
                Type type = value.GetType();
                if (type.IsEnum)
                {
                    this[1, rowIndex] = new DataGridViewComboBoxCell();
                    ((DataGridViewComboBoxCell)this[1, rowIndex]).Items.AddRange(type.GetEnumNames());
                    ((DataGridViewComboBoxCell)this[1, rowIndex]).Value = value.ToString();

                    this.CellValueChanged +=
                        (sender, e) =>
                        {
                            if (e.RowIndex == rowIndex)
                            {
                                array[arrayIndex] = Enum.Parse(type, this[1, rowIndex].Value.ToString());
                                AnythingChanged = true;
                            }
                        };
                }
                else
                {
                    this[1, rowIndex] = new DataGridViewButtonCell();
                    ((DataGridViewButtonCell)this[1, rowIndex]).Value = value;

                    this.CellClick +=
                        (sender, e) =>
                        {
                            if (e.RowIndex == rowIndex)
                            {
                                object prop = _properties[rowIndex].GetValue(_editedObject);

                                propertiesFlyOuts.Add(rowIndex, new PropertiesEditFlyOut(prop));
                                DialogResult result = propertiesFlyOuts[rowIndex].Show();

                                if (result != DialogResult.Cancel && propertiesFlyOuts[rowIndex].EditedObjectChanged)
                                {
                                    array[arrayIndex] = propertiesFlyOuts[rowIndex].EditedObject;
                                    AnythingChanged = true;
                                }
                                // TODO zjistit jiné výsledky dialogResult
                            }
                        };
                }
            }
        }          
        

        private Dictionary<int, PropertiesEditFlyOut> propertiesFlyOuts = new Dictionary<int, PropertiesEditFlyOut>();
        
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

        public DataGridView DisplayedDataGridView
        {
            get { return this; }
        }

        private void PropertiesEditorGrid_Paint(object sender, PaintEventArgs e)
        {
            if (allRowsInitialized) PropertiesEditorGridLoaded(this); // Event showing its parent that the grid is loaded
        }
    }

    internal static partial class Extensions
    {
        public static bool IsDirectlyEditableValueByGrid(this object value)
        {
            Type type = value.GetType();
            return PropertiesEditorGrid.DIRECTLY_EDITABLE_TYPES_STR.Contains(type.Name);
        }
    }
}
