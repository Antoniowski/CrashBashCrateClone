using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementHandler : MonoBehaviour
{
    
    PlayerInputHandler playerInputHandler;
    CharacterController characterController;
    AnimationHandler animationHandler;
    PlayerManager playerManager;

    [SerializeField] AnimationCurve playerSpeed;
    public Vector2 velocity;
    public float verticalVelocity;
    public float movementTimer;

    float distToGround;
    CapsuleCollider colliderRiferimento;

    float ROTATION_SPEED = 20f;
    float FRICTION = 10f;
    float GRAVITY = 10f;
    float JUMP_POWER = 7f;

    void Awake()
    {
        playerInputHandler = GetComponent<PlayerInputHandler>();
        characterController = GetComponent<CharacterController>();
        animationHandler = GetComponent<AnimationHandler>();
        playerManager = GetComponent<PlayerManager>();
        colliderRiferimento = GetComponentInChildren<CapsuleCollider>();
    }

    void Start()
    {
        animationHandler.Init();
        movementTimer = 0;
        distToGround = colliderRiferimento.bounds.extents.y;
    }

    // Update is called once per frame
    void Update()
    {
        playerManager.isGrounded = IsGrounded();
        float delta = Time.deltaTime;
        MovePlayer(delta);
    }

    void MovePlayer(float delta){
        ApplyGravity(delta);
        
        if(playerInputHandler.inputMagnitude != 0 && !playerInputHandler.isInteracting)
        {
            movementTimer += delta;
            velocity =  new Vector2(playerInputHandler.horizontal, playerInputHandler.vertical) * playerSpeed.Evaluate(movementTimer) ;
            HandlePlayerOrientation(delta);
        }
        else
        {
            movementTimer = 0;
            velocity = Vector2.MoveTowards(velocity, Vector2.zero, FRICTION);
        }

        //ATTIVA LE ANIMAZIONI DI CAMMINATA
        if(!playerInputHandler.isInteracting) animationHandler.UpdateAnimatorMovementValues(playerInputHandler.inputMagnitude,0);
        characterController.Move(new Vector3(velocity.x, verticalVelocity, velocity.y)*delta);
    }

    public void Jump()
    {
        animationHandler.PlayAnimationTargetNO_INTERACTING("Jump");
        verticalVelocity = JUMP_POWER;
    }

    void ApplyGravity(float delta)
    {
        verticalVelocity -= GRAVITY*delta;
    }
    void HandlePlayerOrientation(float delta)
    {
        Quaternion toRotation = Quaternion.LookRotation(new Vector3(playerInputHandler.horizontal, 0, playerInputHandler.vertical), Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, ROTATION_SPEED*delta);
    }

    bool IsGrounded()
    {
       return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }

    public void ResetVelocity()
    {
        movementTimer = 0;
        velocity = Vector2.zero;
    }
}
