using System;
using System.Xml.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace TranslatorApp.Translator
{
    public class CognitiveTranslator
    {
        #region Constants

        private const string OCP_APIM_SUBSCRIPTION_KEY_HEADER = "Ocp-Apim-Subscription-Key";
        private const string TRANSLATE_URL_TEMPLATE = "http://api.microsofttranslator.com/v2/http.svc/translate?text={0}&from={1}&to={2}&category={3}";

        #endregion

        #region Fields

        protected string AzureSubscriptionKey;

        #endregion

        #region Properties

        public string SourceLanguage { get; }

        public string DestinationLanguage { get; }

        #endregion

        #region Constructors

        public CognitiveTranslator(string sourceLanguage, string destinationLanguage, string azureSubscriptionKey)
        {
            SourceLanguage = sourceLanguage;
            DestinationLanguage = destinationLanguage;
            AzureSubscriptionKey = azureSubscriptionKey;
        }

        #endregion

        #region Public methods

        public virtual async Task<string> TranslateAsync(string text)
        {
            var request = await TranslateRequest(string.Format(TRANSLATE_URL_TEMPLATE, text, SourceLanguage, DestinationLanguage, "general"), AzureSubscriptionKey);

            var response = await request.Content.ReadAsStringAsync();
            if (request.IsSuccessStatusCode)
                return XElement.Parse(response).Value;;
                
            throw new Exception(response);
        }

        public virtual Task<string[]> TranslateAsync(string[] text)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private methods

        private static async Task<HttpResponseMessage> TranslateRequest(string url, string azureSubscriptionKey)
        {
            using (var client = new HttpClient())
            { 
                client.DefaultRequestHeaders.Add(OCP_APIM_SUBSCRIPTION_KEY_HEADER, azureSubscriptionKey);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                return await client.GetAsync(url);
            }
        }

        #endregion
    }
}
