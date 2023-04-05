using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class EA_PlayerAddedArgs : EventArgs
{

    public int PlayerID { get; set; }
}

public class S_InputManager : MonoBehaviour
{
    [SerializeField]
    private int maxPlayerCount = 4;
    private int currentPlayerCount = 0;
    
    [SerializeField]
    private SO_InputActions defaultInputActions;
    public static S_InputManager Instance { get; private set; }

    private S_PlayerInputs[] _playerInputs = new S_PlayerInputs[1];

    public delegate void OnPlayerAdded(object sender, EA_PlayerAddedArgs args);
    public event OnPlayerAdded OnPlayerAddedEvent;
    private void Awake() 
    { 
        // If there is an instance, and it's not me, delete myself.
    
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        }

        if (_playerInputs == null)
        {
            _playerInputs = new S_PlayerInputs[maxPlayerCount];
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public EA_PlayerAddedArgs AddPlayer()
    {
        EA_PlayerAddedArgs args = new EA_PlayerAddedArgs();
        if(currentPlayerCount >= maxPlayerCount)
        {
            args.PlayerID = -1;
            return args;
        }

        if (defaultInputActions == null)
        {
            defaultInputActions = ScriptableObject.CreateInstance<SO_InputActions>();
        }
        _playerInputs[currentPlayerCount] = new S_PlayerInputs(defaultInputActions.getInputActionDatas());
        args.PlayerID = currentPlayerCount;
        currentPlayerCount++;
        if (OnPlayerAddedEvent != null) {
            OnPlayerAddedEvent(this, args);
        }

        return args;
    }
    
    public S_PlayerInputs GetPlayerInput()
    {
        return GetPlayerInput(0);
    }
    
    public S_PlayerInputs GetPlayerInput(int playerIndex)
    {
        if(playerIndex >= currentPlayerCount)
        {
            return null;
        }
        return _playerInputs[playerIndex];
    }
    
    

    public List<string> GetDevices()
    {
        List<string> devices = new List<string>();
        var allGamepads = InputSystem.devices;
        foreach (var gamepad in allGamepads)
        {
            devices.Add(gamepad.name);
        }

        return devices;
    }
    
    public void DisableInputs()
    {
        for (int i = 0; i < currentPlayerCount; i++)
        {
            _playerInputs[i].Disable();
        }
    }
    
    public void EnableInputs()
    {
        for (int i = 0; i < currentPlayerCount; i++)
        {
            _playerInputs[i].Enable();
        }
    }
    
    public void DisablePlayerInputs(int playerIndex)
    {
        if(playerIndex >= currentPlayerCount)
        {
            return;
        }
        _playerInputs[playerIndex].Disable();
    }
    
    public void EnablePlayerInputs(int playerIndex)
    {
        if(playerIndex >= currentPlayerCount)
        {
            return;
        }
        _playerInputs[playerIndex].Enable();
    }

    public void LoadData(ControlSaveFile save)
    {
        _playerInputs[0].LoadData(save);
    }

    public void SaveData(ref ControlSaveFile save)
    {
        _playerInputs[0].SaveData(ref save);
    }
}