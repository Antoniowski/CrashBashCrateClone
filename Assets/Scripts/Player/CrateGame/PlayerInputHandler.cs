using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    PlayerInput input;
    PlayerAttackHandler attackHandler;
    PlayerManager playerManager;
    PlayerMovementHandler playerMovementHandler;
    GrabAndLaunchHandler grabAndLaunchHandler;

    
    Vector2 inputDecoder;
    public float horizontal;
    public float vertical;
    public float inputMagnitude;


    
    #region BUTTONS
    
    public bool jumpButton;
    public bool attackButton;
    public bool grabAndLaunchButton;

    #endregion 


    public bool isInteracting = false;


    void OnEnable()
    {
        if(input == null)
        {
            input = new PlayerInput();
            SetUpPlayerMovementInput();
            SetUpAttackInput();
            SetUpJumpInput();
            SetUpGrabAndLaunchInput();
        }

        input.Enable();
    }

    void OnDisable()
    {
        input.Disable();
    }
    void Awake()
    {
        attackHandler = GetComponent<PlayerAttackHandler>();
        playerManager = GetComponent<PlayerManager>();
        playerMovementHandler = GetComponent<PlayerMovementHandler>();
        grabAndLaunchHandler = GetComponent<GrabAndLaunchHandler>();
    }



    #region SET UP FUNCTIONS
    void SetUpPlayerMovementInput()
    {
        input.CrateGame.Movement.started += input => inputDecoder = input.ReadValue<Vector2>();
        input.CrateGame.Movement.canceled += input => inputDecoder = input.ReadValue<Vector2>();
        input.CrateGame.Movement.performed += input => inputDecoder = input.ReadValue<Vector2>();
    }

    void SetUpJumpInput()
    {
        input.CrateGame.Jump.started += input => jumpButton = input.ReadValueAsButton();
        input.CrateGame.Jump.performed += input => jumpButton = input.ReadValueAsButton();
        input.CrateGame.Jump.canceled += input => jumpButton = input.ReadValueAsButton();
    }

    void SetUpAttackInput()
    {
        input.CrateGame.Attack.started += input => attackButton = input.ReadValueAsButton();
        input.CrateGame.Attack.performed += input => attackButton = input.ReadValueAsButton();
        input.CrateGame.Attack.canceled += input => attackButton = input.ReadValueAsButton();
    }


    void SetUpGrabAndLaunchInput()
    {
        input.CrateGame.GrabAndLaunch.started += input => grabAndLaunchButton = input.ReadValueAsButton();
        input.CrateGame.GrabAndLaunch.performed += input => grabAndLaunchButton = input.ReadValueAsButton();
        input.CrateGame.GrabAndLaunch.canceled += input => grabAndLaunchButton = input.ReadValueAsButton();
    }
    #endregion

    public void TickInput(float delta)
    {
        Move(delta);
        HandleJump(delta);
        HandleAttackInput(delta);
        HandleGrabInput(delta);
        HandleLaunchInput(delta);
    }

    void Move(float delta)
    {
        horizontal = inputDecoder.x;
        vertical = inputDecoder.y;
        inputMagnitude = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
    }

    void HandleJump(float delta)
    {
        if(!jumpButton)
            return;

        if(isInteracting)
            return;
        
        if(!playerManager.isGrounded)
            return;

        playerMovementHandler.Jump();
        
    }

    void HandleAttackInput(float delta)
    {
        if(playerManager.hasBox)
            return;

        if(!attackButton)
            return;

        if(isInteracting)
            return;

        attackHandler.HandleAttack();
    }

    void HandleGrabInput(float delta)
    {
        if(playerManager.hasBox)
            return;

        if(!grabAndLaunchButton)
            return;

        if(isInteracting)
            return;

        grabAndLaunchHandler.HandleGrab();
    }

    void HandleLaunchInput(float delta)
    {
        if(!playerManager.hasBox)
            return;
        
        if(!grabAndLaunchButton)
            return;
        
        grabAndLaunchHandler.HandleLaunch();
    }
}
