using System.Collections.Generic;
using UnityEngine;

namespace Vices.Scripts.Core
{
    [CreateAssetMenu(fileName = nameof(LocalizationInfo), menuName = "JabberwockyWorld/" + nameof(LocalizationInfo))]
    public class LocalizationInfo : ScriptableObject
    {
        public LanguageEnum Language;
        public Dictionary<string, LocalizationData> _localizations;

        public void Initialize()
        {
            _localizations = new Dictionary<string, LocalizationData>();
        }

        public void AddLocalization(string key, LanguageEnum language, string text)
        {
            if (!_localizations.ContainsKey(key))
            {
                _localizations.Add(key, new LocalizationData());
            }

            _localizations.TryGetValue(key, out var localization);
            localization.AddLocalization(language, text);
        }

        public bool TryGetLocalization(string key, out string result)
        {
            result = "";

            if (!_localizations.TryGetValue(key, out var localization))
            {
                return false;
            }

            if (!localization.TryGetLocalization(Language, out result))
            {
                return false;
            }

            return true;
        }
    }
}