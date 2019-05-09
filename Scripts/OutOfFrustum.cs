using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Utility to test bounds if they enter the viewing frustum
/// </summary>
public class OutOfFrustum : MonoBehaviour
{
    [SerializeField] Camera _camera = null;
    [Tooltip("Custom limiting rectangle used for calculating the custom viewing frustum, leave empty if you want to use the camera frustum")]
    [SerializeField] RectTransform limitingRect;

    [SerializeField] float farPlaneDistance = 100f;

    private Plane[] planes;

    private void Awake()
    {
        planes = CreateFrustumPlanes();
    }

    private void Update()
    {
        planes = CreateFrustumPlanes();
    }

    /// <summary>
    /// Create 6 planes representing the viewing frustum
    /// </summary>
    /// <returns>Array with 6 planes</returns>
    private Plane[] CreateFrustumPlanes()
    {
        if (limitingRect == null)
        {
            // Calculate the planes from the main camera's view frustum
            return GeometryUtility.CalculateFrustumPlanes(_camera);
        }


        /// proceed with custom frustum calculation,
        /// the limiting rect is meant to be a canvas rect placed in fron of camera
        /// defining limiting view area for devices like hololens


        // get rect world corners positions
        Vector3[] corners = new Vector3[4];
        limitingRect.GetWorldCorners(corners);


        Vector3 camPos = _camera.transform.position;
        Vector3 camForward = _camera.transform.forward;

        /// Sets a plane using three points that lie within it. 
        /// The points go around clockwise as you look down on the top surface of the plane.
        var planes = new Plane[]
        {
            new Plane(camPos, corners[1], corners[0]), // left
            new Plane(camPos, corners[2], corners[1]), // up
            new Plane(camPos, corners[3], corners[2]), // right
            new Plane(camPos, corners[0], corners[3]), // down
            new Plane(-camForward, camPos + camForward * farPlaneDistance), // far
            new Plane(camForward, camPos + camForward * _camera.nearClipPlane), // near
        };

        //debug
        {
            //var colors = new Color[] { Color.blue, Color.red, Color.yellow, Color.green, Color.gray, Color.cyan };
            //for (int i = 0; i < planes.Length; i++)
            //{
            //    Debug.DrawRay(camPos, planes[i].normal, colors[i]);
            //}
        }

        return planes;
    }

    /// <summary>
    /// Test if given biubds are visible inside the camera frustum or custom viewing frustum
    /// </summary>
    /// <param name="bounds">Bounds of the tracked object</param>
    /// <returns>true if even a part of bounds is visible inside the viewing frustum</returns>
    public bool TestVisisbility(Bounds bounds)
    {
        return GeometryUtility.TestPlanesAABB(planes, bounds);
    }
}
