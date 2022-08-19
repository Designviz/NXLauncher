using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NXLauncher
{
    /// <summary>
    /// Interaction logic for NXParamsWindow.xaml
    /// </summary>
    /// 

    public partial class NXParamsWindow : Window
    {
        NXInstallation? installation;
        NXConfigProfile? editingProfile = null;
        NXParam? selectedParam = null;
        int index = -1;
        bool isInit = false;
        bool isEditing = false;
        ParamVType vtype = ParamVType.VVALUE;
        public NXParamsWindow()
        {
            InitializeComponent();
            installation = new NXInstallation();
            isInit = true;
            editingProfile = new NXConfigProfile();
            Initialized += NXParamsWindow_Initialized;

        }



        private void NXParamsWindow_Initialized(object? sender, EventArgs e)
        {

        }

        public NXParamsWindow(NXConfigProfile p, NXInstallation i)
        {
            InitializeComponent();
            installation = i;
            ParametersList.ItemsSource = i.parameters.Params;
            editingProfile = p;
            isInit = true;
            Initialized += NXParamsWindow_Initialized;
        }

        public NXParamsWindow(NXConfigProfile p, NXInstallation i, NXParam pm)
        {
            InitializeComponent();
            installation = i;
            ParametersList.ItemsSource = i.parameters.Params;           
            editingProfile = p;
            isInit = true;
            if (pm != null)
            {
                
                ParametersList.SelectedItem = i.parameters.Params.Find(pa => pa.Name.Equals(pm.Name));
                vtype = pm.VType;
                isEditing = true;
                index = editingProfile.paramList.IndexOf(pm);
                selectedParam = pm;
            }

            SourceInitialized += NXParamsWindow_SourceInitialized;
        }

        private void NXParamsWindow_SourceInitialized(object? sender, EventArgs e)
        {
            if (selectedParam == null)
                return;
            Add_Button.Content = "Update";
            ParamName_Box.Text = selectedParam.Name;
            ParamValue_TextBox.Text = selectedParam.Value;
            ParamValue_FolderBox.Text = selectedParam.Value;
            ParamValue_FileBox.Text = selectedParam.Value;
            switch (vtype)
            {
                case ParamVType.VVALUE:
                    ParamValue_Radio_Checked(null, null);
                    break;
                case ParamVType.VFOLDER:
                    ParamFolder_Radio_Checked(null, null);
                    break;
                case ParamVType.VFILE:
                    ParamFile_Radio_Checked(null, null);
                    break;
                case ParamVType.VBOOLEAN:
                    ParamBool_Radio_Checked(null, null);
                    bool result = false;
                    bool.TryParse(selectedParam.Value, out result);
                    if (result)
                    {
                        ParamValue_ComboBox.SelectedIndex = 0;
                    }
                    else
                    {
                        ParamValue_ComboBox.SelectedIndex = 1;
                    }
                    break;
            }
        }

        private void NXParamsWindow_Loaded(object sender, RoutedEventArgs e)
        {
 
        }

        private void ParamValue_Radio_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInit)
                return;
            ParamValue_TextBox.IsEnabled = true;
            ParamValue_TextBox.Visibility = Visibility.Visible;
            File_VTYPE.IsEnabled = false;
            File_VTYPE.Visibility = Visibility.Collapsed;
            Folder_VTYPE.IsEnabled = false;
            Folder_VTYPE.Visibility = Visibility.Collapsed;
            Folder_VTYPE_Bool.IsEnabled = false;
            Folder_VTYPE_Bool.Visibility = Visibility.Collapsed;
            vtype = ParamVType.VVALUE;
        }

        private void ParamFile_Radio_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInit)
                return;
            ParamValue_TextBox.IsEnabled = false;
            ParamValue_TextBox.Visibility = Visibility.Collapsed;
            File_VTYPE.IsEnabled = true;
            File_VTYPE.Visibility = Visibility.Visible;
            Folder_VTYPE.IsEnabled = false;
            Folder_VTYPE.Visibility = Visibility.Collapsed;
            Folder_VTYPE_Bool.IsEnabled = false;
            Folder_VTYPE_Bool.Visibility = Visibility.Collapsed;
            vtype = ParamVType.VFILE;
        }

        private void ParamFolder_Radio_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInit)
                return;
            ParamValue_TextBox.IsEnabled = false;
            ParamValue_TextBox.Visibility = Visibility.Collapsed;
            File_VTYPE.IsEnabled = false;
            File_VTYPE.Visibility = Visibility.Collapsed;
            Folder_VTYPE.IsEnabled = true;
            Folder_VTYPE.Visibility = Visibility.Visible;
            Folder_VTYPE_Bool.IsEnabled = false;
            Folder_VTYPE_Bool.Visibility = Visibility.Collapsed;
            vtype = ParamVType.VFOLDER;
        }

        private void ParamBool_Radio_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInit)
                return;
            ParamValue_TextBox.IsEnabled = false;
            ParamValue_TextBox.Visibility = Visibility.Collapsed;
            File_VTYPE.IsEnabled = false;
            File_VTYPE.Visibility = Visibility.Collapsed;
            Folder_VTYPE.IsEnabled = false;
            Folder_VTYPE.Visibility = Visibility.Collapsed;
            Folder_VTYPE_Bool.IsEnabled = true;
            Folder_VTYPE_Bool.Visibility = Visibility.Visible;
            vtype = ParamVType.VBOOLEAN;
        }

        private void ParamValue_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ParamFile_Browse_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Param_FolderBrowse_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ParamValue_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ParamValue_FolderBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ParamValue_FileBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            string value = "";
            switch(vtype)
            {
                case ParamVType.VVALUE:
                    value = ParamValue_TextBox.Text;
                    break;

                case ParamVType.VFILE:
                    value = ParamValue_FileBox.Text;
                    break;

                case ParamVType.VFOLDER:
                    value = ParamValue_FolderBox.Text;
                    break;

                case ParamVType.VBOOLEAN:
                    value = ParamValue_ComboBox.SelectedIndex == 0 ? "FALSE" : "TRUE";
                    break;
            }
            if (!isEditing)
            {
                editingProfile.paramList.Add(new NXParam(selectedParam.Name, value));
                
            } else
            {

                editingProfile.paramList[index] = new NXParam(selectedParam.Name, value);    
            }           
            Close();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedParam = (NXParam)ParametersList.SelectedItem;
            if(selectedParam != null)
            {
                ParamName_Box.Text = selectedParam.Name;
                ParamValue_TextBox.Text = selectedParam.Value;
                ParamValue_FolderBox.Text = selectedParam.Value;
                ParamValue_FileBox.Text = selectedParam.Value;
                bool result = false;
                bool.TryParse(selectedParam.Value, out result);
                if(result)
                {
                    ParamValue_ComboBox.SelectedIndex = 0;
                } else
                {
                    ParamValue_ComboBox.SelectedIndex = 1;
                }
            }

        }

        private void SearchParams_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (installation == null)
                return;

            if (SearchParams != null)
            {
                if (SearchParams.Text.Length > 0)
                {
                    ParametersList.ItemsSource = installation.parameters.Params.FindAll(pp => pp.Name.Contains(SearchParams.Text.ToUpper())); // i.parameters.Params;
                    ParametersList.UnselectAll();
                    ParametersList.Items.Refresh();
                } else
                {
                    ParametersList.ItemsSource = installation.parameters.Params;
                    ParametersList.UnselectAll();
                    ParametersList.Items.Refresh();
                }
            }
        }
    }
}
