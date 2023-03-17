using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackHandler : MonoBehaviour
{
    AnimationHandler animationHandler;
    PlayerMovementHandler playerMovementHandler;

    public LayerMask boxLayerMask;
    public LayerMask characterLayerMask;

    void Awake()
    {
        animationHandler = GetComponent<AnimationHandler>();
        playerMovementHandler = GetComponent<PlayerMovementHandler>();
    }

    public void HandleAttack()
    {
        playerMovementHandler.ResetVelocity();
        animationHandler.PlayAnimationTarget("Attack", true);
    }


    //DEPRECATED
    /*
    public void EnableCollider()
    {
        GetComponentInChildren<BoxCollider>().enabled = true;
    }

    public void DisableCollider()
    {
        GetComponentInChildren<BoxCollider>().enabled = false;
    }*/

    public void Attack()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position + transform.forward*0.6f, new Vector3(0.25f,0.65f, 0.5f), Quaternion.identity, boxLayerMask);
        foreach (Collider c in colliders)
        {
            Box box = c.gameObject.GetComponent<Box>();
            if(box.isActive)
                break;
            
            if(box.type == Box.BoxType.Normal)
            {
                StartCoroutine(box.Move(transform.forward));
                break;
            }

            if(box.type == Box.BoxType.TNT)
            {
                box.Explode(box.explosionRadius);
                StartCoroutine(box.Move(transform.forward));
                break; 
            }

            if(box.type == Box.BoxType.Nitro)
            {
                box.Explode(box.explosionRadius);
            }
        }
    }
}
