using UnityEngine;

public class Player_Movement : BaseState
{
    private readonly string AnimationHelper_Idle = "Idle";
    private readonly string AnimationHelper_Moving = "Moving";
    private Vector2 CurrentMovement = Vector2.zero;

    private PlayerInputDriver InputController;
    private Rigidbody2D PlayerRB;

    private SpriteRenderer SpriteRend;
    private Animator SpriteAnimator;

    private float Acceleration = 15f, MaxSpeed = 5f;
    private float CurrentSpeed = 0f;
    public override StateMachine Assigned_SM { get => base.Assigned_SM; 
        set 
        { 
            base.Assigned_SM = value;


            if (value.Player == null)
            {
#if UNITY_EDITOR
                Debug.LogError("Player State Machine Does Not Have an Active Player Set.");
#endif
                return;
            }

            InputController = value.Player.Get_InputController;
            PlayerRB = value.Player.PlayerRB;
            SpriteRend = value.Player.SpriteRend;
            SpriteAnimator = value.Player.SpriteAnimator;

            Acceleration = ClientGameController.Controller.Characters.GetCharacterByID(value.Player.ID).Acceleration;
            MaxSpeed = ClientGameController.Controller.Characters.GetCharacterByID(value.Player.ID).MaxSpeed;
        } 
    }

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
        PlayerRB.MovePosition((Vector2)PlayerRB.transform.position + (CurrentMovement.normalized * CurrentSpeed) * Time.fixedDeltaTime);
    }

    public override void onInactiveUpdate()
    {
        
    }

    public override void onLateUpdate()
    {
        
    }

    public override void onUpdate()
    {
        CurrentMovement = InputController.Get_Movement.ReadValue<Vector2>();
        SpriteAnimator.SetBool(AnimationHelper_Moving, CurrentMovement.magnitude > 0);
        CurrentSpeed = Mathf.Lerp(CurrentSpeed, CurrentMovement.magnitude * MaxSpeed, Time.deltaTime * Acceleration);
        if (CurrentMovement.x == 0)
        {
            return;
        }
        SpriteRend.flipX = CurrentMovement.x > 0 ? false : true;
    }
}
