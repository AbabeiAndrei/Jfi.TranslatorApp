using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using CommandLine;

using TranslatorApp.Utils;
using TranslatorApp.Datasource;
using TranslatorApp.Translator;
using TranslatorApp.Properties;

namespace TranslatorApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            __Main(args).GetAwaiter().GetResult();
        }

        private static async Task __Main(string[] args)
        {
            try
            {
                Options options = null;

                Parser.Default.ParseArguments<Options>(args)
                           .WithParsed(opts => options = opts)
                           .WithNotParsed(errs => Console.WriteLine(string.Join(Environment.NewLine, errs)));
                
#if DEBUG
                options = new Options
                {
                    Source = @"C:\Users\Andrei\Desktop\values.txt",
                    Destination = @"C:\Users\Andrei\Desktop\values.resx",
                    SourceLanguage = "bg",
                    ResultLanguages = "en;ro"
                };
#endif
                if(options == null)
                    return;

                var reader = new DatasourceReader();
                var ds = reader.Read(options.Source).ToList();
                var translatedDs = new List<List<string>>();

                Console.OutputEncoding = System.Text.Encoding.UTF8;

                foreach (var strs in ds)
                    Console.WriteLine(string.Join("|", strs));

                
                Console.WriteLine("--");

                Console.WriteLine();
                var translators = options.ResultLanguages.Split(';')
                                         .Select(lang => new CognitiveTranslator(options.SourceLanguage,
                                                                                 lang,
                                                                                 Settings.Default.ApiKey1));
                

                foreach (var strs in ds)
                {
                    var list = new List<string>();
                    foreach (var str in strs)
                        list.Add(await translator.TranslateAsync(str));

                    translatedDs.Add(list);

                }
                
                foreach (var strs in translatedDs)
                    Console.WriteLine(string.Join("|", strs));


                var writer = new DatasourceWriter(Path.ChangeExtension(options.Destination, "resx"), new []{translator.SourceLanguage, translator.DestinationLanguage});
                
                //todo make rule naming resources

                const string nameOfDictionary = "Rule";
                const int indexCyrilic = 2;
                const int indexNameRule = 1;

                foreach (var strs in ds.Skip(1))
                    writer.Write($"{nameOfDictionary}_{strs[indexNameRule]}", translator.SourceLanguage, strs[indexCyrilic]);

                foreach (var strs in translatedDs.Skip(1))
                    writer.Write($"{nameOfDictionary}_{strs[indexNameRule]}", translator.DestinationLanguage, strs[indexCyrilic]);

                writer.Flush();

                Console.WriteLine("--");
                Console.WriteLine("Finished!");
            }
            catch (Exception mex)
            {
                Console.Error.WriteLine(mex.Message);
            }

            finally
            {
                Console.ReadKey();
            }
        }
    }
}
