using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class PlayerInputDriver
{
    //Subscribe through the Player Controller ---
    private InputSystem_Actions Input_Controller;
    public InputSystem_Actions Get_Input => Input_Controller;

    private InputAction Movement;
    public InputAction Get_Movement => Movement;

    private InputAction Boost;
    public InputAction Get_Boost => Boost;

    private InputAction Primary_Action;
    public InputAction Get_PrimaryAction => Primary_Action;

    public InputAction Secondary_Action;
    public InputAction Get_SecondaryAction => Secondary_Action;

    public PlayerInputDriver()
    {
        Input_Controller = new InputSystem_Actions();
        Input_Controller.GameStateActive.Enable();

        Movement = Input_Controller.GameStateActive.Move;
        Primary_Action = Input_Controller.GameStateActive.Attack;
        Secondary_Action = Input_Controller.GameStateActive.Shield;
        Boost = Input_Controller.GameStateActive.Boost;
    }

    public void OnEnable()
    {
        Movement.Enable();
        Primary_Action.Enable();
        Secondary_Action.Enable();
        Boost.Enable();
    }

    public void OnDisable()
    {
        Movement.Disable();
        Boost.Disable();
        Primary_Action.Disable();
        Secondary_Action.Disable();
    }

    protected void OnApplicationQuit()
    {
        Movement.Dispose();
        Boost.Dispose();
        Primary_Action.Dispose();
        Secondary_Action.Dispose();
    }

    // Add cleanup
    protected void OnDestroy()
    {
        Movement.Dispose();
        Boost.Dispose();
        Primary_Action.Dispose();
        Secondary_Action.Dispose();
    }
}