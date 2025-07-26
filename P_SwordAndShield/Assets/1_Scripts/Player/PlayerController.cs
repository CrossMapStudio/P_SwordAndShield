using UnityEngine;
using UnityEngine.Events;

public class PlayerController : Entity
{
    private PlayerStateMachine Player_SM;
    public Rigidbody2D PlayerRB { get; private set; }
    [field: SerializeField] public SpriteRenderer SpriteRend { get; private set; }
    [field: SerializeField] public Animator SpriteAnimator { get; private set; }

    //Used for Queued Input on Exit :::
    public PlayerState QueueState { get; private set; }
    //ID for Selected Character:
    [field: SerializeField] public Character ID { get; private set; }
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
        overrideController["Dodge"] = _Character.Dodging;
        overrideController["Stun"] = _Character.Stunned;
        SpriteAnimator.runtimeAnimatorController = overrideController;
        #endregion

        PlayerRB = GetComponent<Rigidbody2D>();
        Player_SM = new PlayerStateMachine(ID, this, PlayerRB, SpriteRend, SpriteAnimator);
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

    //Entity
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
    public void OnDestroy()
    {
        OnDamageArmor.RemoveAllListeners();
        OnDamageNoArmor.RemoveAllListeners();
        OnPlayerDead.RemoveAllListeners();
    }
}
