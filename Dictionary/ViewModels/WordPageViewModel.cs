﻿using Dictionary.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Dictionary.ViewModels
{
    public class WordPageViewModel : INotifyPropertyChanged
    {
        private string selectedFromLanguage = string.Empty;
        private string selectedToLanguage = string.Empty;
        private bool searchForSynonyms = false;
        private Translation translation;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

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

        public List<string> ValidLanguageCombinations { get; set; } = new List<string>();

        public List<string> Languages { get; set; } = new List<string>();

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

        public bool IsLanguageCombinationValid
        {
            get
            {
                string combination = $"{selectedFromLanguage}-{selectedToLanguage}";
                return ValidLanguageCombinations.Contains(combination);
            }
        }

        public bool IsLanguageErrorVisible
        {
            get => !IsLanguageCombinationValid && !SearchForSynonyms || string.IsNullOrEmpty(SelectedFromLanguage);
        }

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
        /// Searches for the given keyword
        /// </summary>
        /// <param name="word">Keyword to search for</param>
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

        public void SwitchLanguages()
        {
            string temp = SelectedFromLanguage;
            SelectedFromLanguage = SelectedToLanguage;
            SelectedToLanguage = temp;
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // Raise the PropertyChanged event, passing the name of the property whose value has changed.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
