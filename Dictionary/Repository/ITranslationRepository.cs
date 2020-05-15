using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dictionary.Repository
{
    /// <summary>
    /// Interface for accessing a word repository.
    /// </summary>
    public interface ITranslationRepository
    {
        /// <summary>
        /// Get supported languages from Repository.
        /// </summary>
        /// <returns>Languages</returns>
        Task<IEnumerable<string>> GetLanguagesAsync();

        /// <summary>
        /// Get translation from Repository.
        /// </summary>
        /// <param name="word">Word to be translated.</param>
        /// <param name="fromLanguage">Source language of translation.</param>
        /// <param name="toLanguage">Target language of translation.</param>
        /// <returns>Translation</returns>
        Task<Models.Translation> GetTranslationAsync(string word, string fromLanguage, string toLanguage);

        /// <summary>
        /// Get synonyms from Repository.
        /// </summary>
        /// <param name="word">Word whose synonyms are searched.</param>
        /// <param name="language">Language of word.</param>
        /// <returns>Synonyms</returns>
        Task<Models.Translation> GetSynonymsAsync(string word, string language);
    }
}
