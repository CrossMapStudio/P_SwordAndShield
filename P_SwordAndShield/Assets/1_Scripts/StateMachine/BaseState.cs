using UnityEngine;

public abstract class BaseState
{
    public BaseState(string ID)
    {
        this.ID = ID;
    }
    public string ID { get; private set; }
    public abstract void onEnter();
    public abstract void onUpdate();
    public abstract void onFixedUpdate();
    public abstract void onLateUpdate();
    public abstract void onExit();
    public abstract void onInactiveUpdate();
    public abstract bool checkValid();
}

public abstract class PlayerState : BaseState
{
    public virtual PlayerStateMachine Assigned_SM { get; set; }
    public PlayerState(string ID) : base(ID) { }
    internal PlayerInputDriver InputController { get; set; }
    internal Rigidbody2D PlayerRigidBody { get; set; }
    internal CapsuleCollider2D PlayerCapsuleCollider { get; set; }
    internal SpriteRenderer PlayerSpriteRenderer { get; set; }
    internal Animator PlayerSpriteAnimator { get; set; }
}

public abstract class WeaponState : BaseState
{
    public virtual WeaponStateMachine Assigned_SM { get; set; }
    public WeaponState(string ID) : base(ID) { }
    internal PlayerInputDriver InputController { get; set; }
    internal Rigidbody2D WeaponRigidBody2D { get; set; }
    internal SpriteRenderer WeaponSpriteRenderer { get; set; }
}
