using System.IO;
using UnityEngine;

namespace Core.SaveSystem
{
    public static class SavePath
    {
        private const string SaveFolder = "Saves";

        public static string Root =>
            Path.Combine(Application.persistentDataPath, SaveFolder);

        public static string GetFilePath(string key)
        {
            return Path.Combine(Root, $"{key}.json");
        }

        public static void EnsureDirectory()
        {
            if (!Directory.Exists(Root))
            {
                Directory.CreateDirectory(Root);
            }
        }
    }
}