using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotMovementManager : MonoBehaviour
{
    //Referenze
    BotManager botManager;
    BotGrabAndLaunchHandler botGrabAndLaunchHandler;
    AnimationHandler animationHandler;
    CharacterController characterController;
    
    //STATE
    BotReachBoxState botReachBoxState;
    BotSearchEnemyState searchEnemyState;

    //Variabili per layer di detection
    public LayerMask boxDetectionLayer;
    public LayerMask characterDetectionLayer;

    //Variabili target
    public Box targetBox;
    private Vector3 targetDirection;
    public GenericCharacterManager targetCharacter;
    public float minDistanceBoxDetection = 0.25f;

    //Variabili movimento
    [SerializeField] AnimationCurve movementSpeed;
    float movementTimer;
    float verticalVelocity;
    Vector2 velocity;

    CapsuleCollider colliderRiferimento;
    float distToGround;

    //CONSTANTS
    float ROTATION_SPEED = 20f;
    float FRICTION = 10f;
    float GRAVITY = 10f;
    float JUMP_POWER = 7f;
    
    void Awake()
    {
        botManager = GetComponent<BotManager>();
        botGrabAndLaunchHandler = GetComponent<BotGrabAndLaunchHandler>();
        animationHandler = GetComponent<AnimationHandler>();
        characterController = GetComponent<CharacterController>();
        botReachBoxState = GetComponentInChildren<BotReachBoxState>();
        searchEnemyState = GetComponentInChildren<BotSearchEnemyState>();
        colliderRiferimento = GetComponentInChildren<CapsuleCollider>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        distToGround = colliderRiferimento.bounds.extents.y;
    }

    // Update is called once per frame
    void Update()
    {
        float delta = Time.deltaTime;
        ApplyGravity(delta);
        if(botManager.hasBox)
        {
            MoveToTarget(delta, targetCharacter == null ? Vector3.zero : targetCharacter.gameObject.transform.position);
        }
        else
        {
            MoveToTarget(delta, targetBox == null ? Vector3.zero : targetBox.gameObject.transform.position);
        }
        
        //Vector3 moveVec3 = botManager.isGrounded ? new Vector3(velocity.x, 0, velocity.y)*delta : new Vector3(velocity.x, verticalVelocity, velocity.y)*delta; 
        //characterController.Move(new Vector3(0,verticalVelocity,0)); 
    }

    public void HandleBoxDetection()
    {
        Debug.Log("Cerco Box");
        Collider[] colliders = Physics.OverlapSphere(transform.position, botManager.boxDetectionRadius, boxDetectionLayer);

        foreach (Collider c in colliders)
        {
            if(targetBox != null)
                return;

            Box box = c.gameObject.transform.GetComponent<Box>();
            
            if(box != null)
            {
                //Evita box di tipo nitro
                if(box.type == Box.BoxType.Nitro)
                    continue;
                
                if(box.isActive)
                    continue;
                
                if(box.isPicked)
                    continue;

                if(box.isThrown)
                    continue;

                targetDirection = box.transform.position - transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, transform.position);

                //Aggiungere eventualmente if con angolo di visione
                targetBox = box;
            }
            
        }

    }


    public void MoveToTarget(float delta, Vector3 position)
    {
        //ApplyGravity(delta);
        
        if(botManager.isMoving)
        {
            movementTimer += delta;
            /*if(!botManager.isGrounded)
            {
                velocity =  new Vector2(position.x, position.z).normalized * movementSpeed.Evaluate(movementTimer) * 3f;
                HandeOrientation(delta, position - transform.position);
                goto Skip;
            }*/

            if(botManager.isPerformingAction)
            {   
                movementTimer = 0;
                goto Skip;
            }
                

            /*velocity =  new Vector2(position.x - transform.position.x, position.z - transform.position.z).normalized * movementSpeed.Evaluate(movementTimer) ;*/
            Vector2 moveVec = Vector2.MoveTowards( new Vector2(transform.position.x, transform.position.z), new Vector2(position.x, position.z), movementSpeed.Evaluate(movementTimer));
            transform.position = new Vector3(moveVec.x, transform.position.y, moveVec.y);
            HandeOrientation(delta, position - transform.position);
            if(targetBox != null) CheckForBoxDistance();
            if(targetCharacter != null) CheckForPlayerDistance();
        }
        else
        {
            movementTimer = 0;
            velocity = Vector2.MoveTowards(velocity, Vector2.zero, FRICTION);
        }

        Skip:
        //ATTIVA LE ANIMAZIONI DI CAMMINATA
        if(!botManager.isPerformingAction) animationHandler.UpdateAnimatorMovementValues(botManager.isMoving ? 1 : 0,0);
        //Vector3 moveVec3 = botManager.isGrounded ? new Vector3(velocity.x, 0, velocity.y)*delta : new Vector3(velocity.x, verticalVelocity, velocity.y)*delta; 
        //characterController.Move(moveVec3); 
    }
    
    private void CheckForBoxDistance()
    {
        botReachBoxState.reachedBox = Vector3.Distance(transform.position, targetBox.gameObject.transform.position) < minDistanceBoxDetection ? true : false ;
    }

    private void CheckForPlayerDistance()
    {
        searchEnemyState.enemyInRange = Vector3.Distance(transform.position, targetCharacter.gameObject.transform.position) < 2f ? true : false;
    }

    void ApplyGravity(float delta)
    {
        verticalVelocity -= GRAVITY*delta;
    }
    public void HandeOrientation(float delta, Vector3 target)
    {
        Quaternion toRotation = Quaternion.LookRotation(new Vector3(target.x, 0, target.z), Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, ROTATION_SPEED*delta);
    }

    bool IsGrounded()
    {
       return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }

    //Stop
    public void ResetVelocity()
    {
        movementTimer = 0;
        velocity = Vector2.zero;
    }
}
