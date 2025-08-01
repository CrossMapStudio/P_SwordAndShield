using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System;
using System.Linq;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class PlayerController : Entity
{
    private PlayerStateMachine Player_SM;
    private CharacterEntry CharacterData;
    private PlayerInputDriver InputDriver;
    public Rigidbody2D PlayerRB { get; private set; }
    public CapsuleCollider2D PlayerCapsuleCollider { get; private set; }
    [field: SerializeField] public SpriteRenderer SpriteRend { get; private set; }
    [field: SerializeField] public Animator SpriteAnimator { get; private set; }
    public PlayerState QueueState { get; private set; }
    [field: SerializeField] public Character ID { get; private set; }

    private Vector2 PlayerVelocity;
    private bool CachedQueryStartInColliders;
    private float FrameLeftGround = float.MinValue;
    //Need Accessor for Animation ---
    private bool PlayerGrounded;
    public bool _PlayerGrounded{ get { return PlayerGrounded; } private set 
        { 
            PlayerGrounded = value; 
            if (value)
            {
                GroundReset_Jump = CharacterData.JumpCount;
                GroundReset_Dodge = CharacterData.DodgeCount;
            }
        } 
    }
    private int GroundReset_Jump, GroundReset_Dodge;

    public UnityEvent ApplyBaseMovement { get; private set; } = new UnityEvent();
    public Action<InputAction.CallbackContext> ApplyJump { get; private set; }
    public Action<InputAction.CallbackContext> ApplyShield { get; private set; }
    public Action<InputAction.CallbackContext> ReleaseShield { get; private set; }
    public Action ApplyDodge { get; private set; }
    public Action ResetVelocity { get; private set; }

    public void Start()
    {
        InitializeEntity(ID);
        Debug.Log($"Base Health: {BaseHealth}");
        Debug.Log($"Base Armor: {BaseArmor}");

        #region State Animations Set
        AnimatorOverrideController overrideController = new AnimatorOverrideController(SpriteAnimator.runtimeAnimatorController);
        var _Character = ClientGameController.Controller.Characters.GetCharacterByID(ID);
        overrideController["Idle"] = _Character.Idle;
        overrideController["Run"] = _Character.Running;
        overrideController["Action_1"] = _Character.Action_1;
        overrideController["Stun"] = _Character.Stunned;
        SpriteAnimator.runtimeAnimatorController = overrideController;
        #endregion

        //Tie Events ->
        ApplyBaseMovement.AddListener(CheckCollisions);
        ApplyBaseMovement.AddListener(HandleMovement);
        ApplyBaseMovement.AddListener(HandleGravity);
        ApplyBaseMovement.AddListener(ApplyMovement);
        ApplyJump += HandleJump;
        ApplyShield += HandleShieldActive;
        ReleaseShield += HandleShieldRelease;
        ApplyDodge += HandleDodge;
        ResetVelocity += ResetAllVelocity;

        PlayerRB = GetComponent<Rigidbody2D>();
        PlayerCapsuleCollider = GetComponent<CapsuleCollider2D>();
        Player_SM = new PlayerStateMachine(ID, this, PlayerRB, PlayerCapsuleCollider, SpriteRend, SpriteAnimator);
        CharacterData = ClientGameController.Controller.Characters.GetCharacterByID(ID);
        InputDriver = Player_SM.InputDriver;

        GroundReset_Jump = CharacterData.JumpCount;
        GroundReset_Dodge = CharacterData.DodgeCount;
    }

    public void Update()
    {
        Player_SM.executeStateUpdate();
    }

    public void FixedUpdate()
    {
        Player_SM.executeStateFixedUpdate();
    }

    public void LateUpdate()
    {
        Player_SM.executeStateLateUpdate();
    }

    public override void TakeDamage(int _Damage)
    {
        if (Player_SM.getCurrentStateName() == "P_Dodge") return;
        if (CurrentArmor > 0)
        {
            CurrentArmor -= _Damage;
            if (CurrentArmor < 0)
                CurrentArmor = 0;
            OnDamageArmor?.Invoke();
            #if UNITY_EDITOR
                        Debug.Log($"Current Armor: {CurrentArmor}");
            #endif
            return;
        }

        CurrentHealth -= _Damage;
        if (CurrentHealth < 0)
        {
            CurrentHealth = 0;
            OnPlayerDead?.Invoke();
            #if UNITY_EDITOR
                        Debug.Log("Player Dead");
            #endif
            return;
        }

        OnDamageNoArmor?.Invoke();
        #if UNITY_EDITOR
                Debug.Log($"Current Health: {CurrentHealth}");
        #endif
    }
    private void CheckCollisions()
    {
        Physics2D.queriesStartInColliders = false;

        // Ground and Ceiling

        if (_PlayerGrounded && PlayerVelocity.y > 0)
        {
            _PlayerGrounded = false;
        }
        else
        {
            _PlayerGrounded = Physics2D.CapsuleCast(PlayerCapsuleCollider.bounds.center, PlayerCapsuleCollider.size, PlayerCapsuleCollider.direction, 0, Vector2.down,
                CharacterData.GrounderDistance, ~CharacterData.PlayerLayer).collider != null;
            Debug.Log($"Grounded Status: {PlayerGrounded}");
        }

        bool ceilingHit = Physics2D.CapsuleCast(PlayerCapsuleCollider.bounds.center, PlayerCapsuleCollider.size, PlayerCapsuleCollider.direction, 0, Vector2.up,
            CharacterData.GrounderDistance, ~CharacterData.PlayerLayer).collider != null;

        // Hit a Ceiling
        if (ceilingHit) PlayerVelocity.y = Mathf.Min(0, PlayerVelocity.y);
        Physics2D.queriesStartInColliders = CachedQueryStartInColliders;
    }
    private void HandleMovement()
    {
        var Input = InputDriver.Get_Movement.ReadValue<Vector2>();
        Input = new Vector2(Input.x > 0 ? 1 : -1, 0);
        if (InputDriver.Get_Movement.ReadValue<Vector2>().x == 0)
        {
            var deceleration = PlayerGrounded ? CharacterData.GroundDeceleration : CharacterData.AirDeceleration;
            PlayerVelocity.x = Mathf.MoveTowards(PlayerVelocity.x, 0, deceleration * Time.fixedDeltaTime);
        }
        else
        {
            PlayerVelocity.x = Mathf.MoveTowards(PlayerVelocity.x, Input.x * CharacterData.MaxSpeed, CharacterData.Acceleration * Time.fixedDeltaTime);
        }
    }
    private void HandleJump(InputAction.CallbackContext context)
    {
        if (GroundReset_Jump <= 0) return;
        GroundReset_Jump--;
        PlayerVelocity.y = CharacterData.JumpPower;
    }
    private void HandleShieldActive(InputAction.CallbackContext context)
    {
        Player_SM.changeState(Player_SM.Player_States.FirstOrDefault(c => c.ID == "P_Shield"));
    }
    private void HandleShieldRelease(InputAction.CallbackContext context)
    {
        Player_SM.changeState(Player_SM.Player_States.FirstOrDefault(c => c.ID == "P_Movement"));
    }
    private void HandleDodge()
    {
        if (GroundReset_Dodge <= 0) return;
        GroundReset_Dodge--;
        Player_SM.changeState(Player_SM.Player_States.FirstOrDefault(c => c.ID == "P_Dodge"));
    }
    private void HandleGravity()
    {
        if (PlayerGrounded && PlayerVelocity.y <= 0f)
        {
            PlayerVelocity.y = CharacterData.GroundingForce;
        }
        else
        {
            var inAirGravity = CharacterData.FallAcceleration;
            PlayerVelocity.y = Mathf.MoveTowards(PlayerVelocity.y, -CharacterData.MaxFallSpeed, inAirGravity * Time.fixedDeltaTime);
        }
    }
    private void ApplyMovement() => PlayerRB.linearVelocity = PlayerVelocity;
    private void ResetAllVelocity() => PlayerVelocity = Vector2.zero;


    public void OnDestroy()
    {
        OnDamageArmor.RemoveAllListeners();
        OnDamageNoArmor.RemoveAllListeners();
        OnPlayerDead.RemoveAllListeners();
    }
}
