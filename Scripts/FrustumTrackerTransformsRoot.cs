using UnityEngine;

[RequireComponent(typeof(OutOfFrustum))]
public class FrustumTrackerTransformsRoot : FrustrumTrackerBase
{
    [SerializeField] OutOfFrustum outOfFrustum;

    private void Start()
    {
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
            TestTrackedObject(ft);
        }
    }

    void RefreshAll()
    {
        foreach (var ft in trackedObjects) ft.RefreshState();
    }

    protected override void CreateInitialTrackedObjects()
    {
        foreach (var t in trackedObjectsOnStart)
        {
            AddTrackedObject(t);
        }
    }

    private void TestTrackedObject(FrustumTrackedObject obj)
    {
        // TODO: find a wayt to hide this public method for a tracked object
        obj.SetVisibility(outOfFrustum.TestVisisbility(obj.Bounds));
    }
}
