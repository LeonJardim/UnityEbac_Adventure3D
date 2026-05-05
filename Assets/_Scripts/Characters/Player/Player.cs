using Animation;
using DG.Tweening;
using Items;
using Leon.PlayerInputs;
using Leon.StateMachine;
using Skin;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    #region VARIABLES
    private PlayerInputs _input;

    [Header("References")]
    public Collider capCollider;
    [SerializeField] private SkinChanger skinChanger;
    [SerializeField] private ParticleSystem _dustParticles;
    [HideInInspector] public Camera cam;
    [HideInInspector] public AnimationBase animationBase;
    [HideInInspector] public CharacterController characterController;
    public HealthBase health;
    private Vector3 _initialPosition;
    private SOInt _lifePack;

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

     // INPUT VARIABLES
    private Vector3 _moveDirection;
    private Vector3 _rotationDirection;
    private float _velocity_Y;

    // UTILITY VARIABLES
    [HideInInspector] public IInteractable interactableObj;
    [HideInInspector] public bool mouse_captured = false;
    [HideInInspector] public bool isDead = false;
    private float _damageMultiplier = 1f;
    private bool _firstTimeDamage = true;
    private bool _jumping = false;
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
        _input.Player.Interact.started += i => Interact();
        _input.Player.Item.started += i => UseLifePack();
        _input.Player.Escape.started += i => EscapeInput();
        _input.Player._1.performed += i => SwitchGun(0);
        _input.Player._2.performed += i => SwitchGun(1);

        _input.Player.Move.canceled += i => moveInput = Vector2.zero;
        _input.Player.Jump.canceled += i => jumpInput = false;
        _input.Player.Attack.canceled += i => MouseUnclick();
    }
    #endregion


    private void Start()
    {
        cam = Camera.main;
        animationBase = GetComponent<AnimationBase>();
        characterController = GetComponent<CharacterController>();
        _lifePack = ItemManager.Instance.GetItemByType(ItemType.LIFE_PACK).soInt;
        health = GetComponent<HealthBase>();
        if (health != null) health.OnKill += OnKill;
        health.SetHealth(SaveManager.Instance.saveSetup.health);
        _initialPosition = transform.position;
        StateMachineInit();
    }

    private void Update()
    {
        stateMachine.Update();
        ApplyMove();
        ApplyGravity();
    }


    #region HEALTH / DAMAGE
    public void Damage(int amount)
    {
        health.TakeDamage((int)(amount * _damageMultiplier));
        if (_firstTimeDamage && _lifePack.Value > 0)
        {
            _firstTimeDamage = false;
            ItemManager.Instance.ShowItemTip();
        }
    }
    public void Damage(int amount, Vector3 dir)
    {
        health.TakeDamage((int)(amount * _damageMultiplier));
        if (_firstTimeDamage && _lifePack.Value > 0)
        {
            _firstTimeDamage = false;
            ItemManager.Instance.ShowItemTip();
        }
        transform.DOMove(transform.position - dir, 0.2f);
        if (FXManager.Instance != null)
        {
            FXManager.Instance.FlashVignette();
            FXManager.Instance.ScreenShake();
        }
    }

    private void OnKill()
    {
        health.OnKill -= OnKill;
        stateMachine.SwitchState(PlayerStates.DEAD);
        Invoke(nameof(Respawn), 3f);
    }

    public void Respawn()
    {
        health.OnKill += OnKill;
        health.ResetLife();
        stateMachine.SwitchState(PlayerStates.IDLE);
        animationBase.PlayAnimationByTrigger(AnimationType.REVIVE);
        if (CheckPointManager.Instance.HasCheckPoint())
        {
            transform.position = CheckPointManager.Instance.GetPositionFromLastCheckpoint();
        }
        else
        {
            transform.position = _initialPosition;
        }
    }
    #endregion

    #region MOVEMENT
    private void ApplyGravity()
    {
        if (!IsOnFloor())
        {
            if (_velocity_Y > -1.0f)
                _velocity_Y -= _gravity * Time.deltaTime;
        }
        else
        {
            if (_jumping)
            {
                animationBase.PlayAnimationByTrigger(AnimationType.LAND);
                _dustParticles.Play();
                _jumping = false;
            }
        }

    }
    private void ApplyMove()
    {
        if (!characterController.enabled) return;
        _moveDirection.y = _velocity_Y;
        characterController.Move(moveSpeed * Time.deltaTime * _moveDirection);
    }

    public void HandleJump()
    {
        if (jumpInput && IsOnFloor())
        {
            jumpInput = false;
            _jumping = true;
            _dustParticles.Stop();
            _velocity_Y = jumpForce;
            animationBase.PlayAnimationByTrigger(AnimationType.JUMP);
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

    #region GAMEPLAY SKIN POWER EFFECTS
    public void ChangeSpeed(float speed, float duration)
    {
        StartCoroutine(ChangeSpeedCoroutine(speed, duration));
    }
    IEnumerator ChangeSpeedCoroutine(float speed, float duration)
    {
        var defaultSpeed = moveSpeed;
        moveSpeed *= speed;
        yield return new WaitForSeconds(duration);
        moveSpeed = defaultSpeed;
    }


    public void ChangeTexture(SkinSetup skinSetup, float duration)
    {
        StartCoroutine(ChangeTextureCoroutine(skinSetup, duration));
    }
    IEnumerator ChangeTextureCoroutine(SkinSetup skinSetup, float duration)
    {
        skinChanger.ChangeTexture(skinSetup);
        yield return new WaitForSeconds(duration);
        skinChanger.ResetTexture();
    }


    public void ChangeDamageMultiply(float multiplier, float duration)
    {
        StartCoroutine(ChangeDamageMultiplyCoroutine(multiplier, duration));
    }
    IEnumerator ChangeDamageMultiplyCoroutine(float multiplier, float duration)
    {
        _damageMultiplier = multiplier;
        yield return new WaitForSeconds(duration);
        _damageMultiplier = 1f;
    }
    #endregion

    #region ACTIONS

    private void MouseClick()
    {
        if (GameManager.Instance.gamePaused) return;
        if (!mouse_captured) CaptureMouse();
        else if (isDead) return;
        else
        {
            abilityShoot.StartShoot();
        }
    }
    private void MouseUnclick()
    {
        abilityShoot.StopShoot();
    }
    private void Interact()
    {
        interactableObj?.Action();
    }

    private void UseLifePack()
    {
        if (_lifePack.Value > 0 && health.currentLife < health.startingLife)
        {
            ItemManager.Instance.RemoveByType(ItemType.LIFE_PACK);
            health.Heal(health.startingLife / 2);
        }
    }

    private void SwitchGun(int f)
    {
        abilityShoot.SwitchGun(f);
    }

    private void EscapeInput()
    {
        if (mouse_captured)
        {
            ReleaseMouse();
            GameManager.Instance.PauseGame();
        }
        else
        {
            CaptureMouse();
            GameManager.Instance.UnPauseGame();
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
