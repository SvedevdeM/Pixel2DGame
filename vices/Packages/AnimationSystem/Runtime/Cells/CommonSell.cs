using System;
using UnityEngine;


namespace JabberwockyWorld.AnimationSystem.Scripts
{
    [Serializable]
    public class CommonCel : ICEL
    {
        [SerializeField] protected Sprite sprite;
        [SerializeField] protected DriverDictionary drivers = new DriverDictionary();

        public void ChangeSprite(Sprite newsprite)
        {
            this.sprite = newsprite;
        }

        public CommonCel()
        {
        }

        public CommonCel(Sprite sprite = null, DriverDictionary drivers = null)
        {
            if (sprite != null)
                this.sprite = sprite;
            if (drivers != null)
                this.drivers = drivers;
        }

        public virtual void ApplyToRenderer(
            AnimatorState previousState,
            AnimatorState nextState,
            SpriteRenderer renderer
        )
        {
            nextState.Merge(drivers);
            renderer.sprite = sprite;
            renderer.flipX = previousState.ShouldFlip();
        }
    }
}