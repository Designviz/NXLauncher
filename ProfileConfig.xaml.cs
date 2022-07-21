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
        public ProfileConfig()
        {
            InitializeComponent();
            editingProfile = new NXConfigProfile();
            installation = new NXInstallation();
        }

        public ProfileConfig(NXConfigProfile profile, NXInstallation i)
        {            
            InitializeComponent();
            editingProfile = profile;
            installation = i;
            SetData();
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
                EnvironmentFolder_Utilities.IsEnabled = true;
                GenerateEnvParams_Panel.IsEnabled = true;

            } else
            {
                UseExisting_Radio.IsChecked = true;
                UGIIENVFile_Panel.IsEnabled = true;
                EnvironmentFolder_Panel.IsEnabled = false;
                EnvironmentFolder_Utilities.IsEnabled = false;
                GenerateEnvParams_Panel.IsEnabled = false;
            }
            NXParams_List.ItemsSource = editingProfile.paramList;
        }


        private void Generated_Radio_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void UseExisting_Radio_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void UGIIENVPath_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void UGIIENVBrowse_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EnvironmentFolderBrowse_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void NXParams_List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AddParam_Button_Click(object sender, RoutedEventArgs e)
        {
            if(nXParamsWindow!=null)
            { 
                nXParamsWindow.Close();
                nXParamsWindow = null;
            }

            nXParamsWindow = new NXParamsWindow(editingProfile, installation);
            nXParamsWindow.Show();

        }

        private void RemoveParam_Button_Click(object sender, RoutedEventArgs e)
        {

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
    }
}
