using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace Dictionary.Repository
{
    public class RestTranslationRepository : ITranslationRepository
    {
        private static readonly Uri baseAddress = new Uri("https://dictionary.yandex.net/api/v1/dicservice.json/");
        private static readonly HttpClient httpClient;
        private readonly string apiKey = "dict.1.1.20200508T120242Z.0190a48daee8e510.c6cc8065cc43e19df744a91a403def65c5b7123b";

        static RestTranslationRepository()
        {
            var filter = new HttpBaseProtocolFilter();
            filter.CacheControl.ReadBehavior = HttpCacheReadBehavior.Default;
            httpClient = new HttpClient(filter);
        }

        public async Task<IEnumerable<string>> GetLanguagesAsync()
        {
            var response = await httpClient.GetAsync(new Uri(baseAddress, $"getLangs?key={apiKey}"));
            string json = await response.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<IEnumerable<string>>(json);
            return obj;
        }

        public async Task<Models.Translation> GetTranslationAsync(string word, string fromLanguage, string toLanguage)
        {
            var response = await httpClient.GetAsync(new Uri(baseAddress, $"lookup?key={apiKey}&lang={fromLanguage}-{toLanguage}&text={word}"));
            string json = await response.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<RestTranslation>(json);
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

        public async Task<Models.Translation> GetSynonymsAsync(string word, string language)
        {
            var response = await httpClient.GetAsync(new Uri(baseAddress, $"lookup?key={apiKey}&lang={language}-{language}&text={word}"));
            string json = await response.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<RestTranslation>(json);
            var synonyms = new List<string>();
            if (obj.Def == null || obj.Def.Length == 0)
            {
                return new Models.Translation
                {
                    SourceWord = word,
                    Synonyms = new List<string>(new[] { "No synonyms found" })
                };
            }
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
