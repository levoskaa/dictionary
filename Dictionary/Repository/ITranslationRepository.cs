using Dictionary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dictionary.Repository
{
    public interface ITranslationRepository
    {
        Task<IEnumerable<string>> GetLanguagesAsync();

        Task<Translation> GetTranslationAsync(string word, string fromLanguage, string toLanguage);
    }
}
