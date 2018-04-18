using CommandLine;

namespace TranslatorApp.Utils
{
    public class Options
    {
        [Option('s', "source", HelpText = "Source file to parse", Required = true)]
        public string Source { get; set; }

        [Option('d', "destination", HelpText = "Destination file", Required = true)]
        public string Destination { get; set; }

        [Option('l', "slang", HelpText = "Source Language", Required = true)]
        public string SourceLanguage { get; set; }

        [Option('r', "rlang", HelpText = "Languages to translate to (separated by ';')", Required = true)]
        public string ResultLanguages { get; set; }
        
        [Option('a', "append", HelpText = "Append to existing resources (if exists) - if no, the files will be overwrited")]
        public bool Append { get; set; }
    }
}
