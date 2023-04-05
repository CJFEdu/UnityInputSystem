using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class EA_PlayerInputArguments : EventArgs
{
    // public enum EInputType {Rebind, Up, Down, Left, Right, LeftArm, RightArm, LeftArmToggle, RightArmToggle, LeftArmActivate, 
    //     RightArmActivate, LeftArmActivate2, RightArmActivate2, Jump, Look, Shift, Pause, Interact};
    public enum EInputType {Rebind, Up, Down, Left, Right, LeftArmToggle, RightArmToggle, LeftArmActivate, 
        RightArmActivate, LeftArmActivate2, RightArmActivate2, Jump, Look, Shift, Pause, Interact, Crouch};

    public EInputType PlayerInputType { get; set; }
    public float XAxis { get; set; }
    public float YAxis { get; set; }
    public float ZAxis { get; set; }
    
    public InputAction.CallbackContext ctx { get; set; }
}

public class EA_RebindEventArguments : EventArgs
{
    public EA_PlayerInputArguments.EInputType InputType { get; set; }
    public string NewBinding { get; set; }
    public bool IsKeyboard { get; set; }

    public bool Success { get; set; } = true;
}

public class PlayerKeybindingData
{
    public EA_PlayerInputArguments.EInputType InputType { get; set; }
    public string Name { get; set; }
    public string KMBinding { get; set; }
    public string GamepadBinding { get; set; }
    
    public PlayerKeybindingData(EA_PlayerInputArguments.EInputType inputType, string name, string kbBinding, string gamepadBinding)
    {
        InputType = inputType;
        Name = name;
        KMBinding = kbBinding;
        GamepadBinding = gamepadBinding;
    }

    public PlayerKeybindingData(EA_PlayerInputArguments.EInputType inputType, string name, string kbBinding)
    {
        InputType = inputType;
        Name = name;
        KMBinding = kbBinding;
        GamepadBinding = null;
    }
    public PlayerKeybindingData(EA_PlayerInputArguments.EInputType inputType, string name)
    {
        InputType = inputType;
        Name = name;
        KMBinding = null;
        GamepadBinding = null;
    }
}

public class PlayerInputEventHandler
{
    public enum InputActionGroups {Keyboard, Gamepad};

    public enum InputActionValueType {Button, Axis, Vector2, Vector3, Quaternion, Pose, Transform, String, Float, Integer, Boolean, Object, Array, Custom};
    public delegate void OnInput(object sender, EA_PlayerInputArguments args);
    public event OnInput OnInputEvent;
    
    //public delegate void OnInputPressed(object sender, EA_PlayerInputArguments args);
    public event OnInput OnInputPressedEvent;
    
    //public delegate void OnInputReleased(object sender, EA_PlayerInputArguments args);
    public event OnInput OnInputReleasedEvent;
    
    protected  InputAction _inputAction;

    protected EA_PlayerInputArguments.EInputType _inputType;

    private InputActionValueType _inputActionValueType;
    
    public InputActionType inputActionType { get; private set; }

    public PlayerInputEventHandler(EA_PlayerInputArguments.EInputType inputType ,S_PlayerInputs.InputActionData inputActionData)
    {
        _inputType = inputType;
        _inputActionValueType = inputActionData.ValueType;
        inputActionType = inputActionData.Type;
        _inputAction = new InputAction(type: inputActionData.Type, processors: "AxisDeadzone(min=0.1)");
        if(inputActionData.kmBinding != null)
        {
            _inputAction.AddBinding(inputActionData.kmBinding,groups:BindingGroupToString(InputActionGroups.Keyboard));
        }
        if (inputActionData.gpBinding != null)
        {
            _inputAction.AddBinding(inputActionData.gpBinding,groups:BindingGroupToString(InputActionGroups.Gamepad));
        }
        _inputAction.performed += RaiseOnInputEvent;
        _inputAction.canceled += RaiseOnInputEvent;
        _inputAction.Enable();
    }
    
    


