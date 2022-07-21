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
        public NXInstallation()
        {
            DisplayName = "NA";
            DisplayIcon = "";
            DisplayVersion = "0";
            Directory = "";
            parameters = new NXParams();
        }

        public NXInstallation(string n, string i, string v, string d)
        {
            DisplayName=n;
            DisplayIcon=i;
            DisplayVersion = v;
            Directory = d;
            parameters = new NXParams();
            parameters.LoadParameters(this);
        }

        public string DisplayName { get; set; }
        public string DisplayIcon { get; set; }
        public string DisplayVersion { get; set; }
        public string Directory { get; set; }

        public NXParams parameters { get; set; }
    }

    [System.Serializable]
    public class NXInstallatationManager
    {

        public static NXInstallatationManager? Instance { get; private set; }
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

                                

                                //List<string> nameKeys = subkey.GetValueNames().ToList();
                                string DisplayName = subkey.GetValue("DisplayName") != null ? subkey.GetValue("DisplayName").ToString() : "NA";
                                string DisplayVersion = subkey.GetValue("DisplayVersion") != null ? subkey.GetValue("DisplayVersion").ToString() : "NA";
                                string DisplayIcon = subkey.GetValue("DisplayIcon") != null ? subkey.GetValue("DisplayIcon").ToString() : "NA";
                                string Directory = subkey.GetValue("DisplayName") != null ? subkey.GetValue("InstallLocation").ToString() : "NA";


                                NXInstallation nx = new NXInstallation(DisplayName, DisplayIcon, DisplayVersion, Directory);
                                //NXConfigProfileManager.Instance.LoadParams(nx);

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
