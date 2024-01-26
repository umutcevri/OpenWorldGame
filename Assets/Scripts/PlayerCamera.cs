using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    InputManager inputManager;
    public Transform objectToFollow;
    Transform cameraPivot;
    float maxLookAngle = 60f;
    float minLookAngle = -30f;
    float cameraXAngle;
    float cameraYAngle;
    public float cameraXSpeed = 220f;
    public float cameraYSpeed = 220f;
    public Vector3 cameraVelocity;
    public float cameraSmoothSpeed = 1f;
    void Start()
    {
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        cameraPivot = transform.GetChild(0).gameObject.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        CameraActions();
    }

    void CameraActions()
    {
        FollowPlayer();
        RotateCamera();
    }

    void FollowPlayer()
    {
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, objectToFollow.position, ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
        transform.position = targetPosition;
    }

    void RotateCamera()
    {
        cameraXAngle += inputManager.lookInput.x * cameraXSpeed * Time.deltaTime;
        cameraYAngle -= inputManager.lookInput.y * cameraYSpeed * Time.deltaTime;

        cameraYAngle = Mathf.Clamp(cameraYAngle, minLookAngle, maxLookAngle);

        transform.rotation = Quaternion.Euler(0, cameraXAngle, 0);
        cameraPivot.localRotation = Quaternion.Euler(cameraYAngle, 0, 0);
    }
}