    public virtual void RaiseOnInputEvent(InputAction.CallbackContext ctx) {
        EA_PlayerInputArguments args = new EA_PlayerInputArguments();
        args.PlayerInputType = _inputType;
        args.ctx = ctx;
        switch (_inputActionValueType)
        {
            case InputActionValueType.Vector2:
                Vector2 vec2 = ctx.ReadValue<Vector2>();
                args.XAxis = vec2.x;
                args.YAxis = vec2.y;
                break;
            case InputActionValueType.Float:
                args.XAxis = ctx.ReadValue<float>();
                break;
            case InputActionValueType.Integer:
                args.XAxis = ctx.ReadValue<int>();
                break;
            
        }
        CallEvents(ctx, args);
    }
    
    protected void CallEvents(InputAction.CallbackContext ctx, EA_PlayerInputArguments args)
    {
        if (ctx.performed)
        {
            if (OnInputPressedEvent != null)
            {
                OnInputPressedEvent(this, args);
            }
        }
        else if (ctx.canceled)
        {
            if (OnInputReleasedEvent != null)
            {
                OnInputReleasedEvent(this, args);
            }
        }
        if (OnInputEvent != null) {
            OnInputEvent(this, args);
        }
        
    }

    public void SetActionPath(string strPath)
    {
        SetActionPath(strPath, InputActionGroups.Keyboard);
    }
    
    public void SetActionPath(string strPath, InputActionGroups bindingGroup)
    {
        _inputAction.ChangeBinding(_inputAction.GetBindingIndex(BindingGroupToString(bindingGroup))).WithPath(strPath);
        _inputAction.Enable();
    }
    
    private string BindingGroupToString(InputActionGroups bindingIndex)
    {
        switch (bindingIndex)
        {
            case InputActionGroups.Keyboard:
                return "Keyboard";
            case InputActionGroups.Gamepad:
                return "Gamepad";
            default:
                return "Keyboard";
        }
    }
    
    public ReadOnlyArray<InputBinding> GetActionBindings()
    {
        return _inputAction.bindings;
    }

    public void Disable()
    {
        _inputAction.Disable();
    }
    
    public void Enable()
    {
        _inputAction.Enable();
    }

}

public class RebindEventHandler : PlayerInputEventHandler
{
    public RebindEventHandler(EA_PlayerInputArguments.EInputType inputType ,S_PlayerInputs.InputActionData inputActionData)
        : base(inputType, inputActionData)
    {
    }
    
    public void StartRebindListener(EA_PlayerInputArguments.EInputType inputType, InputActionType typeActionType, bool isKeyboard)
    {
        _inputType = inputType;
        _inputAction = new InputAction(type: typeActionType, processors: "AxisDeadzone(min=0.1)");
        _inputAction.performed += RaiseOnInputEvent;
        //_inputAction.canceled += RaiseOnInputEvent;
        if (isKeyboard)
        {
            _inputAction.AddBinding("/*/<key>");
        }
        else
        {

            var allGamepads = InputSystem.devices;
            foreach (var gamepad in allGamepads)
            {
                //Debug.Log(gamepad.name);
                if (!gamepad.name.Contains("Mouse") && !gamepad.name.Contains("Keyboard"))
                {
                    foreach (var ic in gamepad.children)
                    {
                        if (inputType == EA_PlayerInputArguments.EInputType.Look)
                        {
                            Debug.Log(ic.path);
                            _inputAction.AddBinding(ic.path);
                        }else{

                            //Debug.Log(ic.path);
                            if (ic.children.Count > 0)
                            {
                                foreach (InputControl control in ic.children)
                                {
                                    Debug.Log(control.path);
                                    _inputAction.AddBinding(control.path);
                                }
                            }
                            else
                            {
                                Debug.Log(ic.path);
                                _inputAction.AddBinding(ic.path);
                            }
                        }
                    }
                }
            }
        }

        _inputAction.Enable();
    }

