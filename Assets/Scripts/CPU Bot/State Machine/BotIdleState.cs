using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotIdleState : State
{
    [SerializeField] State attackState;
    [SerializeField] State searchBoxState;
    public bool enemyInRange = false;
    float timer;
    BotAttackManager attackManager;


    void Awake()
    {
        attackManager = transform.parent.gameObject.GetComponentInParent<BotAttackManager>();
    }

    public override State RunCurrentState()
    {
        if(timer < 2f)
        {
            timer += Time.deltaTime;
            return this;
        }

        timer = 0;
        Debug.Log("Cerco Player");
        attackManager.HandlePlayerDetection();

        if(enemyInRange)
        {
            Debug.Log("IDLE -> ATTACK STATE");
            enemyInRange = false;
            return attackState;  
        }
        
        Debug.Log("IDLE -> SEARCH BOX STATE");
        return searchBoxState;
    }
}
