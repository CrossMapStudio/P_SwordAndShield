using UnityEngine;
using System;
using System.Threading;
using Cysharp.Threading.Tasks;

public class Weapon_HoldToFire : Weapon
{
    public override void OnEnter()
    {

    }

    public override void OnPrimaryDown()
    {
        Debug.Log("Weapon Start");       
    }

    public override void OnPrimaryUp()
    {
        Debug.Log("Weapon Stop");
    }

    public override void OnReloadComplete()
    {

    }

    public override void OnReloadStart(Action Callback)
    {

    }
}