    public override void RaiseOnInputEvent(InputAction.CallbackContext ctx)
    {
        Debug.Log("Rebind Event");
        EA_PlayerInputArguments args = new EA_PlayerInputArguments();
        args.PlayerInputType = _inputType;
        args.ctx = ctx;
        CallEvents(ctx, args);
    }
}

public class S_PlayerInputs 
{
    public delegate void OnInput(object sender, EA_PlayerInputArguments args);
    public event OnInput OnInputEvent;
    
    public delegate void OnInputPressed(object sender, EA_PlayerInputArguments args);
    public event OnInputPressed OnInputPressedEvent;
    
    public delegate void OnInputReleased(object sender, EA_PlayerInputArguments args);
    public event OnInputReleased OnInputReleasedEvent;
    
    public delegate void OnRebind(object sender, EA_RebindEventArguments args);
    
    public event OnRebind OnRebindEvent;


    
    private Dictionary<EA_PlayerInputArguments.EInputType, PlayerInputEventHandler> _inputHandlers;
    
    private RebindEventHandler _rebindListener;
    
    private bool _rebindKeyboard = false;
    
    public struct InputActionData
    {
        public InputActionType Type;
        public string kmBinding;
        public string gpBinding;
        public PlayerInputEventHandler.InputActionValueType ValueType;
        public string Name;
        public string DisplayName;
        
        public InputActionData(InputActionType type, string name, string displayName, string newKMBinding, PlayerInputEventHandler.InputActionValueType valueType = PlayerInputEventHandler.InputActionValueType.Button)
        {
            Type = type;
            kmBinding = newKMBinding;
            gpBinding = null;
            ValueType = valueType;
            Name = name;
            DisplayName = displayName;
        }
        public InputActionData(InputActionType type, string name, string displayName, string newKMBinding, string newGPBinding, PlayerInputEventHandler.InputActionValueType valueType = PlayerInputEventHandler.InputActionValueType.Button)
        {
            Type = type;
            kmBinding = newKMBinding;
            gpBinding = newGPBinding;
            ValueType = valueType;
            Name = name;
            DisplayName = displayName;
        }
    }

    private Dictionary<EA_PlayerInputArguments.EInputType, InputActionData> _inputActionDatas =
        new Dictionary<EA_PlayerInputArguments.EInputType, InputActionData>();

    // private Dictionary<EA_PlayerInputArguments.EInputType, string> _keybindingNames =
    //     new Dictionary<EA_PlayerInputArguments.EInputType, string>()
    //     {
    //         {EA_PlayerInputArguments.EInputType.Up, "Up"},
    //         {EA_PlayerInputArguments.EInputType.Down, "Down"},
    //         {EA_PlayerInputArguments.EInputType.Left, "Left"},
    //         {EA_PlayerInputArguments.EInputType.Right, "Right"},
    //         {EA_PlayerInputArguments.EInputType.LeftArmToggle, "Left Arm Toggle"},
    //         {EA_PlayerInputArguments.EInputType.RightArmToggle, "Right Arm Toggle"},
    //         {EA_PlayerInputArguments.EInputType.LeftArmActivate, "Left Arm Activate"},
    //         {EA_PlayerInputArguments.EInputType.RightArmActivate, "Right Arm Activate"},
    //         { EA_PlayerInputArguments.EInputType.LeftArmActivate2, "Left Arm Activate 2" },
    //         { EA_PlayerInputArguments.EInputType.RightArmActivate2, "Right Arm Activate 2" },
    //         {EA_PlayerInputArguments.EInputType.Jump, "Jump"},
    //         {EA_PlayerInputArguments.EInputType.Look, "Look"},
    //         {EA_PlayerInputArguments.EInputType.Shift, "Shift"},
    //         { EA_PlayerInputArguments.EInputType.Pause, "Pause"},
    //         { EA_PlayerInputArguments.EInputType.Interact, "Interact"},
    //         { EA_PlayerInputArguments.EInputType.Crouch, "Crouch"}
    //     };

