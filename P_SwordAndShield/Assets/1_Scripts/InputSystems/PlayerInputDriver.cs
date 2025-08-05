using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class PlayerInputDriver
{
    public enum ControlScheme
    {
        Keyboard,
        Gamepad
    }

    public static ControlScheme CurrentControlScheme { get; private set; }

    //Subscribe through the Player Controller ---
    private InputSystem_Actions Input_Controller;
    public InputSystem_Actions Get_Input => Input_Controller;

    private InputAction Movement;
    public InputAction Get_Movement => Movement;

    private InputAction Weapon;
    public InputAction Get_Weapon => Weapon;

    private InputAction Boost;
    public InputAction Get_Boost => Boost;

    private InputAction Primary_Action;
    public InputAction Get_PrimaryAction => Primary_Action;

    private InputAction Secondary_Action;
    public InputAction Get_SecondaryAction => Secondary_Action;

    private InputAction Dive;
    public InputAction Get_Dive => Dive;

    public PlayerInputDriver()
    {
        Input_Controller = new InputSystem_Actions();
        Input_Controller.GameStateActive.Enable();

        Movement = Input_Controller.GameStateActive.Move;

        //For Controlling the Scheme ---
        CurrentControlScheme = ControlScheme.Keyboard;
        Movement.performed += Set_ControlScheme;

        Weapon = Input_Controller.GameStateActive.Weapon;
        Primary_Action = Input_Controller.GameStateActive.Attack;
        Secondary_Action = Input_Controller.GameStateActive.Shield;
        Boost = Input_Controller.GameStateActive.Boost;
        Dive = Input_Controller.GameStateActive.Dive;
    }

    public void OnEnable()
    {
        Movement.Enable();
        Weapon.Enable();
        Primary_Action.Enable();
        Secondary_Action.Enable();
        Boost.Enable();
        Dive.Enable();
    }

    public void OnDisable()
    {
        Movement.Disable();
        Weapon.Disable();
        Boost.Disable();
        Primary_Action.Disable();
        Secondary_Action.Disable();
        Dive.Enable();
    }

    protected void OnApplicationQuit()
    {
        Movement.Dispose();
        Weapon.Dispose();
        Boost.Dispose();
        Primary_Action.Dispose();
        Secondary_Action.Dispose();
        Dive.Dispose();
    }

    // Add cleanup
    protected void OnDestroy()
    {
        Movement.Dispose();
        Weapon.Dispose();
        Boost.Dispose();
        Primary_Action.Dispose();
        Secondary_Action.Dispose();
        Dive.Dispose();
    }

    private void Set_ControlScheme(InputAction.CallbackContext Context)
    {
        if (Context.control.device is Keyboard)
        {
            CurrentControlScheme = ControlScheme.Keyboard;
        }
        else if (Context.control.device is Gamepad)
        {
            CurrentControlScheme = ControlScheme.Gamepad;
        }
    }
}