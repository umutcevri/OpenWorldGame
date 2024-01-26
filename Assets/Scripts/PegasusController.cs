using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PegasusController : MonoBehaviour
{
    
    bool lockRotation = false;
    public float canterSpeed = 4f;
    public float gallopSpeed = 8f;
    public bool isControlled = false;
    bool changingSpeed = false;
    Animator animator;
    public float currentSpeed = 0f;
    Transform playerCamera;
    CharacterController characterController;
    public float rotationSpeed = 1f;

    public float groundRotationSpeed = 5f;

    int groundLayerMask; 
    void Start()
    {
        playerCamera = GameObject.Find("PlayerCamera").transform;
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        animator.SetFloat("GroundSpeed", currentSpeed);
        groundLayerMask = 1 << LayerMask.NameToLayer("Ground");
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("GroundSpeed", currentSpeed);
        if(isControlled)
        {
            Move();
            if(!lockRotation)
                Rotate();
        }
    }

    public void Gallop()
    {
        if(isControlled && !changingSpeed)
        {
            if(currentSpeed == canterSpeed)
            {
                StartCoroutine(ChangeSpeed(gallopSpeed));
            }
        }
    }

    public void Halt()
    {
        if(isControlled && !changingSpeed)
        {
            if(currentSpeed == gallopSpeed)
            {
                StartCoroutine(ChangeSpeed(canterSpeed));
            }
            else if(currentSpeed == canterSpeed)
            {
                lockRotation = true;
                StartCoroutine(ChangeSpeed(0f));
            }
        }
    }

    public void Canter()
    {
        if(isControlled && !changingSpeed)
        {
            if(currentSpeed == 0f)
            {
                lockRotation = true;
                StartCoroutine(ChangeSpeed(canterSpeed));
            }
        }
    }

    public void Dismount()
    {
        Debug.Log("Dismounting");
        StopAllCoroutines();
        StartCoroutine(ChangeSpeed(0f, 3f));
    }

    IEnumerator ChangeSpeed(float targetSpeed, float changeTime = 1f)
    {
        changingSpeed = true;
        float elapsedTime = 0f;
        float startSpeed = currentSpeed;
        float speedDifference = targetSpeed - startSpeed;

        while (elapsedTime < changeTime)
        {
            float t = elapsedTime / changeTime;
            currentSpeed = startSpeed + t * speedDifference;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        currentSpeed = targetSpeed;
        lockRotation = false;
        changingSpeed = false;

        if (!isControlled)
        {
            Move();
        }
    }  

    void Move()
    {
        Vector3 moveDirection = transform.forward * currentSpeed + new Vector3(0, -9.81f, 0);
        
        characterController.Move(moveDirection * Time.deltaTime);
    }

    void Rotate()
    {
        if(currentSpeed == 0)
        {
            return;
        }

        Vector3 lookDirection = playerCamera.forward * currentSpeed + playerCamera.right;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), rotationSpeed * Time.deltaTime);

        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;

        
        if(Physics.Raycast(ray, out hit, 2f, groundLayerMask))
        {
            //transform.rotation = Quaternion.Slerp(transform.rotation, (Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation), groundRotationSpeed * Time.deltaTime);

            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

            float targetXRotation = targetRotation.eulerAngles.x;

            Quaternion newXRotation = Quaternion.Euler(targetXRotation, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

            transform.rotation = Quaternion.Slerp(transform.rotation, newXRotation, groundRotationSpeed * Time.deltaTime);
        }
    }
}
