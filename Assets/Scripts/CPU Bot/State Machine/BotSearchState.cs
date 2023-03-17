using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSearchState : State
{
    [SerializeField] State idleState;
    [SerializeField] State reachBoxState;

    BotMovementManager botMovementManager;

    public bool boxFound;


    void Awake()
    {
        botMovementManager = transform.parent.gameObject.GetComponentInParent<BotMovementManager>();
    }


    public override State RunCurrentState()
    {
        botMovementManager.HandleBoxDetection();
        if(botMovementManager.targetBox != null)
            boxFound = true;
        
        if(boxFound)
        {
            boxFound = false;
            Debug.Log("SEARCH BOX -> REACH BOX STATE");
            return reachBoxState;
        }
        
        Debug.Log("SEARCH BOX -> IDLE");
        return idleState;
    }
}
