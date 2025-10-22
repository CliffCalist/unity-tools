using System;
using UnityEditor;

namespace WhiteArrowEditor
{
    public static class Project
    {
        public static void CreateFullFolderPath(string path)
        {
            if (!AssetDatabase.IsValidFolder(path))
            {
                var partsOfPath = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
                var accumulatedPath = "Assets";

                foreach (var part in partsOfPath)
                {
                    if (part == "Assets")
                        continue;

                    AssetDatabase.CreateFolder(accumulatedPath, part);
                    accumulatedPath += "/" + part;
                }
            }
        }
    }
}