    private List<EA_PlayerInputArguments.EInputType> _keybindingInputTypes =
        new List<EA_PlayerInputArguments.EInputType>()
        {
            {EA_PlayerInputArguments.EInputType.Up},
            {EA_PlayerInputArguments.EInputType.Down},
            {EA_PlayerInputArguments.EInputType.Left},
            {EA_PlayerInputArguments.EInputType.Right},
            {EA_PlayerInputArguments.EInputType.LeftArmToggle},
            {EA_PlayerInputArguments.EInputType.RightArmToggle},
            {EA_PlayerInputArguments.EInputType.LeftArmActivate},
            {EA_PlayerInputArguments.EInputType.RightArmActivate},
            { EA_PlayerInputArguments.EInputType.LeftArmActivate2 },
            { EA_PlayerInputArguments.EInputType.RightArmActivate2 },
            {EA_PlayerInputArguments.EInputType.Jump},
            { EA_PlayerInputArguments.EInputType.Crouch},
            { EA_PlayerInputArguments.EInputType.Interact},
            {EA_PlayerInputArguments.EInputType.Look},
            {EA_PlayerInputArguments.EInputType.Shift},
            { EA_PlayerInputArguments.EInputType.Pause}
        };

    public S_PlayerInputs(Dictionary<EA_PlayerInputArguments.EInputType, InputActionData> newInputActionDatas)
    {
        foreach (var inputActionData in newInputActionDatas)
        {
            _inputActionDatas[inputActionData.Key] = inputActionData.Value;
        }
        _inputHandlers = new Dictionary<EA_PlayerInputArguments.EInputType, PlayerInputEventHandler>();
        foreach (KeyValuePair<EA_PlayerInputArguments.EInputType,InputActionData> inputActionData in _inputActionDatas)
        {
            _inputHandlers.Add(inputActionData.Key, new PlayerInputEventHandler(inputActionData.Key, inputActionData.Value));
            _inputHandlers[inputActionData.Key].OnInputEvent +=  (sender, args) => RaiseOnInputEvent(args.PlayerInputType, args.ctx);;
        }
        _rebindListener = new RebindEventHandler(EA_PlayerInputArguments.EInputType.Rebind, new InputActionData(InputActionType.Button, "Rebind","Rebind", null,null));
        _rebindListener.OnInputEvent += RegisterNewKeyBinding;
    }
    
    public void AddOnRebindEvent(OnRebind eh)
    {
        OnRebindEvent += eh;
    }
    
    public void AddOnInputEvent(EA_PlayerInputArguments.EInputType inputType, PlayerInputEventHandler.OnInput eh)
    {
        _inputHandlers[inputType].OnInputEvent += eh;
    }
    
    public void RemoveOnInputEvent(EA_PlayerInputArguments.EInputType inputType, PlayerInputEventHandler.OnInput eh)
    {
        _inputHandlers[inputType].OnInputEvent -= eh;
    }
    
    public void AddOnInputPressedEvent(EA_PlayerInputArguments.EInputType inputType, PlayerInputEventHandler.OnInput eh)
    {
        _inputHandlers[inputType].OnInputPressedEvent += eh;
    }
    
    public void RemoveOnInputPressedEvent(EA_PlayerInputArguments.EInputType inputType, PlayerInputEventHandler.OnInput eh)
    {
        _inputHandlers[inputType].OnInputPressedEvent -= eh;
    }
    
    public void AddOnInputReleasedEvent(EA_PlayerInputArguments.EInputType inputType, PlayerInputEventHandler.OnInput eh)
    {
        _inputHandlers[inputType].OnInputReleasedEvent += eh;
    }
    
    public void RemoveOnInputReleasedEvent(EA_PlayerInputArguments.EInputType inputType, PlayerInputEventHandler.OnInput eh)
    {
        _inputHandlers[inputType].OnInputReleasedEvent -= eh;
    }

    public void RaiseOnInputEvent(EA_PlayerInputArguments.EInputType inputType,InputAction.CallbackContext ctx) {
        RaiseOnInputEvent(inputType, ctx, 0, 0, 0);
    }
    
