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
    public enum ParamVType
    {
        VVALUE,
        VFILE,
        VFOLDER,
        VBOOLEAN,
    }
    public partial class NXParamsWindow : Window
    {
        NXInstallation? installation;
        NXConfigProfile? editingProfile = null;
        NXParam? selectedParam = null;
        bool isInit = false;
        ParamVType vtype = ParamVType.VVALUE;
        public NXParamsWindow()
        {
            InitializeComponent();
            installation = new NXInstallation();
            isInit = true;
            editingProfile = new NXConfigProfile();
        }

        public NXParamsWindow(NXConfigProfile p, NXInstallation i)
        {
            InitializeComponent();
            installation = i;
            ParametersList.ItemsSource = i.parameters.Params;
            editingProfile = p;
            isInit = true;
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

            editingProfile.paramList.Add(new NXParam(selectedParam.Name, value));
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
    }
}
