using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrustumMarkerWithText : FrustumMarker
{
    [SerializeField] Transform textAnchor;
    [SerializeField] TextMesh trackedNameText;

    private void Update()
    {
        textAnchor.transform.forward = -transform.parent.right;
    }

    public override void SetMarkerData(TrackedObjectData data)
    {
        if(data != null)
            trackedNameText.text = data.name;
    }
}
