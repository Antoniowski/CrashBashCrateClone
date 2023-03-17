using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotManager : GenericCharacterManager
{

    BotMovementManager botMovementManager;
    Animator animator;
    AnimationHandler animationHandler;

    public float grabRadius;
    public int boxDetectionRadius;


    public bool isPerformingAction;
    public bool isMoving;
    

    void Awake()
    {
        botMovementManager = GetComponent<BotMovementManager>();  
        animator = GetComponentInChildren<Animator>();  
        animationHandler = GetComponent<AnimationHandler>();
        animationHandler.Init();
    }

    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        float delta = Time.deltaTime;
        isPerformingAction = animator.GetBool("IsInteracting");
        //HandleCurrentAction(delta);
    }


    public void TakeDamage(int value)
    {
        if(isStunned)
            return;
            
        animationHandler.PlayAnimationTarget("GetHit", true);
        botMovementManager.ResetVelocity();
        isStunned = true;
        health -= value;
        if(health <= 0)
        {
            health = 0;
            animationHandler.PlayAnimationTarget("Death", true);
            //Handle Death
        }
    }

    public void StartStunnedAnimation()
    {
        animationHandler.PlayAnimationTarget("Stunned", true);
        StartCoroutine(EndStunnedAnimation());
    }

    private IEnumerator EndStunnedAnimation()
    {
        yield return new WaitForSeconds(1.5f);
        animationHandler.PlayAnimationTarget("Empty", false);
        isStunned = false;
    }
}
