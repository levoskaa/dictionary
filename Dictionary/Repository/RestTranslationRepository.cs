using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace Dictionary.Repository
{
    /// <summary>
    /// Rest Repository of words.
    /// </summary>
    public class RestTranslationRepository : ITranslationRepository
    {
        /// <summary>
        /// Address of the API.
        /// </summary>
        private static readonly Uri baseAddress = new Uri("https://dictionary.yandex.net/api/v1/dicservice.json/");
        private static readonly HttpClient httpClient;
        private readonly string apiKey = "dict.1.1.20200508T120242Z.0190a48daee8e510.c6cc8065cc43e19df744a91a403def65c5b7123b";

        /// <summary>
        /// Static constructor for the initialization of static members.
        /// </summary>
        static RestTranslationRepository()
        {
            var filter = new HttpBaseProtocolFilter();
            // HttpClient will always check HttpCache first before sending a request to the API
            filter.CacheControl.ReadBehavior = HttpCacheReadBehavior.Default;
            httpClient = new HttpClient(filter);
        }

        /// <summary>
        /// Get supported languages from Repository.
        /// </summary>
        /// <returns>Languages</returns>
        public async Task<IEnumerable<string>> GetLanguagesAsync()
        {
            var response = await httpClient.GetAsync(new Uri(baseAddress, $"getLangs?key={apiKey}"));
            string json = await response.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<IEnumerable<string>>(json);
            return obj;
        }

        /// <summary>
        /// Get translation from Repository.
        /// </summary>
        /// <param name="word">Word to be translated.</param>
        /// <param name="fromLanguage">Source language of translation.</param>
        /// <param name="toLanguage">Target language of translation.</param>
        /// <returns>Translation</returns>
        public async Task<Models.Translation> GetTranslationAsync(string word, string fromLanguage, string toLanguage)
        {
            var response = await httpClient.GetAsync(new Uri(baseAddress, $"lookup?key={apiKey}&lang={fromLanguage}-{toLanguage}&text={word}"));
            string json = await response.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<RestTranslation>(json);
            // If response body or the translations array is empty,
            // there was an error during the request or no translations were found.
            if (obj.Def == null || obj.Def.Length == 0)
            {
                return new Models.Translation
                {
                    SourceWord = word,
                    TranslatedWord = "No translation found"
                };
            }
            else
            {
                return new Models.Translation
                {
                    SourceWord = obj.Def[0].Text,
                    TranslatedWord = obj.Def[0].Tr[0].Text
                };
            }
        }

        /// <summary>
        /// Get synonyms from Repository.
        /// </summary>
        /// <remark>The source and target languages are the same.</remark>
        /// <param name="word">Word whose synonyms are searched.</param>
        /// <param name="language">Language of word.</param>
        /// <returns>Synonyms</returns>
        public async Task<Models.Translation> GetSynonymsAsync(string word, string language)
        {
            var response = await httpClient.GetAsync(new Uri(baseAddress, $"lookup?key={apiKey}&lang={language}-{language}&text={word}"));
            string json = await response.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<RestTranslation>(json);
            var synonyms = new List<string>();
            // If response body or the translations array is empty,
            // there was an error during the request or no translations were found.
            if (obj.Def == null || obj.Def.Length == 0)
            {
                return new Models.Translation
                {
                    SourceWord = word,
                    Synonyms = new List<string>(new[] { "No synonyms found" })
                };
            }
            // Add the synonyms of the translations to the result
            foreach (Translation translation in obj.Def[0].Tr)
            {
                synonyms.Add(translation.Text);
                if (translation.Syn != null)
                {
                    foreach (Synonym synonym in translation.Syn)
                    {
                        synonyms.Add(synonym.Text);
                    }
                }
            }
            return new Models.Translation
            {
                SourceWord = obj.Def[0].Text,
                Synonyms = synonyms
            };
        }
    }
}
