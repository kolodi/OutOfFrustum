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


    public override void AddTrackedObject(Transform objTransform)
    {
        var ft = objTransform.GetOrAddComponent<FrustumTrackedTransformRoot>();
        if (trackedObjects.Contains(ft)) return;
        trackedObjects.Add(ft);
        // this will trigger to display marker somewhere if the tracked object is out of frustum initially
        TestTrackedObject(ft);
        ft.RefreshState();
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
            AddTrackedObject(t);
        }
    }

    private void TestTrackedObject(FrustumTrackedTransformRoot obj)
    {
        // TODO: find a wayt to hide this public method for a tracked object
        obj.SetVisibility(outOfFrustum.TestVisisbility(obj.Bounds));
    }
}
