using UnityEngine;
using System.Collections.Generic;
public abstract class DB_ScriptableBase : ScriptableObject
{
    public abstract void Initialize();
    public abstract int GetEntryCount();
}

[System.Serializable]
public abstract class DB_Entry<T>
{
    [SerializeField] private T ID;
    public virtual T Get_ID => ID;
}


#region Entry Keys
public enum Character
{
    Knight,
    RedKnight,
}

#endregion

#region Entry Types

[System.Serializable]
public class CharacterEntry : DB_Entry<Character>
{
    [field: SerializeField] public float MaxSpeed { get; private set; }
    [field: SerializeField] public float Acceleration { get; private set; }
    [field: SerializeField] public int BaseHealth { get; private set; }
    [field: SerializeField] public int BaseArmor { get; private set; }
    [field: SerializeField] public AnimationClip Idle { get; private set; }
    [field: SerializeField] public AnimationClip Running { get; private set; }
    [field: SerializeField] public AnimationClip Action_1 { get; private set; }
    [field: SerializeField] public AnimationClip Stunned { get; private set; }
    [field: SerializeField] public List<Weapon> WeaponList { get; private set; }

    [Header("LAYERS")]
    [Tooltip("Set this to the layer your player is on")]
    public LayerMask PlayerLayer;

    [Tooltip("The pace at which the player comes to a stop")]
    public float GroundDeceleration = 60;

    [Tooltip("Deceleration in air only after stopping input mid-air")]
    public float AirDeceleration = 30;

    [Tooltip("A constant downward force applied while grounded. Helps on slopes"), Range(0f, -10f)]
    public float GroundingForce = -1.5f;

    [Tooltip("The detection distance for grounding and roof detection"), Range(0f, 0.5f)]
    public float GrounderDistance = 0.05f;

    [Header("JUMP")]
    [Tooltip("The immediate velocity applied when jumping")]
    public float JumpPower = 36;

    [Tooltip("The maximum vertical movement speed")]
    public float MaxFallSpeed = 40;

    [Tooltip("The player's capacity to gain fall speed. a.k.a. In Air Gravity")]
    public float FallAcceleration = 110;

    [Tooltip("The gravity multiplier added when jump is released early")]
    public float JumpEndEarlyGravityModifier = 3;

    [Tooltip("The time before coyote jump becomes unusable. Coyote jump allows jump to execute even after leaving a ledge")]
    public float CoyoteTime = .15f;

    [Tooltip("The amount of time we buffer a jump. This allows jump input before actually hitting the ground")]
    public float JumpBuffer = .2f;

    [field: SerializeField] public int JumpCount { get; private set; }
    [field: SerializeField] public int DodgeCount { get; private set; }
}

#endregion