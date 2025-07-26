using UnityEngine;
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
public class Weapon_Carry : WeaponState
{
    public Weapon_Carry(string ID) : base(ID) { }
    public override bool checkValid()
    {
        return true;
    }

    public override void onEnter()
    {

    }

    public override void onExit()
    {

    }

    public override void onFixedUpdate()
    {

    }

    public override void onInactiveUpdate()
    {

    }

    public override void onLateUpdate()
    {
        var target = Vector3.Lerp(Assigned_SM.AssignedWeapon.transform.position, Assigned_SM.Controller.transform.position + new Vector3(0f, .55f, 0f), Time.deltaTime * 50f);
        Assigned_SM.AssignedWeapon.WeaponRigidBody.MovePosition(target);
    }

    public override void onUpdate()
    {

    }
}
public class Weapon_Aim : WeaponState
{
    public Weapon_Aim(string ID) : base(ID) { }
    public override bool checkValid()
    {
        return true;
    }

    public override void onEnter()
    {

    }

    public override void onExit()
    {

    }

    public override void onFixedUpdate()
    {

    }

    public override void onInactiveUpdate()
    {

    }

    public override void onLateUpdate()
    {
        var target = Vector3.Lerp(Assigned_SM.AssignedWeapon.transform.position, Assigned_SM.Controller.transform.position + new Vector3(0f, .15f, 0f), Time.deltaTime * 50f);
        Assigned_SM.AssignedWeapon.WeaponRigidBody.MovePosition(target);
    }

    public override void onUpdate()
    {

    }
}
public class Weapon_Throw : WeaponState
{
    public Weapon_Throw(string ID) : base(ID) { }
    public override bool checkValid()
    {
        return true;
    }

    public override void onEnter()
    {

    }

    public override void onExit()
    {

    }

    public override void onFixedUpdate()
    {

    }

    public override void onInactiveUpdate()
    {

    }

    public override void onLateUpdate()
    {

    }

    public override void onUpdate()
    {

    }
}
public class Weapon_Recharge : WeaponState
{
    public Weapon_Recharge(string ID) : base(ID) { }
    public override bool checkValid()
    {
        return true;
    }

    public override void onEnter()
    {

    }

    public override void onExit()
    {

    }

    public override void onFixedUpdate()
    {

    }

    public override void onInactiveUpdate()
    {

    }

    public override void onLateUpdate()
    {

    }

    public override void onUpdate()
    {

    }
}
