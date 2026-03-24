using Leon.PlayerInputs;
using Leon.StateMachine;
using UnityEngine;
using static GameManager;

public class Player : MonoBehaviour
{
    #region VARIABLES
    private PlayerInputs _input;

    [Header("References")]
    public Camera cam;
    [HideInInspector] public CharacterController characterController;

    [Header("Speeds")]
    public float moveSpeed = 2f;
    public float jumpForce = 8f;
    [SerializeField] private float _gravity = 10f;
    
    [Header("Inputs")]
    public Vector2 moveInput;
    public bool jumpTrigger;

    private Vector3 _moveDirection;
    private float _velocity_Y;
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


    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        PerformInputs();
    }

    private void Start()
    {
        cam = Camera.main;
        StateMachineInit();
    }

    private void Update()
    {
        ApplyGravity();
        stateMachine.Update();
        ApplyMove();
    }


    private void OnEnable() => _input.Player.Enable();
    private void OnDisable() => _input.Player.Disable();
    private void OnDestroy() => _input.Dispose();


    private void PerformInputs()
    {
        _input = new PlayerInputs();

        _input.Player.Move.performed += i => moveInput = i.ReadValue<Vector2>();
        _input.Player.Jump.started += i => jumpTrigger = true;

        _input.Player.Move.canceled += i => moveInput = Vector2.zero;
    }


    private void ApplyGravity()
    {
        if (!IsOnFloor())
            _velocity_Y -= _gravity * Time.deltaTime;
    }
    private void ApplyMove()
    {
        _moveDirection.y = _velocity_Y;
        characterController.Move(moveSpeed * Time.deltaTime * _moveDirection);
    }
    public void HandleJump()
    {
        if (jumpTrigger && IsOnFloor())
        {
            jumpTrigger = false;
            _velocity_Y = jumpForce;
        }
    }
    public void HandleGroundMovement()
    {
        _moveDirection = cam.transform.forward * moveInput.y;
        _moveDirection += cam.transform.right * moveInput.x;
        _moveDirection.y = 0f;
        _moveDirection.Normalize();
    }

    public bool IsOnFloor() => characterController.isGrounded;
}
