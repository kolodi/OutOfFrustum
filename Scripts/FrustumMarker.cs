using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Visual marker to display the direction of the target object which is out of the viewing frustum
/// </summary>
public class FrustumMarker : MonoBehaviour
{
    public virtual void SetMarkerData(TrackedObjectData data)
    {
        Debug.Log("Nem marker created with name " + data.name);
    }
}
