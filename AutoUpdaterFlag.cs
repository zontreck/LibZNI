
using System;
using System.Collections.Generic;
using System.Text;

namespace LibZNI
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple =false, Inherited =true)]
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

        public string AsFinalURL(string CIServer)
        {
            return CIServer + AutoUpdaterProjectPath + "/lastSuccessfulBuild/artifact/" + AutoUpdateArtifact.Replace("!os!", Tools.GetOSShortID());
        }
    }
}
