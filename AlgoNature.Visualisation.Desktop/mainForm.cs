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

        public mainForm()
        {
            InitializeComponent();

            splitViewPanel.AutoSizeMode = AutoSizeMode.GrowOnly;

            assemblyControls = getAlgoNatureAssemblyUserControlsTypes();

            
            // Assigning controls to the controlsComboBox
            string[] typeStrs = new string[assemblyControls.Length];
            for (int i = 0; i < assemblyControls.Length; i++) typeStrs[i] = assemblyControls[i].FullName;
            controlsComboBox.Items.AddRange(typeStrs);
            
            doFormTranslation();

            controlsComboBox.SelectedIndex = selectedAssemblyControlIndex;
            setMainSplitContainerSplitterDistance();
            Thread.Sleep(100);
            ReinitializeControl();

            Program.MainWindow = this;
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
                }
                catch { }
            }
            return result.ToArray();
        }

        private void ReinitializeControl()
        {
            initializeExportabilityAndIGrowability();

            mainSplitContainer.Panel2.Controls.Clear();
            drawnUserControl = Activator.CreateInstance(assemblyControls[selectedAssemblyControlIndex]);
            mainSplitContainer.Panel2.Controls.Add((Control)drawnUserControl);

            reinitializePropertyGrids();
        }
        

        private void initializeExportabilityAndIGrowability()
        {
            showIGrowableSettings = assemblyControls[selectedAssemblyControlIndex].ImplementsInterface(typeof(IGrowableGraphicChild));
            exportButton.Visible = assemblyControls[selectedAssemblyControlIndex].ImplementsInterface(typeof(IBitmapGraphicChild));
        }

        private void reinitializePropertyGrids()
        {
            //Clear panels' grids
            this.propertiesSplitContainer.Panel1.Controls.Clear();
            this.propertiesSplitContainer.Panel2.Controls.Clear();

            //Determining whether to show IGrowable properties Grid
            if (showIGrowableSettings)
            {
                this.propertiesSplitContainer.SplitterDistance = PROPERTIES_SPLITCONTAINER_SPLITTER_DISTANCE_WHEN_IGROWABLE;
            }
            else
            {
                this.propertiesSplitContainer.SplitterDistance = this.propertiesSplitContainer.Height;
            }

            PropertiesEditorGrid grid = new PropertiesEditorGrid(drawnUserControl,
                new Type[3] { typeof(UserControl), typeof(IGrowableGraphicChild), typeof(IBitmapGraphicChild) }, false);
            //grid.PropertiesEditorGridLoaded += setMainSplitContainerSplitterDistance;
            this.propertiesSplitContainer.Panel1.Controls.Add(grid);
            if (showIGrowableSettings)
            {
                PropertiesEditorGrid grid2 = new PropertiesEditorGrid(drawnUserControl, new Type[1] { typeof(IGrowableGraphicChild) }, true);
                //grid2.PropertiesEditorGridLoaded += setMainSplitContainerSplitterDistance;
                this.propertiesSplitContainer.Panel2.Controls.Add(grid2);
            }
            /*// Initializing own properties
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
            }*/

            doPropertiesGridsTranslation();

            setMainSplitContainerSplitterDistance();

            // Custom docking due to resizing grids container
            Type tt = assemblyControls[selectedAssemblyControlIndex];
            MethodInfo method = tt.GetMethod("DockOnSize");
            method.Invoke(drawnUserControl, new object[] { mainSplitContainer.Panel2.Size });
        }

        //bool refreshedGridViews;
        //private delegate void ThreadStart();
        private void setMainSplitContainerSplitterDistance(object sender) => setMainSplitContainerSplitterDistance();
        private void setMainSplitContainerSplitterDistance()
        {
            Thread.Sleep(100);
            DataGridView propertiesDataGridView = ((PropertiesEditorGrid)propertiesSplitContainer.Panel1.Controls[0]).DisplayedDataGridView;
            int widthToAdd = propertiesDataGridView.Columns[0].Width + propertiesDataGridView.Columns[1].Width - propertiesSplitContainer.Panel1.Width;
            int height = propertiesDataGridView.ColumnHeadersHeight + propertiesDataGridView.RowCount * propertiesDataGridView.Rows[0].Height;
            if (height > propertiesDataGridView.Height) widthToAdd += SCROLLBAR_SIZE;

            if (showIGrowableSettings)
            {
                DataGridView iGrowablePropertiesDataGridView = ((PropertiesEditorGrid)propertiesSplitContainer.Panel2.Controls[0]).DisplayedDataGridView;
                int widthToAdd2 = iGrowablePropertiesDataGridView.Columns[0].Width + iGrowablePropertiesDataGridView.Columns[1].Width - propertiesSplitContainer.Panel2.Width;
                height = iGrowablePropertiesDataGridView.ColumnHeadersHeight + iGrowablePropertiesDataGridView.RowCount * iGrowablePropertiesDataGridView.Rows[0].Height;
                if (height > iGrowablePropertiesDataGridView.Height) widthToAdd += SCROLLBAR_SIZE;

                if (widthToAdd2 > widthToAdd) widthToAdd = widthToAdd2;
            }

            mainSplitContainer.SplitterDistance += widthToAdd;
            //Console.WriteLine(propertiesDataGridView.Height + "   " + nameof(propertiesDataGridView) + " Height");
        }

        private void doFormTranslation()
        {

        }

        private void doPropertiesGridsTranslation()
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
