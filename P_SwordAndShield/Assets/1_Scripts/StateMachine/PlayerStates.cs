using UnityEngine;
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.InputSystem;

public class Player_Hold : PlayerState
{
    public Player_Hold(string ID) : base(ID) { }
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

    public override void onUpdate()
    {

    }

    public override void onFixedUpdate()
    {

    }

    public override void onLateUpdate()
    {

    }

    public override void onInactiveUpdate()
    {

    }
}
public class Player_Movement : PlayerState
{
    public Player_Movement(string ID) : base(ID) { }
    public override PlayerStateMachine Assigned_SM
    {
        get => base.Assigned_SM;
        set
        {
            base.Assigned_SM = value;
            //Acceleration = ClientGameController.Controller.Characters.GetCharacterByID(Assigned_SM.CharacterDB_ID).Acceleration;
            //MaxSpeed = ClientGameController.Controller.Characters.GetCharacterByID(Assigned_SM.CharacterDB_ID).MaxSpeed;
        }
    }

    public override bool checkValid()
    {
        return true;
    }

    public override void onEnter()
    {
        Assigned_SM.InputDriver.Get_Boost.performed += Assigned_SM.Controller.ApplyJump;
        Assigned_SM.InputDriver.Get_SecondaryAction.performed += Assigned_SM.Controller.ApplyShield;
    }

    public override void onExit()
    {
        Assigned_SM.InputDriver.Get_Boost.performed -= Assigned_SM.Controller.ApplyJump;
        Assigned_SM.InputDriver.Get_SecondaryAction.performed -= Assigned_SM.Controller.ApplyShield;
    }

    public override void onFixedUpdate()
    {
        Assigned_SM.Controller.ApplyBaseMovement.Invoke();
    }

    public override void onInactiveUpdate()
    {

    }

    public override void onLateUpdate()
    {

    }

    public override void onUpdate()
    {
        Vector2 Input = InputController.Get_Movement.ReadValue<Vector2>().normalized;
        Input.y = 0;
        Assigned_SM.PlayerInput = Input;
        #region Animator
        AnimatorStateInfo CurrentClipState = PlayerSpriteAnimator.GetCurrentAnimatorStateInfo(0);
       
        //If the Player is not Grounded ->
        if (!Assigned_SM.Controller._PlayerGrounded && !CurrentClipState.IsName("Action_1"))
        {
            PlayerSpriteAnimator.Play("Action_1");
        }
        else if (Assigned_SM.Controller._PlayerGrounded)
        {
            //If the Player is Grounded ->
            if (Assigned_SM.PlayerInput != Vector2.zero && !CurrentClipState.IsName("Run"))
                PlayerSpriteAnimator.Play("Run");
            else if (Assigned_SM.PlayerInput == Vector2.zero && !CurrentClipState.IsName("Idle"))
                PlayerSpriteAnimator.Play("Idle");
            #endregion
        }

        if (Assigned_SM.PlayerInput.x == 0)
        {
            return;
        }

        PlayerSpriteRenderer.flipX = Assigned_SM.PlayerInput.x > 0 ? false : true;
    }
}
public class Player_Shield : PlayerState
{
    public Player_Shield(string ID) : base(ID) { }
    public override bool checkValid()
    {
        return true;
    }

    public override void onEnter()
    {
        Debug.Log("Player Entered Shield State");
        Assigned_SM.InputDriver.Get_SecondaryAction.canceled += Assigned_SM.Controller.ReleaseShield;
    }

    public override void onExit()
    {
        Debug.Log("Player Exited Shield State");
        Assigned_SM.InputDriver.Get_SecondaryAction.canceled -= Assigned_SM.Controller.ReleaseShield;
    }

