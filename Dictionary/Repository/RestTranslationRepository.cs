using Dictionary.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Dictionary.Repository
{
    public class RestTranslationRepository : ITranslationRepository
    {
        private static readonly HttpClient httpClient = new HttpClient { BaseAddress = new Uri("https://dictionary.yandex.net/api/v1/dicservice.json/") };
        private readonly string apiKey = "dict.1.1.20200508T120242Z.0190a48daee8e510.c6cc8065cc43e19df744a91a403def65c5b7123b";

        public async Task<IEnumerable<string>> GetLanguagesAsync()
        {
            var response = await httpClient.GetAsync($"getLangs?key={apiKey}");
            string json = await response.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<IEnumerable<string>>(json);
            return obj;
        }

        public async Task<Translation> GetTranslationAsync(string word, string fromLanguage, string toLanguage)
        {
            var response = await httpClient.GetAsync($"lookup?key={apiKey}&lang={fromLanguage}-{toLanguage}&text={word}");
            string json = await response.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<Translation>(json);
            return obj;
        }
    }
}
