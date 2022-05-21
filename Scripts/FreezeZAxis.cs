using UnityEngine;
using UnityEditor;

namespace AHAKuo.LayerOrganizer
{
    /// <summary>
    /// Simply freezes the object in Z axis to it remains there even if moved accidentally.
    /// </summary>
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    public class FreezeZAxis : MonoBehaviour
    {
#if UNITY_EDITOR
        [MenuItem("GameObject/AHAKuo/Renderer Functions/Fixate To Z Axis 0", false, -10)] //A context menu item function.
        public static void FixToZ()
        {
            foreach (GameObject obj in Selection.gameObjects)
            {
                obj.AddComponent(typeof(FreezeZAxis));
            }
        }

        private void Update()
        {
            if (Application.isPlaying)
            {
                return;
            }

            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
        }
#endif
    }
}
