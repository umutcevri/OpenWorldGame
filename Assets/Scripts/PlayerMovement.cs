using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    float gravity = -9.81f;
    bool isRunning = false;
    Animator animator;
    Transform playerCamera;
    public float rotationSpeed = 1f;
    public float moveSpeed = 5f;
    CharacterController controller;
    PlayerManager playerManager;

    InputManager inputManager;
    void Start()
    {
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        controller = GetComponent<CharacterController>();
        playerCamera = GameObject.Find("PlayerCamera").transform;
        animator = GetComponent<Animator>();
        playerManager = GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerManager.isPlayerFree)
        {
            animator.SetBool("isRunning", isRunning);
            Move(inputManager.movementInput);
            Rotate(inputManager.movementInput);
        }
    }

    void Move(Vector2 direction)
    {   
        if(direction == Vector2.zero)
            isRunning = false;
        else
            isRunning = true;
        
        Vector3 moveDirection = playerCamera.forward * direction.y + playerCamera.right * direction.x + Vector3.up * gravity;
        controller.Move(moveDirection * Time.deltaTime * moveSpeed);
    }

    void Rotate(Vector2 direction)
    {
        if(direction == Vector2.zero)
        {
            return;
        }

        Vector3 lookDirection = playerCamera.forward * direction.y + playerCamera.right * direction.x;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), rotationSpeed * Time.deltaTime);
    }
}
