using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AHAKuo.LayerOrganizer
{
    /// <summary>
    /// A class to simply name the nest of renderers and mark them to the layer organizer.
    /// </summary>
    [DisallowMultipleComponent] [ExecuteInEditMode]
    public class RendererNest : MonoBehaviour
    {
    #if UNITY_EDITOR
        public void NameSelf(string Suffix = " Frame []")
        {
            gameObject.name = "Sprite Nest " + transform.GetSiblingIndex() + Suffix;
        }
    #endif
    }
}


