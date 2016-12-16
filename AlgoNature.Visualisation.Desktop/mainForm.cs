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
using System.Resources;
using AlgoNature.Components;
using System.IO;
using static AlgoNature.Visualisation.Desktop.Extensions;
using System.Collections;

namespace AlgoNature.Visualisation.Desktop
{
    public partial class mainForm : Form
    {
        const int PROPERTIES_SPLITCONTAINER_SPLITTER_DISTANCE_WHEN_IGROWABLE = 387;
        const int SCROLLBAR_SIZE = 20;
        //const string TRANSLATION_RESOURCEFILENAME_WITHOUT_EXTENSION = "mainForm.Translation";

        //private string exportFileDialogFilter;

        public Assembly ANC = Assembly.GetAssembly(typeof(AlgoNature.Components.IGrowableGraphicChild));

        private Type[] assemblyControls;
        private int selectedAssemblyControlIndex = 0;

        private dynamic drawnUserControl;

        private bool showIGrowableSettings;


        FormWindowState _lastWindowState;
        public mainForm()
        {
            InitializeComponent();

            doFormTranslation();

            splitViewPanel.AutoSizeMode = AutoSizeMode.GrowOnly;

            assemblyControls = getAlgoNatureAssemblyUserControlsTypes();

            // Assigning controls to the controlsComboBox and trying to translate them
            string[] typeStrs = new string[assemblyControls.Length];
            for (int i = 0; i < assemblyControls.Length; i++)
            {
                try
                {
                    string trans = (string)assemblyControls[i].GetProperty("NameTranslated").GetValue(null);
                    typeStrs[i] = (trans != null) ? trans : assemblyControls[i].FullName;

                }
                catch
                {
                    typeStrs[i] = assemblyControls[i].FullName;
                }
            }

            controlsComboBox.Items.AddRange(typeStrs);

            controlsComboBox.SelectedIndex = selectedAssemblyControlIndex;
            setMainSplitContainerSplitterDistance();
            Thread.Sleep(100);
            ReinitializeControl();

            _lastWindowState = this.WindowState;

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

        bool initializedResizeHandler = false;
        private void ReinitializeControl()
        {
            manuallyResized = false;
            initializeExportabilityAndIGrowability();

            resetWithoutLosingSettingsButton.Visible = assemblyControls[selectedAssemblyControlIndex].GetMethod("ResetGraphicalAppearanceWithoutLosingSettings") != null;
                //assemblyControls[selectedAssemblyControlIndex].ImplementsInterface(typeof(IResettableGraphicComponentWithoutLosingSettings<>));

            if (!initializedResizeHandler)
            {
                //this.mainSplitContainer.Panel2.Resize += new System.EventHandler(this.mainSplitContainer_Panel2_Resize);
                initializedResizeHandler = true;
            }

            mainSplitContainer.Panel2.Controls.Clear();
            drawnUserControl = Activator.CreateInstance(assemblyControls[selectedAssemblyControlIndex]);
            mainSplitContainer.Panel2.Controls.Add((Control)drawnUserControl);

            reinitializePropertyGrids();
        }


        private void initializeExportabilityAndIGrowability()
        {
            showIGrowableSettings = assemblyControls[selectedAssemblyControlIndex].ImplementsInterface(typeof(IGrowableGraphicChild));
            iGrowableButtonsTableLayoutPanel.Visible = showIGrowableSettings;
            exportButton.Visible = assemblyControls[selectedAssemblyControlIndex].ImplementsInterface(typeof(IBitmapGraphicChild));
        }

        private void reinitializePropertyGrids()
        {
            //Clear panels' grids
            try
            {
                this.propertiesSplitContainer.Panel1.Controls[0].Dispose();
            }
            catch { }
            this.propertiesSplitContainer.Panel1.Controls.Clear();
            try
            {
                this.propertiesSplitContainer.Panel2.Controls[0].Dispose();
            }
            catch { }
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
                new Type[4] { typeof(UserControl), typeof(IGrowableGraphicChild), typeof(IBitmapGraphicChild), typeof(ITranslatable) }, false);
            //grid.PropertiesEditorGridLoaded += setMainSplitContainerSplitterDistance;
            //grid.Resize += setMainSplitContainerSplitterDistance;
            //grid.ColumnWidthChanged += setMainSplitContainerSplitterDistance;
            if (!showIGrowableSettings) grid.Paint += setMainSplitContainerSplitterDistance; // if show, this will be served in the second grid
            this.propertiesSplitContainer.Panel1.Controls.Add(grid);

            PropertiesEditorGrid grid2;
            if (showIGrowableSettings)
            {
                grid2 = new PropertiesEditorGrid(drawnUserControl, new Type[1] { typeof(IGrowableGraphicChild) }, true);
                //grid2.PropertiesEditorGridLoaded += setMainSplitContainerSplitterDistance;
                //grid2.Resize += setMainSplitContainerSplitterDistance;
                //grid.ColumnWidthChanged += setMainSplitContainerSplitterDistance;
                grid2.Paint += setMainSplitContainerSplitterDistance;
                this.propertiesSplitContainer.Panel2.Controls.Add(grid2);
            }

            // Adding resize handlers now so far because adding them before could cause nullReference to iGrowableGrid


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

            //doPropertiesGridsTranslation();

            //setMainSplitContainerSplitterDistance();

            // Docking will be served in view panel's Resize handler
            dockComponent();
        }

