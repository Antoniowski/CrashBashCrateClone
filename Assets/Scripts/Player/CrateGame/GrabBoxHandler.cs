using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabBoxHandler : MonoBehaviour
{
    PlayerManager playerManager;
    GrabAndLaunchHandler grabAndLaunchHandler;
    Animator animator;
    [SerializeField] GameObject grabPoint; 

    void Awake()
    {
        playerManager = GetComponentInParent<PlayerManager>();
        grabAndLaunchHandler = GetComponentInParent<GrabAndLaunchHandler>();
        animator = gameObject.transform.parent.GetComponentInChildren<Animator>();
        
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Box")
        {
            grabAndLaunchHandler.pickedBox = collider.gameObject;
            Rigidbody rigidbody = collider.gameObject.GetComponent<Rigidbody>();
            Box box = collider.gameObject.GetComponent<Box>();
            switch(box.type)
            {
                case Box.BoxType.Normal:
                    playerManager.hasBox = true;
                    box.isPicked = true;
                    animator.CrossFade("CastingLoop", 0.2f);
                    box.GetComponentInParent<BoxCollider>().enabled = false;
                    Destroy(rigidbody);
                    collider.gameObject.transform.position = grabPoint.transform.position;
                    collider.gameObject.transform.parent = grabPoint.transform;
                    break;
                
                case Box.BoxType.TNT:
                    playerManager.hasBox = true;
                    box.isPicked = true;
                    animator.CrossFade("CastingLoop", 0.2f);
                    box.GetComponentInParent<BoxCollider>().enabled = false;
                    Destroy(rigidbody);
                    box.Explode(box.explosionRadius);
                    collider.gameObject.transform.position = grabPoint.transform.position;
                    collider.gameObject.transform.parent = grabPoint.transform;
                    break;

                case Box.BoxType.Nitro:
                    box.Explode(box.explosionRadius);
                    break;
            }
        }
    }
}
