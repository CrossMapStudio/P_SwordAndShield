using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.Events;

#region State Machines
public class StateMachine
{
    private BaseState currentState, previousState;
    public void changeState(BaseState newState, GameObject stateIdentity = null)
    {
        if (!newState.checkValid())
            return;

        if (currentState != null)
        {
            this.currentState.onExit();
        }
        this.previousState = this.currentState;
        this.currentState = newState;
        this.currentState.onEnter();
    }
    public virtual void executeStateUpdate()
    {
        var runningState = this.currentState;
        if (runningState != null)
        {
            this.currentState.onUpdate();
        }
    }
    public virtual void executeStateFixedUpdate()
    {
        var runningState = this.currentState;
        if (runningState != null)
        {
            this.currentState.onFixedUpdate();
        }
    }
    public virtual void executeStateLateUpdate()
    {
        var runningState = this.currentState;
        if (runningState != null)
        {
            this.currentState.onLateUpdate();
        }
    }
    public void previousStateSwitch()
    {
        if (this.previousState != null)
        {
            this.currentState.onExit();
            this.currentState = this.previousState;
            this.currentState.onEnter();
        }
        else
        {
            return;
        }
    }
    //To Allow Us to Check for the State
    public BaseState getCurrentState()
    {
        return currentState;
    }
    public string getCurrentStateName()
    {
        return currentState.ID;
    }
}

//----------------------------------------------------------------------------------
/// <summary>
/// Player State Machine
/// </summary>
public class PlayerStateMachine : StateMachine
{
    public PlayerInputDriver InputDriver { get; private set; }
    public PlayerController Controller { get; private set; }
    public CharacterEntry CharacterData { get; private set; }
    public Rigidbody2D PlayerRigidBody { get; private set; }
    public CapsuleCollider2D PlayerCapsuleCollider { get; private set; }
    public List<PlayerState> Player_States { get; private set; }
    public Character CharacterDB_ID { get; private set; }

    Dictionary<string, HashSet<string>> StateLinks;
    //Shared Values Between States
    public Vector2 PlayerInput { get; internal set; }
    //Create Our Weapon State Machine and Populate Weapons:::
    private WeaponStateMachine Weapon_SM;

    #region On Fixed Update Action Ties
    public UnityEvent ApplyBaseMovement { get; private set; } = new UnityEvent();
    public UnityEvent ApplyDiveMovement { get; private set; } = new UnityEvent();
    public UnityEvent ApplyForces { get; private set; } = new UnityEvent();
    public Action ApplyCollision { get; private set; }
    public Action ResetVelocity { get; private set; }
    #endregion
    #region On Input Action Ties
    public Action<InputAction.CallbackContext> _StartJump { get; private set; }
    public Action<InputAction.CallbackContext> _StartShieldActive { get; private set; }
    public Action<InputAction.CallbackContext> _StartShieldRelease { get; private set; }
    public Action _StartDodge { get; private set; }
    public Action<InputAction.CallbackContext> _StartDive { get; private set; }
    #endregion

