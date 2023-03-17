using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    //singleton
    private InputManager instance;
    [HideInInspector] public PlayerInput input;

    //DPad - LStick
    Vector2 movementInput;
    public float vertical;
    public float horizontal;
    public float inputMagnitude;

    //Buttons
    bool jumpButton; //A
    bool actionButton; //X
    bool attackButton; //B
    
    public void OnEnable(){
        if(input == null){
            input = new PlayerInput();
            SetUpUIMovementInput();
        }

        input.Enable();
    }

    public void OnDisable(){
        input.Disable();
    }


    // Start is called before the first frame upwasdadate
    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        UINavigationInput();
    }


    void SetUpUIMovementInput()
    {
        input.UI.Move.started += input => movementInput = input.ReadValue<Vector2>();
        input.UI.Move.canceled += input => movementInput = input.ReadValue<Vector2>();
        input.UI.Move.performed += input => movementInput = input.ReadValue<Vector2>();
    }



    void UINavigationInput()
    {
        horizontal = movementInput.x;
        vertical = movementInput.y;
        inputMagnitude = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
    }
}
