using UnityEngine;

public class Weapon_Sword : Weapon
{
    [SerializeField] private int Damage;
    [SerializeField] private float Speed;
    [SerializeField] private float Radius;
    private Vector2 Target;

    public override void OnPrimaryUse_Release()
    {
        base.OnPrimaryUse_Release();
    }

    public void FixedUpdate()
    {
        if (CurrentState == W_State.Primary)
            WeaponRigidBody.MovePosition(WeaponRigidBody.position + (Vector2)transform.right * Speed * Time.fixedDeltaTime);
    }

    public override void OnTriggerEnter2D(Collider2D Source)
    {
        base.OnTriggerEnter2D(Source);
        if (Source.tag == "Entity")
        {
            Source.TryGetComponent<Entity>(out var entity);
            if (entity == null) return;
            entity.TakeDamage(Damage);
        }
    }
}
