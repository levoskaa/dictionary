using System.Collections.Generic;

namespace Dictionary.Models
{
    public class Translation
    {
        public string SourceWord { get; set; }

        public string TranslatedWord { get; set; }

        public List<string> Synonyms { get; set; }
    }
}
