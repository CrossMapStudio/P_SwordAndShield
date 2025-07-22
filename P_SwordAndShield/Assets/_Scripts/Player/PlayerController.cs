using UnityEngine;
using System.Collections.Generic;
[RequireComponent(typeof(PlayerInputDriver))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private PlayerInputDriver InputController;
    public Rigidbody2D PlayerRB { get; private set; }
    public PlayerInputDriver Get_InputController => InputController;
    private StateMachine Player_SM;
    private List<BaseState> Player_States;

    [field: SerializeField] public SpriteRenderer SpriteRend { get; private set; } 
    [field: SerializeField] public Animator SpriteAnimator { get; private set; }

    private enum PlayerStateIndexMap
    {
        PlayerMovement = 0
    }
    private PlayerStateIndexMap StateIndex = PlayerStateIndexMap.PlayerMovement;

    //ID for Selected Character:
    [field: SerializeField] public Character ID { get; private set; }
    
    public void Start()
    {
        InputController = GetComponent<PlayerInputDriver>();
        PlayerRB = GetComponent<Rigidbody2D>();

        Player_States = new List<BaseState>();
        Player_States.Add(new Player_Movement());
        Player_SM = new StateMachine(Player_States[(int)StateIndex], this);

        //Assign SM to All Available States :::
        Player_States[(int)StateIndex].Assigned_SM = Player_SM;
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
}
