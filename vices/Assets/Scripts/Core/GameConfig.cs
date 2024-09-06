using JabberwockyWorld.DialogueSystem.Scripts;
using JabberwockyWorld.DialogueSystem.Scripts.Data;
using JabberwockyWorld.Quest.Scripts;
using UnityEngine;
using Vices.Scripts.Game;

namespace Vices.Scripts.Core
{
    [CreateAssetMenu(fileName = nameof(GameConfig), menuName = "Vices/" + nameof(GameConfig))]
    public class GameConfig : ScriptableObject
    {
        public AssetsContext AssetsContext;
        public LocalizationInfo Localization;
        public DialogueContainerInfo DialogueContainerInfo;
        public DialogueInfo DialogueInfo;
        public CutsceneInfo CutsceneInfo;
        public QuestInfo QuestInfo;
        public CutscenesInfo CutscenesInfo;
    }
}
