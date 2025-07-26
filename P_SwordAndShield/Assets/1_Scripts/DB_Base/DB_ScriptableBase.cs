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
    [field: SerializeField] public AnimationClip Dodging { get; private set; }
    [field: SerializeField] public AnimationClip Stunned { get; private set; }
    [field: SerializeField] public List<Weapon> WeaponList { get; private set; }
}

#endregion