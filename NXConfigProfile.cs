using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NXLauncher
{
    [System.Serializable]
    public class NXConfigProfileManager
    {
        [NonSerialized]
        public static NXConfigProfileManager Instance = new();

        public Dictionary<string, NXConfigProfiles> NXConfigurations = new Dictionary<string, NXConfigProfiles>();
        public Dictionary<string, NXParams> NXParameters = new Dictionary<string, NXParams>();

        public NXConfigProfileManager()
        {
            Instance = this;
        }

        public void LoadParams(string key)
        {
            NXParams parameters;
            try
            {
                parameters = NXParameters[key];
            }catch(Exception e)
            {
                //no key for config, create new entry.
                parameters = new NXParams();
                NXParameters.Add(key, parameters);
                
            }
            NXConfigProfiles profile = GetConfigurationProfiles(key);

            SaveProfiles();
        }
        public void AddProfile(string key)
        {
            NXConfigProfiles profile = GetConfigurationProfiles(key);
            profile.configProfiles.Add(new NXConfigProfile("Profile_" + profile.configProfiles.Count + 1));
            SaveProfiles();
        }

        public void RemoveProfile(string key, NXConfigProfile pfile)
        {
            if (pfile == null)
                return;
            NXConfigProfiles profile = GetConfigurationProfiles(key);
            profile.configProfiles.Remove(pfile);
            SaveProfiles();
        }

        public static void LoadProfiles()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            Directory.CreateDirectory(path + "/DesignVisionaries/NXLauncher/data");
            if(File.Exists(path + "/DesignVisionaries/NXLauncher/data/Profiles.json"))
            {
                string data = File.ReadAllText(path + "/DesignVisionaries/NXLauncher/data/Profiles.json");
                Instance = JsonConvert.DeserializeObject<NXConfigProfileManager>(data);
            }
                
        }

        public static void SaveProfiles()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            Directory.CreateDirectory(path + "/DesignVisionaries/NXLauncher/data");
            string data = JsonConvert.SerializeObject(Instance);
            File.WriteAllText(path + "/DesignVisionaries/NXLauncher/data/Profiles.json", data);

        }

        public NXConfigProfiles GetConfigurationProfiles(string key)
        {
            NXConfigProfiles nxConfigs;
            try
            {
                nxConfigs = NXConfigurations[key];
            }
            catch (Exception ex)
            {
                //no key for config, create new entry.
                nxConfigs = new NXConfigProfiles();
                NXConfigurations.Add(key, nxConfigs);
                SaveProfiles();

            }
            return nxConfigs;
        }
    }

    [System.Serializable]
    public class NXConfigProfiles
    {
        public List<NXConfigProfile> configProfiles = new List<NXConfigProfile>();
    }
    [System.Serializable]
    public class NXConfigProfile
    {
        public NXConfigProfile()
        {
            Name = "Profile";
            File = "";
            Description = "";
            Generated = true;
            ENVFile = "";
            Icon = "";
        }
        public NXConfigProfile(string n)
        {
            Name = n;
            File = "";
            Description = "";
            Generated = true;
            ENVFile = "";
            Icon = "";
        }
        public string Name { get; set; }
        public string File { get; set; }
        public string Description { get; set; }
        public bool Generated { get; set; }
        public string ENVFile { get; set; }
        public string Icon { get; set; }
        public List<NXParam> paramList = new List<NXParam>();
    }

    [System.Serializable]
    public class NXParams
    {
        public List<NXParam> Params = new List<NXParam>();
    }

    [System.Serializable]
    public class NXParam
    {
        public NXParam()
        {
            Name = "UGII_NULL";
            Value = "";

        }
        public NXParam(string n, string v)
        {
            Name = n;
            Value = v;
        }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
