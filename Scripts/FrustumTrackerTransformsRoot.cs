using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(OutOfFrustum))]
public class FrustumTrackerTransformsRoot : FrustrumTrackerBase
{
    private OutOfFrustum outOfFrustum;

    private void Start()
    {
        outOfFrustum = GetComponent<OutOfFrustum>();
        TestAll();
        RefreshAll();
    }

    public override FrustumTrackedObject AddTrackedObject(Transform obj, TrackedObjectData data = null)
    {
        var ft = base.AddTrackedObject<FrustumTrackedTransformRoot>(obj, data);
        TestTrackedObject(ft);
        ft.RefreshState();
        return ft;
    }

    // Update is called once per frame
    void Update()
    {
        TestAll();
    }

    void TestAll()
    {
        /// Test visibility for each of tracked objetcs using OutOfFrustum utility
        foreach (var ft in trackedObjects)
        {
            TestTrackedObject(ft as FrustumTrackedTransformRoot);
        }
    }

    void RefreshAll()
    {
        foreach (var ft in trackedObjects) ft.RefreshState();
    }

    protected override void CreateInitialTrackedObjects()
    {
        trackedObjects = new List<FrustumTrackedObject>();
        foreach (var t in trackedObjectsOnStart)
        {
            AddTrackedObject<FrustumTrackedTransformRoot>(t);
        }
    }

    private void TestTrackedObject(FrustumTrackedTransformRoot obj)
    {
        // TODO: find a wayt to hide this public method for a tracked object
        obj.SetVisibility(outOfFrustum.TestVisisbility(obj.Bounds));
    }
}
