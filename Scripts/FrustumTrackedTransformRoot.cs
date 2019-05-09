using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrustumTrackedTransformRoot : FrustumTrackedObject
{
    Bounds cachedBounds;
    Vector3 boundsRelatvePos;

    private void Start()
    {
        cachedBounds = CacheBounds();
        boundsRelatvePos = cachedBounds.center - transform.position;
    }

    protected override Bounds GetBounds()
    {
        cachedBounds.center = transform.position + boundsRelatvePos;
        return cachedBounds;
    }


    /// <summary>
    /// Calculate total bounds for all renderers inside object's hierarchy
    /// </summary>
    /// <returns></returns>
    Bounds CacheBounds()
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
