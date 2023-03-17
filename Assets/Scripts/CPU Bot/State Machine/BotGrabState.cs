using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotGrabState : State
{

    BotGrabAndLaunchHandler grabAndLaunchHandler;
    BotManager botManager;
    BotMovementManager botMovementManager;

    [SerializeField] State searchBoxState;
    [SerializeField] State searchEnemyState;

    void Awake()
    {
        botManager = GetComponentInParent<BotManager>();
        botMovementManager = GetComponentInParent<BotMovementManager>();
        grabAndLaunchHandler = GetComponentInParent<BotGrabAndLaunchHandler>();
    }

    public override State RunCurrentState()
    {
        if(botMovementManager.targetBox.isPicked)
        {
            botMovementManager.targetBox = null;
            Debug.Log("GRAB STATE -> SEARCH STATE");
            return searchBoxState;
        }

        grabAndLaunchHandler.GetBox();
        Debug.Log("GRAB STATE -> SEARCH ENEMY STATE");
        return searchEnemyState;
    }
}
