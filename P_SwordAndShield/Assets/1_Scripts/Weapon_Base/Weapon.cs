using UnityEngine;
using System;

public abstract class Weapon : MonoBehaviour
{
    public Entity.Team AssignedTeam { get; set; }
    [field: SerializeField] public bool TargetTeam { get; private set; }
    [field: SerializeField] public Rigidbody2D WeaponRigidBody { get; private set; }
    [field: SerializeField] public SpriteRenderer WeaponSpriteRenderer { get; private set; }

    public abstract void OnEnter();
    public abstract void OnPrimaryDown();
    public abstract void OnPrimaryUp();
    public abstract void OnReloadStart(Action Callback);
    public abstract void OnReloadComplete();

    public virtual void LateUpdate()
    {
        /*
        var target = Vector3.Lerp(transform.position, Controller.transform.position + new Vector3(0f, 1f, 0f), Time.deltaTime * 50f);
        Assigned_SM.AssignedWeapon.WeaponRigidBody.MovePosition(target);
        */

        /*
        if (PlayerInputDriver.CurrentControlScheme == PlayerInputDriver.ControlScheme.Keyboard)
        {

        }
        else
        {

        }
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        WeaponRigidBody.MoveRotation(angle);
        */
    }
}


