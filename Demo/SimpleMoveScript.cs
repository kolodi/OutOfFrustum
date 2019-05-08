using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMoveScript : MonoBehaviour
{
    public Camera _camera;
    public Transform cameraMountingPoint;
    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;

    public Transform head;
    public Transform body;
    public float headRotationSpped = 5f;

    private Vector3 moveDirection = Vector3.zero;

    private void Start()
    {
        if (_camera == null)
        {
            Debug.LogError("Please reference the camera from CharacterMove component", this);
            return;
        }
        _camera.transform.SetParent(cameraMountingPoint, false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        CharacterController controller = GetComponent<CharacterController>();
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;

        }
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);

        body.Rotate(Vector3.up, Input.GetAxis("Mouse X") * Time.deltaTime * headRotationSpped);
        head.Rotate(Vector3.right, -Input.GetAxis("Mouse Y") * Time.deltaTime * headRotationSpped);



    }
}
