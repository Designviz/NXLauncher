using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NXLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public NXInstallatationManager NXInstalls = new NXInstallatationManager();
        public NXInstallation? selectedInstallation = null;
        public NXConfigProfile? selectedProfile = null;
        public bool isDefaultLaunch = true;
        public ProfileConfig? configWindow = null;
        public MainWindow()
        {
            InitializeComponent();
            NXVersions.ItemsSource = NXInstallatationManager.Instance.GetInstalledNXVersions();
            LoadConfigurationProfiles();
        }

        public void LoadConfigurationProfiles()
        {
            if(NXConfigProfileManager.Instance == null)
                NXConfigProfileManager.Instance = new NXConfigProfileManager();

            NXConfigProfileManager.LoadProfiles();
        }



        private void NXVersions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedInstallation = (NXInstallation)NXVersions.SelectedItem;
            if(selectedInstallation != null)
            {
                ResetProfileSelection_Panel(NXConfigProfileManager.Instance.GetConfigurationProfiles(selectedInstallation.DisplayName + "_" + selectedInstallation.DisplayVersion));
            } else
            {
                //Create a new 
                ResetProfileSelection_Panel(null);
            }
        }

        private void ResetProfileSelection_Panel(NXConfigProfiles? p)
        {
            UseDefault_Radio_Checked();
            if (p!=null)
            {
                ProfileSelection_Panel.IsEnabled = true;
                Profile_Listbox.ItemsSource = p.configProfiles;

            } else
            {
                ProfileSelection_Panel.IsEnabled = false;
                LaunchButton.IsEnabled = false;
            }
            

        }



        private void UseDefault_Radio_Checked()
        {
            UseDefault_Radio.IsChecked = true;
            Profiles_Panel.IsEnabled = false;
            LaunchButton.IsEnabled = true;
            isDefaultLaunch = true;
        }
        private void UseDefault_Radio_Checked(object sender, RoutedEventArgs e)
        {
            //UseProfile_Radio.IsChecked = false;
            Profiles_Panel.IsEnabled = false;
            LaunchButton.IsEnabled = true;
            isDefaultLaunch = true;
        }
        private void UseProfile_Radio_Checked()
        {
            //UseDefault_Radio.IsChecked = false;
            Profiles_Panel.IsEnabled = true;
            LaunchButton.IsEnabled = false;
            isDefaultLaunch = false;
        }
        private void UseProfile_Radio_Checked(object sender, RoutedEventArgs e)
        {
            //UseProfile_Radio.IsChecked = false;
            Profiles_Panel.IsEnabled = true;
            LaunchButton.IsEnabled = false;
            isDefaultLaunch = false;
        }

        private void Profile_Listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedProfile = (NXConfigProfile)Profile_Listbox.SelectedItem;
            if(selectedProfile != null)
                LaunchButton.IsEnabled = true;
        }

        private void Add_Profile_Button_Click(object sender, RoutedEventArgs e)
        {
            if(selectedInstallation==null)
                return;
            
            NXConfigProfileManager.Instance.AddProfile(selectedInstallation.DisplayName + "_" + selectedInstallation.DisplayVersion);
            NXConfigProfiles? p = NXConfigProfileManager.Instance.GetConfigurationProfiles(selectedInstallation.DisplayName + "_" + selectedInstallation.DisplayVersion);
            Profile_Listbox.ItemsSource = p.configProfiles;
            Profile_Listbox.Items.Refresh();

        }

        private void Cofigure_Profile_Button_Click(object sender, RoutedEventArgs e)
        {
            if (selectedInstallation == null)
                return;

            if (selectedProfile == null)
                return;

            if(configWindow!=null)
            {
                configWindow.Close();
                configWindow = null;
            }
            configWindow = new ProfileConfig();
            configWindow.Show();
        }

        private void Remove_Profile_Button_Click(object sender, RoutedEventArgs e)
        {
            if (selectedInstallation == null)
                return;

            if(selectedProfile == null)
                return;

            
            NXConfigProfileManager.Instance.RemoveProfile(selectedInstallation.DisplayName + "_" + selectedInstallation.DisplayVersion, selectedProfile);
            NXConfigProfiles? p = NXConfigProfileManager.Instance.GetConfigurationProfiles(selectedInstallation.DisplayName + "_" + selectedInstallation.DisplayVersion);
            Profile_Listbox.ItemsSource = p.configProfiles;
            Profile_Listbox.Items.Refresh();
        }

        private void LaunchButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
