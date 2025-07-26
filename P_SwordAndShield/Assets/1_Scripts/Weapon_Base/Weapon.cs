using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private int Damage;
    [SerializeField] private float ThrowRadius;
    [field: SerializeField] public Rigidbody2D WeaponRigidBody { get; private set; }
    [field: SerializeField] public SpriteRenderer WeaponSpriteRenderer { get; private set; }
    //On Recharge
    public virtual void OnCarry_Enter()
    {

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

    }

    public virtual void OnSecondaryUse_Release()
    {

    }

    private void OnTriggerEnter2D(Collider2D Source)
    {
        if (Source.tag == "Entity")
        {
            Source.TryGetComponent<Entity>(out var entity);
            if (entity == null) return;
            entity.TakeDamage(Damage);
        }
    }
}


