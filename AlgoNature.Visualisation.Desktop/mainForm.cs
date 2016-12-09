using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using AlgoNature.Components;
using static AlgoNature.Visualisation.Desktop.Extensions;

namespace AlgoNature.Visualisation.Desktop
{
    public partial class mainForm : Form
    {
        const int PROPERTIES_SPLITCONTAINER_SPLITTER_DISTANCE_WHEN_IGROWABLE = 409;
        const int SCROLLBAR_SIZE = 20;

        //private string exportFileDialogFilter;

        public Assembly ANC = Assembly.GetAssembly(typeof(AlgoNature.Components.IGrowableGraphicChild));

        private Type[] assemblyControls;
        private int selectedAssemblyControlIndex = 0;

        private dynamic drawnUserControl;

        private bool showIGrowableSettings;

        //private string[] AllProperties; // All properties of the control
        private string[] UserControlProperties;
        private string[] OwnNotIGrowableProperties;
        //private Type[] OwnNotIGrowablePropertiesTypes;
        private string[] IGrowableProperties;
        //private Type[] IGrowablePropertiesTypes;

        public mainForm()
        {
            InitializeComponent();

            splitViewPanel.AutoSizeMode = AutoSizeMode.GrowOnly;

            assemblyControls = getAlgoNatureAssemblyUserControlsTypes();

            initializeDefaultPropertyArrays();

            // Assigning controls to the controlsComboBox
            string[] typeStrs = new string[assemblyControls.Length];
            for (int i = 0; i < assemblyControls.Length; i++) typeStrs[i] = assemblyControls[i].FullName;
            controlsComboBox.Items.AddRange(typeStrs);
            
            doFormTranslation();

            controlsComboBox.SelectedIndex = selectedAssemblyControlIndex;
            setMainSplitContainerSplitterDistance();
            Thread.Sleep(100);
            ReinitializeControl();
        }

        private void ReinitializeControl()
        {
            initializeSelectedComponentPropertyArrays();

            mainSplitContainer.Panel2.Controls.Clear();
            drawnUserControl = Activator.CreateInstance(assemblyControls[selectedAssemblyControlIndex]);
            mainSplitContainer.Panel2.Controls.Add((Control)drawnUserControl);

            reinitializePropertyTables();
        }
        

        private void initializeDefaultPropertyArrays()
        {
            // IGrowable
            PropertyInfo[] properties = typeof(AlgoNature.Components.IGrowableGraphicChild).GetProperties();
            IGrowableProperties = new string[properties.Length];
            for (int i = 0; i < properties.Length; i++) IGrowableProperties[i] = properties[i].Name;

            // UserControl
            properties = typeof(UserControl).GetProperties();
            UserControlProperties = new string[properties.Length];
            for (int i = 0; i < properties.Length; i++) UserControlProperties[i] = properties[i].Name;
        }

        private void initializeSelectedComponentPropertyArrays()
        {
            showIGrowableSettings = assemblyControls[selectedAssemblyControlIndex].ImplementsInterface(typeof(IGrowableGraphicChild));
            exportButton.Visible = assemblyControls[selectedAssemblyControlIndex].ImplementsInterface(typeof(IBitmapGraphicChild));

            PropertyInfo[] properties = assemblyControls[selectedAssemblyControlIndex].GetProperties();

            List<string> ownProps = new List<string>();
            string name;
            foreach (PropertyInfo propInfo in properties)
            {
                name = propInfo.Name;
                
                if (!IGrowableProperties.Contains(name) && !UserControlProperties.Contains(name))
                {
                    ownProps.Add(name);
                }
            }
            OwnNotIGrowableProperties = ownProps.ToArray();
        }

        private Type[] getAlgoNatureAssemblyUserControlsTypes()
        {
            List<Type> result = new List<Type>();
            
            Type[] types = ANC.GetTypes();

            foreach (Type type in types)
            {
                try
                {
                    if (type.BaseType.Name == typeof(DockableUserControl<>).Name)
                    {
                        result.Add(type);
                    }
                } catch { }                
            }

            //Console.WriteLine(Assembly.GetAssembly(typeof(AlgoNature.Components.IGrowableGraphicChild)));
            //Console.WriteLine()
            return result.ToArray();
        }

