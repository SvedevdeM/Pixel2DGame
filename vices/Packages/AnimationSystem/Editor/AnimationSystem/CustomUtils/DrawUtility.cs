using UnityEngine;

namespace JabberwockyWorld.AnimationSystem.Editor
{
    public static class DrawUtility
    {
        public static void DrawTexturePreview(Rect position, Sprite sprite)
        {
            var fullSize = new Vector2(sprite.texture.width, sprite.texture.height);
            var size = new Vector2(sprite.textureRect.width, sprite.textureRect.height);

            var coords = sprite.textureRect;
            coords.x /= fullSize.x;
            coords.width /= fullSize.x;
            coords.y /= fullSize.y;
            coords.height /= fullSize.y;

            Vector2 ratio;
            ratio.x = position.width / size.x;
            ratio.y = position.height / size.y;
            float minRatio = Mathf.Min(ratio.x, ratio.y);

            var center = position.center;
            position.width = size.x * minRatio;
            position.height = size.y * minRatio;
            position.center = center;

            GUI.DrawTextureWithTexCoords(position, sprite.texture, coords);
        }
    }
}