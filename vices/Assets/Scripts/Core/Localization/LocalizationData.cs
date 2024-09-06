using System;
using System.Collections.Generic;

namespace Vices.Scripts.Core
{
    [Serializable]
    public class LocalizationData
    {
        public Dictionary<LanguageEnum, string> _dictionary;

        public LocalizationData() 
        {
            _dictionary = new Dictionary<LanguageEnum, string>();
        }

        public void AddLocalization(LanguageEnum language, string text)
        {
            if (_dictionary.TryGetValue(language, out var result))
            {             
                return;
            }

            _dictionary.Add(language, text);
        }

        public bool TryGetLocalization(LanguageEnum language, out string result)
        {
            result = "";

            if (_dictionary.TryGetValue(language, out result))
            {
                return true;
            }

            return false;
        }
    }
}