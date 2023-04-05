using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "InputActions", menuName = "ScriptableObjects/SO_InputActions", order = 1)]
public class SO_InputActions : ScriptableObject
{
    [System.Serializable]
   public struct ActionData
   {
       public EA_PlayerInputArguments.EInputType inputType;
       
       public string kmInputBinding;
       public string gpInputBinding;
       //public InputActionType inputActionType;
       //public PlayerInputEventHandler.InputActionValueType inputActionValueType;
       //public ActionData(EA_PlayerInputArguments.EInputType inputType, InputActionType iat, string inputBinding, PlayerInputEventHandler.InputActionValueType inputActionDataValueType)
       public ActionData(EA_PlayerInputArguments.EInputType inputType, string kmInputBinding, string gpInputBinding)
         {
              this.inputType = inputType;
              //inputActionValueType = inputActionDataValueType;
              //inputActionType = iat;
              this.kmInputBinding = kmInputBinding;
              this.gpInputBinding = gpInputBinding;
         }
   }
   
    //[SerializeField]
    // private Dictionary<EA_PlayerInputArguments.EInputType, S_PlayerInputs.InputActionData> _inputActionDatas = new Dictionary<EA_PlayerInputArguments.EInputType, S_PlayerInputs.InputActionData>()
    // {
    //     {EA_PlayerInputArguments.EInputType.Up, new S_PlayerInputs.InputActionData(InputActionType.Button,  "/Keyboard/w", "/<Gamepad>/leftStick/up")},
    //     {EA_PlayerInputArguments.EInputType.Down, new S_PlayerInputs.InputActionData(InputActionType.Button, "/Keyboard/s", "/<Gamepad>/leftStick/down")},
    //     {EA_PlayerInputArguments.EInputType.Left, new S_PlayerInputs.InputActionData(InputActionType.Button, "/Keyboard/a", "/<Gamepad>/leftStick/left")},
    //     {EA_PlayerInputArguments.EInputType.Right, new S_PlayerInputs.InputActionData(InputActionType.Button, "/Keyboard/d", "/<Gamepad>/leftStick/right")},
    //     {EA_PlayerInputArguments.EInputType.LeftArmToggle, new S_PlayerInputs.InputActionData(InputActionType.Button, "/Keyboard/q", "/<Gamepad>/dpad/left")},
    //     {EA_PlayerInputArguments.EInputType.RightArmToggle, new S_PlayerInputs.InputActionData(InputActionType.Button, "/Keyboard/e","/<Gamepad>/dpad/right")},
    //     {EA_PlayerInputArguments.EInputType.LeftArmActivate, new S_PlayerInputs.InputActionData(InputActionType.Button, "/Mouse/leftButton", "/<Gamepad>/leftTrigger")},
    //     {EA_PlayerInputArguments.EInputType.RightArmActivate, new S_PlayerInputs.InputActionData(InputActionType.Button, "/Mouse/rightButton", "/<Gamepad>/rightTrigger")},
    //     {EA_PlayerInputArguments.EInputType.LeftArmActivate2, new S_PlayerInputs.InputActionData(InputActionType.Button, null, "/<Gamepad>/leftShoulder")},
    //     {EA_PlayerInputArguments.EInputType.RightArmActivate2, new S_PlayerInputs.InputActionData(InputActionType.Button, null, "/<Gamepad>/rightShoulder")},
    //     {EA_PlayerInputArguments.EInputType.Jump, new S_PlayerInputs.InputActionData(InputActionType.Button, "/Keyboard/space", "/<Gamepad>/buttonSouth")},
    //     {EA_PlayerInputArguments.EInputType.Crouch, new S_PlayerInputs.InputActionData(InputActionType.Button, "/Keyboard/ctrl", "/<Gamepad>/buttonEast")},
    //     {EA_PlayerInputArguments.EInputType.Interact, new S_PlayerInputs.InputActionData(InputActionType.Button, "/Keyboard/f", "/<Gamepad>/buttonWest")},
    //     {EA_PlayerInputArguments.EInputType.Look, new S_PlayerInputs.InputActionData(InputActionType.Value, "/Mouse/delta","/<Gamepad>/rightStick", PlayerInputEventHandler.InputActionValueType.Vector2)},
    //     {EA_PlayerInputArguments.EInputType.Shift, new S_PlayerInputs.InputActionData(InputActionType.Button, "/Keyboard/shift", null)},
    //     {EA_PlayerInputArguments.EInputType.Pause, new S_PlayerInputs.InputActionData(InputActionType.Button, "/Keyboard/z", "/<Gamepad>/start")},
    // };
    
