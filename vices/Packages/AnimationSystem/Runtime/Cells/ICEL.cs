using UnityEngine;

namespace JabberwockyWorld.AnimationSystem.Scripts
{
    public interface ICEL
    {
        public void ApplyToRenderer(
            AnimatorState previousState,
            AnimatorState nextState,
            SpriteRenderer renderer
        );

        //public void ChangeSprite(Sprite newsprite);
    }
}