    public void RaiseOnInputEvent(EA_PlayerInputArguments.EInputType inputType,InputAction.CallbackContext ctx,float xAxis, float yAxis, float zAxis) {
        EA_PlayerInputArguments args = new EA_PlayerInputArguments();
        args.PlayerInputType = inputType;
        args.XAxis = xAxis;
        args.YAxis = yAxis;
        args.ZAxis = zAxis;
        args.ctx = ctx;
        if (OnInputEvent != null) {
            OnInputEvent(this, args);
        }

        if (ctx.performed)
        {
            if (OnInputPressedEvent != null)
            {
                OnInputPressedEvent(this, args);
            }
        }
        else if (ctx.canceled)
        {
            if (OnInputReleasedEvent != null)
            {
                OnInputReleasedEvent(this, args);
            }
        }
    }

    public void SetActionPath(EA_PlayerInputArguments.EInputType inputType, string strPath)
    {
        SetActionPath(inputType, strPath, true);
    }
    
    public void SetActionPath(EA_PlayerInputArguments.EInputType inputType, string strPath, bool isKeyboard)
    {
        if(isKeyboard)
            _inputHandlers[inputType].SetActionPath(strPath, PlayerInputEventHandler.InputActionGroups.Keyboard);
        else
            _inputHandlers[inputType].SetActionPath(strPath, PlayerInputEventHandler.InputActionGroups.Gamepad);
    }
    
    public void ListenForKeyBinding(EA_PlayerInputArguments.EInputType inputType,bool isKeyboard = true)
    {
        foreach (KeyValuePair<EA_PlayerInputArguments.EInputType, PlayerInputEventHandler> inputHandler in _inputHandlers)
        {
            inputHandler.Value.Disable();
        }
        _rebindKeyboard = isKeyboard;
        _rebindListener = new RebindEventHandler(inputType, new InputActionData(InputActionType.Button,"Rebind","Rebind", null, null));
        _rebindListener.StartRebindListener(inputType, _inputHandlers[inputType].inputActionType, isKeyboard);
        _rebindListener.OnInputPressedEvent += RegisterNewKeyBinding;
    }
    
    public void RegisterNewKeyBinding(object o, EA_PlayerInputArguments args)
    {
        Debug.Log("RegisterNewKeyBinding");
        Debug.Log(args.ctx.control.path);
        EA_RebindEventArguments rebindArgs = new EA_RebindEventArguments();
        if (_rebindKeyboard && (args.ctx.control.path.Contains("Mouse") || args.ctx.control.path.Contains("Keyboard")))
        {
            if (args.ctx.control.path.Contains("Mouse/position"))
            {
                rebindArgs.Success = false;
                CallAndClearRebindEvent(rebindArgs);
                return;
            }

            SetActionPath(args.PlayerInputType, args.ctx.control.path);
        }
        else if (!_rebindKeyboard)
        {
            SetActionPath(args.PlayerInputType, args.ctx.control.path, false);
        }
        else
        {
            rebindArgs.Success = false;
            CallAndClearRebindEvent(rebindArgs);
            return;
        }
        _rebindListener.Disable();
        rebindArgs.InputType = args.PlayerInputType;
        rebindArgs.IsKeyboard = _rebindKeyboard;
        PlayerKeybindingData playerKeybindingData = GetActionKeybindingData(args.PlayerInputType, _inputHandlers[args.PlayerInputType]);
        if (_rebindKeyboard)
        {
            rebindArgs.NewBinding = playerKeybindingData.KMBinding;
        }else
        {
            rebindArgs.NewBinding = playerKeybindingData.GamepadBinding;
        }
        CallAndClearRebindEvent(rebindArgs);

        foreach (KeyValuePair<EA_PlayerInputArguments.EInputType, PlayerInputEventHandler> inputHandler in _inputHandlers)
        {
            inputHandler.Value.Enable();
        }
        
    }

