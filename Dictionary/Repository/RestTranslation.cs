namespace Dictionary.Repository
{
    /// <summary>
    /// Class representing the body of the response from the API.
    /// </summary>
    public class RestTranslation
    {
        public Head Head { get; set; }
        public Definition[] Def { get; set; }
    }

    public class Head
    {
    }

    public class Definition
    {
        public string Text { get; set; }
        public string Pos { get; set; }
        public string Ts { get; set; }
        public Translation[] Tr { get; set; }
    }

    public class Translation
    {
        public string Text { get; set; }
        public string Pos { get; set; }
        public string Gen { get; set; }
        public Synonym[] Syn { get; set; }
        public Meaning[] Mean { get; set; }
    }

    public class Synonym
    {
        public string Text { get; set; }
        public string Pos { get; set; }
        public string Gen { get; set; }
    }

    public class Meaning
    {
        public string Text { get; set; }
    }
}