using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    public GameObject pegasus;
    public bool isPlayerFree = true;
    public Vector2 lookInput;
    CharacterController controller;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f);
        foreach (var hitCollider in hitColliders)
        {
            if(hitCollider.gameObject.tag == "Pegasus")
            {
                SwitchToPegasus();
            }
        }
    }

    public void Attack()
    {
        isPlayerFree = false;
    }

    public void AttackEnd()
    {
        isPlayerFree = true;
    }

    public void Dismount()
    {
        if(pegasus.GetComponent<PegasusController>().isControlled)
            SwitchToPlayer();
    }

    void SwitchToPegasus()
    {
        isPlayerFree = false;
        controller.enabled = false;
        pegasus.GetComponent<PegasusController>().isControlled = true;
        animator.SetLayerWeight(0, 0);
        animator.SetLayerWeight(1, 1);
        transform.parent = pegasus.transform;
        transform.localPosition = new Vector3(0, 1.2f, 0);
        transform.localRotation = Quaternion.identity;
    }

    void SwitchToPlayer()
    {
        isPlayerFree = true;
        controller.enabled = true;
        pegasus.GetComponent<PegasusController>().isControlled = false;
        animator.SetLayerWeight(0, 1);
        animator.SetLayerWeight(1, 0);
        transform.parent = null;
        pegasus.GetComponent<PegasusController>().Dismount();
    }


}
