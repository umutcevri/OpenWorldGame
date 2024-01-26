using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    int comboCounter = 0;

    bool canCombo = false;
    PlayerManager playerManager;
    Animator animator;
    void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MainHandAttack()
    {
        if(comboCounter == 0)
        {
            playerManager.Attack();
            comboCounter = 1;
            canCombo = false;
            animator.SetTrigger("FirstAttack");
        }
        else if(canCombo && comboCounter == 1)
        {
            canCombo = false;
            animator.SetTrigger("SecondAttack");
        }
    }

    public void AttackFinished()
    {
        comboCounter = 0;
        canCombo = false;
        playerManager.AttackEnd();
    }

    public void AllowCombo()
    {
        canCombo = true;
    }


}
