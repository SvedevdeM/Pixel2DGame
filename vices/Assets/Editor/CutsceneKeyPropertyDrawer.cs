using System;
using System.Collections.Generic;
using Vices.Scripts.Game;
using UnityEditor;
using UnityEngine;

namespace Vices.Editor
{
    [CustomPropertyDrawer(typeof(CutsceneKey))]
    public class CutsceneKeyPropertyDrawer : PropertyDrawer
    {
        //TODO: Change this onGUI method for easier reading
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var keyType = property.FindPropertyRelative("Type");
            var keyTime = property.FindPropertyRelative("Time");

            position.width = 45;
            EditorGUI.LabelField(position, "Time:");

            position.x += Space(position);
            position.width = 80;

            keyTime.floatValue = float.Parse(EditorGUI.TextField(position, keyTime.floatValue.ToString()));

            position.x += Space(position, 15);
            position.width = 120;

            Dictionary<CutsceneActionType, int> typeEnums = new Dictionary<CutsceneActionType, int>();
            CutsceneActionType[] values = (CutsceneActionType[])Enum.GetValues(typeof(CutsceneActionType));
            for (int i = 0; i < values.Length; i++)
            {
                typeEnums.Add(values[i], i);
            }

            if (keyType.enumValueIndex >= values.Length) keyType.enumValueIndex = 0;
            else
            {
                CutsceneActionType type = (CutsceneActionType)EditorGUI.EnumPopup(position, values[keyType.enumValueIndex]);
                if (typeEnums.TryGetValue(type, out int index)) keyType.enumValueIndex = index;
            }

            position.x += Space(position, 15);
            position.width = 80;

            switch (values[keyType.enumValueIndex])
            {
                case CutsceneActionType.PlayDialogue:
                    var dialogue = property.FindPropertyRelative("Dialogue");
                    EditorGUILayout.ObjectField(dialogue);
                    break;
                case CutsceneActionType.PlaySound:
                    var audio = property.FindPropertyRelative("Audio");
                    EditorGUILayout.ObjectField(audio);
                    break;
                case CutsceneActionType.PlayAnimation:
                    var cutsceneAnimation = property.FindPropertyRelative("CutsceneAnimation");
                    var animationTarget = cutsceneAnimation.FindPropertyRelative("Target");
                    var switchNode = cutsceneAnimation.FindPropertyRelative("Node");
                    var driveName = cutsceneAnimation.FindPropertyRelative("Drive");
                    var animationValue = cutsceneAnimation.FindPropertyRelative("Value");
                    var flipAnimation = cutsceneAnimation.FindPropertyRelative("Flip");
                    EditorGUILayout.ObjectField(animationTarget);
                    EditorGUILayout.ObjectField(switchNode);
                    driveName.stringValue = EditorGUILayout.TextField("Controll Name:", $"{driveName.stringValue}");
                    animationValue.intValue = EditorGUILayout.IntField("Animation order:", animationValue.intValue);
                    flipAnimation.boolValue = EditorGUILayout.Toggle("Flip Animation", flipAnimation.boolValue);
                    break;
                case CutsceneActionType.CameraMove:
                    var camera = property.FindPropertyRelative("CameraTarget");
                    var cameraTarget = camera.FindPropertyRelative("Transform");
                    var cameraDuration = camera.FindPropertyRelative("Duration");

                    cameraDuration.floatValue = EditorGUILayout.FloatField("Duration:", cameraDuration.floatValue);
                    EditorGUILayout.ObjectField(cameraTarget);
                    break;
                case CutsceneActionType.TargetMove:
                    var move = property.FindPropertyRelative("MoveTarget");
                    var moveTarget = move.FindPropertyRelative("Target");
                    var movePosition= move.FindPropertyRelative("Point");
                    var moveDuration = move.FindPropertyRelative("Duration");

                    moveDuration.floatValue = EditorGUILayout.FloatField("Duration:", moveDuration.floatValue);
                    EditorGUILayout.ObjectField(moveTarget);
                    EditorGUILayout.ObjectField(movePosition);
                    break;
                case CutsceneActionType.EnableGameObject:
                    var gameObjectTarget = property.FindPropertyRelative("GameObjectEnable");
                    var gameObject = gameObjectTarget.FindPropertyRelative("GameObject");
                    var enable = gameObjectTarget.FindPropertyRelative("Enable");

                    EditorGUILayout.ObjectField(gameObject);
                    enable.boolValue = EditorGUILayout.Toggle("Enable:", enable.boolValue);
                    break;
                case CutsceneActionType.WaitForInput:
                    var inputAmount = property.FindPropertyRelative("InputAmount");
                    inputAmount.intValue = EditorGUILayout.IntField("Input Amount:", inputAmount.intValue);
                    break;
                case CutsceneActionType.ExecuteEvent:
                    var eventData = property.FindPropertyRelative("EventData");
                    EditorGUILayout.ObjectField(eventData);
                    break;
                case CutsceneActionType.LoadScene:
                    var sceneName = property.FindPropertyRelative("SceneName");
                    sceneName.stringValue = EditorGUILayout.TextField("Scene Name:", $"{sceneName.stringValue}");
                    break;
                case CutsceneActionType.ReturnToPlayer:
                    var targetCamera = property.FindPropertyRelative("CameraTarget");
                    var cameraDurations = targetCamera.FindPropertyRelative("Duration");

                    cameraDurations.floatValue = EditorGUILayout.FloatField("Duration:", cameraDurations.floatValue);
                    break;
                case CutsceneActionType.PlayPlayerAnimation:
                    cutsceneAnimation = property.FindPropertyRelative("CutsceneAnimation");
                    switchNode = cutsceneAnimation.FindPropertyRelative("Node");
                    driveName = cutsceneAnimation.FindPropertyRelative("Drive");
                    animationValue = cutsceneAnimation.FindPropertyRelative("Value");
                    flipAnimation = cutsceneAnimation.FindPropertyRelative("Flip");
                    EditorGUILayout.ObjectField(switchNode);
                    driveName.stringValue = EditorGUILayout.TextField("Controll Name:", $"{driveName.stringValue}");
                    animationValue.intValue = EditorGUILayout.IntField("Animation order:", animationValue.intValue);
                    flipAnimation.boolValue = EditorGUILayout.Toggle("Flip Animation", flipAnimation.boolValue);
                    break;
                case CutsceneActionType.MovePlayer:
                    move = property.FindPropertyRelative("MoveTarget");
                    movePosition = move.FindPropertyRelative("Point");
                    moveDuration = move.FindPropertyRelative("Duration");

                    moveDuration.floatValue = EditorGUILayout.FloatField("Duration:", moveDuration.floatValue);
                    EditorGUILayout.ObjectField(movePosition);
                    break;
                default:
                    break;
            }
        }

        private float Space(Rect position, int addSpace = 0)
        {
            return position.width + addSpace;
        }
    }
}
