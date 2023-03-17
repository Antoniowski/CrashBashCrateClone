using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotThrowState : State
{

    BotGrabAndLaunchHandler botGrabAndLaunchHandler;
    BotMovementManager botMovementManager;
    BotManager botManager;
    

    [SerializeField] State idleState;


    void Awake()
    {
        botGrabAndLaunchHandler = GetComponentInParent<BotGrabAndLaunchHandler>();
        botMovementManager = GetComponentInParent<BotMovementManager>();
        botManager = GetComponentInParent<BotManager>();
    }

    public override State RunCurrentState()
    {
        botManager.hasBox = false;
        botGrabAndLaunchHandler.HandleLaunch();
        botMovementManager.targetCharacter = null;
        Debug.Log("THROW STATE -> IDLE");
        return idleState;
    }
}
