using Leon.PlayerInputs;
using Leon.StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;
using static GameManager;
using static UnityEngine.GridBrushBase;

public class Player : MonoBehaviour
{
    #region VARIABLES
    private PlayerInputs _input;
    
    [Header("References")]
    public Animator animator;
    [HideInInspector] public Camera cam;
    [HideInInspector] public CharacterController characterController;

    [Header("Speeds")]
    public float moveSpeed = 2f;
    public float jumpForce = 8f;
    [SerializeField] private float _modelRotationSpeed = 13f;
    [SerializeField] private float _gravity = 10f;

    [Header("Abilities")]
    public PlayerAbilityShoot abilityShoot;

    [Header("Inputs")]
    public Vector2 moveInput;
    public bool jumpInput;
    public bool escapeInput;


     // INPUT VARIABLES
    private Vector3 _moveDirection;
    private Vector3 _rotationDirection;
    private float _velocity_Y;

    // UTILITY VARIABLES
    [HideInInspector] public bool mouse_captured = false;
    #endregion

    #region STATE MACHINE
    public StateMachine<PlayerStates> stateMachine;
    public enum PlayerStates
    {
        IDLE,
        RUN,
        DEAD
    }

    public void StateMachineInit()
    {
        stateMachine = new StateMachine<PlayerStates>();
        stateMachine.Init();
        stateMachine.RegisterStates(PlayerStates.IDLE, new PState_Idle(this));
        stateMachine.RegisterStates(PlayerStates.RUN, new PState_Run(this));
        stateMachine.RegisterStates(PlayerStates.DEAD, new PState_Dead(this));

        stateMachine.SwitchState(PlayerStates.IDLE);
    }
    #endregion

    #region AWAKE / INPUTS
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        PerformInputs();
    }

    private void OnEnable() => _input.Player.Enable();
    private void OnDisable() => _input.Player.Disable();
    private void OnDestroy() => _input.Dispose();
    
    private void PerformInputs()
    {
        _input = new PlayerInputs();

        _input.Player.Move.performed += i => moveInput = i.ReadValue<Vector2>();
        _input.Player.Jump.started += i => jumpInput = true;
        _input.Player.Attack.started += i => MouseClick();
        _input.Player.Escape.started += i => escapeInput = true;
        _input.Player._1.performed += i => SwitchGun(0);
        _input.Player._2.performed += i => SwitchGun(1);

        _input.Player.Move.canceled += i => moveInput = Vector2.zero;
        _input.Player.Jump.canceled += i => jumpInput = false;
        _input.Player.Attack.canceled += i => MouseUnclick();
        _input.Player.Escape.canceled += i => escapeInput = false;
    }
    #endregion


    private void Start()
    {
        cam = Camera.main;
        StateMachineInit();
    }

    private void Update()
    {
        ApplyGravity();
        stateMachine.Update();
        HandleAllMainActionInputs();
        ApplyMove();
    }


    #region MOVEMENT
    private void ApplyGravity()
    {
        if (!IsOnFloor() && _velocity_Y > -1.0f)
            _velocity_Y -= _gravity * Time.deltaTime;
    }
    private void ApplyMove()
    {
        _moveDirection.y = _velocity_Y;
        characterController.Move(moveSpeed * Time.deltaTime * _moveDirection);
    }

    public void HandleJump()
    {
        if (jumpInput && IsOnFloor())
        {
            jumpInput = false;
            _velocity_Y = jumpForce;
        }
    }
    public void HandleGroundMovement()
    {
        _moveDirection = cam.transform.forward * moveInput.y;
        _moveDirection += cam.transform.right * moveInput.x;
        _moveDirection.y = 0f;
        _moveDirection.Normalize();

        HandleModelRotation();
    }
    private void HandleModelRotation()
    {
        _rotationDirection = _moveDirection;
        if (_rotationDirection == Vector3.zero)
        {
            _rotationDirection = transform.forward;
        }

        Quaternion newRotation = Quaternion.LookRotation(_rotationDirection);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, _modelRotationSpeed * Time.deltaTime);
        transform.rotation = targetRotation;
    }
    public bool IsOnFloor() => characterController.isGrounded;
    #endregion

    #region ACTIONS
    public void HandleAllMainActionInputs()
    {
        HandleEscapeInput();
    }

    private void MouseClick()
    {
        if (!mouse_captured) CaptureMouse();
        else
        {
            abilityShoot.StartShoot();
        }
    }
    private void MouseUnclick()
    {
        abilityShoot.StopShoot();
    }

    private void SwitchGun(int f)
    {
        abilityShoot.SwitchGun(f);
    }

    private void HandleEscapeInput()
    {
        if (escapeInput)
        {
            escapeInput = false;
            if (mouse_captured) ReleaseMouse();
        }
    }
    #endregion

    #region MOUSE FUNCTIONS
    private void CaptureMouse()
    {
        mouse_captured = true;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void ReleaseMouse()
    {
        mouse_captured = false;
        Cursor.lockState = CursorLockMode.None;
    }
    #endregion
}