        private void dockComponent()
        {
            // Custom docking due to resizing grids container
            Type tt = assemblyControls[selectedAssemblyControlIndex];
            MethodInfo method = tt.GetMethod("DockOnSize");
            method.Invoke(drawnUserControl, new object[] { mainSplitContainer.Panel2.Size });
            //((UserControl)drawnUserControl).Refresh();
        }

        //bool refreshedGridViews;
        //private delegate void ThreadStart();
        private bool settingMainSplitterDistance = false;
        private bool mainSplitterDistanceSetAfterInit = false;
        private void setMainSplitContainerSplitterDistance(object sender, EventArgs e)
        {
            if (!manuallyResized) setMainSplitContainerSplitterDistance();
        }
        private void setMainSplitContainerSplitterDistance()
        {
            //Thread.Sleep(100);
            settingMainSplitterDistance = true;
            DataGridView propertiesDataGridView = ((PropertiesEditorGrid)propertiesSplitContainer.Panel1.Controls[0]).DisplayedDataGridView;
            int widthToAdd = propertiesDataGridView.Columns[0].Width + propertiesDataGridView.Columns[1].Width - propertiesSplitContainer.Panel1.Width;
            int height = propertiesDataGridView.ColumnHeadersHeight + propertiesDataGridView.RowCount * propertiesDataGridView.Rows[0].Height;
            if (height > propertiesDataGridView.Height) widthToAdd += SCROLLBAR_SIZE;

            if (showIGrowableSettings)
            {
                try // trying because if propertiesDataGridView is initialized and iGrowablePropertiesDataGridView not yet, there could be an error
                {
                    DataGridView iGrowablePropertiesDataGridView = ((PropertiesEditorGrid)propertiesSplitContainer.Panel2.Controls[0]).DisplayedDataGridView;
                    int widthToAdd2 = iGrowablePropertiesDataGridView.Columns[0].Width + iGrowablePropertiesDataGridView.Columns[1].Width - propertiesSplitContainer.Panel2.Width;
                    height = iGrowablePropertiesDataGridView.ColumnHeadersHeight + iGrowablePropertiesDataGridView.RowCount * iGrowablePropertiesDataGridView.Rows[0].Height;
                    if (height > iGrowablePropertiesDataGridView.Height) widthToAdd += SCROLLBAR_SIZE;

                    if (widthToAdd2 > widthToAdd) widthToAdd = widthToAdd2;

                    //iGrowablePropertiesDataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; // Is set yet now because everything is served - all needed widths are set
                }
                catch { }
            }

            mainSplitContainer.SplitterDistance += widthToAdd;
            settingMainSplitterDistance = false;

            // Set both grids' last column resize mode to fill - first is set above
            //propertiesDataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            mainSplitterDistanceSetAfterInit = true;
            //Console.WriteLine(propertiesDataGridView.Height + "   " + nameof(propertiesDataGridView) + " Height");
            setAutoColumnSize();
        }

