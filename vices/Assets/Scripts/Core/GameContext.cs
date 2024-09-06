using JabberwockyWorld.DialogueSystem.Scripts;
using JabberwockyWorld.DialogueSystem.Scripts.Data;
using JabberwockyWorld.Quest.Scripts;
using Vices.Scripts.Game;
using UnityEngine;

namespace Vices.Scripts.Core
{
    public class GameContext
    {
        public GameContext(GameConfig config)
        {
            Context = this;
            AssetsContext = config.AssetsContext;
            Localization = config.Localization;
            DialogueContainerInfo = config.DialogueContainerInfo;
            DialogueInfo = config.DialogueInfo;
            CutsceneInfo = config.CutsceneInfo;
            QuestInfo = config.QuestInfo;
            CutscenesInfo = config.CutscenesInfo;

            InteractablePool = new InteractablePool(AssetsContext, "InteractableUI Pool", "InteractableUI", 2);
        }

        public static GameContext Context { get; private set; }

        public AssetsContext AssetsContext { get; private set; }
        public LocalizationInfo Localization { get; private set; }
        public DialogueContainerInfo DialogueContainerInfo { get; private set; }
        public DialogueInfo DialogueInfo { get; private set; }
        public CutsceneInfo CutsceneInfo { get; private set; }
        public QuestInfo QuestInfo { get; private set; }
        public CutscenesInfo CutscenesInfo { get; private set; }
        public InteractablePool InteractablePool { get; private set; }
        public Vector3 PreviousTeleportPosition { get; set; }
        public string PreviousScene { get; set; }
    }
}