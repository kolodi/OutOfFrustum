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
        Vector3 leftToCam = camPos - corners[0] + leftToUp / 2f;
        Vector3 upToCam = camPos - corners[1] + leftToRight / 2f;
        Vector3 rightToCam = camPos - corners[2] + DownToUp / 2f;
        Vector3 bottomToCam = camPos - corners[3] + LeftToRight / 2f;

        // calculate normals for frustum planes https://docs.unity3d.com/ScriptReference/Vector3.Cross.html
        Vector3 normalLeft = Vector3.Cross(leftToCam, leftToUp);
        Vector3 normalUp = Vector3.Cross(upToCam, leftToRight);
        Vector3 normalRight = Vector3.Cross(rightToCam, DownToUp);
        Vector3 normalBottom = Vector3.Cross(bottomToCam, LeftToRight);

        // creating 6 custom frustum planes
        Plane planeLeft = new Plane(normalLeft, camPos);
        Plane planeRight = new Plane(normalUp, camPos);
        Plane planeUp = new Plane(normalRight, camPos);
        Plane planeBottom = new Plane(normalBottom, camPos);
        Plane planeFar = new Plane(-camForward, camPos + camForward * 100);
        Plane planeNear = new Plane(camForward, camPos + camForward * _camera.nearClipPlane);

        // debug normals
        //Debug.DrawRay(corners[0] + leftToUp / 2f, normalLeft);
        //Debug.DrawRay(corners[1] + leftToRight / 2f, normalUp);
        //Debug.DrawRay(corners[2] + DownToUp / 2f, normalRight);
        //Debug.DrawRay(corners[3] + LeftToRight / 2f, normalBottom);
        //Debug.DrawRay(camPos + camForward * 100, -camForward);
        //Debug.DrawRay(camPos, camForward);


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