        private void setAutoColumnSize()
        {
            if (showIGrowableSettings) ((PropertiesEditorGrid)propertiesSplitContainer.Panel2.Controls[0]).Paint -= setMainSplitContainerSplitterDistance;
            else ((PropertiesEditorGrid)propertiesSplitContainer.Panel1.Controls[0]).Paint -= setMainSplitContainerSplitterDistance;
            ((PropertiesEditorGrid)propertiesSplitContainer.Panel1.Controls[0]).DisplayedDataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            if (showIGrowableSettings)
                ((PropertiesEditorGrid)propertiesSplitContainer.Panel2.Controls[0]).DisplayedDataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void doFormTranslation()
        {
            System.Resources.ResourceManager RM = AlgoNature.Visualisation.Desktop.mainForm_Translation.ResourceManager;
            
            resetWithoutLosingSettingsButton.Text = RM.TryTranslate(resetWithoutLosingSettingsButton.Text);
            resetToDefaultButton.Text = RM.TryTranslate(/*translationValues, TRANSLATION_RESOURCEFILENAME_WITHOUT_EXTENSION,*/ resetToDefaultButton.Text);
            exportButton.Text = RM.TryTranslate(/*translationValues, TRANSLATION_RESOURCEFILENAME_WITHOUT_EXTENSION,*/ exportButton.Text);
            startGrowingButton.Text = RM.TryTranslate(/*translationValues, TRANSLATION_RESOURCEFILENAME_WITHOUT_EXTENSION,*/ startGrowingButton.Text);
            stopGrowingButton.Text = RM.TryTranslate(/*translationValues, TRANSLATION_RESOURCEFILENAME_WITHOUT_EXTENSION,*/ stopGrowingButton.Text);
            dieButton.Text = RM.TryTranslate(/*translationValues, TRANSLATION_RESOURCEFILENAME_WITHOUT_EXTENSION,*/ dieButton.Text);
            this.Text = RM.TryTranslate(this.Text);
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
            //Console.WriteLine("Resize");
            if (this.WindowState != _lastWindowState)
            {
                this.splitViewPanel.Size = this.Size - new Size(41, 90);
                _lastWindowState = this.WindowState;
                this.ResumeLayout();
                dockComponent();
                ((UserControl)drawnUserControl).Size = ((UserControl)drawnUserControl).Size;
            }
        }

        private bool manuallyResized = false;
        private bool secondTimeDocked = false;
        private void mainSplitContainer_Panel2_Resize(object sender, EventArgs e)
        {
            /*if (!secondTimeDocked)
            {
                secondTimeDocked = true;
            }
            else
            {
                dockComponent();
                secondTimeDocked = false;
            }*/
            if (!movingSplitter) dockComponent();
        }

        private void mainSplitContainer_SplitterMoved(object sender, SplitterEventArgs e)
        {
            setAutoColumnSize();
            movingSplitter = false;
            if (manuallyResized || mainSplitterDistanceSetAfterInit) dockComponent();
            /*else if (mainSplitterDistanceSetAfterInit)
            {
                mainSplitterDistanceSetAfterInit = false;
            }*/
        }

        bool movingSplitter;
        private void mainSplitContainer_SplitterMoving(object sender, SplitterCancelEventArgs e)
        {
            movingSplitter = true;
            if (!settingMainSplitterDistance)
            {
                manuallyResized = true;
                mainSplitterDistanceSetAfterInit = false;
            }
        }

        private void mainForm_Paint(object sender, PaintEventArgs e)
        {

        }

        // Resizing
        private Size tempResizeSize;
        private void mainForm_ResizeBegin(object sender, EventArgs e)
        {
            //Console.WriteLine("ResizeBegin");
            //this.SuspendLayout();
            tempResizeSize = this.Size;
        }
        private void mainForm_ResizeEnd(object sender, EventArgs e)
        {
            //Console.WriteLine("ResizeEnd");
            Size diff = this.Size - tempResizeSize;
            splitViewPanel.Size += diff;
            //this.ResumeLayout();
            dockComponent();
        }

        private void resetWithoutLosingSettingsButton_Click(object sender, EventArgs e)
        {
            assemblyControls[selectedAssemblyControlIndex].GetMethod("ResetGraphicalAppearanceWithoutLosingSettings").Invoke(drawnUserControl, null);
        }
    }
}
