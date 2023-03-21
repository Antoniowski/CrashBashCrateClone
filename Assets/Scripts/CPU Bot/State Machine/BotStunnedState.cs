using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotStunnedState : State
{

    BotManager botManager;
    BotMovementManager movementManager;

    [SerializeField] State idleState;
    
    public bool stunFinished = false;

    void Awake()
    {
        botManager = GetComponentInParent<BotManager>();
        movementManager = GetComponentInParent<BotMovementManager>();
    }
    public override State RunCurrentState()
    {   
        botManager.isMoving = false;
        movementManager.targetBox = null;
        movementManager.targetCharacter = null;
        
        stunFinished = !botManager.isStunned;

        if(stunFinished)
            return idleState;

        return this;
    }
}
