using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PegasusController pegasusController;
    PlayerManager playerManager;

    CombatSystem combatSystem;
    public Vector2 movementInput;
    public Vector2 lookInput;
    GameInput inputActions;
    void Start()
    {
        GameObject player = GameObject.Find("Player");
        playerManager = player.GetComponent<PlayerManager>();
        combatSystem = player.GetComponent<CombatSystem>();

        pegasusController = playerManager.pegasus.GetComponent<PegasusController>();
        
        inputActions = new GameInput();
        inputActions.Enable();
        inputActions.Player.Interact.performed += ctx => playerManager.Interact();

        inputActions.Pegasus.Canter.performed += ctx => pegasusController.Canter();
        inputActions.Pegasus.Gallop.performed += ctx => pegasusController.Gallop();
        inputActions.Pegasus.Halt.performed += ctx => pegasusController.Halt();

        inputActions.Player.Dismount.performed += ctx => playerManager.Dismount();

        inputActions.Player.MainHandAttack.performed += ctx => combatSystem.MainHandAttack();
    }

    // Update is called once per frame
    void Update()
    {
        movementInput = inputActions.Player.Move.ReadValue<Vector2>();
        lookInput = inputActions.Player.Look.ReadValue<Vector2>();
    }
}
