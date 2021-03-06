﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrustumTrackedObjectWithRenderer : FrustumTrackedObject
{

    protected override bool GetVisibilityStatus()
    {
        return GetComponent<Renderer>().isVisible;
    }

    Renderer r;

    private void Start()
    {
        r = GetComponent<Renderer>();
    }

    protected override Bounds GetBounds()
    {
        return r.bounds;
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