    [SerializeField]
    private Dictionary<EA_PlayerInputArguments.EInputType, S_PlayerInputs.InputActionData> _inputActionDatas = new Dictionary<EA_PlayerInputArguments.EInputType, S_PlayerInputs.InputActionData>()
        {
            { EA_PlayerInputArguments.EInputType.Up, new S_PlayerInputs.InputActionData(InputActionType.Button, "Up", "Up", "/Keyboard/w", "/<Gamepad>/leftStick/up")},
            {EA_PlayerInputArguments.EInputType.Down, new S_PlayerInputs.InputActionData(InputActionType.Button, "Down", "Down", "/Keyboard/s", "/<Gamepad>/leftStick/down")},
            {EA_PlayerInputArguments.EInputType.Left, new S_PlayerInputs.InputActionData(InputActionType.Button, "Left", "Left", "/Keyboard/a", "/<Gamepad>/leftStick/left")},
            {EA_PlayerInputArguments.EInputType.Right, new S_PlayerInputs.InputActionData(InputActionType.Button, "Right", "Right", "/Keyboard/d", "/<Gamepad>/leftStick/right")},
            {EA_PlayerInputArguments.EInputType.LeftArmToggle, new S_PlayerInputs.InputActionData(InputActionType.Button, "LeftArmToggle", "Left Arm Toggle", "/Keyboard/q", "/<Gamepad>/dpad/left")},
            {EA_PlayerInputArguments.EInputType.RightArmToggle, new S_PlayerInputs.InputActionData(InputActionType.Button, "RightArmToggle", "Right Arm Toggle", "/Keyboard/e","/<Gamepad>/dpad/right")},
            {EA_PlayerInputArguments.EInputType.LeftArmActivate, new S_PlayerInputs.InputActionData(InputActionType.Button, "LeftArmActivate", "Left Arm Activate Ability", "/Mouse/leftButton", "/<Gamepad>/leftTrigger")},
            {EA_PlayerInputArguments.EInputType.RightArmActivate, new S_PlayerInputs.InputActionData(InputActionType.Button, "RightArmActivate", "Right Arm Activate Ability", "/Mouse/rightButton", "/<Gamepad>/rightTrigger")},
            {EA_PlayerInputArguments.EInputType.LeftArmActivate2, new S_PlayerInputs.InputActionData(InputActionType.Button, "LeftArmActivate2", "Left Arm Activate Movement", null, "/<Gamepad>/leftShoulder")},
            {EA_PlayerInputArguments.EInputType.RightArmActivate2, new S_PlayerInputs.InputActionData(InputActionType.Button, "RightArmActivate2", "Right Arm Activate Movement", null, "/<Gamepad>/rightShoulder")},
            {EA_PlayerInputArguments.EInputType.Jump, new S_PlayerInputs.InputActionData(InputActionType.Button, "Jump", "Jump", "/Keyboard/space", "/<Gamepad>/buttonSouth")},
            {EA_PlayerInputArguments.EInputType.Crouch, new S_PlayerInputs.InputActionData(InputActionType.Button, "Crouch", "Crouch", "/Keyboard/ctrl", "/<Gamepad>/buttonEast")},
            {EA_PlayerInputArguments.EInputType.Interact, new S_PlayerInputs.InputActionData(InputActionType.Button, "Interact", "Interact", "/Keyboard/f", "/<Gamepad>/buttonWest")},
            {EA_PlayerInputArguments.EInputType.Look, new S_PlayerInputs.InputActionData(InputActionType.Value, "Look", "Look", "/Mouse/delta","/<Gamepad>/rightStick", PlayerInputEventHandler.InputActionValueType.Vector2)},
            {EA_PlayerInputArguments.EInputType.Shift, new S_PlayerInputs.InputActionData(InputActionType.Button, "Shift", "Use Movement", "/Keyboard/shift", null)},
            {EA_PlayerInputArguments.EInputType.Pause, new S_PlayerInputs.InputActionData(InputActionType.Button, "Pause", "Pause", "/Keyboard/z", "/<Gamepad>/start")},
        };

    
    
    [SerializeField]
    ActionData[] _actionDatas = Array.Empty<ActionData>();
    
    public Dictionary<EA_PlayerInputArguments.EInputType, S_PlayerInputs.InputActionData> getInputActionDatas()
    {
        Dictionary<EA_PlayerInputArguments.EInputType, S_PlayerInputs.InputActionData> inputActionDatas = _inputActionDatas;
        foreach (ActionData actionData in _actionDatas)
        {
            inputActionDatas[actionData.inputType] = new S_PlayerInputs.InputActionData(
                inputActionDatas[actionData.inputType].Type, 
                _inputActionDatas[actionData.inputType].Name,
                _inputActionDatas[actionData.inputType].DisplayName,
                actionData.kmInputBinding, 
                actionData.gpInputBinding,
                inputActionDatas[actionData.inputType].ValueType
                );  
        }

        return inputActionDatas;
    }
}
