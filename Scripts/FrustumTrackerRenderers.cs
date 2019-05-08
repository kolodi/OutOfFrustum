using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Simple tracking objects manager, 
/// using unity's build in OnBecameVisible and OnBecameInvisible callbacks
/// </summary>
public class FrustumTrackerRenderers : FrustrumTrackerBase
{
    private void Start()
    {
        /// Initial visibility check
        foreach (var ft in trackedObjects)
        {
            var ftwr = ft as FrustumTrackedObjectWithRenderer;
            
        }
    }

    public override void AddTrackedObject(Transform objTransform)
    {
        var ft = objTransform.GetOrAddComponent<FrustumTrackedObjectWithRenderer>();
        if (trackedObjects.Contains(ft)) return;
        trackedObjects.Add(ft);
    }

    protected override void CreateInitialTrackedObjects()
    {
        trackedObjects = new List<FrustumTrackedObject>();
        foreach (var t in trackedObjectsOnStart) AddTrackedObject(t);
    }
}
