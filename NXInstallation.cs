using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NXLauncher
{
    [System.Serializable]
    public class NXInstallation
    {
        public string DisplayName { get; set; }
        public string DisplayIcon { get; set; }
        public string DisplayVersion { get; set; }
        public string Directory { get; set; }
    }

    [System.Serializable]
    public class NXInstallatationManager
    {

        public static NXInstallatationManager Instance { get; private set; }
        public List<NXInstallation> Installs { get; set; }
        public NXInstallatationManager()
        {
            Instance = this;
            Installs = new List<NXInstallation>();
        }


        public List<NXInstallation> GetInstalledNXVersions()
        {
            Installs.Clear();
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
                                Installs.Add(nx);
                            }
                        }
                    }
                }
            }
            string output = JsonConvert.SerializeObject(this);
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            Directory.CreateDirectory(path + "/DesignVisionaries/NXLauncher/data");
            //File.Create(path+"/DesignVisionaries/NXLauncher/data/NXInstalls.json");
            File.WriteAllText(path + "/DesignVisionaries/NXLauncher/data/NXInstalls.json", output);
            return Installs;
        }

    }

}
