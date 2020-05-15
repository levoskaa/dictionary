using Dictionary.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Dictionary.ViewModels
{
    /// <summary>
    /// Contains the logic and data belonging to WordPageView.
    /// </summary>
    public class WordPageViewModel : INotifyPropertyChanged
    {
        private string selectedFromLanguage = string.Empty;
        private string selectedToLanguage = string.Empty;
        private bool searchForSynonyms = false;
        private Translation translation;

        /// <summary>
        /// An event that notifies the bindings when a bound property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        /// <value>
        /// Gets or sets the value of translation data.
        /// </value>
        public Models.Translation Translation
        {
            get { return translation; }
            set
            {
                if (!Translation?.Equals(value) ?? true)
                {
                    translation = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <value>
        /// Language combinations the Repository supports.
        /// </value>
        public List<string> ValidLanguageCombinations { get; private set; } = new List<string>();

        /// <value>
        /// Languages the Repository supports.
        /// </value>
        public List<string> Languages { get; set; } = new List<string>();

        /// <value>
        /// True when synonyms are searched.
        /// </value>
        public bool SearchForSynonyms
        {
            get { return searchForSynonyms; }
            set
            {
                if (searchForSynonyms != value)
                {
                    searchForSynonyms = value;
                    Translation = new Translation();
                    OnPropertyChanged();
                    OnPropertyChanged("IsLanguageErrorVisible");
                }
            }
        }

        /// <value>
        /// Whether the language combination is supported by the Repository.
        /// </value>
        public bool IsLanguageCombinationValid
        {
            get
            {
                string combination = $"{selectedFromLanguage}-{selectedToLanguage}";
                return ValidLanguageCombinations.Contains(combination);
            }
        }

        /// <value>
        /// Whether the language error is visible.
        /// </value>
        public bool IsLanguageErrorVisible
        {
            get => !IsLanguageCombinationValid && !SearchForSynonyms || string.IsNullOrEmpty(SelectedFromLanguage);
        }

        /// <value>
        /// The source language of the translation.
        /// </value>
        public string SelectedFromLanguage
        {
            get { return selectedFromLanguage; }
            set
            {
                if (!selectedFromLanguage.Equals(value))
                {
                    selectedFromLanguage = value;
                    OnPropertyChanged();
                    OnPropertyChanged("IsLanguageCombinationValid");
                    OnPropertyChanged("IsLanguageErrorVisible");
                }
            }
        }

        /// <value>
        /// The target language of translation.
        /// </value>
        public string SelectedToLanguage
        {
            get { return selectedToLanguage; }
            set
            {
                if (!selectedToLanguage.Equals(value))
                {
                    selectedToLanguage = value;
                    OnPropertyChanged();
                    OnPropertyChanged("IsLanguageCombinationValid");
                    OnPropertyChanged("IsLanguageErrorVisible");
                }
            }
        }

        /// <summary>
        /// Loads data from the Repository.
        /// </summary>
        public async void LoadData()
        {
            ValidLanguageCombinations = (List<string>)await App.Repository.GetLanguagesAsync();
            if (ValidLanguageCombinations.Count != 0)
            {
                Languages.Clear();
                foreach (string item in ValidLanguageCombinations)
                {
                    Languages.Add(item.Split('-')[0]);
                }
                Languages = Languages.Distinct().ToList();
                Languages.Sort();
                OnPropertyChanged("Languages");
            }
            else
            {
                Languages = new List<string>(new[] { "Failed to load" });
            }
        }

        /// <summary>
        /// Searches the Repository for the given keyword.
        /// </summary>
        /// <param name="word">Keyword to search for.</param>
        public async void Search(string word)
        {
            if (word == null || word == string.Empty || IsLanguageErrorVisible)
            {
                return;
            }
            if (SearchForSynonyms)
            {
                Translation = await App.Repository.GetSynonymsAsync(word, SelectedFromLanguage);
            }
            else
            {
                Translation = await App.Repository.GetTranslationAsync(word, SelectedFromLanguage, SelectedToLanguage);
            }
        }

        /// <summary>
        /// Switch the source and target language.
        /// </summary>
        public void SwitchLanguages()
        {
            string temp = SelectedFromLanguage;
            SelectedFromLanguage = SelectedToLanguage;
            SelectedToLanguage = temp;
        }

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">Name of the changed property.</param>
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
