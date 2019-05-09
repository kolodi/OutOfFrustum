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


        // calculate important directions
        Vector3 leftToUp = corners[1] - corners[0];
        Vector3 leftToRight = corners[2] - corners[1];
        Vector3 DownToUp = corners[3] - corners[2];
        Vector3 LeftToRight = corners[0] - corners[3];

        // calculate rectangle mid points
        Vector3 left = corners[0] + leftToUp / 2f;
        Vector3 up = corners[1] + leftToRight / 2f;
        Vector3 right = corners[2] + DownToUp / 2f;
        Vector3 down = corners[3] + LeftToRight / 2f;

        // calculate normals for frustum planes https://docs.unity3d.com/ScriptReference/Vector3.Cross.html
        Vector3 normalLeft = Vector3.Cross(camPos - left, leftToUp);
        Vector3 normalUp = Vector3.Cross(camPos - up, leftToRight);
        Vector3 normalRight = Vector3.Cross(camPos - right, DownToUp);
        Vector3 normalBottom = Vector3.Cross(camPos - down, LeftToRight);

        // creating 6 custom frustum planes
        Plane planeLeft = new Plane(normalLeft, camPos);
        Plane planeRight = new Plane(normalUp, camPos);
        Plane planeUp = new Plane(normalRight, camPos);
        Plane planeBottom = new Plane(normalBottom, camPos);
        Plane planeFar = new Plane(-camForward, camPos + camForward * farPlaneDistance);
        Plane planeNear = new Plane(camForward, camPos + camForward * _camera.nearClipPlane);

        //debug
        {
            // debug normals
            Debug.DrawRay(corners[0] + leftToUp / 2f, normalLeft, Color.blue);
            Debug.DrawRay(corners[1] + leftToRight / 2f, normalUp, Color.blue);
            Debug.DrawRay(corners[2] + DownToUp / 2f, normalRight, Color.blue);
            Debug.DrawRay(corners[3] + LeftToRight / 2f, normalBottom, Color.blue);
            Debug.DrawRay(camPos + camForward * 100, -camForward, Color.blue);
            Debug.DrawRay(camPos, camForward, Color.blue);

            // debug corners
            foreach (var corner in corners)
            {
                Debug.DrawLine(camPos, camPos + (corner - camPos).normalized * farPlaneDistance, Color.gray);
            }

            //debug mid points
            Debug.DrawLine(camPos, left, Color.cyan);
            Debug.DrawLine(camPos, up, Color.cyan);
            Debug.DrawLine(camPos, right, Color.cyan);
            Debug.DrawLine(camPos, down, Color.cyan);
        }


        return new Plane[6]
        {
            planeLeft, planeUp, planeRight, planeBottom, planeFar, planeNear
        };
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
