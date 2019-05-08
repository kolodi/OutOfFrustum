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

    /// <summary>
    /// Add new object for being tracked
    /// </summary>
    /// <param name="obj">Object's transform</param>
    public abstract void AddTrackedObject(Transform obj);

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
