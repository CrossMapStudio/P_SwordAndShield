using UnityEngine;

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
    Goblin
}

#endregion

#region Entry Types

[System.Serializable]
public class CharacterEntry : DB_Entry<Character>
{
    [field: SerializeField] public float MaxSpeed { get; private set; }
    [field: SerializeField] public float Acceleration { get; private set; }
    [field: SerializeField] public AnimationClip Idle { get; private set; }
    [field: SerializeField] public AnimationClip Running { get; private set; }
}

#endregion