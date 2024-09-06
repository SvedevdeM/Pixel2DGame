using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using JabberwockyWorld.AnimationSystem.Scripts;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
namespace JabberwockyWorld.AnimationSystem.Editor
{
    [CustomEditor(typeof(SimpleAnimationNode))]
    public class SimpleAnimationNodeEditor : AnimationNodeEditor
    {
        [MenuItem("Assets/Create/Animation System/Simple Animation (From Textures)", false, 400)]
        private static void CreateFromTextures()
        {
            var trailingNumbersRegex = new Regex(@"(\d+$)");

            var sprites = new List<Sprite>();
            var textures = Selection.GetFiltered<Texture2D>(SelectionMode.Assets);
            foreach (var texture in textures)
            {
                string path = AssetDatabase.GetAssetPath(texture);
                sprites.AddRange(AssetDatabase.LoadAllAssetsAtPath(path).OfType<Sprite>());
            }

            var cels = sprites
                .OrderBy(
                    sprite =>
                    {
                        var match = trailingNumbersRegex.Match(sprite.name);
                        return match.Success ? int.Parse(match.Groups[0].Captures[0].ToString()) : 0;
                    }
                )
                .Select(sprite => new CommonCel(sprite))
                .ToArray();

            var asset = SimpleAnimationNode.Create<SimpleAnimationNode>(
                cels: cels
            );
            string baseName = trailingNumbersRegex.Replace(textures[0].name, "");
            asset.name = baseName + "_animation";

            string assetPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(textures[0]));
            AssetDatabase.CreateAsset(asset, Path.Combine(assetPath ?? Application.dataPath, asset.name + ".asset"));
            AssetDatabase.SaveAssets();
        }

        [MenuItem("Assets/Create/Animation System/Simple Animation (From Textures)", true, 400)]
        private static bool CreateFromTexturesValidation()
        {
            return Selection.GetFiltered<Texture2D>(SelectionMode.Assets).Length > 0;
        }
    }
}
#endif