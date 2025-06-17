using System;
using System.Collections.Generic;
using UnityEngine;

namespace HelloWorld.Utils
{
    public class LocalizationController : Singleton<LocalizationController>
    {
        [SerializeField] private LocalizationSettings settings;
        private Dictionary<string, string> localizedText;

        private string currentLanguage = "portugues";
        private List<string> availableLanguages;

        private const string LANGUAGE_PREF_KEY = "SelectedLanguage";

        public static event Action<string> OnChangeLanguage;

        public static bool Initialized;

        private void Start()
        {
            LoadLocalization();
        }

        private string DetectSystemLanguage()
        {
            SystemLanguage systemLang = Application.systemLanguage;
            string lang = systemLang.ToString(); // Obtém o nome do idioma no formato "English", "Portuguese", etc.

            // Se o idioma do sistema existir no CSV, retorna ele
            if (availableLanguages.Contains(lang))
            {
                return lang;
            }

            // Tentativa de fallback para variantes regionais, como "Portuguese (Brazil)" -> "Portuguese"
            foreach (string availableLang in availableLanguages)
            {
                if (lang.StartsWith(availableLang))
                {
                    return availableLang;
                }
            }

            return null; // Caso não tenha um idioma correspondente
        }

        private void LoadLocalization()
        {
            localizedText = new Dictionary<string, string>();
            availableLanguages = new List<string>();
            if (settings.csvFile == null)
            {
                Debug.LogError("Arquivo de tradução não encontrado!");
                return;
            }

            if (PlayerPrefs.HasKey(LANGUAGE_PREF_KEY))
            {
                currentLanguage = PlayerPrefs.GetString(LANGUAGE_PREF_KEY);
            }
            else
            {
                currentLanguage = DetectSystemLanguage();
            }

            string[] lines = settings.csvFile.text.Split('\n');
            string[] headers = lines[0].Split(',');

            int langIndex = 1; // Default: English
            for (int i = 1; i < headers.Length; i++)
            {
                string lang = headers[i].Trim();
                if (!string.IsNullOrEmpty(lang))
                {
                    availableLanguages.Add(lang);
                }

                if (lang == currentLanguage)
                {
                    langIndex = i;
                }
            }



            // Garantir que o idioma seja válido
            if (!availableLanguages.Contains(currentLanguage))
            {
                currentLanguage = availableLanguages.Count > 0 ? availableLanguages[0] : "English";
            }

            localizedText.Clear();

            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;
                string[] columns = lines[i].Split(',');

                if (columns.Length > langIndex)
                {
                    string key = columns[0].Trim();
                    string value = columns[langIndex].Trim();
                    localizedText[key] = value;
                }
            }

            Debug.Log("Tradução carregada para " + currentLanguage);
            Initialized = true;
        }

        public string GetText(string key)
        {
            return localizedText.TryGetValue(key, out string value) ? value : key;
        }

        public void ChangeLanguage(string newLanguage)
        {
            currentLanguage = newLanguage;
            PlayerPrefs.SetString(LANGUAGE_PREF_KEY, newLanguage);
            PlayerPrefs.Save();
            LoadLocalization();
            OnChangeLanguage?.Invoke(newLanguage);
        }

        public List<string> GetAvailableLanguages()
        {
            return new List<string>(availableLanguages);
        }

        public string GetCurrentLanguage()
        {
            return currentLanguage;
        }

        public int GetCurrentLanguageIndex()
        {
            return availableLanguages.IndexOf(currentLanguage);
        }
    }
}