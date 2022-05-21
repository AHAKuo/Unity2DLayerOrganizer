using UnityEngine;

namespace AHAKuo.LayerOrganizer
{
    /// <summary>
    /// This class is added and removed and controlled dynamically by the layer organizer. It also has some functions to override the layer organizer settings.
    /// </summary>
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class RendererMarker : MonoBehaviour
    {
        // Marker used to give this gameobject's renderer the appropriate sorting group layer.

        Renderer Renderer;

        RendererMarker[] MarkersInChildren;

        /// <summary>
        /// Should this renderer ignore calculations and scaling features for distance?
        /// </summary>
        [Header("Sorter Overrides")]
        [Tooltip("Should this renderer ignore calculations and scaling features for distance?")]
        public bool IgnoreOrganizer = true;

        /// <summary>
        /// Should this renderer ignore orders and layer names?
        /// </summary>
        [Tooltip("Should this renderer ignore orders and layer names?")]
        [Space(15)]
        public bool IgnoreSorting = false;

        /// <summary>
        /// Should this renderer ignore just layer name sorting?
        /// </summary>
        [Tooltip("Should this renderer ignore just layer name sorting?")]
        [Space(15)]
        public bool OnlyIgnoreSortingName = false;

        /// <summary>
        /// If this is a background image, usually, we want the children to be rendered at the back. Unless otherwise.
        /// </summary>
        [Header("Extra Options")]
        [Tooltip("If this is a background image, usually, we want the children to be rendered at the back. Unless otherwise.")]
        [Space(25)]
        public bool RenderedInFrontOfChildren = false;

        /// <summary>
        /// Do we want to mark this renderer as nest? Object that contains other images. This is only a mark.
        /// </summary>
        [Tooltip("Do we want to mark this renderer as nest? Object that contains other images. This is only a mark.")]
        [Space(15)]
        public bool Nest = false;

        /// <summary>
        /// If any previous renderer higher in the hierarchy than this one renders in front of its children, check this to fix the sorting layer order of this renderer by adding 1 to its order. This should be checked for all renderers after that one.
        /// </summary>
        [Tooltip("If any previous renderer higher in the hierarchy than this one renders in front of its children, check this to fix the sorting layer order of this renderer by adding 1 to its order. This should be checked for all renderers after that one.")]
        [Space(15)]
        public bool PreviousRendersInFront = false;

        /// <summary>
        /// An option that can be used with sprites that have edges we need to hide when they are far from the camera. This option will allow them to scale with the Z distance.
        /// </summary>
        [Tooltip("An option that can be used with sprites that have edges we need to hide when they are far from the camera. This option will allow them to scale with the Z distance.")]
        [Space(15)]
        public bool ScaleWithDistance;

        /// <summary>
        /// The factor that influences the scale.
        /// </summary>
        [Tooltip("The factor that influences the scale.")]
        [Space(15)]
        [Range(0.7f, 2.5f)]
        public float ScaleFactor = 0.7f;

        /// <summary>
        /// The factor that influences the minimum scale the object can reach.
        /// </summary>
        [Tooltip("The factor that influences the minimum scale the object can reach.")]
        [Space(15)]
        [Range(1f, 3f)]
        public float MinScaleLimit = 1.3f;

        /// <summary>
        /// The factor that influences the maximum scale the object can reach.
        /// </summary>
        [Tooltip("The factor that influences the maximum scale the object can reach.")]
        [Space(15)]
        [Range(3f, 10f)]
        public float MaxScaleLimit = 3f;

        /// <summary>
        /// Should the scaling feature work during play mode?
        /// </summary>
        [Header("Editor Settings")]
        [Space(25)]
        [Tooltip("Should the scaling feature work during play mode?")]
        public bool ScaleDuringPlayMode = false;

        /// <summary>
        /// Should basic functions like sorting and ordering work in play mode?
        /// </summary>
        [Tooltip("Should basic functions like sorting and ordering work in play mode?")]
        [Space(15)]
        public bool BasicFunctionsDuringPlayMode = false;

#if UNITY_EDITOR

        public void SetSortingLayer(int order, string sortingLayerName)
        {
            if (IgnoreSorting)
            {
                return;
            }

            Renderer = GetComponent<Renderer>();
            MarkersInChildren = GetComponentsInChildren<RendererMarker>();

            // we perform checks and add up the final order number

            int Order = order; //first set the initial order received from LayerOrganizer.cs

            if (Renderer == null) // check if renderer is null
            {
                return;
            }

            if (!OnlyIgnoreSortingName) // apply layer name sort if allowed
            {
                Renderer.sortingLayerName = sortingLayerName;
            }

            if (RenderedInFrontOfChildren) // check if is rendered in front of children
            {
                if (transform.childCount > 0)
                {
                    Order += MarkersInChildren.Length;
                }
            }

            if (PreviousRendersInFront) // check if any previous markers render in front
            {
                if (RenderedInFrontOfChildren) // if it already renders in front of children, then we don't need to add 1 to the order anymore.
                {
                    return;
                }
                Order++;
            }


            Renderer.sortingOrder = Order; // set final order
        }


        public void SetSortingLayerNoName(int order)
        {
            if (IgnoreSorting)
            {
                return;
            }

            Renderer = GetComponent<Renderer>();
            MarkersInChildren = GetComponentsInChildren<RendererMarker>();

            // we perform checks and add up the final order number

            int Order = order; //first set the initial order received from LayerOrganizer.cs

            if (Renderer == null) // check if renderer is null
            {
                return;
            }

            if (RenderedInFrontOfChildren) // check if is rendered in front of children
            {
                if (transform.childCount > 0)
                {
                    Order += MarkersInChildren.Length;
                }
            }

            if (PreviousRendersInFront) // check if any previous markers render in front
            {
                if (RenderedInFrontOfChildren) // if it already renders in front of children, then we don't need to add 1 to the order anymore.
                {
                    return;
                }
                Order++;
            }


            Renderer.sortingOrder = Order; // set final order
        }

        private void Update()
        {
            if (IgnoreOrganizer)
            {
                if (Application.isPlaying && !BasicFunctionsDuringPlayMode)
                {
                    return;
                }

                if (GetComponent<SiblingMark>() != null)
                {
                    DestroyImmediate(gameObject.GetComponent<SiblingMark>());
                }

                if (transform.localPosition.z != 0) //Since this object ignores organizer, that means it's at the front of everything. We make sure it is zeroed on the z axis. This makes the Freeze Z Axis script unnecessary.
                {
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
                }
            }
            else
            {
                if (Application.isPlaying && !BasicFunctionsDuringPlayMode)
                {
                    return;
                }

                if (GetComponent<SiblingMark>() == null)
                {
                    gameObject.AddComponent<SiblingMark>();
                }

            }

            if (Nest)
            {
                if (Application.isPlaying && !BasicFunctionsDuringPlayMode)
                {
                    return;
                }

                if (GetComponent<RendererNest>() == null)
                {
                    gameObject.AddComponent<RendererNest>();
                }
            }
            else
            {
                if (Application.isPlaying && !BasicFunctionsDuringPlayMode)
                {
                    return;
                }

                if (GetComponent<RendererNest>() != null)
                {
                    DestroyImmediate(gameObject.GetComponent<RendererNest>());
                }
            }

            if (ScaleWithDistance)
            {
                if (IgnoreOrganizer)
                {
                    return;
                }

                if (Application.isPlaying && !ScaleDuringPlayMode)
                {
                    return;
                }

                float ScaleObtained = (transform.localPosition.z / 18) * ScaleFactor;

                if (ScaleObtained < MinScaleLimit)
                {
                    ScaleObtained = MinScaleLimit;
                }
                else if (ScaleObtained > MaxScaleLimit)
                {
                    ScaleObtained = MaxScaleLimit;
                }

                transform.localScale = new Vector3(ScaleObtained, ScaleObtained, ScaleObtained);
            }
        }
#endif
    }
}
