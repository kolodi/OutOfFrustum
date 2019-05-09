using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Base class for managing tracking objetcs
/// </summary>
public abstract class FrustrumTrackerBase : MonoBehaviour
{

    [SerializeField] protected Transform[] trackedObjectsOnStart;

    protected List<FrustumTrackedObject> trackedObjects;

    protected abstract void CreateInitialTrackedObjects();

    private void Awake()
    {
        CreateInitialTrackedObjects();
    }

    public abstract FrustumTrackedObject AddTrackedObject(Transform obj, TrackedObjectData data = null);

    protected virtual T AddTrackedObject<T>(Transform obj) where T : FrustumTrackedObject
    {
        var ft = obj.gameObject.AddComponent<T>();
        trackedObjects.Add(ft);
        return ft;
    }

    protected virtual T AddTrackedObject<T>(Transform obj, TrackedObjectData data = null) where T : FrustumTrackedObject
    {
        var ft = AddTrackedObject<T>(obj);
        ft.data = data;
        return ft;
    }

    /// <summary>
    /// Remove an object for being tracked
    /// </summary>
    /// <param name="objTransform">Object's transform</param>
    public virtual void RemoveTrackedObject(Transform objTransform)
    {
        for (int i = 0; i < trackedObjects.Count; i++)
        {
            if (trackedObjects[i].gameObject == objTransform.gameObject)
            {
                Destroy(trackedObjects[i]); // destroy component
                trackedObjects.RemoveAt(i);
                break;
            }
        }
    }
}
