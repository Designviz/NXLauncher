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
        public MainWindow()
        {
            InitializeComponent();
            GetInstalledNXVersions();
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
                            if (subkey.GetValue("DisplayName").ToString().Contains("Siemens NX"))
                            {

                                NXInstallation nx = new NXInstallation();

                                //List<string> nameKeys = subkey.GetValueNames().ToList();
                                nx.DisplayName = subkey.GetValue("DisplayName") != null ? subkey.GetValue("DisplayName").ToString() : "NA";
                                nx.DisplayVersion = subkey.GetValue("DisplayVersion") != null ? subkey.GetValue("DisplayVersion").ToString() : "NA";
                                nx.DisplayIcon = subkey.GetValue("DisplayIcon") != null ? subkey.GetValue("DisplayIcon").ToString() : "NA";
                                nx.Directory = subkey.GetValue("UninstallString") != null ? subkey.GetValue("UninstallString").ToString() : "NA";
                                NXInstalls.Installs.Add(nx);
                            }
                        }
                    }
                }
            }
            string output = JsonConvert.SerializeObject(NXInstalls);
            File.WriteAllText("Files.json", output);
            NXVersions.ItemsSource = NXInstalls.Installs;
        }
    }
}
