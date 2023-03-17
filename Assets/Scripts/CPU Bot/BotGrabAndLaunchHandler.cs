using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotGrabAndLaunchHandler : MonoBehaviour
{

    //REFERENZE
    BotManager botManager;
    BotMovementManager botMovementManager;
    Animator animator;
    AnimationHandler animationHandler;

    [SerializeField] GameObject grabPoint;
    
    [Header("Throw Stats")]
    [SerializeField] float ThrowPower = 10;
    public GameObject pickedBox;


    void Awake()
    {
        botManager = GetComponent<BotManager>();
        botMovementManager = GetComponent<BotMovementManager>();
        animator = GetComponentInChildren<Animator>();
        animationHandler = GetComponent<AnimationHandler>();
    }

    public void GetBox()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, botManager.grabRadius, botMovementManager.boxDetectionLayer);
        if(colliders.Length > 0)
        {
            //PERICOLOSO
            GrabBox(colliders[0].gameObject);
            //botMovementManager.ResetVelocity();
            botMovementManager.targetBox = null;
            return;
        }
    }

    public void GrabBox(GameObject boxToPick)
    {
            pickedBox = boxToPick;
            Rigidbody rigidbody = boxToPick.GetComponent<Rigidbody>();
            Box box = boxToPick.GetComponent<Box>();
            switch(box.type)
            {
                case Box.BoxType.Normal:
                    botManager.hasBox = true;
                    box.isPicked = true;
                    animator.CrossFade("CastingLoop", 0.2f);
                    box.GetComponentInParent<BoxCollider>().enabled = false;
                    Destroy(rigidbody);
                    boxToPick.transform.position = grabPoint.transform.position;
                    boxToPick.transform.parent = grabPoint.transform;
                    break;
                
                case Box.BoxType.TNT:
                    botManager.hasBox = true;
                    box.isPicked = true;
                    animator.CrossFade("CastingLoop", 0.2f);
                    box.GetComponentInParent<BoxCollider>().enabled = false;
                    Destroy(rigidbody);
                    box.Explode(box.explosionRadius);
                    boxToPick.transform.position = grabPoint.transform.position;
                    boxToPick.transform.parent = grabPoint.transform;
                    break;

                case Box.BoxType.Nitro:
                    box.Explode(box.explosionRadius);
                    break;
            }
    }

    public void HandleLaunch()
    {   
        //botManager.hasBox = false;
        //Rimuovere dal parent
        pickedBox.transform.parent = null;
        //rendere "attiva" la cassa
        Box box = pickedBox.GetComponent<Box>();
        box.isThrown = true;
        //aggiungere RigidBody
        Rigidbody rd =  pickedBox.AddComponent<Rigidbody>();
        pickedBox.GetComponentInParent<BoxCollider>().enabled = true;
        
        //LANCIO
        rd.AddForce(transform.forward*ThrowPower, ForceMode.Impulse);
        animator.CrossFade("Grab Empty",0.2f);
        animationHandler.PlayAnimationTarget("Throw", true);
        
        pickedBox = null;
        botMovementManager.targetBox = null;
        

        //StartCoroutine(ThrowBox());

    }
}