    public override void onUpdate()
    {
        Vector2 Input = InputController.Get_Movement.ReadValue<Vector2>().normalized;
        Input.y = 0;
        Assigned_SM.PlayerInput = Input;
        if (Mathf.Abs(Input.x) > 0) Assigned_SM.Controller.ApplyDodge?.Invoke();
    }
    public override void onFixedUpdate()
    {
        Assigned_SM.Controller.ApplyBaseMovement.Invoke();
    }
    public override void onLateUpdate()
    {

    }
    public override void onInactiveUpdate()
    {

    }
}
public class Player_Dodge : PlayerState
{
    private Vector2 DodgeDirection, CurrentMovement;
    private float DodgeDuration = .35f;
    private float DodgeSpeed = 15f, DodgeCurrentSpeed;
    private float DodgeDeceleration = 15f;
    private CancellationTokenSource Token;

    public Player_Dodge(string ID) : base(ID) { }
    public override bool checkValid()
    {
        if (Assigned_SM.PlayerInput != Vector2.zero)
            return true;
        return false;
    }

    public override void onEnter()
    {
        DodgeCurrentSpeed = DodgeSpeed;
        DodgeDirection = Assigned_SM.PlayerInput;
        PlayerSpriteAnimator.Play("Action_1");

        Token = new CancellationTokenSource();
        _ = StartDodgeTimer(Token);
    }

    public override void onExit()
    {
        Assigned_SM.Controller.ResetVelocity?.Invoke();
        if (Token == null) return;
        Token.Cancel();
        Token = null;
    }

    public override void onUpdate()
    {
        DodgeCurrentSpeed = Mathf.Lerp(DodgeCurrentSpeed, 0f, DodgeDeceleration * Time.deltaTime);
        CurrentMovement = DodgeDirection.normalized * DodgeCurrentSpeed;
    }

    public override void onFixedUpdate()
    {
        PlayerRigidBody.MovePosition((Vector2)PlayerRigidBody.transform.position + CurrentMovement * Time.fixedDeltaTime);
    }

    public override void onLateUpdate()
    {
        
    }

    public override void onInactiveUpdate()
    {

    }

    private async UniTask StartDodgeTimer(CancellationTokenSource _Token)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(DodgeDuration), cancellationToken: _Token.Token);
        Token = null;
        //Completed Dodge ->
        Assigned_SM.changeState(Assigned_SM.Player_States.FirstOrDefault(c => c.ID == "P_Movement"));
    }
}
public class Player_Hit : PlayerState
{
    public Player_Hit(string ID) : base(ID) { }
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

    public override void onUpdate()
    {

    }

    public override void onFixedUpdate()
    {

    }

    public override void onLateUpdate()
    {

    }

    public override void onInactiveUpdate()
    {

    }
}
public class Player_Dead : PlayerState
{
    public Player_Dead(string ID) : base(ID) { }
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

    public override void onUpdate()
    {

    }

    public override void onFixedUpdate()
    {

    }

    public override void onLateUpdate()
    {

    }

    public override void onInactiveUpdate()
    {

    }
}
public class Player_Stunned : PlayerState
{
    private float StunDuration = 1.25f;
    private CancellationTokenSource Token;

    public Player_Stunned(string ID) : base(ID) { }
    public override bool checkValid()
    {
        return true;
    }

    public override void onEnter()
    {
        PlayerSpriteAnimator.Play("Stun");
        PlayerSpriteRenderer.color = Color.red;
        Token = new CancellationTokenSource();
        _ = StartStunnedTimer(Token);
    }

    public override void onExit()
    {
        PlayerSpriteRenderer.color = Color.white;
        if (Token == null) return;
        Token.Cancel();
        Token = null;
    }

    public override void onUpdate()
    {

    }

    public override void onFixedUpdate()
    {

    }

    public override void onLateUpdate()
    {

    }

    public override void onInactiveUpdate()
    {

    }

    private async UniTask StartStunnedTimer(CancellationTokenSource _Token)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(StunDuration), cancellationToken: _Token.Token);
        Token = null;
        //Completed Stun State ->
        Assigned_SM.changeState(Assigned_SM.Player_States.FirstOrDefault(c => c.ID == "P_Movement"));
    }
}