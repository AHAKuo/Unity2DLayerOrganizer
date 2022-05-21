using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AHAKuo.LayerOrganizer
{
    /// <summary>
    /// This class is used on global art that will be following the camera. This is stuff like sky art and the like. It handles placing it on the camera as a child.
    /// </summary>
    public class GlobalArtReset : MonoBehaviour
    {
        public float DistanceFromCamera = 18;

        private void Awake()
        {
            if (Application.isPlaying)
            {
                transform.parent = Camera.main.transform;
                transform.localPosition = new Vector3(0, 0, DistanceFromCamera);
                return;
            }
        }
    }
}

