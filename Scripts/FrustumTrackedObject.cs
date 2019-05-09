using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for tracked object component, 
/// a game object having this component will generate events when 
/// it becomes visible or unvisible inside a viewing frustum
/// </summary>
public abstract class FrustumTrackedObject : MonoBehaviour
{
    /// <summary>
    /// This event will fire each time when visibility status is changed
    /// </summary>
    public static event Action<FrustumTrackedObject, bool> OnVisibilityChanged = delegate { };

    public TrackedObjectData data;

    protected bool isVisible;
    public bool IsVisible { get { return isVisible; } }

    protected virtual bool GetVisibilityStatus()
    {
        return isVisible;
    }

    protected Bounds bounds;
    public Bounds Bounds => bounds;

    protected void VisibilityStateChange(bool isVisible)
    {
        OnVisibilityChanged(this, isVisible);
    }

    private void OnDestroy()
    {
        VisibilityStateChange(true);
    }

    internal void RefreshState()
    {
        VisibilityStateChange(isVisible);
    }

    internal void SetVisibility(bool v)
    {
        if (isVisible != v)
        {
            isVisible = v;
            VisibilityStateChange(v);
        }
    }
}
