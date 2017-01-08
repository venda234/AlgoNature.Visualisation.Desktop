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
using System.Resources;

namespace AlgoNature.Visualisation.Desktop
{
    public partial class PropertiesEditorGrid : DataGridView
    {
        // Boolean will be displayed other way
        internal const string DIRECTLY_EDITABLE_TYPES_STR = "SByteCharDateTimeDecimalDoubleUInt16UInt32UInt64SingleString";

        public PropertiesEditorGrid()
        {
            InitializeComponent();
            doTranslation();
        }

        public PropertiesEditorGrid(object objWhosePropertiesToDisplay, PropertyInfo[] propertiesToDisplay)
        {
            InitializeComponent();
            doTranslation();
            _editedObject = objWhosePropertiesToDisplay;
            _properties = propertiesToDisplay.Where(new Func<PropertyInfo, bool>((property) => (!property.Name.Contains("Name") && !property.Name.Contains("Translated")))).ToArray();
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

        //public delegate void PropertiesEditorGridLoadedEventHandler(object sender);
        //public event PropertiesEditorGridLoadedEventHandler PropertiesEditorGridLoaded;

        //public void l
        //private PointConverter pc;

        private void doTranslation()
        {
            ResourceManager RM = PropertiesEditorGridAndFlyOut_Translation.ResourceManager;
            propertiesDataGridViewPropertyColumn.HeaderText = RM.TryTranslate(propertiesDataGridViewPropertyColumn.HeaderText);
            propertiesDataGridViewValueColumn.HeaderText = RM.TryTranslate(propertiesDataGridViewValueColumn.HeaderText);
        }


        private void initializeGrid()
        {
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            //allRowsInitialized = false;
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
                        initializeEditedObjectPropertyRow(i, i);
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
            _editingCells = new bool[this.RowCount];

            this.CellBeginEdit +=
                    (sender, e) =>
                    {
                        _editingCells[Convert.ToInt32(((DataGridViewTextBoxCell)this[2, e.RowIndex]).Value)] = true;
                    };
            this.CellContentClick +=
                    (sender, e) =>
                    {
                        if (e.RowIndex >= 0) _editingCells[Convert.ToInt32(((DataGridViewTextBoxCell)this[2, e.RowIndex]).Value)] = true;
                    };

            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout();
            //allRowsInitialized = true;
        }
        //bool allRowsInitialized;

        /// <summary>
        /// Method that initializes an array
        /// </summary>
        private void initializeArrayObject()
        {
            int length = ((object[])_editedObject).Length;
            for (int i = 0; i < length; i++) initializeArrayRow(i, i); 
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
            if (this[2, rowIndex].Value == null) this[2, rowIndex].Value = rowIndex;
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
            if (EditedObject.GetType().ImplementsInterface(typeof(AlgoNature.Components.ITranslatable)))
                this[0, rowIndex].Value = ((AlgoNature.Components.ITranslatable)EditedObject).TryTranslate(propertyName);
            else this[0, rowIndex].Value = propertyName;
            if (this[2, rowIndex].Value == null) this[2, rowIndex].Value = rowIndex;
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
        private void initializeEditedObjectPropertyRow(int rowIndex, int propertiesIndex)
        {
            string propertyName = _properties[propertiesIndex].Name;

            // initialize row
            initializePropertyRowWithPropertyName(rowIndex, propertyName);

            var value = _properties[propertiesIndex].GetValue(_editedObject);
            Type valType = value.GetType();
            //if (DIRECTLY_EDITABLE_TYPES_STR.Contains(value.GetType().Name))
            // If it is directly editable value type, just assign, the dataGridView will manage it itself
            if (value.IsDirectlyEditableValueByGrid()) // Boolean will be displayed as a checkbbox or so
            {
                initializeDirectValuePropertyCell(rowIndex, propertiesIndex);
            }
            else // Displaying other object types
            {
                initializeOtherObjectTypePropertyCell(rowIndex, propertiesIndex);
            }
        }

        bool[] _editingCells;
        /// <summary>
        /// A method for initializing a cell by its row index a value of which is directly editable
        /// </summary>
        /// <param name="rowIndex">Index of the particular DataGridView row</param>
        /// <param name="valueProperty">A property value of which shall be displayed and edited</param>
        private void initializeDirectValuePropertyCell(int rowIndex, int propertiesIndex)
        {
            var value = _properties[propertiesIndex].GetValue(_editedObject);
            this[1, rowIndex].Value = value;

            Type valType = value.GetType();

            this[1, rowIndex].ValueType = valType;

            if (_properties[propertiesIndex].SetMethod?.IsPublic == true)
            {
                // Anonymous method for changing back the value
                this.CellEndEdit +=
                    (sender, e) =>
                    {
                        if (Convert.ToInt32(((DataGridViewTextBoxCell)this[2, e.RowIndex]).Value) == rowIndex && e.ColumnIndex != 0 && _editingCells[rowIndex])
                        {
                            try
                            {
                                _properties[propertiesIndex].SetValue(_editedObject, Convert.ChangeType(this[1, rowIndex].Value, valType));
                            }
                            catch
                            {
                                if (_editedObject is Pen)
                                {
                                    _editedObject = new Pen(((Pen)_editedObject).Color, (float)this[1, rowIndex].Value); // Only width is displayed
                                }
                                else showPropertyUnableToBeSetMessage(_properties[propertiesIndex].Name);
                            }

                            _editingCells[rowIndex] = false;
                            _editingCells[rowIndex] = !(AnythingChanged = true);
                        }
                    };
            }
            else
            {
                this[1, rowIndex].ReadOnly = true; // Don't allow changing the property if it is readonly
                this[0, rowIndex].Style.ForeColor = Color.Gray;
                this[1, rowIndex].Style.ForeColor = Color.Gray;
            }
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
                    if (Convert.ToInt32(((DataGridViewTextBoxCell)this[2, e.RowIndex]).Value) == rowIndex && e.ColumnIndex != 0 && _editingCells[rowIndex])
                    {
                        arr[arrayIndex] = this[1, rowIndex].Value;
                        _editingCells[rowIndex] = !(AnythingChanged = true);
                    }
                };
        }

        private void initializeOtherObjectTypePropertyCell(int rowIndex, int propertiesIndex)
        {
            var value = _properties[propertiesIndex].GetValue(_editedObject);

            /*if (!value.GetType().IsValueType)
            {
                try
                {
                    MethodInfo method = value.GetType().GetMethod("Clone");
                    value = method.Invoke(value, new object[0]);
                }
                catch
                {
                    value = value.CloneObject();
                }
            }*/

            if (_properties[propertiesIndex].SetMethod?.IsPublic != true)
            {
                this[1, rowIndex].ReadOnly = true; // Don't allow changing the property if it is readonly
                this[0, rowIndex].Style.ForeColor = Color.Gray;
                this[1, rowIndex].Style.ForeColor = Color.Gray;
            }


            if (value is bool)
            {
                this[1, rowIndex] = new DataGridViewCheckBoxCell(false);
                ((DataGridViewCheckBoxCell)this[1, rowIndex]).Value = value;

                // Defocus after click - then the CellValueChanged event will be thrown
                this.CellContentClick +=
                    (sender, e) =>
                    {
                        if (e.RowIndex >= 0)
                        if (Convert.ToInt32(((DataGridViewTextBoxCell)this[2, e.RowIndex]).Value) == rowIndex && e.ColumnIndex != 0 && _editingCells[rowIndex])
                        {
                            _previouslyReloadedValue = false;
                            this[0, e.RowIndex].Selected = true;
                        }
                    };
                    
                this.CellValueChanged +=
                    (sender, e) =>
                    {
                        if (Convert.ToInt32(((DataGridViewTextBoxCell)this[2, e.RowIndex]).Value) == rowIndex && e.ColumnIndex != 0 && _editingCells[rowIndex])
                        {
                            if (!_previouslyReloadedValue)
                            {
                                try
                                {
                                    _properties[rowIndex].SetValue(_editedObject, this[1, e.RowIndex].Value);
                                }
                                catch
                                {
                                    if (_editedObject is Pen)
                                    {
                                        _editedObject = new Pen(((Pen)_editedObject).Color, (float)this[1, e.RowIndex].Value);
                                    }
                                    else showPropertyUnableToBeSetMessage(_properties[rowIndex].Name);
                                }

                                _editingCells[rowIndex] = !(AnythingChanged = true);
                            }
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
                        if (e.RowIndex >= 0)
                            if (Convert.ToInt32(((DataGridViewTextBoxCell)this[2, e.RowIndex]).Value) == rowIndex && e.ColumnIndex != 0 /*&& _editingCells[rowIndex]*/)
                        {
                            ColorDialog colDialog = new ColorDialog();
                            DialogResult result = colDialog.ShowDialog();

                            if (result == DialogResult.OK)
                            {
                                _properties[propertiesIndex].SetValue(_editedObject, colDialog.Color);
                                this[1, rowIndex].Value = colDialog.Color;
                                ((DataGridViewButtonCell)this[1, e.RowIndex]).Style.BackColor = colDialog.Color;
                                _editingCells[rowIndex] = !(AnythingChanged = true);
                            }
                            // TODO zjistit jiné výsledky dialogResult
                        }
                    };
            }
            else if (value is SolidBrush)
            {
                this[1, rowIndex] = new DataGridViewButtonCell();
                this[1, rowIndex].Value = ((SolidBrush)value).Color;
                ((DataGridViewButtonCell)this[1, rowIndex]).FlatStyle = FlatStyle.Popup;
                ((DataGridViewButtonCell)this[1, rowIndex]).Style.BackColor = ((SolidBrush)value).Color;

                this.CellContentClick +=
                    (sender, e) =>
                    {
                        if (e.RowIndex >= 0)
                            if (Convert.ToInt32(((DataGridViewTextBoxCell)this[2, e.RowIndex]).Value) == rowIndex && e.ColumnIndex != 0 /*&& _editingCells[rowIndex]*/)
                        {
                            ColorDialog colDialog = new ColorDialog();
                            DialogResult result = colDialog.ShowDialog();

                            if (result == DialogResult.OK)
                            {
                                SolidBrush proprt = (SolidBrush)_properties[propertiesIndex].GetValue(_editedObject);
                                proprt.Color = colDialog.Color;
                                try
                                {
                                    _properties[propertiesIndex].SetValue(_editedObject, proprt);
                                }
                                catch
                                {
                                    if (_editedObject is Pen)
                                    {
                                        _editedObject = new Pen(proprt, ((Pen)_editedObject).Width);
                                    }
                                    else showPropertyUnableToBeSetMessage(_properties[propertiesIndex].Name);
                                }
                                
                                this[1, rowIndex].Value = colDialog.Color;
                                ((DataGridViewButtonCell)this[1, e.RowIndex]).Style.BackColor = colDialog.Color;
                                _editingCells[rowIndex] = !(AnythingChanged = true);
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

                PropertyInfo[] propsToDisplay = new PropertyInfo[2]
                {
                    typeof(Pen).GetProperty("Brush"),
                    typeof(Pen).GetProperty("Width")
                };

                this.CellContentClick +=
                    (sender, e) =>
                    {
                        if (e.RowIndex >= 0)
                            if (Convert.ToInt32(((DataGridViewTextBoxCell)this[2, e.RowIndex]).Value) == rowIndex && e.ColumnIndex != 0 /*&& _editingCells[rowIndex]*/)
                        {
                            /*ColorDialog colDialog = new ColorDialog();
                            DialogResult result = colDialog.ShowDialog();

                            if (result == DialogResult.OK)
                            {
                                Pen proprt = (Pen)_properties[propertiesIndex].GetValue(_editedObject);
                                proprt = (Pen)proprt.Clone();
                                proprt.Color = colDialog.Color;
                                try
                                {
                                    _properties[propertiesIndex].SetValue(_editedObject, proprt);
                                }
                                catch
                                {
                                    showPropertyUnableToBeSetMessage(_properties[propertiesIndex].Name);
                                }

                                this[1, rowIndex].Value = colDialog.Color;
                                ((DataGridViewButtonCell)this[1, e.RowIndex]).Style.BackColor = colDialog.Color;
                                _editingCells[rowIndex] = !(AnythingChanged = true);
                            }*/
                            object prop = _properties[propertiesIndex].GetValue(_editedObject);
                            PropertiesEditFlyOut flyout = (PropertiesEditFlyOut)Activator.CreateInstance(typeof(PropertiesEditFlyOut), prop, propsToDisplay, ((DataGridViewTextBoxCell)this[0, rowIndex]).Value);
                            //propertiesFlyOuts[rowIndex].Name = String.Format("PropertiesEditFlyOut-{0}-{1}", this.Name, rowIndex);

                            flyout.EditingFinished +=
                                (result, editedObjectChanged, editedObject) =>
                                {
                                    if (result != DialogResult.Cancel && editedObjectChanged)
                                    {
                                        _properties[propertiesIndex].SetValue(_editedObject, editedObject);
                                        ((DataGridViewButtonCell)this[1, e.RowIndex]).Style.BackColor = ((Pen)editedObject).Color;
                                        ((DataGridViewButtonCell)this[1, e.RowIndex]).Value = ((Pen)editedObject).Width;
                                        _editingCells[rowIndex] = !(AnythingChanged = true);
                                    }
                                    // TODO zjistit jiné výsledky dialogResult
                                    flyout.Dispose();
                                };

                            flyout.Show();
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
                        if (Convert.ToInt32(((DataGridViewTextBoxCell)this[2, e.RowIndex]).Value) == rowIndex && e.ColumnIndex != 0 && _editingCells[rowIndex])
                        {
                            _properties[propertiesIndex].SetValue(_editedObject, DateTime.Parse(this[1, rowIndex].Value.ToString()));
                            _editingCells[rowIndex] = !(AnythingChanged = true);
                        }
                    };
            }
            else if (value is TimeSpan)
            {
                //this[1, rowIndex].ValueType = typeof(DateTime);
                this[1, rowIndex].Value = value;

                this.CellEndEdit +=
                    (sender, e) =>
                    {
                        if (Convert.ToInt32(((DataGridViewTextBoxCell)this[2, e.RowIndex]).Value) == rowIndex && e.ColumnIndex != 0 && _editingCells[rowIndex])
                        {
                            _properties[propertiesIndex].SetValue(_editedObject, TimeSpan.Parse(this[1, rowIndex].Value.ToString()));
                            _editingCells[rowIndex] = !(AnythingChanged = true);
                        }
                    };
            }
            else if (value is Point)
            {
                this[1, rowIndex].Value = value;

                this.CellEndEdit +=
                    (sender, e) =>
                    {
                        if (Convert.ToInt32(((DataGridViewTextBoxCell)this[2, e.RowIndex]).Value) == rowIndex && e.ColumnIndex != 0 && _editingCells[rowIndex])
                        {
                            _properties[propertiesIndex].SetValue(_editedObject, ((string)this[1, rowIndex].Value).ToPoint());
                            _editingCells[rowIndex] = !(AnythingChanged = true);
                        }
                    };
            }
            else if (value is PointF)
            {
                this[1, rowIndex].Value = value;

                this.CellEndEdit +=
                    (sender, e) =>
                    {
                        if (Convert.ToInt32(((DataGridViewTextBoxCell)this[2, e.RowIndex]).Value) == rowIndex && e.ColumnIndex != 0 && _editingCells[rowIndex])
                        {
                            _properties[propertiesIndex].SetValue(_editedObject, ((string)this[1, rowIndex].Value).ToPointF());
                            _editingCells[rowIndex] = !(AnythingChanged = true);
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
                            if (Convert.ToInt32(((DataGridViewTextBoxCell)this[2, e.RowIndex]).Value) == rowIndex && e.ColumnIndex != 0 && _editingCells[rowIndex])
                            {
                                _properties[propertiesIndex].SetValue(_editedObject, Enum.Parse(type, this[1, e.RowIndex].Value.ToString()));
                                //this[0, e.RowIndex].Selected = true;
                                _editingCells[rowIndex] = !(AnythingChanged = true);
                            }
                        };
                }
                else if (type.BaseType == typeof(UserControl) || type.BaseType.Name.Contains("UserControl"))
                {
                    PropertyInfo[] props = type.GetProperties().FilterPropertiesBasedOnOtherTypes(new Type[1] { typeof(UserControl) }, false);
                    if (type.ImplementsInterface(typeof(AlgoNature.Components.IBitmapGraphicChild)))
                        props = props.FilterPropertiesBasedOnOtherTypes(new Type[1] { typeof(AlgoNature.Components.IBitmapGraphicChild) }, false);

                    this[1, rowIndex] = new DataGridViewButtonCell();
                    ((DataGridViewButtonCell)this[1, rowIndex]).Value = value;

                    this.CellContentClick +=
                        (sender, e) =>
                        {
                            if (e.RowIndex >= 0)
                                if (Convert.ToInt32(((DataGridViewTextBoxCell)this[2, e.RowIndex]).Value) == rowIndex && e.ColumnIndex != 0 /*&& _editingCells[rowIndex]*/)
                                {
                                    object prop = _properties[propertiesIndex].GetValue(_editedObject);

                                    PropertiesEditFlyOut flyout = (PropertiesEditFlyOut)Activator.CreateInstance(typeof(PropertiesEditFlyOut), prop, props, ((DataGridViewTextBoxCell)this[0, rowIndex]).Value);

                                    flyout.EditingFinished +=
                                        (result, editedObjectChanged, editedObject) =>
                                        {
                                            if (result != DialogResult.Cancel && editedObjectChanged)
                                            {
                                                _properties[propertiesIndex].SetValue(_editedObject, editedObject);
                                                _editingCells[rowIndex] = !(AnythingChanged = true);
                                            }
                                            // TODO zjistit jiné výsledky dialogResult
                                            flyout.Dispose();
                                        };

                                    flyout.Show();
                                }
                        };
                }
                else
                {
                    this[1, rowIndex] = new DataGridViewButtonCell();
                    ((DataGridViewButtonCell)this[1, rowIndex]).Value = value;

                    this.CellContentClick +=
                        (sender, e) =>
                        {
                            if (e.RowIndex >= 0)
                                if (Convert.ToInt32(((DataGridViewTextBoxCell)this[2, e.RowIndex]).Value) == rowIndex && e.ColumnIndex != 0 /*&& _editingCells[rowIndex]*/)
                                {
                                    object prop = _properties[propertiesIndex].GetValue(_editedObject);

                                    PropertiesEditFlyOut flyout = (PropertiesEditFlyOut)Activator.CreateInstance(typeof(PropertiesEditFlyOut), prop, ((DataGridViewTextBoxCell)this[0, rowIndex]).Value);

                                    flyout.EditingFinished +=
                                        (result, editedObjectChanged, editedObject) =>
                                        {
                                            if (result != DialogResult.Cancel && editedObjectChanged)
                                            {
                                                _properties[propertiesIndex].SetValue(_editedObject, editedObject);
                                                _editingCells[rowIndex] = !(AnythingChanged = true);
                                            }
                                            // TODO zjistit jiné výsledky dialogResult
                                            flyout.Dispose();
                                        };

                                    flyout.Show();
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

                /*this.CellContentClick +=
                    (sender, e) =>
                    {
                        if (Convert.ToInt32(((DataGridViewTextBoxCell)this[2, e.RowIndex]).Value) == rowIndex && e.ColumnIndex != 0 && _editingCells[rowIndex])
                        {
                            if (!_previouslyReloadedValue)
                            {
                                array[arrayIndex] = this[1, e.RowIndex].Value;
                                this[0, e.RowIndex].Selected = true;
                                _editingCells[rowIndex] = !(AnythingChanged = true);
                            }
                            else _previouslyReloadedValue = false;
                        }
                    };*/
                // Defocus after click - then the CellValueChanged event will be thrown
                this.CellContentClick +=
                    (sender, e) =>
                    {
                        if (e.RowIndex >= 0)
                            if (Convert.ToInt32(((DataGridViewTextBoxCell)this[2, e.RowIndex]).Value) == rowIndex && e.ColumnIndex != 0 && _editingCells[rowIndex])
                        {
                            _previouslyReloadedValue = false;
                            this[0, e.RowIndex].Selected = true;
                        }
                    };

                this.CellValueChanged +=
                    (sender, e) =>
                    {
                        if (Convert.ToInt32(((DataGridViewTextBoxCell)this[2, e.RowIndex]).Value) == rowIndex && e.ColumnIndex != 0 && _editingCells[rowIndex])
                        {
                            if (!_previouslyReloadedValue)
                            {
                                try
                                {
                                    array[arrayIndex] = this[1, e.RowIndex].Value;
                                }
                                catch
                                {
                                    showPropertyUnableToBeSetMessage("[" + arrayIndex + "]");
                                }

                                _editingCells[rowIndex] = !(AnythingChanged = true);
                            }
                        }
                    };
            }
            else if (value is Color)
            {
                this[1, rowIndex] = new DataGridViewButtonCell();
                this[1, rowIndex].Value = value;
                ((DataGridViewButtonCell)this[1, rowIndex]).Style.BackColor = (Color)value;

                this.CellContentClick +=
                    (sender, e) =>
                    {
                        if (e.RowIndex >= 0)
                            if (Convert.ToInt32(((DataGridViewTextBoxCell)this[2, e.RowIndex]).Value) == rowIndex && e.ColumnIndex != 0 /*&& _editingCells[rowIndex]*/)
                        {
                            ColorDialog colDialog = new ColorDialog();
                            DialogResult result = colDialog.ShowDialog();

                            if (result == DialogResult.OK)
                            {
                                array[arrayIndex] = colDialog.Color;
                                this[1, rowIndex].Value = colDialog.Color;
                                ((DataGridViewButtonCell)this[1, e.RowIndex]).Style.BackColor = colDialog.Color;
                                _editingCells[rowIndex] = !(AnythingChanged = true);
                            }
                            // TODO zjistit jiné výsledky dialogResult
                        }
                    };
            }
            else if (value is SolidBrush)
            {
                this[1, rowIndex] = new DataGridViewButtonCell();
                this[1, rowIndex].Value = ((SolidBrush)value).Color;
                ((DataGridViewButtonCell)this[1, rowIndex]).FlatStyle = FlatStyle.Popup;
                ((DataGridViewButtonCell)this[1, rowIndex]).Style.BackColor = ((SolidBrush)value).Color;

                this.CellContentClick +=
                    (sender, e) =>
                    {
                        if (e.RowIndex >= 0)
                            if (Convert.ToInt32(((DataGridViewTextBoxCell)this[2, e.RowIndex]).Value) == rowIndex && e.ColumnIndex != 0 /*&& _editingCells[rowIndex]*/)
                        {
                            ColorDialog colDialog = new ColorDialog();
                            DialogResult result = colDialog.ShowDialog();

                            if (result == DialogResult.OK)
                            {
                                SolidBrush proprt = (SolidBrush)array[arrayIndex];
                                proprt.Color = colDialog.Color;
                                array[arrayIndex] = proprt;
                                this[1, rowIndex].Value = colDialog.Color;
                                ((DataGridViewButtonCell)this[1, e.RowIndex]).Style.BackColor = colDialog.Color;
                                _editingCells[rowIndex] = !(AnythingChanged = true);
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

                PropertyInfo[] propsToDisplay = new PropertyInfo[2]
                {
                    typeof(Pen).GetProperty("Brush"),
                    typeof(Pen).GetProperty("Width")
                };

                this.CellContentClick +=
                    (sender, e) =>
                    {
                        if (e.RowIndex >= 0)
                            if (Convert.ToInt32(((DataGridViewTextBoxCell)this[2, e.RowIndex]).Value) == rowIndex && e.ColumnIndex != 0 /*&& _editingCells[rowIndex]*/)
                        {
                            object prop = array[arrayIndex];
                            PropertiesEditFlyOut flyout = (PropertiesEditFlyOut)Activator.CreateInstance(typeof(PropertiesEditFlyOut), prop, propsToDisplay, ((DataGridViewTextBoxCell)this[0, rowIndex]).Value);
                            //propertiesFlyOuts.Add(rowIndex, (PropertiesEditFlyOut)Activator.CreateInstance(typeof(PropertiesEditFlyOut), prop, propsToDisplay));
                            //propertiesFlyOuts[rowIndex].Name = String.Format("PropertiesEditFlyOut-{0}-{1}", this.Name, rowIndex);

                            flyout.EditingFinished +=
                            (result, editedObjectChanged, editedObject) =>
                                {
                                    if (result != DialogResult.Cancel && editedObjectChanged)
                                    {
                                        array[arrayIndex] = editedObject;
                                        ((DataGridViewButtonCell)this[1, e.RowIndex]).Style.BackColor = ((Pen)editedObject).Color;
                                        ((DataGridViewButtonCell)this[1, e.RowIndex]).Value = ((Pen)editedObject).Width;
                                        _editingCells[rowIndex] = !(AnythingChanged = true);
                                    }
                                    // TODO zjistit jiné výsledky dialogResult
                                    flyout.Dispose();
                                };

                            flyout.Show();
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
                        if (Convert.ToInt32(((DataGridViewTextBoxCell)this[2, e.RowIndex]).Value) == rowIndex && e.ColumnIndex != 0 && _editingCells[rowIndex])
                        {
                            array[arrayIndex] = DateTime.Parse(this[1, rowIndex].Value.ToString());
                            _editingCells[rowIndex] = !(AnythingChanged = true);
                        }
                    };
            }
            else if (value is TimeSpan)
            {
                //this[1, rowIndex].ValueType = typeof(DateTime);
                this[1, rowIndex].Value = value;

                this.CellEndEdit +=
                    (sender, e) =>
                    {
                        if (Convert.ToInt32(((DataGridViewTextBoxCell)this[2, e.RowIndex]).Value) == rowIndex && e.ColumnIndex != 0 && _editingCells[rowIndex])
                        {
                            array[arrayIndex] =  TimeSpan.Parse(this[1, rowIndex].Value.ToString());
                            _editingCells[rowIndex] = !(AnythingChanged = true);
                        }
                    };
            }
            else if (value is Point)
            {
                this[1, rowIndex].Value = value;

                this.CellEndEdit +=
                    (sender, e) =>
                    {
                        if (Convert.ToInt32(((DataGridViewTextBoxCell)this[2, e.RowIndex]).Value) == rowIndex && e.ColumnIndex != 0 && _editingCells[rowIndex])
                        {
                            array[arrayIndex] = this[1, rowIndex].Value;
                            _editingCells[rowIndex] = !(AnythingChanged = true);
                        }
                    };
            }
            else if (value is PointF)
            {
                this[1, rowIndex].Value = value;

                this.CellEndEdit +=
                    (sender, e) =>
                    {
                        if (Convert.ToInt32(((DataGridViewTextBoxCell)this[2, e.RowIndex]).Value) == rowIndex && e.ColumnIndex != 0 && _editingCells[rowIndex])
                        {
                            array[arrayIndex] = this[1, rowIndex].Value;
                            _editingCells[rowIndex] = !(AnythingChanged = true);
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
                            if (Convert.ToInt32(((DataGridViewTextBoxCell)this[2, e.RowIndex]).Value) == rowIndex && e.ColumnIndex != 0 && _editingCells[rowIndex])
                            {
                                array[arrayIndex] = Enum.Parse(type, this[1, e.RowIndex].Value.ToString());
                                _editingCells[rowIndex] = !(AnythingChanged = true);
                            }
                        };
                }
                else if (type.BaseType == typeof(UserControl) || type.BaseType.Name.Contains("UserControl"))
                {
                    PropertyInfo[] props = type.GetProperties().FilterPropertiesBasedOnOtherTypes(new Type[1] { typeof(UserControl) }, false);
                    if (type.ImplementsInterface(typeof(AlgoNature.Components.IBitmapGraphicChild)))
                        props = props.FilterPropertiesBasedOnOtherTypes(new Type[1] { typeof(AlgoNature.Components.IBitmapGraphicChild) }, false);

                    this[1, rowIndex] = new DataGridViewButtonCell();
                    ((DataGridViewButtonCell)this[1, rowIndex]).Value = value;

                    this.CellContentClick +=
                        (sender, e) =>
                        {
                            if (e.RowIndex >= 0)
                                if (Convert.ToInt32(((DataGridViewTextBoxCell)this[2, e.RowIndex]).Value) == rowIndex && e.ColumnIndex != 0 /*&& _editingCells[rowIndex]*/)
                                {
                                    object prop = array[arrayIndex];

                                    PropertiesEditFlyOut flyout = (PropertiesEditFlyOut)Activator.CreateInstance(typeof(PropertiesEditFlyOut), prop, props, ((DataGridViewTextBoxCell)this[0, rowIndex]).Value);

                                    flyout.EditingFinished +=
                                        (result, editedObjectChanged, editedObject) =>
                                        {
                                            if (result != DialogResult.Cancel && editedObjectChanged)
                                            {
                                                array[arrayIndex] = editedObject;
                                                _editingCells[rowIndex] = !(AnythingChanged = true);
                                            }
                                            // TODO zjistit jiné výsledky dialogResult
                                            flyout.Dispose();
                                        };

                                    flyout.Show();
                                }
                        };
                }
                else
                {
                    this[1, rowIndex] = new DataGridViewButtonCell();
                    ((DataGridViewButtonCell)this[1, rowIndex]).Value = value;

                    this.CellContentClick +=
                        (sender, e) =>
                        {
                            if (e.RowIndex >= 0)
                                if (Convert.ToInt32(((DataGridViewTextBoxCell)this[2, e.RowIndex]).Value) == rowIndex && e.ColumnIndex != 0 /*&& _editingCells[rowIndex]*/)
                                {
                                    object prop = array[arrayIndex];
                                    PropertiesEditFlyOut flyout = (PropertiesEditFlyOut)Activator.CreateInstance(typeof(PropertiesEditFlyOut), prop, ((DataGridViewTextBoxCell)this[0, rowIndex]).Value);
                                    //propertiesFlyOuts.Add(rowIndex, (PropertiesEditFlyOut)Activator.CreateInstance(typeof(PropertiesEditFlyOut), prop));

                                    flyout.EditingFinished +=
                                        (result, editedObjectChanged, editedObject) =>
                                        {
                                            if (result != DialogResult.Cancel && editedObjectChanged)
                                            {
                                                array[arrayIndex] = editedObject;
                                                _editingCells[rowIndex] = !(AnythingChanged = true);
                                            }
                                            // TODO zjistit jiné výsledky dialogResult
                                            flyout.Dispose();
                                        };

                                    flyout.Show();
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

        public DataGridView DisplayedDataGridView
        {
            get { return this; }
        }

        private void PropertiesEditorGrid_Paint(object sender, PaintEventArgs e)
        {
            //if (allRowsInitialized) PropertiesEditorGridLoaded(this); // Event showing its parent that the grid is loaded
        }

        private void showPropertyUnableToBeSetMessage(string propertyName)
        {
            MessageBox.Show(Program.MainWindow, String.Format(PropertiesEditorGridAndFlyOut_Translation.unableToEditMessageText, propertyName), 
                PropertiesEditorGridAndFlyOut_Translation.unableToEditMessageHeader);
        }

        private bool _previouslyReloadedValue = false;
        private void PropertiesEditorGrid_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex != this.SelectedCells[0].RowIndex && e.RowIndex >= 0)
            {
                _previouslyReloadedValue = true;
                if (_editedObject.GetType().IsArray)
                {
                    initializeArrayRow(e.RowIndex, Convert.ToInt32(this[2, e.RowIndex].Value));
                }
                else
                {
                    initializeEditedObjectPropertyRow(e.RowIndex, Convert.ToInt32(this[2, e.RowIndex].Value));
                }
            }
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
