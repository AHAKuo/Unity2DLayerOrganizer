using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AHAKuo.LayerOrganizer
{
    /// <summary>
    /// A class that is added and removed automatically which is what sets up the renderers distance on the Z axis.
    /// </summary>
    [DisallowMultipleComponent]
    public class SiblingMark : MonoBehaviour
    {
#if UNITY_EDITOR
        //sibling marker used to identify which renderer we ignore in the layer organizer calculation.

        public void CalculateAndSetDistances(float MaxDistance, int Index, int NumberOfSiblings, bool ForeGround) //A calculation method which will take into account different factors and give this object its appropriate z axis position. It is called by the layer organizer script which fills its number of siblings.
        {
            //We first calculate the probable distance factor based on hierarchy index.
            float Distance;
            float MultFactor;


            if (ForeGround)
            {
                MultFactor = MaxDistance / NumberOfSiblings;
                Distance = MultFactor * Index * -1;
                // Then we set the z position we get and leave it without absolution since it is foreground.
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, Distance);
            }
            else
            {
                MultFactor = MaxDistance / NumberOfSiblings;
                Distance = Mathf.Abs(MultFactor * Index - MaxDistance);
                // Then we set the z position we get but make sure it is absolute since background values should be positive.
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, Distance);
            }
        }
#endif
    }
}
