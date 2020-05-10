using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dictionary.Repository
{
    public interface ITranslationRepository
    {
        Task<IEnumerable<string>> GetLanguagesAsync();

        Task<Models.Translation> GetTranslationAsync(string word, string fromLanguage, string toLanguage);

        Task<Models.Translation> GetSynonymsAsync(string word, string language);
    }
}
