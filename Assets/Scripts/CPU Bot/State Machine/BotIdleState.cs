using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotIdleState : State
{
    #region  REFERENCES
    BotAttackManager attackManager;
    BotManager botManager;
    #endregion

    [SerializeField] State attackState;
    [SerializeField] State searchBoxState;

    [Header("Idle Duration")]
    [SerializeField] float minIdleDuration = 1;
    [SerializeField] float maxIdleDuration = 1.75f;

    public bool enemyInRange = false;
    float timer;



    void Awake()
    {
        attackManager = transform.parent.gameObject.GetComponentInParent<BotAttackManager>();
        botManager = GetComponentInParent<BotManager>();
        enemyInRange = false;
        timer = 0;
    }

    public override State RunCurrentState()
    {
        botManager.hasBox = false;
        float scelta = Random.Range(minIdleDuration, maxIdleDuration);
        if(timer < scelta)
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
