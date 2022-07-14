using System;
using System.Collections.Generic;
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
    public class NXInstallatations
    {
        public List<NXInstallation> Installs { get; set; }
    }

}
