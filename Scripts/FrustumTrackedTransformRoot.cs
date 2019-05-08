using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrustumTrackedTransformRoot : FrustumTrackedObject
{
    private void Start()
    {
        bounds = GetBounds();
    }

    /// <summary>
    /// Calculate total bounds for all renderers inside object's hierarchy
    /// </summary>
    /// <returns></returns>
    Bounds GetBounds()
    {
        var renderers = GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return new Bounds(transform.position, Vector3.zero);
        Bounds b = renderers[0].bounds;
        foreach (var r in renderers)
        {
            b.Encapsulate(r.bounds);
        }
        return b;
    }
}
