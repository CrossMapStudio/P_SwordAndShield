using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputDriver : MonoBehaviour
{
    #region Inputs
    //Subscribe through the Player Controller ---
    private InputSystem_Actions Input_Controller;
    public InputSystem_Actions Get_Input => Input_Controller;
    public static PlayerInputDriver Input_Driver;

    private static InputActionMap ActiveMap;

    #region Movement
    private InputAction Movement;
    public InputAction Get_Movement => Movement;

    private static InputAction Boost;
    public static InputAction Get_Boost => Boost;

    private static InputAction Slide;
    public static InputAction Get_Slide => Slide;

    private static InputAction Sprint;
    public static InputAction Get_Sprint => Sprint;

    private static InputAction Rotation;
    public static InputAction Get_Rotation => Rotation;
    #endregion
    
    #region Interact/Inventory
    private static InputAction Interact;
    public static InputAction Get_Interact => Interact;
    #endregion

    #region Menus
    private static InputAction Inventory;
    public static InputAction Get_Inventory => Inventory;

    public static InputAction Pause;
    public static InputAction Get_Pause => Pause;
    #endregion

    #region Combat
    private static InputAction Primary_Action;
    public static InputAction Get_PrimaryAction => Primary_Action;

    public static InputAction Secondary_Action;
    public static InputAction Get_SecondaryAction => Secondary_Action;

    public static InputAction Reload;
    public static InputAction Get_Reload => Reload;

    private static InputAction Primary_Spell;
    public static InputAction Get_PrimarySpell => Primary_Spell;

    public static InputAction Seconday_Spell;
    public static InputAction Get_SecondarySpell => Seconday_Spell;

    private static InputAction Primary_Equipment;
    public static InputAction Get_PrimaryEquipment => Primary_Equipment;

    public static InputAction Seconday_Equipment;
    public static InputAction Get_SecondaryEquipment => Seconday_Equipment;

    public static InputAction Infuse_Weapon;
    public static InputAction Get_InfuseWeapon => Infuse_Weapon;

    public static InputAction Swap_Spells;
    public static InputAction Get_SwapSpells => Swap_Spells;

    public static InputAction Swap_Equipment;
    public static InputAction Get_SwapEquipment => Swap_Equipment;

    private static InputAction Health_Recharge;
    public static InputAction Get_HealthRecharge => Health_Recharge;
    #endregion

    #region Cursor Mode
    //Cursor Modes
    public static InputAction Cursor_Primary;
    public static InputAction Get_CursorPrimaryAction => Cursor_Primary;

    public static InputAction Cursor_Secondary;
    public static InputAction Get_CursorSecondaryAction => Cursor_Secondary;

    public static InputAction Cursor_Exit;
    public static InputAction Get_CursorExit => Cursor_Exit;
    #endregion
    #endregion

    public enum Input_MapID
    {
        active,
        cursor,
        menu
    }

    private Dictionary<Input_MapID, InputActionMap> ActionMapDictionary;
    public Dictionary<Input_MapID, InputActionMap> Get_ActionMapDictionary => ActionMapDictionary;

    protected void Awake()
    {
        //Create the Input System ---
        Input_Driver = this;
        Input_Controller = new InputSystem_Actions();
        Input_Controller.GameStateActive.Enable();

        ActionMapDictionary = new Dictionary<Input_MapID, InputActionMap>();
        ActionMapDictionary.Add(Input_MapID.active, Input_Controller.GameStateActive);

        //Set all Callbacks
        Movement = Input_Controller.GameStateActive.Move;
        Primary_Action = Input_Controller.GameStateActive.Attack;
        Secondary_Action = Input_Controller.GameStateActive.Attack2;
        Boost = Input_Controller.GameStateActive.Boost;
        Reload = Input_Controller.GameStateActive.Reload;
        Interact = Input_Controller.GameStateActive.Interact;
        //Slide = Input_Controller.GameStateActive.Slide;
        /*
        Crouch = Input_Controller.GameStateActive.Crouch;
        Slide = Input_Controller.GameStateActive.Slide;
        Rotation = Input_Controller.GameStateActive.PlayerRotation; 
        Interact = Input_Controller.GameStateActive.Interact;
        Inventory = Input_Controller.GameStateMenu.Inventory_Toggle;
        Pause = Input_Controller.GameStateMenu.Pause_Toggle;
        */


        /*
        Primary_Action = Input_Controller.GameStateActive.PrimaryAction;
        Secondary_Action = Input_Controller.GameStateActive.SecondaryAction;
        Reload = Input_Controller.GameStateActive.Reload;
        Primary_Spell = Input_Controller.GameStateActive.PrimarySpell;
        Seconday_Spell = Input_Controller.GameStateActive.SecondarySpell;
        Primary_Equipment = Input_Controller.GameStateActive.PrimaryEquipment;
        Seconday_Equipment = Input_Controller.GameStateActive.SecondaryEquipment;
        Infuse_Weapon = Input_Controller.GameStateActive.WeaponInfusion;
        Swap_Spells = Input_Controller.GameStateActive.SwapSpells;
        Swap_Equipment = Input_Controller.GameStateActive.SwapEquipment;
        Health_Recharge = Input_Controller.GameStateActive.HealthRecharge;

        Cursor_Primary = Input_Controller.GameStateCursor.PrimaryAction;
        Cursor_Secondary = Input_Controller.GameStateCursor.SecondaryAction;
        Cursor_Exit = Input_Controller.GameStateCursor.Interact;

        ActiveMap = Input_Controller.GameStateActive;
        */
    }

    public void OnEnable()
    {
        //Enable the Input ---
        Input_Controller.GameStateActive.Enable();
        Movement.Enable();
        Primary_Action.Enable();
        Secondary_Action.Enable();
        Boost.Enable();
        Reload.Enable();
        Interact.Enable();
        //Slide.Enable();

        /*
        Crouch.Enable();
        Slide.Enable();
        Rotation.Enable();
        Sprint.Enable();
        Interact.Enable();
        Pause.Enable();
        Inventory.Enable();

        Primary_Action.Enable();
        Secondary_Action.Enable();
        Reload.Enable();
        Primary_Spell.Enable();
        Seconday_Spell.Enable();
        Primary_Equipment.Enable();
        Seconday_Equipment.Enable();
        Infuse_Weapon.Enable();
        Swap_Spells.Enable();
        Swap_Equipment.Enable();
        Health_Recharge.Enable();

        Cursor_Primary.Enable();
        Cursor_Secondary.Enable();
        Cursor_Exit.Enable();
        */
    }

    public void OnDisable()
    {
        //Enable the Input ---
        Input_Controller.GameStateActive.Disable();
        Movement.Disable();
        Boost.Disable();
        Reload.Disable();
        Primary_Action.Disable();
        Secondary_Action.Disable();
        Interact.Disable();
        //Slide.Disable();
        /*
        Jump.Disable();
        Crouch.Disable();
        Slide.Disable();
        Sprint.Disable();
        Rotation.Disable();
        Interact.Disable();
        Pause.Disable();
        Inventory.Disable();

        Primary_Action.Disable();
        Secondary_Action.Disable();
        Reload.Disable();
        Primary_Spell.Disable();
        Seconday_Spell.Disable();
        Primary_Equipment.Disable();
        Seconday_Equipment.Disable();
        Infuse_Weapon.Disable();
        Swap_Spells.Disable();
        Swap_Equipment.Disable();
        Health_Recharge.Disable();

        Cursor_Primary.Disable();
        Cursor_Secondary.Disable();
        Cursor_Exit.Disable();
        */
    }

    protected void OnApplicationQuit()
    {
        Input_Controller.GameStateActive.Disable();
        Input_Controller.Dispose();
        Movement.Dispose();
        Boost.Dispose();
        Reload.Dispose();
        Primary_Action.Dispose();
        Secondary_Action.Dispose();
        Interact.Dispose();
        //Slide.Dispose();
        /*
        Jump.Dispose();
        Crouch.Dispose();
        Slide.Dispose();
        Sprint.Dispose();
        Rotation.Dispose();
        Interact.Dispose();
        Pause.Dispose();
        Inventory.Dispose();

        Primary_Action.Dispose();
        Secondary_Action.Dispose();
        Reload.Dispose();
        Primary_Spell.Dispose();
        Seconday_Spell.Dispose();
        Primary_Equipment.Dispose();
        Seconday_Equipment.Dispose();
        Infuse_Weapon.Dispose();
        Swap_Spells.Dispose();
        Swap_Equipment.Dispose();
        Health_Recharge.Dispose();

        Cursor_Primary.Dispose();
        Cursor_Secondary.Dispose();
        Cursor_Exit.Dispose();
        */
    }

    // Add cleanup
    protected void OnDestroy()
    {
        Input_Controller.GameStateActive.Disable();
        Input_Controller.Dispose();
        Movement.Dispose();
        Boost.Dispose();
        Reload.Dispose();
        Primary_Action.Dispose();
        Secondary_Action.Dispose();
        Interact.Dispose();
    }

    public void Update()
    {

    }

    /*
    public static void ChangeInputMap(Input_MapID ID)
    {
        var temp = ActiveMap;
        temp.Disable();

        ActiveMap = Input_Driver.Get_ActionMapDictionary[ID];
        ActiveMap.Enable();
    }

    public static void ToggleInput_Cursor(bool toggleValue)
    {
        if (toggleValue)
        {
            ActiveMap.Disable();
            //Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            ActiveMap.Enable();
            //Cursor.lockState = CursorLockMode.Locked;
        }
    }
    */
}