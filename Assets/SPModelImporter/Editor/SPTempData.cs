using System;

namespace SPModelImporter.Editor
{
    [Serializable]
    public class SPTempData
    {
        public string SourcePath;
        public string BaseColorPrefix;
        public string MetallicPrefix;
        public string NormalPrefix;
        public string HeightPrefix;
        public string AoPrefix;

        public SPTempData(string sourcePath,
            string baseColorPrefix,
            string metallicPrefix,
            string normalPrefix,
            string heightPrefix,
            string aoPrefix
        )
        {
            this.SourcePath = sourcePath;
            this.BaseColorPrefix = baseColorPrefix;
            this.MetallicPrefix = metallicPrefix;
            this.NormalPrefix = normalPrefix;
            this.HeightPrefix = heightPrefix;
            this.AoPrefix = aoPrefix;
        }
    }
}