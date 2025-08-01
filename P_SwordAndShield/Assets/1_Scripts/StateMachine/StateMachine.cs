using UnityEngine;
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

public class PlayerStateMachine : StateMachine
{
    public PlayerInputDriver InputDriver { get; private set; }
    public PlayerController Controller { get; private set; }
    public List<PlayerState> Player_States { get; private set; }
    public Character CharacterDB_ID { get; private set; }
    Dictionary<string, HashSet<string>> StateLinks;
    //Shared Values Between States
    public Vector2 PlayerInput { get; internal set; }
    //Create Our Weapon State Machine and Populate Weapons:::
    private WeaponStateMachine Weapon_SM;

    public PlayerStateMachine(Character ID, PlayerController _Controller, Rigidbody2D _PlayerRigidBody, CapsuleCollider2D _PlayerCapsuleCollider, SpriteRenderer _PlayerSpriteRenderer, Animator _SpriteAnimator)
    {
        CharacterDB_ID = ID;
        Controller = _Controller;
        InputDriver = new PlayerInputDriver();
        Player_States = new List<PlayerState>();

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
        //Weapon_SM = new WeaponStateMachine(CharacterDB_ID, InputDriver, Controller);
    }

    public override void executeStateUpdate()
    {
        base.executeStateUpdate();
        //Weapon_SM.executeStateUpdate();
    }

    public override void executeStateFixedUpdate()
    {
        base.executeStateFixedUpdate();
        //Weapon_SM.executeStateFixedUpdate();
    }

    public override void executeStateLateUpdate()
    {
        base.executeStateLateUpdate();
        //Weapon_SM.executeStateLateUpdate();
    }

    public void QueueInput_Dodge(InputAction.CallbackContext Context)
    {
        if (PlayerInput == Vector2.zero) return;
        if (StateLinks["P_Dodge"].Contains(getCurrentStateName())) return;

        changeState(Player_States.FirstOrDefault(c => c.ID == "P_Dodge"));
    }
}

public class WeaponStateMachine : StateMachine
{
    private PlayerInputDriver InputDriver;
    public PlayerController Controller { get; private set; }
    public List<WeaponState> Weapon_States { get; private set; }
    private List<Weapon> WeaponList;
    private int WeaponIndex = 0;
    public Weapon AssignedWeapon { get; private set; }
    private float RechargeTime;

    public UnityEvent OnAim { get; private set; } 
    public UnityEvent OnRelease { get; private set; }

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

        #region Create the States
        Weapon_States = new List<WeaponState>();
        Weapon_States.Add(new Weapon_Carry("W_Carry")
        {
            Assigned_SM = this,
            InputController = this.InputDriver,
        });
        Weapon_States.Add(new Weapon_Aim("W_Aim")
        {
            Assigned_SM = this,
            InputController = this.InputDriver,
        });
        Weapon_States.Add(new Weapon_Primary("W_Primary")
        {
            Assigned_SM = this,
            InputController = this.InputDriver,
        });
        Weapon_States.Add(new Weapon_Recharge("W_Recharge")
        {
            Assigned_SM = this,
            InputController = this.InputDriver,
        });
        #endregion

        InputDriver.Get_PrimaryAction.performed += OnPrimaryDown;
        InputDriver.Get_PrimaryAction.canceled += OnPrimaryUp;

        OnAim = new UnityEvent();
        OnRelease = new UnityEvent();
        changeState(Weapon_States.FirstOrDefault(c => c.ID == "W_Carry"));
    }

    public void OnPrimaryDown(InputAction.CallbackContext Context)
    {
        if (getCurrentStateName() != "W_Carry") return;
        //If Weapon Available -> Begin Aiming
        if (AssignedWeapon == null) return;
        //Fire/Invoke Tied Portions -> Player State Machine 
        changeState(Weapon_States.FirstOrDefault(c => c.ID == "W_Aim"));
        OnAim.Invoke();
    }

    public void OnPrimaryUp(InputAction.CallbackContext Context)
    {
        if (getCurrentStateName() != "W_Aim") return;
        if (AssignedWeapon == null) return;
        changeState(Weapon_States.FirstOrDefault(c => c.ID == "W_Primary"));
        OnRelease.Invoke();
    }

    public float ClearWeapon()
    {
        var timer = AssignedWeapon.RechargeTarget;
        AssignedWeapon = null;
        return timer;
    }

    public void IterateWeapon()
    {
        if (WeaponIndex >= WeaponList.Count - 1) WeaponIndex = 0; else { WeaponIndex++; }
        AssignedWeapon = WeaponList[WeaponIndex];
        AssignedWeapon.transform.position = Controller.transform.position;
        AssignedWeapon.gameObject.SetActive(true);
        changeState(Weapon_States.FirstOrDefault(c => c.ID == "W_Carry"));
    }
}
#endregion
