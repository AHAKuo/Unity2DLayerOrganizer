using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AHAKuo.LayerOrganizer
{
    /// <summary>
    /// A class to handle the sorting of renderers in the parent and children of the game object it's placed on. This will sort all types of renderers, including Sprite Renderers, Particle Renderers, Mesh Renderers, etc...
    /// </summary>
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    public class LayerOrganizer : MonoBehaviour
    {
        [Header("Sorter Settings")]
        [Tooltip("Should this organizer also set layer order?")]
        public bool UseSorting = false;

        [Space(5)]
        [Tooltip("At which index does the sorting order or renderers start.")]
        public int StartIndex; //For the sorting of renderers layer order.

        [Space(15)]
        [Tooltip("Should this layer organizer also set layer name?")]
        public bool SortLayerNameToo = false;

        [Space(5)]
        [Tooltip("Should this layer organizer also set layer name?")]
        public string sortingLayerName; //For the sorting of renderers layer names.

        [Header("Perspective Settings")]
        [Space(25)]
        [Tooltip("This tool uses a max distance factor and organizes all renderer markers that have a sibling mark on them on the z axis.")]
        public bool SortZAxisPositions = false;

        [Space(15)]
        [Tooltip("If the renderers are on the foreground layer, check this to sort them in the opposite direction.")]
        public bool Foreground;

        [Tooltip("How far should the last image in the scene be in the z axis?")]
        [Space(15)]
        [Range(0, 100)]
        public float MaxDistanceInZAxis = 100;

        [Header("Editor Settings")]
        [Space(25)]
        [Tooltip("Should this organizer work in play mode?")]
        public bool WorkDuringPlaymode = false;

        [Space(15)]
        [Tooltip("If there is a nested renderer, we can name it")]
        public bool NameNests; //Used for the naming of sprite nests (i.e., parents with children sprites).

        [Space(15)]
        [Tooltip("What suffix do we want to add to it?")]
        public string SuffixNest = "Frame []"; //Used for the naming of sprite nests (i.e., parents with children sprites).

        /// <summary>
        /// This field is updated automatically, best to leave it as it is. It handles the distancing of renderers in the Z axis to create depth and parallax automatically. The objects that distance must have 'Ignore Organizer' set to false.
        /// </summary>
        private SiblingMark[] SiblingsToFactor;

        /// <summary>
        /// Used to add RendererMarker component to all children automatically for the sorting of groups.
        /// </summary>
        private Transform[] Children;

        /// <summary>
        /// This field is updated automatically, best to leave it as it is. It handles the actual order of sorting, which is controlled from hierarchy of objects.
        /// </summary>
        private RendererMarker[] RendererMarkers;

        /// <summary>
        /// This field is updated automatically, best to leave it as it is. It handles the actual order of sorting, which is controlled from hierarchy of objects.
        /// </summary>
        private RendererNest[] RendererNests;

#if UNITY_EDITOR

        private void Update() //This will be called each time we change something in the scene. It adds renderer markers to the children gameobjects, and sets their sorting group layer and order if they have a renderer.
        {
            if (!WorkDuringPlaymode)
            {
                if (Application.isPlaying)
                {
                    return;
                }
            }

            if (!Application.isPlaying)
            {
                if (NameNests)
                {
                    RendererNests = GetComponentsInChildren<RendererNest>();

                    for (int i = 0; i < RendererNests.Length; i++)
                    {
                        if (Foreground)
                        {
                            RendererNests[i].NameSelf(" Foreground " + SuffixNest);
                        }
                        else
                        {
                            RendererNests[i].NameSelf(" Background " + SuffixNest);
                        }
                    }
                }
            }

            if (UseSorting) //Should this factor calculator also set the sorting layer order?
            {
                Children = gameObject.GetComponentsInChildren<Transform>();
                for (int i = 0; i < Children.Length; i++) //If the child doesn't have a renderer marker, we add it to them if they have a renderer.
                {
                    if (Children[i].gameObject.GetComponent<Renderer>() != null) //If this child has a renderer of any sort. We add a marker to it.
                    {
                        if (Children[i].gameObject.GetComponent<RendererMarker>() == null)
                        {
                            Children[i].gameObject.AddComponent<RendererMarker>();
                        }
                    }
                }

                RendererMarkers = GetComponentsInChildren<RendererMarker>();

                if (RendererMarkers == null)
                {
                    Debug.LogWarning("We can't calculate anything because no children have a renderer marker.");
                    return;
                }

                for (int i = 0; i < RendererMarkers.Length; i++)
                {
                    if (SortLayerNameToo)
                    {
                        RendererMarkers[i].SetSortingLayer(i + StartIndex, sortingLayerName);
                    }
                    else
                    {
                        RendererMarkers[i].SetSortingLayerNoName(i + StartIndex);
                    }
                }
            }

            if (!SortZAxisPositions)
            {
                return;
            }

            SiblingsToFactor = GetComponentsInChildren<SiblingMark>();
            RendererMarkers = GetComponentsInChildren<RendererMarker>();

            for (int i = 0; i < SiblingsToFactor.Length; i++)
            {
                if (Foreground)
                {
                    SiblingsToFactor[i].CalculateAndSetDistances(MaxDistanceInZAxis, i, SiblingsToFactor.Length, true);
                }
                else
                {
                    SiblingsToFactor[i].CalculateAndSetDistances(MaxDistanceInZAxis, i, SiblingsToFactor.Length, false);
                }
            }
        }

        //////////////////////////////////////////////////////

        //CONTEXT MENU FUNCTIONS\\


        [MenuItem("GameObject/AHAKuo/Renderer Functions/Sort Order", false, -10)] //A context menu item function.
        public static void SortLayersOrder()
        {
            GameObject ActiveSelection = Selection.activeGameObject;

            LayerOrganizer layerOrganizer = ActiveSelection.AddComponent<LayerOrganizer>();

            layerOrganizer.UseSorting = true;
        }

        [MenuItem("GameObject/AHAKuo/Renderer Functions/Sort to Name", false, -10)] //A context menu item function. << This can be duplicated for any layer you have in the project.
        public static void SortLayersToBackground()
        {
            GameObject ActiveSelection = Selection.activeGameObject;

            LayerOrganizer layerOrganizer = ActiveSelection.AddComponent<LayerOrganizer>();

            layerOrganizer.UseSorting = true;

            layerOrganizer.SortLayerNameToo = true;

            layerOrganizer.sortingLayerName = "Name";
        }

        [MenuItem("GameObject/AHAKuo/Renderer Functions/Sort to Player", false, -10)] //A context menu item function. << This can be duplicated for any layer you have in the project.
        public static void SortLayersToPlayer()
        {
            GameObject ActiveSelection = Selection.activeGameObject;

            LayerOrganizer layerOrganizer = ActiveSelection.AddComponent<LayerOrganizer>();

            layerOrganizer.UseSorting = true;

            layerOrganizer.SortLayerNameToo = true;

            layerOrganizer.sortingLayerName = "Player";
        }
#endif
    }
}
