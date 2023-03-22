using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : GenericCharacterManager
{

    PlayerInputHandler playerInputHandler;
    Animator animator;
    AnimationHandler animationHandler;
    PlayerMovementHandler playerMovementHandler;

    void Awake()
    {
        playerInputHandler = GetComponent<PlayerInputHandler>();
        animationHandler = GetComponent<AnimationHandler>();
        animator = GetComponentInChildren<Animator>();
        playerMovementHandler = GetComponent<PlayerMovementHandler>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float delta = Time.deltaTime;
        playerInputHandler.TickInput(delta); 
        playerInputHandler.isInteracting = animator.GetBool("IsInteracting");
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
        playerMovementHandler.ResetVelocity();
        isStunned = true;
        health -= value;
        if(health <= 0)
        {
            health = 0;
            animationHandler.PlayAnimationTarget("Death", true);
            //Handle Death
        }
    }

    public void SetCurrentHealth(int value)
    {
        if(value > maxHealth)
        {
            health = maxHealth;
            return;
        }
        
        health = value;
    }

    public void StartStunnedAnimation()
    {
        animationHandler.PlayAnimationTarget("Stunned", true);
        StartCoroutine(EndStunnedAnimation());
    }

    private IEnumerator EndStunnedAnimation()
    {
        yield return new WaitForSeconds(1.5f);
        animationHandler.PlayAnimationTargetNO_INTERACTING("Empty");
        isStunned = false;
    }



    void LateUpdate()
    {
        ResetButtonPress();
    }

    void ResetButtonPress()
    {
        playerInputHandler.jumpButton = false;
        playerInputHandler.attackButton = false;
        playerInputHandler.grabAndLaunchButton = false;
    }
}
