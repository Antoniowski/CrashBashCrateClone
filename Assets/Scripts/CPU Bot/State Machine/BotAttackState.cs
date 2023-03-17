using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotAttackState : State
{
    [SerializeField] State idleState;

    BotAttackManager attackManager;

    void Awake()
    {
        attackManager = GetComponentInParent<BotAttackManager>();
    }

    public override State RunCurrentState()
    {
        attackManager.HandleAttack();
        Debug.Log("ATTACK STATE -> IDLE");
        return idleState;
    }
}
