using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace IbrahKit
{
    [DefaultExecutionOrder(Execution_Order.local)]
    public class Localization_Manager : Manager_Base
    {
        public const string KEY = "Localization";
        public const string SYS = "SysLanguage";
        private const string langSettKey = "language";
        private const string langDropdownKey = "Language";
        private const string saveDataKey = "LocalizationManager";

        private SaveData data;

        [BoxGroup("Language Settings"), LabelText("Use System Language"), Tooltip("If true, attempts to set the game's language to the system language.")]
        [SerializeField] private bool attemptSetToSystemLanguage = true;

        [BoxGroup("Language Settings"), LabelText("Default Language"), Tooltip("Language used if system language not found.")]
        [SerializeField, Dropdown(langDropdownKey)] private string defaultLanguage;

        [BoxGroup("Localization Assets"), LabelText("Text Files"), Tooltip("Text assets containing localization data.")]
        [SerializeField] private TextAsset[] textLocalization;

        [BoxGroup("Localization Assets"), LabelText("Available Languages"), Tooltip("List of supported languages.")]
        [SerializeField] private List<Localization_Language> languages = new();

        [BoxGroup("Runtime Data"), LabelText("Parsed Localization Data"), ReadOnly, ShowInInspector]
        private Dictionary<string, Dictionary<int, string>> textLocalizationData = new();

        public Action OnLanguageChanged;

        public static Localization_Manager Instance;

        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(gameObject);
            else
            {
                Instance = this;

                Initialize();

                data = (SaveData)Save_Manager.currentFolder.LoadObject(saveDataKey, new SaveData());

                if (!data.first)
                {
                    if (attemptSetToSystemLanguage) SetWindowsLanguage();
                    else GetLanguageSetting().SetValue(GetDefaultLanguage());
                    data.first = true;
                }
            }
        }

        private void OnDestroy()
        {
            if (Instance == this) Save_Manager.currentFolder.SaveObject(saveDataKey, data);
        }

        private void OnValidate()
        {
            if (gameObject.scene.IsValid()) Initialize();
        }

        [Button]
        public void Initialize()
        {
            if (textLocalization == null || textLocalization.Length == 0)
            {
                Debug.LogError("The array textLocalization seems to be empty");
                return;
            }

            textLocalizationData.Clear();

            string _text = "";

            using (var memoryStream = new MemoryStream())
            {
                for (int i = 0; i < textLocalization.Length; i++)
                {
                    if (textLocalization[i] == null)
                    {
                        Debug.LogWarning("LocalizationManager has null references to text asset: " + i +
                                         " If you see this right after you entered Play Mode you can probably ignore this.");
                        continue;
                    }

                    byte[] bytes = textLocalization[i].bytes;
                    memoryStream.Write(bytes, 0, bytes.Length);
                }

                memoryStream.Seek(0, SeekOrigin.Begin);
                _text = Encoding.UTF8.GetString(memoryStream.ToArray());
            }

            if (String_Utilities.IsEmpty(_text))
            {
                Debug.LogWarning("No localization data found");
                return;
            }

            string[] _textSplit = _text.Split("\n");

            int mostRows = 0;

            for (int i = 0; i < _textSplit.Length; i++)
            {
                int localCount = GetAmountOfSeperators(_textSplit[i]);

                if (localCount > mostRows) mostRows = localCount;
            }

            for (int i = 0; i < _textSplit.Length; i++)
            {
                while (GetAmountOfSeperators(_textSplit[i]) < mostRows)
                {
                    _textSplit[i] += ";";
                }
            }

            List<string> _langNames = _textSplit[0].Split(";").ToList();
            List<string> _sysLangNames = _textSplit[1].Split(";").ToList();

            InitializeLanguages(_langNames, _sysLangNames);

            for (int i = 2; i < _textSplit.Length; i++)
            {
                if (IsStringEmpty(_textSplit[i])) continue;

                List<string> _lineSplit = _textSplit[i].Split(";").ToList();

                string key = _lineSplit[0];

                if (IsStringEmpty(key)) continue;

                _lineSplit.RemoveAt(0);

                Dictionary<int, string> localizations = new();

                for (int l = 0; l < _lineSplit.Count; l++)
                {
                    localizations.Add(l, _lineSplit[l]);
                }

                if (!textLocalizationData.TryAdd(key, localizations))
                {
                    throw new ArgumentException($"A localization with the same key '{key}' already exists in the dictionary.");
                }
            }

            String_Utilities.CreateDropdown(textLocalizationData.Keys.Select(x => x.ToString()).ToList(), KEY);
            String_Utilities.CreateDropdown(_langNames, langDropdownKey);
        }

        private int GetAmountOfSeperators(string s)
        {
            int count = 0;

            for (int j = 0; j < s.Length; j++)
            {
                if (s[j] == ';') count++;
            }

            return count;
        }

        private void InitializeLanguages(List<string> _langNames, List<string> _sysLangNames)
        {
            _langNames.RemoveAt(0);
            _sysLangNames.RemoveAt(0);

            for (int i = 0; i < _langNames.Count; i++)
            {
                SystemLanguage language = (SystemLanguage)Enum.Parse(typeof(SystemLanguage), _sysLangNames[i]);

                Localization_Language _lang = new(_langNames[i], language);
                if (_lang != null && languages.Find(x => x.GetSystemLanguage() == _lang.GetSystemLanguage()) == null) languages.Add(_lang);
            }

            if (languages.Count > _langNames.Count) languages.RemoveAt(languages.Count - 1);
        }

        public void SetWindowsLanguage()
        {
            for (int i = 0; i < languages.Count; i++)
            {
                SystemLanguage language = languages[i].GetSystemLanguage();
                SystemLanguage actualSystemLanguage = Application.systemLanguage;

                if (language == actualSystemLanguage && languages[i].IsUsed())
                {
                    SetLanguage(i);
                    break;
                }
            }
        }

        public void SetLanguage(int _newLang)
        {
            GetLanguageSetting().SetValue(_newLang);
            UpdateLanguage();
        }

        public void UpdateLanguage()
        {
            OnLanguageChanged?.Invoke();
        }

        private Localization_Language GetLanguage(int _index)
        {
            try
            {
                return languages[_index];
            }
            catch
            {
                Debug.LogError("Index out of Bounds when trying to get language with the index " + _index);
                return null;
            }
        }

        public Setting GetLanguageSetting()
        {
            if ((Application.isPlaying ? Settings_Manager.Instance : FindAnyObjectByType<Settings_Manager>()).GetSetting(langSettKey, out Setting setting))
            {
                return setting;
            }

            return null;
        }

        public SystemLanguage GetCurrentSysLanguage()
        {
            return GetLanguage((int)GetLanguageSetting().GetValue()).GetSystemLanguage();
        }

        public string GetCurrentLanguageName()
        {
            return GetLanguage((int)GetLanguageSetting().GetValue()).GetName();
        }

        public string[] GetLocalizedString(string[] _keys, string _fallbackText = "", params object[] _variables)
        {
            string[] strings = new string[_keys.Length];
            for (int i = 0; i < _keys.Length; i++) strings[i] = GetLocalizedString(_keys[i], _fallbackText, _variables);
            return strings;
        }

        public string GetLocalizedString(string _key, string _fallbackText = "", params object[] _variables)
        {
            if (IsStringEmpty(_key))
            {
                return _fallbackText;
            }

            int _language = (int)GetLanguageSetting().GetValue();

            if (textLocalizationData.TryGetValue(_key, out Dictionary<int, string> value))
            {
                if (value.TryGetValue(_language, out string _selectedLanguageText) && !IsStringEmpty(_selectedLanguageText))
                {
                    return FormatText(_selectedLanguageText, _variables);
                }
                else if (value.TryGetValue(GetDefaultLanguage(), out string _defaultLanguageText) && !IsStringEmpty(_defaultLanguageText))
                {
                    return FormatText(_defaultLanguageText, _variables);
                }
                else if (!IsStringEmpty(_fallbackText))
                {
                    return FormatText(_fallbackText, _variables);
                }
                else throw new ArgumentException("Localization key found: " + _key + " but no translation for the selected and the default language as well as no fallback defined");
            }
            else if (!IsStringEmpty(_fallbackText))
            {
                return FormatText(_fallbackText, _variables);
            }
            else throw new ArgumentException("Localization with key: " + _key + " not found and no fallback defined");
        }

        private string FormatText(string _input, params object[] _variables)
        {
            _input = _input.Replace("[Break]", "\n");

            try
            {
                if (_variables != null && _variables.Length > 0) _input = string.Format(_input, _variables);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);

                Debug.Log(_input);

                for (int i = 0; i < _variables.Length; i++)
                {
                    Debug.LogError(_variables[i]);
                }
            }

            return _input;
        }

        public int GetNextUsableLanguage(int i)
        {
            int _next = i + 1;
            if (_next >= languages.Count) _next = 0;

            if (languages[i].IsUsed())
            {
                return i;
            }
            return GetNextUsableLanguage(_next);
        }

        public int GetUsedLanguageAmount()
        {
            return languages.FindAll(x => x.IsUsed()).Count;
        }

        private int GetDefaultLanguage()
        {
            return languages.IndexOf(languages.Find(x => x.Equals(defaultLanguage)));
        }

        private bool IsStringEmpty(string _string)
        {
            return String_Utilities.IsEmpty(_string);
        }

        public static bool Exists(bool throwWarning = true)
        {
            bool exists = Instance != null && Instance.gameObject != null;

            if(throwWarning && !exists)
            {
                Debug.LogWarning($"{nameof(Localization_Manager)} does not exist");
            }

            return exists;
        }

        private class SaveData : Savable
        {
            public bool first = false;
        }

        [Serializable]
        private class Localization_Language
        {
            [SerializeField] private bool used = true;
            [SerializeField] private string name;
            [SerializeField] private SystemLanguage sysLanguage;

            public Localization_Language(string _name, SystemLanguage _systemLanguage)
            {
                name = _name;
                sysLanguage = _systemLanguage;
            }

            public SystemLanguage GetSystemLanguage()
            {
                return sysLanguage;
            }

            public string GetName()
            {
                return name;
            }

            public bool Equals(string lang)
            {
                return name.Equals(lang);
            }

            public bool IsUsed()
            {
                return used;
            }
        }
    }
}