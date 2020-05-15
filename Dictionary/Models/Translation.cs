using System.Collections.Generic;

namespace Dictionary.Models
{
    /// <summary>
    /// Model class that stores information about a translation.
    /// </summary>
    public class Translation
    {
        /// <value>Input of the search.</value>
        public string SourceWord { get; set; }

        /// <value>Result of the translation.</value>
        public string TranslatedWord { get; set; }

        /// <value>Synonyms of SourceWord.</value>
        public List<string> Synonyms { get; set; }
    }
}
