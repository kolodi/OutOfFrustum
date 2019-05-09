using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrustumMarkerWithText : FrustumMarker
{
    [SerializeField] Transform textAnchor;

    private void Update()
    {
        textAnchor.transform.forward = -transform.parent.right;
    }
}
