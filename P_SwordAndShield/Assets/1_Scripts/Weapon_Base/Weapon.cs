using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public Entity.Team AssignedTeam { get; set; }
    [field: SerializeField] public bool TargetTeam { get; private set; }
    [field: SerializeField] public Rigidbody2D WeaponRigidBody { get; private set; }
    [field: SerializeField] public SpriteRenderer WeaponSpriteRenderer { get; private set; }
    [field: SerializeField] public float RechargeTarget { get; private set; }

    public enum W_State
    {
        Carry,
        Primary,
        Exit,
    }
    public W_State CurrentState { get; private set; }

    //On Recharge
    public virtual void OnCarry_Enter()
    {
        CurrentState = W_State.Carry;
    }

    public virtual void OnCarry_Update()
    {

    }

    public virtual void OnPrimaryUse_Holding()
    {

    }

    public virtual void OnSecondaryUse_Holding()
    {

    }

    public virtual void OnPrimaryUse_Release()
    {
        CurrentState = W_State.Primary;
    }

    public virtual void OnSecondaryUse_Release()
    {

    }
    public virtual void OnExit()
    {
        CurrentState = W_State.Exit;
    }

    public virtual void OnTriggerEnter2D(Collider2D Source) { }
}


