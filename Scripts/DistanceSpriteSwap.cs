using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AHAKuo.LayerOrganizer
{
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    /// <summary>
    /// A simple script to swap out the current sprite-renderer's sprite with another sprite depeneding on the distance in Z axis. Can be used for things like sprite blur by swapping out sprites.
    /// </summary>
    public class DistanceSpriteSwap : MonoBehaviour
    {
        public string Position = "Close";

        [Tooltip("This script swaps sprite based on their distances. Used mostly for blurred alternatives of sprites. It is disabled by default on addition.")]
        public bool Active = false;

        [Space(15)]
        [Tooltip("This should be either the parent transform if it is decor art or itself.")]
        public Transform RelativeTransform;

        [Tooltip("This script swaps sprite based on their distances. Used mostly for blurred alternatives of sprites. It is disabled by default on addition.")]
        public SpriteRenderer spriteRenderer;

        public Sprite CloseOrDefaultSprite;

        public Sprite MediumSprite;

        public Sprite FarSprite;

        [Tooltip("Setting this sprite as a foreground image will basically divide the distances set below by 4.")]
        public bool Foreground = false;

        public float CloseOrDefaultDistance = 0;

        [Range(0, 100)]
        public float MediumDistance = 30;

        [Range(0, 100)]
        public float FarDistance = 60;

        private void Awake()
        {
            if (Application.isPlaying)
            {
                return;
            }

            if (GetComponent<SpriteRenderer>() != null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }
            else
            {
                Destroy(this);
            }
        }

        private void Update()
        {
            if (Application.isPlaying)
            {
                return;
            }

            if (!Active)
            {
                return;
            }

            if (spriteRenderer == null)
            {
                Destroy(this);
            }

            if (RelativeTransform == null)
            {
                return;
            }

            //Here we check the distances of the transform and apply the sprite swap.

            //if close...

            if (Foreground)
            {
                if (Mathf.Abs(RelativeTransform.position.z) > FarDistance / 4)
                {
                    Position = "Far";
                    spriteRenderer.sprite = FarSprite;
                    return;
                }

                if (Mathf.Abs(RelativeTransform.position.z) > MediumDistance / 4)
                {
                    Position = "Medium";
                    spriteRenderer.sprite = MediumSprite;
                    return;
                }

                if (Mathf.Abs(RelativeTransform.position.z) < MediumDistance / 4)
                {
                    Position = "Close";
                    spriteRenderer.sprite = CloseOrDefaultSprite;
                    return;
                }
            }
            else
            {
                if (Mathf.Abs(RelativeTransform.position.z) > FarDistance)
                {
                    Position = "Far";
                    spriteRenderer.sprite = FarSprite;
                    return;
                }

                if (Mathf.Abs(RelativeTransform.position.z) > MediumDistance)
                {
                    Position = "Medium";
                    spriteRenderer.sprite = MediumSprite;
                    return;
                }

                if (Mathf.Abs(RelativeTransform.position.z) < MediumDistance)
                {
                    Position = "Close";
                    spriteRenderer.sprite = CloseOrDefaultSprite;
                    return;
                }
            }

        }
    }
}
