using UnityEngine;
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

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
    private Vector2 CurrentMovement = Vector2.zero;
    private float Acceleration = 15f, MaxSpeed = 5f;
    public Player_Movement(string ID) : base(ID) { }
    public override PlayerStateMachine Assigned_SM { get => base.Assigned_SM; 
        set 
        {
            base.Assigned_SM = value;
            Acceleration = ClientGameController.Controller.Characters.GetCharacterByID(Assigned_SM.CharacterDB_ID).Acceleration;
            MaxSpeed = ClientGameController.Controller.Characters.GetCharacterByID(Assigned_SM.CharacterDB_ID).MaxSpeed;
        } 
    }

    public override bool checkValid()
    {
        return true;
    }

    public override void onEnter()
    {
        CurrentMovement = Vector2.zero;
    }

    public override void onExit()
    {
        
    }

    public override void onFixedUpdate()
    {
        PlayerRigidBody.MovePosition((Vector2)PlayerRigidBody.transform.position + CurrentMovement * Time.fixedDeltaTime);
    }

    public override void onInactiveUpdate()
    {
        
    }

    public override void onLateUpdate()
    {
        
    }

    public override void onUpdate()
    {
        Assigned_SM.PlayerInput = InputController.Get_Movement.ReadValue<Vector2>().normalized;
        CurrentMovement = Vector2.Lerp(CurrentMovement, Assigned_SM.PlayerInput * MaxSpeed, Acceleration * Time.deltaTime);
        #region Animator
        AnimatorStateInfo CurrentClipState = PlayerSpriteAnimator.GetCurrentAnimatorStateInfo(0);
        if (Assigned_SM.PlayerInput != Vector2.zero && !CurrentClipState.IsName("Run"))
            PlayerSpriteAnimator.Play("Run");
        else if (Assigned_SM.PlayerInput == Vector2.zero && !CurrentClipState.IsName("Idle"))
            PlayerSpriteAnimator.Play("Idle");
        #endregion
        if (Assigned_SM.PlayerInput.x == 0)
        {
            return;
        }
        PlayerSpriteRenderer.flipX = Assigned_SM.PlayerInput.x > 0 ? false : true;
    }
}
public class Player_Dodge : PlayerState
{
    private Vector2 DodgeDirection, CurrentMovement;
    private float DodgeDuration = .5f;
    private float DodgeSpeed = 8f, DodgeCurrentSpeed;
    private float DodgeAcceleration = 5f;
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
        PlayerSpriteAnimator.Play("Dodge");

        Token = new CancellationTokenSource();
        _ = StartDodgeTimer(Token);

        Assigned_SM.Controller.OnEnvironmentCollisionEnter.AddListener(EnterStun_OnEnvironmentCollision);
    }

    public override void onExit()
    {
        Assigned_SM.Controller.OnEnvironmentCollisionEnter.RemoveListener(EnterStun_OnEnvironmentCollision);
        if (Token == null) return;
        Token.Cancel();
        Token = null;
    }

    public override void onUpdate()
    {
        DodgeCurrentSpeed = Mathf.Lerp(DodgeCurrentSpeed, 0f, DodgeAcceleration * Time.deltaTime);
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

    private void EnterStun_OnEnvironmentCollision()
    {
        Assigned_SM.changeState(Assigned_SM.Player_States.FirstOrDefault(c => c.ID == "P_Stunned"));
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