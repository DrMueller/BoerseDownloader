using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MMU.BoerseDownloader.DataAccess.Handler
{
    public class FileHandler<TModel>
    {
        private readonly DirectoryInfo _directoryInfo;

        public FileHandler()
        {
            _directoryInfo = new DirectoryInfo(GetModelPath());
            if (!_directoryInfo.Exists)
            {
                _directoryInfo.Create();
            }
        }

        internal void DeleteFile(string fileName)
        {
            var fullFileName = GetFullFileName(fileName);
            File.Delete(fullFileName);
        }

        internal bool FileExists(string fileName)
        {
            var fullFileName = GetFullFileName(fileName);
            return File.Exists(fullFileName);
        }

        internal IReadOnlyCollection<TModel> LoadAllFiles()
        {
            var result = GetAllFileNames().Select(LoadFromFile).ToList();
            return result.AsReadOnly();
        }

        internal TModel LoadFromFile(string fileName)
        {
            var fullFileName = GetFullFileName(fileName);
            var str = File.ReadAllText(fullFileName);
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<TModel>(str);
            return result;
        }

        internal void SaveFile(TModel model, string fileName)
        {
            var str = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            var fullFileName = GetFullFileName(fileName);

            if (fullFileName.Length > 260)
            {
                throw new Exception($"File-Name for {fileName} is longer than 260 Characters.");
            }

            File.WriteAllText(fullFileName, str);
        }

        private IEnumerable<string> GetAllFileNames()
        {
            var allFiles = _directoryInfo.GetFiles();
            return allFiles.Select(f => f.Name).ToList();
        }

        private string GetFullFileName(string fileName)
        {
            var result = Path.Combine(_directoryInfo.FullName, fileName);
            return result;
        }

        private static string GetModelPath()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string result = Uri.UnescapeDataString(uri.Path);
            result = Path.GetDirectoryName(result);
            result = Path.Combine(result, typeof(TModel).Name);
            return result;
        }
    }
}