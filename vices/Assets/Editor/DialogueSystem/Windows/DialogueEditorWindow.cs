using JabberwockyWorld.DialogueSystem.Editor.Utilities;
using System.IO;

using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace JabberwockyWorld.DialogueSystem.Editor
{
    public class DialogueEditorWindow : EditorWindow
    {
        private DialogueGraphView _graphView;
        private Button _saveButton;
        private string _defaultFileName = "Dialogue Graph";

        private static TextField _fileNameTextField;

        [MenuItem("JabberwockyWorld/Window/Dialogue Graph")]
        public static void ShowExample()
        {
            GetWindow<DialogueEditorWindow>("Dialogue Graph");
        }

        private void OnEnable()
        {
            AddGraphView();
            AddToolbar();

            AddStyles();
        }

        private void AddGraphView()
        {
            _graphView = new DialogueGraphView(this);

            _graphView.StretchToParentSize();

            rootVisualElement.Add(_graphView);
        }

        private void AddToolbar()
        {
            Toolbar toolbar = new Toolbar();

            _fileNameTextField = DialogueElementUtility.CreateTextField(_defaultFileName, "File name:");
            _saveButton = DialogueElementUtility.CreateButton("Save", () => Save());
            Button loadButton = DialogueElementUtility.CreateButton("Load", () => Load());
            Button clearButton = DialogueElementUtility.CreateButton("Clear", () => Clear());
            Button resetButton = DialogueElementUtility.CreateButton("Reset", () => ResetGraph());

            toolbar.Add(_fileNameTextField);
            toolbar.Add(_saveButton);
            toolbar.Add(loadButton);
            toolbar.Add(clearButton);
            toolbar.Add(resetButton);

            toolbar.AddStyleSheets("DialogueSystem/DialogueToolbarStyle.uss");

            rootVisualElement.Add(toolbar);
        }

        private void Save()
        {
            if(string.IsNullOrEmpty(_fileNameTextField.text))
            {
                EditorUtility.DisplayDialog(
                    "Invalid graph name.",
                    "Please make sure the file is typed!",
                    "Ok!"
                    );

                return;
            }

            DialogueIOUtility.Initialize(_graphView, _fileNameTextField.value);
            DialogueIOUtility.Save();
        }

        private void Load()
        {
            string filePath = EditorUtility.OpenFilePanel("Dialogue Graphs", "Assets/Editor/DialogueSystem/Graphs", "asset");

            if (string.IsNullOrEmpty(filePath)) return;

            Clear();

            string fileName = "";
            var values = filePath.Split('/');
            int index = 0;

            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] != "Graphs") continue;
                index = i + 1;
                break;
            }

            for (int i = index; i < values.Length; i++)
            {
                fileName += values[i] + "/";
            }

            var result = fileName.Remove(fileName.Length - 1);
            DialogueIOUtility.Initialize(_graphView, result);
            DialogueIOUtility.Load();
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
            rootVisualElement.AddStyleSheets("DialogueSystem/DialogueVariables.uss");
        }
    }
}
