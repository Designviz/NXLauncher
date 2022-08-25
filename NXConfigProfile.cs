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

        public NXParams? LoadParams(NXInstallation installation)
        {
            if (installation == null)
                return null;

            string key = installation.DisplayName + "_" + installation.DisplayVersion;

            NXParams parameters;
            try
            {
                parameters = NXParameters[key];
            }catch(Exception e)
            {
                //no key for config, create new entry.
                parameters = new NXParams();
                //NXParameters.Add(key, parameters);
                
            }

            parameters.LoadParameters(installation);

            return parameters;

        }
        public void AddProfile(string key)
        {
            NXConfigProfiles profile = GetConfigurationProfiles(key);
            profile.configProfiles.Add(new NXConfigProfile("Profile_" + profile.configProfiles.Count + 1, key));
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
            Key = "";
            Application = NXApplication.NX;
        }
        public NXConfigProfile(string n, string k="")
        {
            Name = n;
            File = "";
            Description = "";
            Generated = true;
            ENVFile = "";
            Icon = "";
            Key = k;
            Application = NXApplication.NX;
        }
        public string Name { get; set; }
        public string File { get; set; }
        public string Description { get; set; }
        public bool Generated { get; set; }
        public string ENVFile { get; set; }
        public string Icon { get; set; }
        public string Key { get; set; }

        public NXApplication Application { get; set; }

        public List<NXParam> paramList = new List<NXParam>();

        public void GenerateEnvFile()
        {
            string ugData = "";
            foreach (var p in paramList)
            {
                switch(p.VType)
                {
                    case ParamVType.VFOLDER:
                    case ParamVType.VFILE:
                        ugData += p.Name + " = \"" + p.Value +"\"\n";
                        break;
                    default:
                        ugData += p.Name + " = " + p.Value+ "\n";
                        break;
                }
                
            }
            System.IO.File.WriteAllText(ENVFile+"\\ugii_env.dat", ugData);
        }

    }

    [System.Serializable]
    public class NXParams
    {
        public List<NXParam> Params = new List<NXParam>();


        public void SaveParameters(NXInstallation installation)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            Directory.CreateDirectory(path + "\\DesignVisionaries\\NXLauncher\\data");
            string data = JsonConvert.SerializeObject(this);
            File.WriteAllText(path + "\\DesignVisionaries\\NXLauncher\\data\\"+ installation.DisplayVersion + "_Params.json", data);

        }


        public bool LoadParameters(string file)
        {
            if (file == "")
                return false;
            string data = File.ReadAllText(file);
            NXParams? pm = JsonConvert.DeserializeObject<NXParams>(data);
            if(pm!=null)
            {
                Params = pm.Params;
                return true;
            }

            return false;
        }

        public void LoadParameters(NXInstallation installation)
        {
            if (installation == null)
                return;

            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            Directory.CreateDirectory(path + "\\DesignVisionaries\\NXLauncher\\data");

            if (File.Exists(path + "\\DesignVisionaries\\NXLauncher\\data\\" + installation.DisplayVersion + "_Params.json"))
            {
                if (LoadParameters(path + "\\DesignVisionaries\\NXLauncher\\data\\" + installation.DisplayVersion + "_Params.json"))
                    return;
            }

            if(Directory.Exists(installation.Directory))
            {
                Params.Clear();
                string[] configData = File.ReadAllLines(installation.Directory+ "UGII\\ugii_env_ug.dat");
                foreach (var p in configData)
                {
                    if (p.Length > 0)
                    {
                        if (!p.Contains("#"))
                        {
                            string[] pdata = p.Split("=",StringSplitOptions.TrimEntries);
                            if (pdata.Length > 1)
                            {
                                NXParam param = new NXParam(pdata[0], pdata[1]);
                                if(Params.Find(pp => pp.Name.Contains(param.Name)) ==null)
                                    Params.Add(param);
                            }
                        } else
                        {
                            if (p.Contains("UGII_"))
                            {
                                int strt = p.IndexOf("UGII_");
                                if (strt != -1)
                                {
                                    string newp = p.Substring(strt, p.Length - strt);
                                    string[] pdata = newp.Split("=", StringSplitOptions.TrimEntries);
                                    if (pdata.Length > 1)
                                    {
                                        NXParam param = new NXParam(pdata[0], pdata[1]);
                                        if (Params.Find(pp => pp.Name.Contains(param.Name)) == null)
                                            Params.Add(param);
                                    }
                                    else
                                    {
                                        newp = newp.Replace("/", " ");
                                        newp = newp.Replace("\\", " ");
                                        newp = newp.Replace(".", " ");
                                        newp = newp.Replace(",", " ");
                                        string[] pdatab = newp.Split(" ", StringSplitOptions.TrimEntries);
                                        NXParam param = new NXParam(pdatab[0], "");
                                        if (Params.Find(pp => pp.Name.Contains(param.Name)) == null)
                                            Params.Add(param);

                                    }
                                }
                            }
                        }
                    }
                }
                SaveParameters(installation);
            }
        }
    }

    [System.Serializable]
    public class NXParam
    {
        public NXParam()
        {
            Name = "UGII_NULL";
            Value = "";
            VType = ParamVType.VVALUE;
        }
        public NXParam(string n, string v)
        {
            Name = n;
            Value = v;
            VType = ParamVType.VVALUE;
        }
        public string Name { get; set; }
        public string Value { get; set; }
        public ParamVType VType { get; set; }
    }


    [System.Serializable]
    public enum NXApplication
    {
        NX = 0,
        NXCAM = 1,
        NXVIEWER = 2,
        NXLAYOUT = 3,
        MECHANTRONICS = 4,
        NXPROMPT = 5

    }

    [System.Serializable]
    public enum ParamVType
    {
        VVALUE,
        VFILE,
        VFOLDER,
        VBOOLEAN,
    }
}
