using System.IO;

namespace Amrit.SaveSystem
{
    public sealed class FileStorage : IStorageBackend
    {
        public FileStorage()
        {
            SavePath.EnsureDirectory();
        }

        public void Write(string key, string content)
        {
            File.WriteAllText(
                SavePath.GetFilePath(key),
                content);
        }

        public string Read(string key)
        {
            return File.ReadAllText(
                SavePath.GetFilePath(key));
        }

        public bool Exists(string key)
        {
            return File.Exists(
                SavePath.GetFilePath(key));
        }

        public void Delete(string key)
        {
            string path = SavePath.GetFilePath(key);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public void DeleteAll()
        {
            if (!Directory.Exists(SavePath.Root))
            {
                return;
            }

            foreach (string file in Directory.GetFiles(SavePath.Root))
            {
                File.Delete(file);
            }
        }
    }
}