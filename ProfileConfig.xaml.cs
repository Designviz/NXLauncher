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
    /// Interaction logic for ProfileConfig.xaml
    /// </summary>
    public partial class ProfileConfig : Window
    {
        NXInstallation? installation;
        NXParamsWindow? nXParamsWindow = null;
        NXConfigProfile? editingProfile = null;
        NXParam? selectedParam = null;
        bool isInitialized = false;
        public ProfileConfig()
        {
            InitializeComponent();
            editingProfile = new NXConfigProfile();
            installation = new NXInstallation();
            isInitialized = true;
        }

        public ProfileConfig(NXConfigProfile profile, NXInstallation i)
        {            
            InitializeComponent();
            editingProfile = profile;
            installation = i;
            SetData();
            isInitialized = true;
        }

        public void SetData()
        {
            if (editingProfile == null)
                return;

            ProfileName_TextBox.Text = editingProfile.Name;
            if(editingProfile.Generated)
            {
                Generated_Radio.IsChecked = true;
                UGIIENVFile_Panel.IsEnabled = false;
                EnvironmentFolder_Panel.IsEnabled = true;
                //EnvironmentFolder_Utilities.IsEnabled = true;
                GenerateEnvParams_Panel.IsEnabled = true;

            } else
            {
                UseExisting_Radio.IsChecked = true;
                UGIIENVFile_Panel.IsEnabled = true;
                EnvironmentFolder_Panel.IsEnabled = false;
                //EnvironmentFolder_Utilities.IsEnabled = false;
                GenerateEnvParams_Panel.IsEnabled = false;
            }
            UGIIENVPath_TextBox.Text = editingProfile.File;
            EnvironmentFolder_TextBox.Text = editingProfile.ENVFile;

            NXParams_List.ItemsSource = editingProfile.paramList;
        }


        private void Generated_Radio_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitialized)
                return;
            UGIIENVFile_Panel.IsEnabled = false;
            EnvironmentFolder_Panel.IsEnabled = true;
            GenerateEnvParams_Panel.IsEnabled = true;
            editingProfile.Generated = true;
        }

        private void UseExisting_Radio_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitialized)
                return;
            UGIIENVFile_Panel.IsEnabled = true;
            EnvironmentFolder_Panel.IsEnabled = false;
            GenerateEnvParams_Panel.IsEnabled=false;
            editingProfile.Generated = false;
        }

        private void UGIIENVPath_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void UGIIENVBrowse_Button_Click(object sender, RoutedEventArgs e)
        {
            if (installation == null)
                return;

            using (System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog())
            {
                openFileDialog.InitialDirectory = installation.Directory+"\\UGII";
                openFileDialog.Filter = "dat files (*.dat)|*.dat|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = false;

                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //Get the path of specified file
                    UGIIENVPath_TextBox.Text = openFileDialog.FileName;
                    editingProfile.File = openFileDialog.FileName;
                }
            }
        }

        private void EnvironmentFolderBrowse_Button_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if(result == System.Windows.Forms.DialogResult.OK)
                {
                    editingProfile.ENVFile = dialog.SelectedPath;
                    EnvironmentFolder_TextBox.Text = dialog.SelectedPath;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void NXParams_List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedParam = (NXParam)NXParams_List.SelectedItem;
            if (selectedParam != null)
            {
                EditParam_Button.IsEnabled = true;
            } else
            {
                EditParam_Button.IsEnabled = false;
            }


        }

        private void AddParam_Button_Click(object sender, RoutedEventArgs e)
        {
            if(nXParamsWindow!=null)
            { 
                nXParamsWindow.Close();
                nXParamsWindow = null;
            }

            nXParamsWindow = new NXParamsWindow(editingProfile, installation);
            nXParamsWindow.Closing += NXParamsWindow_Closing;
            nXParamsWindow.Show();

        }

        private void RemoveParam_Button_Click(object sender, RoutedEventArgs e)
        {
            if (selectedParam == null)
                return;
            if (editingProfile == null)
                return;

            editingProfile.paramList.Remove(selectedParam);
            NXParams_List.UnselectAll();
            NXParams_List.Items.Refresh();
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            NXConfigProfileManager.SaveProfiles();
        }

        private void SaveAndClose_Button_Click(object sender, RoutedEventArgs e)
        {
            NXConfigProfileManager.SaveProfiles();
            Close();
        }

        private void ProfileName_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (editingProfile == null)
                return;
            editingProfile.Name = ProfileName_TextBox.Text;
        }

        private void EditParam_Button_Click(object sender, RoutedEventArgs e)
        {
            if (nXParamsWindow != null)
            {
                nXParamsWindow.Close();
                nXParamsWindow = null;
            }

            nXParamsWindow = new NXParamsWindow(editingProfile, installation,selectedParam);
            nXParamsWindow.Closing += NXParamsWindow_Closing;
            nXParamsWindow.Show();
        }

        private void NXParamsWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            NXParams_List.UnselectAll();
            NXParams_List.Items.Refresh();
            if(nXParamsWindow!=null)
                nXParamsWindow.Closing -= NXParamsWindow_Closing;
        }

        private void NXAppSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (editingProfile == null)
                return;

            editingProfile.Application = (NXApplication)NXAppSelect.SelectedIndex;
        }
    }
}