        private void reinitializePropertyTables()
        {
            //refreshedGridViews = false;
            // Clearing old tables
            // propertiesDataGridView
            this.propertiesDataGridView.Rows.Clear();
            // iGrowablePropertiesDataGridView
            this.iGrowablePropertiesDataGridView.Rows.Clear();

            //Determining whether to show IGrowable properties table
            if (showIGrowableSettings)
            {
                this.propertiesSplitContainer.SplitterDistance = PROPERTIES_SPLITCONTAINER_SPLITTER_DISTANCE_WHEN_IGROWABLE;
                this.iGrowablePropertiesDataGridView.Visible = true;
            }
            else
            {
                this.propertiesSplitContainer.SplitterDistance = 0;
                this.iGrowablePropertiesDataGridView.Visible = false;
            }

            // Initializing own properties
            propertiesDataGridView.RowCount = OwnNotIGrowableProperties.Length;
            for (int i = 0; i < OwnNotIGrowableProperties.Length; i++)
            {
                this.propertiesDataGridView[0, i].Value = OwnNotIGrowableProperties[i];
                this.propertiesDataGridView[1, i].Value = drawnUserControl.GetType().GetProperty(OwnNotIGrowableProperties[i]).GetValue(drawnUserControl);
                this.propertiesDataGridView[1, i].ValueType = this.propertiesDataGridView[1, i].Value.GetType();
                Console.WriteLine(this.propertiesDataGridView[1, i].Value.GetType().Name);
                //PropertyInfo b;
                
            }

            // Initializing IGrowable properties
            if (showIGrowableSettings)
            {
                iGrowablePropertiesDataGridView.RowCount = IGrowableProperties.Length;
                for (int i = 0; i < IGrowableProperties.Length; i++)
                {
                    this.iGrowablePropertiesDataGridView[0, i].Value = IGrowableProperties[i];
                    this.iGrowablePropertiesDataGridView[1, i].Value = drawnUserControl.GetType().GetProperty(IGrowableProperties[i]).GetValue(drawnUserControl);
                    this.iGrowablePropertiesDataGridView[1, i].ValueType = this.iGrowablePropertiesDataGridView[1, i].Value.GetType();
                }
            }

            doPropertiesTablesTranslation();

            setMainSplitContainerSplitterDistance();

            Type tt = assemblyControls[selectedAssemblyControlIndex];
            MethodInfo method = tt.GetMethod("DockOnSize");
                             //.MakeGenericMethod(new Type[] { tt });
            method.Invoke(drawnUserControl, new object[] { mainSplitContainer.Panel2.Size });
        }

        //bool refreshedGridViews;
        private delegate void ThreadStart();
        private void setMainSplitContainerSplitterDistance()
        {
            Thread.Sleep(100);
            int widthToAdd = propertiesDataGridView.Columns[0].Width + propertiesDataGridView.Columns[1].Width - propertiesSplitContainer.Panel1.Width;
            int height = propertiesDataGridView.ColumnHeadersHeight + propertiesDataGridView.RowCount * propertiesDataGridView.Rows[0].Height;
            if (height > propertiesDataGridView.Height) widthToAdd += SCROLLBAR_SIZE;
            mainSplitContainer.SplitterDistance += widthToAdd;
            //Console.WriteLine(propertiesDataGridView.Height + "   " + nameof(propertiesDataGridView) + " Height");
        }

        private void doFormTranslation()
        {

        }

        private void doPropertiesTablesTranslation()
        {

        }

        private void controlsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedAssemblyControlIndex = ((ComboBox)sender).SelectedIndex;

            ReinitializeControl();
        }

        private void iGrowablePropertiesDataGridView_Paint(object sender, PaintEventArgs e)
        {
            setMainSplitContainerSplitterDistance();
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            ReinitializeControl();
        }

        private void startGrowingButton_Click(object sender, EventArgs e)
        {
            ((IGrowableGraphicChild)drawnUserControl).Revive();
        }

        private void stopGrowingButton_Click(object sender, EventArgs e)
        {
            ((IGrowableGraphicChild)drawnUserControl).StopGrowing();
        }

        private void dieButton_Click(object sender, EventArgs e)
        {
            ((IGrowableGraphicChild)drawnUserControl).Die();
        }

        private void exportButton_Click(object sender, EventArgs e)
        {
            DialogResult res = exportImageDialog.ShowDialog();
            if (res == DialogResult.OK)
            {
                if (showIGrowableSettings) ((IGrowableGraphicChild)drawnUserControl).StopGrowing();
                ((IBitmapGraphicChild)drawnUserControl).GetItselfBitmap().Save(exportImageDialog.FileName, getSelectedImageFormat());
                if (showIGrowableSettings) ((IGrowableGraphicChild)drawnUserControl).Revive();
            }
        }

        private ImageFormat getSelectedImageFormat()
        {
            switch (exportImageDialog.FilterIndex)
            {
                case 2: 
                    return ImageFormat.Bmp;
                case 3:
                    return ImageFormat.Jpeg;
                case 1:
                default:
                    return ImageFormat.Png;
            }
        }

        private void mainForm_Resize(object sender, EventArgs e)
        {

        }
    }
}
