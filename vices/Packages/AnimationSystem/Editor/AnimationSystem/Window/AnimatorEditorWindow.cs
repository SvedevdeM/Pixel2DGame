#if UNITY_EDITOR
using JabberwockyWorld.AnimationSystem.Editor.Utiilites;
using JabberwockyWorld.AnimationSystem.Editor.Utilities;
using System.IO;

using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace JabberwockyWorld.AnimationSystem.Editor
{
    public class AnimationEditorWindow : EditorWindow
    {
        private AnimationGraphView _graphView;
        private Button _saveButton;
        private string _defaultFileName = "Animator Graph";

        private static TextField _fileNameTextField;

        [MenuItem("JabberwockyWorld/Animation System/Animator Graph")]
        public static void ShowExample()
        {
            GetWindow<AnimationEditorWindow>("Animator Graph");
        }

        private void OnEnable()
        {
            AddGraphView();
            AddToolbar();
            AddStyles();
        }

        private void AddGraphView()
        {
            _graphView = new AnimationGraphView(this);

            _graphView.StretchToParentSize();

            rootVisualElement.Add(_graphView);
        }

        private void AddToolbar()
        {
            Toolbar toolbar = new Toolbar();

            _fileNameTextField = AnimationElementUtility.CreateTextField(_defaultFileName, "File name:");
            _saveButton = AnimationElementUtility.CreateButton("Save", () => Save());
            Button loadButton = AnimationElementUtility.CreateButton("Load", () => Load());
            Button clearButton = AnimationElementUtility.CreateButton("Clear", () => Clear());
            Button resetButton = AnimationElementUtility.CreateButton("Reset", () => ResetGraph());

            toolbar.Add(_fileNameTextField);
            toolbar.Add(_saveButton);
            toolbar.Add(loadButton);
            toolbar.Add(clearButton);
            toolbar.Add(resetButton);

            toolbar.AddStyleSheets("AnimationSystem/AnimationToolbarStyle.uss");

            rootVisualElement.Add(toolbar);
        }

        private void Save()
        {
            if (string.IsNullOrEmpty(_fileNameTextField.text))
            {
                EditorUtility.DisplayDialog(
                    "Invalid graph name.",
                    "Please make sure the file is typed!",
                    "Ok!"
                    );

                return;
            }

            AnimationIOUtility.Initialize(_graphView, _fileNameTextField.value);
            AnimationIOUtility.Save();
        }

        private void Load()
        {
            string filePath = EditorUtility.OpenFilePanel("Animation Graphs", "Assets/Editor/AnimationSystem/Graphs", "asset");

            if (string.IsNullOrEmpty(filePath)) return;

            Clear();

            AnimationIOUtility.Initialize(_graphView, Path.GetFileNameWithoutExtension(filePath));
            AnimationIOUtility.Load();
        }

        private void Clear()
        {
            _graphView.ClearGraph();
        }

        private void ResetGraph()
        {
            Clear();

            UpdateFileName(_defaultFileName);
        }

        public static void UpdateFileName(string fileName)
        {
            _fileNameTextField.value = fileName;
        }

        public void EnableSaving()
        {
            _saveButton.SetEnabled(true);
        }

        public void DisableSaving()
        {
            _saveButton.SetEnabled(false);
        }

        private void AddStyles()
        {
            rootVisualElement.AddStyleSheets("AnimationSystem/AnimationVariables.uss");
        }
    }
}
#endif