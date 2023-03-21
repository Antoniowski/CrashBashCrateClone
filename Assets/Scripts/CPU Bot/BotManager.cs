using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotManager : GenericCharacterManager
{

    BotMovementManager botMovementManager;
    Animator animator;
    AnimationHandler animationHandler;
    BotStateManager botStateManager;

    [SerializeField] State stunState;
    [SerializeField] State deathState;

    public float grabRadius;
    public int boxDetectionRadius;


    public bool isPerformingAction;
    public bool isMoving;
    

    void Awake()
    {
        botMovementManager = GetComponent<BotMovementManager>();  
        animator = GetComponentInChildren<Animator>();  
        animationHandler = GetComponent<AnimationHandler>();
        botStateManager = GetComponentInChildren<BotStateManager>();
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
    }


    public void TakeDamage(int value)
    {
        if(isStunned)
            return;

        if(hasBox)
        {
            hasBox = false;
            Box box = transform.Find("GrabPoint").gameObject.GetComponentInChildren<Box>();
            animator.CrossFade("Grab Empty",0.2f);
            box.Explode(box.explosionRadius);
        }

        animationHandler.PlayAnimationTarget("GetHit", true);
        botMovementManager.ResetVelocity();
        isStunned = true;
        health -= value;
        if(health <= 0)
        {
            health = 0;
            animationHandler.PlayAnimationTarget("Death", true);
            botStateManager.SwitchToNextState(deathState);
        }
    }

    public void StartStunnedAnimation()
    {
        animationHandler.PlayAnimationTarget("Stunned", true);
        botStateManager.SwitchToNextState(stunState);
        StartCoroutine(EndStunnedAnimation());
    }

    private IEnumerator EndStunnedAnimation()
    {
        yield return new WaitForSeconds(1.5f);
        animationHandler.PlayAnimationTarget("Empty", false);
        isStunned = false;
    }
}
