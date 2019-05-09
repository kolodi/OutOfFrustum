using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Visual markers manager,
/// responsible for adding and removing visual markers
/// and to update their positions on edges of viewing area
/// </summary>
public class FrustumMarkers : MonoBehaviour
{
    [Tooltip("Prefab for the markers")]
    [SerializeField] FrustumMarker frustumMarker;
    [Tooltip("View limiting rectangle")]
    [SerializeField] RectTransform limitingRect;

    /// <summary>
    /// Visual markers dictionary
    /// </summary>
    private Dictionary<FrustumTrackedObject, FrustumMarker> markers;

    private void Awake()
    {
        markers = new Dictionary<FrustumTrackedObject, FrustumMarker>();
    }

    private void OnEnable()
    {
        FrustumTrackedObject.OnVisibilityChanged += OnObjectVisibilityChanged;
    }

    private void OnDisable()
    {
        FrustumTrackedObject.OnVisibilityChanged -= OnObjectVisibilityChanged;
    }

    /// <summary>
    /// Handle for tracked objects events
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="isVisible"></param>
    void OnObjectVisibilityChanged(FrustumTrackedObject obj, bool isVisible)
    {
        //Debug.Log(obj.name + " is now " + (isVisible ? "visible" : "unvisible"));
        if(isVisible)
        {
            RemoveMarker(obj);
        } else
        {
            ShowMarker(obj);
        }

    }

    private FrustumMarker CreateMarker(FrustumTrackedObject obj)
    {
        var marker = Instantiate(frustumMarker, transform, false);
        markers.Add(obj, marker);
        return marker;
    }

    private void ShowMarker(FrustumTrackedObject obj)
    {
        FrustumMarker marker;
        if (markers.TryGetValue(obj, out marker) == false)
        {
            marker = CreateMarker(obj);
        }
        marker.gameObject.SetActive(true);
    }

    private void RemoveMarker(FrustumTrackedObject obj)
    {
        FrustumMarker marker = null;
        if (markers.TryGetValue(obj, out marker))
        {
            Destroy(marker.gameObject);
            markers.Remove(obj);
        }
    }

    /// <summary>
    /// Calculate 4 perpendicular planes to the rectangle edges
    /// </summary>
    /// <returns>Array with 4 planes</returns>
    private Plane[] GetRectPlanes()
    {
        Vector3[] corners = new Vector3[4];
        limitingRect.GetWorldCorners(corners);
        Vector3 h = corners[2] - corners[1];
        Vector3 v = corners[1] - corners[0];

        Plane[] planes = new Plane[] {
            new Plane(h, corners[0]), // left
            new Plane(-h, corners[3]), // right
            new Plane(-v, corners[1]), // up
            new Plane(v, corners[0]) // down
        };

        return planes;
    }

    /// <summary>
    /// raycast from the view center to the tracked object
    /// and get the hit point on one of the 4 limiting planes
    /// </summary>
    /// <param name="planes"></param>
    /// <param name="trackedObjectCenter"></param>
    /// <returns>World position of the hit</returns>
    private Vector3 GetNearestHit(Plane[] planes, Vector3 trackedObjectCenter)
    {
        Vector3 direction3D = trackedObjectCenter - transform.position;
        Ray ray = new Ray(transform.position, direction3D); // assuming rect pivot is in center of the rectangle
        Vector3 nearestHit = Vector3.zero;
        float nearestDistanceHit = float.PositiveInfinity;
        foreach (var p in planes)
        {
            float hitDistance;
            if (p.Raycast(ray, out hitDistance))
            {
                if (hitDistance < nearestDistanceHit)
                {
                    nearestHit = ray.origin + ray.direction * hitDistance;
                    nearestDistanceHit = hitDistance;
                }
            }
        }
        return nearestHit;
    }

    /// <summary>
    /// Project the hit point on limiting plane to the
    /// rectangle plane in order to get the edge point
    /// </summary>
    /// <param name="planeHitPos"></param>
    /// <returns>World position of the edge point</returns>
    private Vector3 ProjectPlaneHitOnLocalRect(Vector3 planeHitPos)
    {
        Vector3 nearestInLocal = transform.InverseTransformPoint(planeHitPos);
        nearestInLocal.z = 0;
        return transform.TransformPoint(nearestInLocal);
    }

    private void Update()
    {
        Plane[] planes = GetRectPlanes();

        /// update markers positions
        foreach (var m in markers)
        {
            var tracked = m.Key;
            var marker = m.Value;
            Vector3 nearestHit = GetNearestHit(planes, tracked.Bounds.center);
            Vector3 edgePoint = ProjectPlaneHitOnLocalRect(nearestHit);
            marker.transform.position = edgePoint;

            Vector3 lookAtPoint = transform.position + (edgePoint - transform.position) * 2f;

            marker.transform.LookAt(lookAtPoint);

            //Debug
            {
                //Debug.DrawLine(transform.position, nearestHit, Color.red);
                //Debug.DrawLine(nearestHit, tracked.Bounds.center, Color.black);
                //Debug.DrawLine(edgePoint, tracked.Bounds.center, Color.green);
            }
        }

    }

}
