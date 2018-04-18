using System;
using System.IO;
using System.Resources;
using System.Collections.Generic;

namespace TranslatorApp.Datasource
{
    public class DatasourceWriter
    {
        #region Fields

        private readonly string _filePath;

        #endregion

        #region Properties

        public IDictionary<string, IDictionary<string, string>> Resources { get; }

        #endregion

        #region Constructors

        public DatasourceWriter(string filePath, IEnumerable<string> languages)
        {
            _filePath = filePath;

            Resources = new Dictionary<string, IDictionary<string, string>>();
            foreach (var language in languages)
                Resources.Add(language, new Dictionary<string, string>());
        }

        #endregion

        #region Public methods

        public void Write(string name, string language, string text)
        {
            if(!Resources.ContainsKey(language))
                throw new Exception("Language not found");

            if(Resources[language].ContainsKey(name))
                throw new Exception($"Key already added for {language}");

            Resources[language].Add(name, text);
        }

        public void Flush()
        {
            foreach (var language in Resources)
                using (var fs = new FileStream(CreateResourceName(_filePath, language.Key), FileMode.Create))
                using (var resx = new ResXResourceWriter(fs))
                {
                    foreach (var resource in language.Value)
                        resx.AddResource(resource.Key, resource.Value);
                    resx.Generate();
                    fs.Flush(true);
                }
        }

        #endregion

        #region Private methods

        private static string CreateResourceName(string filePath, string languageKey)
        {
            var fileInfo = new FileInfo(filePath);

            return Path.ChangeExtension(filePath, $"{languageKey}.{fileInfo.Extension.TrimStart('.')}");
        }

        #endregion
    }
}
