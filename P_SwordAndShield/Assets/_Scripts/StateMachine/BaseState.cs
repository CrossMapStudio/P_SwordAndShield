using UnityEngine;

public abstract class BaseState
{
    public virtual StateMachine Assigned_SM { get; set; }
    public string ID;
    public abstract void onEnter();
    public abstract void onUpdate();
    public abstract void onFixedUpdate();
    public abstract void onLateUpdate();
    public abstract void onExit();
    public abstract void onInactiveUpdate();
    public abstract bool checkValid();
}
