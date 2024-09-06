using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JabberwockyWorld.AnimationSystem.Scripts
{
    [Serializable]
    public class FlippedCel : ICEL
    {
        [SerializeField] protected Sprite sprite;
        [SerializeField] protected DriverDictionary drivers = new DriverDictionary();
        [SerializeField] private Sprite spriteLeft;

        public void ApplyToRenderer(
            AnimatorState previousState,
            AnimatorState nextState,
            SpriteRenderer renderer
        )
        {
            nextState.Merge(drivers);
            renderer.sprite = previousState.ShouldFlip() ? spriteLeft : sprite;
            renderer.flipX = false;
        }
    }
}
