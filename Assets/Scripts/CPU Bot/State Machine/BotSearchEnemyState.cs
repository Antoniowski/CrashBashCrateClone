using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSearchEnemyState : State
{

    BotAttackManager attackManager;
    BotMovementManager botMovementManager;
    BotManager botManager;

    [SerializeField] State throwState;

    public bool enemyInRange = false;

    void Awake()
    {
        attackManager = GetComponentInParent<BotAttackManager>();
        botMovementManager = GetComponentInParent<BotMovementManager>();
        botManager = GetComponentInParent<BotManager>();
    }

    public override State RunCurrentState()
    {
        attackManager.HandlePlayerDetection();

        if(enemyInRange)
        {
            enemyInRange = false;
            botManager.isMoving = false;
            Debug.Log("SEARCH ENEMY STATE -> THROW STATE");
            return throwState;
        }
        
        //Cerca il player
        if(botMovementManager.targetCharacter != null)
        {
            if(!botManager.isMoving)
                botManager.isMoving = true;
        }

        return this;
    }
}
