using System;
using System.IO;
using System.Collections.Generic;

namespace TranslatorApp.Datasource
{
    public class DatasourceReader
    {
        #region Constants

        private const string SEPARATOR = ",";

        #endregion

        #region Public methods

        public IEnumerable<string[]> Read(string filePath)
        {
            if (IsValidFile(filePath))
                return GetDataSourceFromFile(filePath);

            throw new Exception("Invalid datasource defined");
        }

        #endregion

        #region Private methods

        private static bool IsValidFile(string file)
        {
            return !string.IsNullOrEmpty(file) && File.Exists(file);
        }

        private static IEnumerable<string[]> GetDataSourceFromFile(string file)
        {
            using (var stream = File.OpenText(file))
                while (!stream.EndOfStream)
                    yield return CreateDataSourceLineFromFile(stream.ReadLine());
        }

        private static string[] CreateDataSourceLineFromFile(string line)
        {
            return line.Split(new[]{SEPARATOR}, StringSplitOptions.None);
        }

        #endregion
    }
}
