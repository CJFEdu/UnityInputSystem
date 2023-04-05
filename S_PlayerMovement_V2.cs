using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_PlayerMovement_V2 : S_InputReceiver
{
    private int _playerIndex = -1;

    void Start()
    {
    }

    private void Awake()
    {
        RegisterInput(EA_PlayerInputArguments.EInputType.Down, ReceiveInput.Pressed, OnMovementPressed);
        RegisterInput(EA_PlayerInputArguments.EInputType.Down, ReceiveInput.Released, OnMovementReleased);
        RegisterInput(EA_PlayerInputArguments.EInputType.Up, ReceiveInput.Pressed, OnMovementPressed);
        RegisterInput(EA_PlayerInputArguments.EInputType.Up, ReceiveInput.Released, OnMovementReleased);
        RegisterInput(EA_PlayerInputArguments.EInputType.Left, ReceiveInput.Pressed, OnMovementPressed);
        RegisterInput(EA_PlayerInputArguments.EInputType.Left, ReceiveInput.Released, OnMovementReleased);
        RegisterInput(EA_PlayerInputArguments.EInputType.Right, ReceiveInput.Pressed, OnMovementPressed);
        RegisterInput(EA_PlayerInputArguments.EInputType.Right, ReceiveInput.Released, OnMovementReleased);
        RegisterInput(EA_PlayerInputArguments.EInputType.LeftArmActivate, ReceiveInput.Pressed, LeftArmPressed);
        RegisterInput(EA_PlayerInputArguments.EInputType.LeftArmActivate, ReceiveInput.Released, LeftArmReleased);
        RegisterInput(EA_PlayerInputArguments.EInputType.RightArmActivate, ReceiveInput.Pressed, RightArmPressed);
        RegisterInput(EA_PlayerInputArguments.EInputType.RightArmActivate, ReceiveInput.Released, RightArmReleased);
        //RegisterInput(EA_PlayerInputArguments.EInputType.Look, ReceiveInput.Both, OnMouseMoved);
    }
    
    public void RightArmPressed(object o, EA_PlayerInputArguments args)
    {
        Debug.Log("Right Arm Pressed");
    }
    
    public void RightArmReleased(object o, EA_PlayerInputArguments args)
    {
        Debug.Log("Right Arm Released");
    }
    
    public void LeftArmPressed(object o, EA_PlayerInputArguments args)
    {
        Debug.Log("Left Arm Pressed");
    }
    
    public void LeftArmReleased(object o, EA_PlayerInputArguments args)
    {
        Debug.Log("Left Arm Released");
    }

    public void OnMovementPressed(object o, EA_PlayerInputArguments args)
    {
        Debug.Log("Input Pressed");
        switch (args.PlayerInputType)
        {
            case EA_PlayerInputArguments.EInputType.Up:
                Debug.Log("Up");
                break;
            case EA_PlayerInputArguments.EInputType.Down:
                Debug.Log("Down");
                break;
            case EA_PlayerInputArguments.EInputType.Left:
                Debug.Log("Left");
                break;
            case EA_PlayerInputArguments.EInputType.Right:
                Debug.Log("Right");
                break;

        }
    }
    
    public void OnMovementReleased(object o, EA_PlayerInputArguments args)
    {
        Debug.Log("Input Released");
        switch (args.PlayerInputType)
        {
            case EA_PlayerInputArguments.EInputType.Up:
                Debug.Log("Up");
                break;
            case EA_PlayerInputArguments.EInputType.Down:
                Debug.Log("Down");
                break;
            case EA_PlayerInputArguments.EInputType.Left:
                Debug.Log("Left");
                break;
            case EA_PlayerInputArguments.EInputType.Right:
                Debug.Log("Right");
                break;
        }
    }


    public void OnMouseMoved(object o, EA_PlayerInputArguments args)
    {
        Debug.Log("X Movement "+args.XAxis);
        Debug.Log("Y Movement "+args.YAxis);
    }

}
