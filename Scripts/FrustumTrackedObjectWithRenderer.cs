using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrustumTrackedObjectWithRenderer : FrustumTrackedObject
{

    protected override bool GetVisibilityStatus()
    {
        return GetComponent<Renderer>().isVisible;
    }

    private void Start()
    {
        bounds = GetComponent<Renderer>().bounds;
    }

    private void OnBecameVisible()
    {
        VisibilityStateChange(true);
    }

    private void OnBecameInvisible()
    {
        VisibilityStateChange(false);
    }
}
