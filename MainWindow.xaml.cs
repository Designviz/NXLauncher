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
        public NXInstallatations NXInstalls = new NXInstallatations();
        public NXInstallation? selectedInstallation = null;
        public NXConfigProfile? selectedProfile = null;
        public bool isDefaultLaunch = true;
        public ProfileConfig? configWindow = null;
        public MainWindow()
        {
            InitializeComponent();
            GetInstalledNXVersions();
            LoadConfigurationProfiles();
        }

        public void LoadConfigurationProfiles()
        {
            if(NXConfigProfileManager.Instance == null)
                NXConfigProfileManager.Instance = new NXConfigProfileManager();

            NXConfigProfileManager.LoadProfiles();
        }

        public void GetInstalledNXVersions()
        {
            NXInstalls.Installs = new List<NXInstallation>();
            string registry_key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            using (Microsoft.Win32.RegistryKey key = Registry.LocalMachine.OpenSubKey(registry_key))
            {
                foreach (string subkey_name in key.GetSubKeyNames())
                {
                    using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                    {
                        if (subkey == null)
                            continue;

                        if (subkey.GetValue("DisplayName") != null)
                        {
                            if (subkey.GetValue("DisplayName").ToString().Contains("Siemens NX") && !subkey.GetValue("DisplayName").ToString().Contains("Launcher"))
                            {

                                NXInstallation nx = new NXInstallation();

                                //List<string> nameKeys = subkey.GetValueNames().ToList();
                                nx.DisplayName = subkey.GetValue("DisplayName") != null ? subkey.GetValue("DisplayName").ToString() : "NA";
                                nx.DisplayVersion = subkey.GetValue("DisplayVersion") != null ? subkey.GetValue("DisplayVersion").ToString() : "NA";
                                nx.DisplayIcon = subkey.GetValue("DisplayIcon") != null ? subkey.GetValue("DisplayIcon").ToString() : "NA";
                                nx.Directory = subkey.GetValue("DisplayName") != null ? subkey.GetValue("InstallLocation").ToString() : "NA";
                                NXInstalls.Installs.Add(nx);
                            }
                        }
                    }
                }
            }
            string output = JsonConvert.SerializeObject(NXInstalls);
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            Directory.CreateDirectory(path + "/DesignVisionaries/NXLauncher/data");
            //File.Create(path+"/DesignVisionaries/NXLauncher/data/NXInstalls.json");
            File.WriteAllText(path + "/DesignVisionaries/NXLauncher/data/NXInstalls.json", output);
            NXVersions.ItemsSource = NXInstalls.Installs;
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