    private void CallAndClearRebindEvent(EA_RebindEventArguments rebindArgs)
    {
        if (OnRebindEvent != null)
        {
            OnRebindEvent(this, rebindArgs);
            foreach (Delegate D in OnRebindEvent.GetInvocationList())
            {
                OnRebindEvent -= (OnRebind)D;
            }
        }
    }
    
    public void Enable()
    {
        foreach (KeyValuePair<EA_PlayerInputArguments.EInputType, PlayerInputEventHandler> inputHandler in _inputHandlers)
        {
            inputHandler.Value.Enable();
        }
    }
    
    public void Disable()
    {
        foreach (KeyValuePair<EA_PlayerInputArguments.EInputType, PlayerInputEventHandler> inputHandler in _inputHandlers)
        {
            inputHandler.Value.Disable();
        }
    }
    
    public PlayerKeybindingData GetActionKeybindingData(EA_PlayerInputArguments.EInputType inputType,PlayerInputEventHandler inputHandler)
    {
        PlayerKeybindingData playerKeybindingData = new PlayerKeybindingData(inputType, _inputActionDatas[inputType].DisplayName);
        ReadOnlyArray<InputBinding> bindings = inputHandler.GetActionBindings();
        foreach (InputBinding inputBinding in bindings)
        {
            string[] strPath = inputBinding.effectivePath.Split("/",3);
            if(strPath[1].Contains("Mouse") || strPath[1].Contains("Keyboard"))
            {
                playerKeybindingData.KMBinding = strPath[2];
            }
            else 
            {
                playerKeybindingData.GamepadBinding = strPath[2];
            }
        }
        return playerKeybindingData;
    }

    public void LoadData(ControlSaveFile save)
    {
        for(int i = 0; i < _keybindingInputTypes.Count; i++)
        {
            EA_PlayerInputArguments.EInputType inputType = _keybindingInputTypes[i];
            if(save.controllerInputs[i] != null  && save.controllerInputs[i] != "")
            {
                SetActionPath(inputType, save.controllerInputs[i], false);
            }

            if (save.keyboardMouseInputs[i] != null && save.keyboardMouseInputs[i] != "")
            {
                SetActionPath(inputType, save.keyboardMouseInputs[i]);
            }

        }
    }
    
    public void SaveData(ref ControlSaveFile save)
    {
        for(int i = 0; i < _keybindingInputTypes.Count; i++)
        {
            EA_PlayerInputArguments.EInputType inputType = _keybindingInputTypes[i];
            PlayerInputEventHandler inputHandler = _inputHandlers[inputType];
            ReadOnlyArray<InputBinding> bindings = inputHandler.GetActionBindings();
            save.controllerInputs[i] = null;
            save.keyboardMouseInputs[i] = null;
            foreach (InputBinding inputBinding in bindings)
            {
                string strPath = inputBinding.effectivePath;
                if(strPath.Contains("Mouse") || strPath.Contains("Keyboard"))
                {
                    save.keyboardMouseInputs[i] = strPath;
                }
                else 
                {
                    save.controllerInputs[i] = strPath;
                }
            }
        }
    }
    
    public void SetKeybindingData(List<PlayerKeybindingData> keybindingDatas)
    {
        foreach (PlayerKeybindingData keybindingData in keybindingDatas)
        {
            SetActionPath(keybindingData.InputType, keybindingData.KMBinding);
            SetActionPath(keybindingData.InputType, keybindingData.GamepadBinding, false);
        }
    }
    
    public List<PlayerKeybindingData> GetKeybindingData()
    {
        List<PlayerKeybindingData> keybindingDatas = new List<PlayerKeybindingData>();
        foreach (KeyValuePair<EA_PlayerInputArguments.EInputType, PlayerInputEventHandler> inputHandler in _inputHandlers)
        {
            PlayerKeybindingData playerKeybindingData = GetActionKeybindingData(inputHandler.Key, inputHandler.Value);
            keybindingDatas.Add(playerKeybindingData);
        }
        return keybindingDatas;
    }
}