    //State Tie Parameters
    private Vector2 PlayerVelocity;
    public Vector2 Get_PlayerVelocity => PlayerVelocity;
    private bool CachedQueryStartInColliders;
    private float FrameLeftGround = float.MinValue;
    //Need Accessor for Animation ---
    private bool PlayerGrounded;
    public bool _PlayerGrounded
    {
        get { return PlayerGrounded; }
        private set
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

    public PlayerStateMachine(Character ID, PlayerController _Controller, Rigidbody2D _PlayerRigidBody, CapsuleCollider2D _PlayerCapsuleCollider, SpriteRenderer _PlayerSpriteRenderer, Animator _SpriteAnimator)
    {
        CharacterDB_ID = ID;
        CharacterData = ClientGameController.Controller.Characters.GetCharacterByID(CharacterDB_ID);
        Controller = _Controller;
        InputDriver = new PlayerInputDriver();
        PlayerRigidBody = _PlayerRigidBody;
        PlayerCapsuleCollider = _PlayerCapsuleCollider;
        Player_States = new List<PlayerState>();

        GroundReset_Jump = CharacterData.JumpCount;
        GroundReset_Dodge = CharacterData.DodgeCount;

        #region Player States
        Player_States.Add(new Player_Movement("P_Movement")
        {
            Assigned_SM = this,
            InputController = this.InputDriver,
            PlayerRigidBody = _PlayerRigidBody,
            PlayerCapsuleCollider = _PlayerCapsuleCollider,
            PlayerSpriteRenderer = _PlayerSpriteRenderer,
            PlayerSpriteAnimator = _SpriteAnimator
        });
        Player_States.Add(new Player_Shield("P_Shield")
        {
            Assigned_SM = this,
            InputController = InputDriver,
            PlayerRigidBody = _PlayerRigidBody,
            PlayerCapsuleCollider = _PlayerCapsuleCollider,
            PlayerSpriteRenderer = _PlayerSpriteRenderer,
            PlayerSpriteAnimator = _SpriteAnimator
        });
        Player_States.Add(new Player_Dodge("P_Dodge")
        {
            Assigned_SM = this,
            InputController = InputDriver,
            PlayerRigidBody = _PlayerRigidBody,
            PlayerCapsuleCollider = _PlayerCapsuleCollider,
            PlayerSpriteRenderer = _PlayerSpriteRenderer,
            PlayerSpriteAnimator = _SpriteAnimator
        });
        Player_States.Add(new Player_Dive("P_Dive")
        {
            Assigned_SM = this,
            InputController = InputDriver,
            PlayerRigidBody = _PlayerRigidBody,
            PlayerCapsuleCollider = _PlayerCapsuleCollider,
            PlayerSpriteRenderer = _PlayerSpriteRenderer,
            PlayerSpriteAnimator = _SpriteAnimator
        });
        Player_States.Add(new Player_Hit("P_Hit")
        {
            Assigned_SM = this,
            InputController = InputDriver,
            PlayerRigidBody = _PlayerRigidBody,
            PlayerCapsuleCollider = _PlayerCapsuleCollider,
            PlayerSpriteRenderer = _PlayerSpriteRenderer,
            PlayerSpriteAnimator = _SpriteAnimator
        });
        Player_States.Add(new Player_Dead("P_Dead")
        {
            Assigned_SM = this,
            InputController = InputDriver,
            PlayerRigidBody = _PlayerRigidBody,
            PlayerCapsuleCollider = _PlayerCapsuleCollider,
            PlayerSpriteRenderer = _PlayerSpriteRenderer,
            PlayerSpriteAnimator = _SpriteAnimator
        });
        Player_States.Add(new Player_Stunned("P_Stunned")
        {
            Assigned_SM = this,
            InputController = InputDriver,
            PlayerRigidBody = _PlayerRigidBody,
            PlayerCapsuleCollider = _PlayerCapsuleCollider,
            PlayerSpriteRenderer = _PlayerSpriteRenderer,
            PlayerSpriteAnimator = _SpriteAnimator
        });
        Player_States.Add(new Player_Hold("P_Hold")
        {
            Assigned_SM = this,
            InputController = InputDriver,
            PlayerRigidBody = _PlayerRigidBody,
            PlayerCapsuleCollider = _PlayerCapsuleCollider,
            PlayerSpriteRenderer = _PlayerSpriteRenderer,
            PlayerSpriteAnimator = _SpriteAnimator
        });
        #endregion
        #region State Ties
        //All Events that have Multiple Ties are Unity Event for Easy Dispose --- Else Action
        #region On Fixed Update Callers
        ApplyBaseMovement.AddListener(CheckCollisions);
        ApplyBaseMovement.AddListener(HandleMovement);
        ApplyBaseMovement.AddListener(HandleGravity);
        ApplyBaseMovement.AddListener(ApplyMovement);

        ApplyDiveMovement.AddListener(CheckCollisions);
        ApplyDiveMovement.AddListener(HandleMovement);
        ApplyDiveMovement.AddListener(HandleDive);
        ApplyDiveMovement.AddListener(ApplyMovement);

        ApplyForces.AddListener(CheckCollisions);
        ApplyForces.AddListener(HandleGravity);
        ApplyForces.AddListener(HandleMovement_Stop);
        ApplyForces.AddListener(ApplyMovement);

        ApplyCollision += CheckCollisions;
        ResetVelocity += ResetAllVelocity;
        #endregion

        #region On Input Callers
        _StartJump += StartJump;
        _StartShieldActive += StartShieldActive;
        _StartShieldRelease += StartShieldRelease;
        _StartDodge += StartDodge;
        _StartDive += StartDive;
        #endregion

        #endregion

        //Exclusion States -> If the State that is Trying to Take Over is on the List -> We Return OUT
        /* This is a mess and needs to be remedied -> Player State Enums for this : */
        HashSet<string> LinkSet_Dodge = new HashSet<string> { "P_Hit", "P_Dead", "P_Stunned", "P_Dodge" };
        HashSet<string> LinkSet_Hit = new HashSet<string> { "P_Dodge" };
        StateLinks = new Dictionary<string, HashSet<string>>();
        StateLinks.Add("P_Dodge", LinkSet_Dodge);
        StateLinks.Add("P_Hit", LinkSet_Hit);

        changeState(Player_States.FirstOrDefault(c => c.ID == "P_Movement"));
        PlayerInput = InputDriver.Get_Movement.ReadValue<Vector2>();
        //InputDriver.Get_Boost.performed += QueueInput_Dodge;

        //Generate the Weapons for the Character --->
        Weapon_SM = new WeaponStateMachine(CharacterDB_ID, InputDriver, Controller);
    }
    
    
    //--------------------------------------------------------------------
    /// <summary>
    /// Main State Functions
    /// </summary>

    public override void executeStateUpdate()
    {
        base.executeStateUpdate();
        Weapon_SM.executeStateUpdate();
    }
    public override void executeStateFixedUpdate()
    {
        base.executeStateFixedUpdate();
        Weapon_SM.executeStateFixedUpdate();
    }
    public override void executeStateLateUpdate()
    {
        base.executeStateLateUpdate();
        Weapon_SM.executeStateLateUpdate();
    }


    //-----------------------------------------------------------------
    /// <summary>
    /// All Actions/Events needed for the States to Tie/Call To ---
    /// </summary>

    #region On Fixed Update Actions/Events
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
    private void HandleMovement_Stop()
    {
        PlayerVelocity.x = Mathf.MoveTowards(PlayerVelocity.x, 0, 15 * Time.fixedDeltaTime);
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
    private void HandleDive()
    {
        if (PlayerGrounded && PlayerVelocity.y <= 0f)
        {
            PlayerVelocity.y = CharacterData.GroundingForce;
            changeState(Player_States.FirstOrDefault(c => c.ID == "P_Stunned"));
            return;
        }
        else
        {
            var inAirGravity = CharacterData.FallAcceleration * 5f;
            PlayerVelocity.y = Mathf.MoveTowards(PlayerVelocity.y, -CharacterData.MaxFallSpeed * 1.25f, inAirGravity * Time.fixedDeltaTime);
        }
    }
    #endregion
    #region On Input Start Actions
    private void StartJump(InputAction.CallbackContext context)
    {
        if (GroundReset_Jump <= 0) return;
        GroundReset_Jump--;
        PlayerVelocity.y = CharacterData.JumpPower;
    }
    private void StartShieldActive(InputAction.CallbackContext context)
    {
        changeState(Player_States.FirstOrDefault(c => c.ID == "P_Shield"));
    }
    private void StartShieldRelease(InputAction.CallbackContext context)
    {
        changeState(Player_States.FirstOrDefault(c => c.ID == "P_Movement"));
    }
    private void StartDodge()
    {
        if (GroundReset_Dodge <= 0) return;
        GroundReset_Dodge--;
        changeState(Player_States.FirstOrDefault(c => c.ID == "P_Dodge"));
    }
    private void StartDive(InputAction.CallbackContext context)
    {
        changeState(Player_States.FirstOrDefault(c => c.ID == "P_Dive"));
    }
    #endregion
    private void ApplyMovement() => PlayerRigidBody.linearVelocity = PlayerVelocity;
    private void ResetAllVelocity() => PlayerVelocity = Vector2.zero;
}


//----------------------------------------------------------------------------------
/// <summary>
/// Weapon State Machine
/// </summary>
public class WeaponStateMachine : StateMachine
{
    private PlayerInputDriver InputDriver;
    public PlayerController Controller { get; private set; }
    private List<Weapon> WeaponList;
    private int WeaponIndex = 0;
    public Weapon AssignedWeapon { get; private set; }
    public WeaponStateMachine(Character ID, PlayerInputDriver _InputDriver, PlayerController _Controller)
    {
        InputDriver = _InputDriver;
        Controller = _Controller;
        WeaponList = new List<Weapon>();
        List<Weapon> weapons = ClientGameController.Controller.Characters.GetCharacterByID(ID).WeaponList;
        for (int i = 0; i < weapons.Count; i++)
        {
            WeaponList.Add(GameObject.Instantiate(weapons[i], _Controller.transform.position, Quaternion.identity));
            WeaponList[i].AssignedTeam = _Controller.AssignedTeam;
            if (i == 0)
            {
                AssignedWeapon = WeaponList[0];
                continue;
            }

            WeaponList[i].gameObject.SetActive(false);
        }

        InputDriver.Get_PrimaryAction.performed += OnPrimaryDown;
        InputDriver.Get_PrimaryAction.canceled += OnPrimaryUp;
    }

    public void OnPrimaryDown(InputAction.CallbackContext Context)
    {
        AssignedWeapon.OnPrimaryDown();
    }
    public void OnPrimaryUp(InputAction.CallbackContext Context)
    {
        AssignedWeapon.OnPrimaryUp();
    }
    public void IterateWeapon()
    {
        /*
        if (WeaponIndex >= WeaponList.Count - 1) WeaponIndex = 0; else { WeaponIndex++; }
        AssignedWeapon = WeaponList[WeaponIndex];
        AssignedWeapon.transform.position = Controller.transform.position;
        AssignedWeapon.gameObject.SetActive(true);
        changeState(Weapon_States.FirstOrDefault(c => c.ID == "W_Carry"));
        */
    }
}
#endregion
