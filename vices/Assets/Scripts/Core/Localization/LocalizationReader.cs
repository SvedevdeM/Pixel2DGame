using System;
using UnityEngine;

namespace Vices.Scripts.Core
{
    public class LocalizationReader
    {     
        public LocalizationReader(LocalizationInfo localization)
        {
            localization.Initialize();

            AddLocalizations(Resources.Load<TextAsset>("Localization/Vices_Localization - Dialogues_Story_Localization"), localization);
            AddLocalizations(Resources.Load<TextAsset>("Localization/Vices_Localization - Dialogues_SPQuest_Localization"), localization);
            AddLocalizations(Resources.Load<TextAsset>("Localization/Vices_Localization - Dialogues_SQuest_Localization"), localization);
            AddLocalizations(Resources.Load<TextAsset>("Localization/Vices_Localization - Dialogues_Usual_Localization"), localization);
            AddLocalizations(Resources.Load<TextAsset>("Localization/Vices_Localization - Dialogues_Object_Localization"), localization);
            AddLocalizations(Resources.Load<TextAsset>("Localization/Vices_Localization - Diary_Localization"), localization);
            AddLocalizations(Resources.Load<TextAsset>("Localization/Vices_Localization - Menu_Localization"), localization);
        }

        private void AddLocalizations(TextAsset asset, LocalizationInfo localization)
        {
            var languageEnums = (LanguageEnum[])Enum.GetValues(typeof(LanguageEnum));

            string[] lines = asset.text.Split('\n');
            int languages = lines[0].Split('	').Length - 4;
            string fullname = "";

            for (int i = 1; i < lines.Length; i++)
            {
                var line = lines[i];

                string[] values = line.Split('	');
                fullname = values[1] + "." + values[2] + "." + values[3];

                for (int j = 0; j < languages; j++)
                {
                    localization.AddLocalization(fullname, languageEnums[j + 1], values[j + 4]);
                }
            }
        }
    }
}
