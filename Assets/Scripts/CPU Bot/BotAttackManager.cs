using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotAttackManager : MonoBehaviour
{
    AnimationHandler animationHandler;
    BotMovementManager botMovementManager;
    BotManager botManager;
    
    BotIdleState idleState;
    BotSearchEnemyState searchEnemyState;

    public LayerMask boxLayerMask;
    public LayerMask characterLayerMask;

    private Vector3 enemyDirection;
    

    public float enemyDetectionRadius = 1;

    void Awake()
    {
        animationHandler = GetComponent<AnimationHandler>();
        botMovementManager = GetComponent<BotMovementManager>();
        botManager = GetComponent<BotManager>();
        idleState = GetComponentInChildren<BotIdleState>();
        searchEnemyState = GetComponentInChildren<BotSearchEnemyState>();
    }

    public void HandlePlayerDetection()
    {
        //Esci se gia ha un target
        if(botMovementManager.targetCharacter != null)
            return;

        Collider[] colliders = Physics.OverlapSphere(transform.position, botManager.hasBox == true ? enemyDetectionRadius*30 : enemyDetectionRadius, characterLayerMask);
        if(colliders.Length > 0)
        {
            if(colliders.Length == 1 && colliders[0].gameObject.Equals(gameObject))
                return;

            for(int i = 0; i<colliders.Length; i++)
            {
                if(!colliders[i].gameObject.Equals(gameObject))
                {
                    GameObject enemy = colliders[i].gameObject;
                    idleState.enemyInRange = true;
                    botMovementManager.targetCharacter = enemy.GetComponent<GenericCharacterManager>();
                    enemyDirection = enemy.transform.position - transform.position;
                    return;
                }
                
            }
        }
    }

    public void HandleAttack()
    {   
        if(botManager.isPerformingAction)
            return;
        botMovementManager.ResetVelocity();
        animationHandler.PlayAnimationTarget("Attack", true);
        Attack();
    }

    private void Attack()
    {
        AttackBox();
        AttackPlayer();
    }

    private void AttackBox()
    {
        botMovementManager.HandeOrientation(Time.deltaTime, enemyDirection);
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

    private void AttackPlayer()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position + transform.forward*0.6f, new Vector3(0.25f,0.65f, 0.5f), Quaternion.identity, characterLayerMask);
        foreach (Collider c in colliders)
        {
            //Evita di auto-attaccarsi
            if(c.gameObject.Equals(gameObject))
                continue;

            if(c.gameObject.tag == "Player")
            {
                PlayerManager character = c.gameObject.GetComponent<PlayerManager>();
                character.TakeDamage(1);
            }
            else
            {
                BotManager character = c.gameObject.GetComponent<BotManager>();
                character.TakeDamage(1);
            }
        }
    }
}
