using System;
using System.Collections.Generic;
using System.Text;

namespace LibZNI
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class AutoUpdater : Attribute
    {
        public string AutoUpdaterProjectPath;
        public bool AutoUpdateEnabled = false;
        public string AutoUpdateArtifact = "";
        public AutoUpdater(string path, string artifact)
        {
            AutoUpdaterProjectPath = path;
            AutoUpdateEnabled = true;
            AutoUpdateArtifact = artifact;
        }
    }
}
