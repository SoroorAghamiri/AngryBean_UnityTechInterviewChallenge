using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BeanInputHandler : MonoBehaviour
{
[Header("Input Action Assets")]
    [SerializeField] private InputActionAsset playerControls;

    [Header("Action Map Name References")]
    [SerializeField] private string actionMapName = "Bean";

    [Header("Action Name Reference")]
    [SerializeField] private string move = "Move";
    [SerializeField] private string look = "Look";
    [SerializeField] private string jump = "Jump";
    [SerializeField] private string shoot = "Shoot";
    [SerializeField] private string aim = "Aim";
    [SerializeField] private string run = "Run";

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction shootAction;
    private InputAction aimAction;
    private InputAction runAction;

    public Vector2 MoveInput {get; private set;}
    public Vector2 LookInput {get; private set;}
    public bool JumpTriggered {get; private set;}
    public float ShootValue {get; private set;}
    public float AimValue {get; private set;}
    public float RunValue {get; private set;}

    public static BeanInputHandler Instance {get; private set;}

    private void Awake(){
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else{
            Destroy(gameObject);
        }

        moveAction = playerControls.FindActionMap(actionMapName).FindAction(move);
        lookAction = playerControls.FindActionMap(actionMapName).FindAction(look);
        jumpAction = playerControls.FindActionMap(actionMapName).FindAction(jump);
        shootAction = playerControls.FindActionMap(actionMapName).FindAction(shoot);
        aimAction = playerControls.FindActionMap(actionMapName).FindAction(aim);
        runAction = playerControls.FindActionMap(actionMapName).FindAction(run);
        RegisterInputActions();
    }

    void RegisterInputActions(){
        moveAction.performed += context => MoveInput = context.ReadValue<Vector2>();
        moveAction.canceled += context => MoveInput = Vector2.zero;

        lookAction.performed += context => LookInput = context.ReadValue<Vector2>();
        lookAction.canceled += context => LookInput = Vector2.zero;

        jumpAction.performed += context => JumpTriggered = true;
        jumpAction.canceled += context => JumpTriggered = false;

        shootAction.performed += context => ShootValue = context.ReadValue<float>();
        shootAction.canceled += context => ShootValue = 0f;
        
        aimAction.performed += context => AimValue = context.ReadValue<float>();
        aimAction.canceled += context => AimValue = 0f;

        runAction.performed += context => RunValue = context.ReadValue<float>();
        runAction.canceled += context => RunValue = 0f;
    }

    private void OnEnable() {
        moveAction.Enable();
        jumpAction.Enable();
        lookAction.Enable();
        shootAction.Enable();
        aimAction.Enable();
        runAction.Enable();
    }

    private void OnDisable() {
        moveAction.Disable();
        jumpAction.Disable();
        lookAction.Disable();
        shootAction.Disable();
        aimAction.Disable();
        runAction.Disable();
    }
}
