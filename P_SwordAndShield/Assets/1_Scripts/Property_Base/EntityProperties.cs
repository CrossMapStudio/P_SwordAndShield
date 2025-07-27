using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Entity : MonoBehaviour
{
    public enum Team
    {
        Team1,
        Team2,
        Team3,
        Team4
    }

    [field: SerializeField] public Team AssignedTeam { get; private set; }

    public UnityEvent OnDamageArmor { get; set; }
    public UnityEvent OnDamageNoArmor { get; set; }
    public UnityEvent OnPlayerDead { get; set; }
    public UnityEvent OnEnvironmentCollisionEnter { get; set; }
    public int BaseHealth { get; private set; }
    public int BaseArmor { get; private set; }
    protected int CurrentHealth { get; set; }
    protected int CurrentArmor { get; set; }

    public void Awake()
    {
        OnDamageArmor = new UnityEvent();
        OnDamageNoArmor = new UnityEvent();
        OnPlayerDead = new UnityEvent();
        OnEnvironmentCollisionEnter = new UnityEvent();
    }

    public void InitializeEntity(Character ID)
    {
        BaseArmor = CurrentArmor = ClientGameController.Controller.Characters.GetCharacterByID(ID).BaseArmor;
        BaseHealth = CurrentHealth = ClientGameController.Controller.Characters.GetCharacterByID(ID).BaseHealth;

        //Init UI ->
    }

    public void InitializeEntity(int Health, int Armor)
    {
        BaseArmor = CurrentArmor = Armor;
        BaseHealth = CurrentHealth = Health; 
    }

    public abstract void TakeDamage(int _Damage);

    public void OnCollisionEnter2D(Collision2D Source)
    {
        if (Source.collider.tag == "Environment")
        {
            Debug.Log("Hit Environment");
            if (OnEnvironmentCollisionEnter != null)
                OnEnvironmentCollisionEnter.Invoke();
        }
    }

    public void OnRespawn()
    {
        CurrentArmor = BaseArmor;
        CurrentHealth = BaseHealth;

        //Update UI ->
    }
}
