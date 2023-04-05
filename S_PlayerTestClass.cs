using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_PlayerTestClass : MonoBehaviour
{
    private void Awake()
    {
        S_InputManager.Instance.OnPlayerAddedEvent += InitKeyBindings;
    }

    // Start is called before the first frame update
    void Start()
    {
        S_InputManager.Instance.AddPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void InitKeyBindings(object o, EA_PlayerAddedArgs args)
    {
        S_PlayerInputs playerInput = S_InputManager.Instance.GetPlayerInput(args.PlayerID);
        if (playerInput != null)
        {
            playerInput.SetActionPath(EA_PlayerInputArguments.EInputType.Up, "<Keyboard>/w");
            playerInput.SetActionPath(EA_PlayerInputArguments.EInputType.Down, "<Keyboard>/s");
            playerInput.SetActionPath(EA_PlayerInputArguments.EInputType.Left, "<Keyboard>/a");
            playerInput.SetActionPath(EA_PlayerInputArguments.EInputType.Right, "<Keyboard>/d");
            playerInput.SetActionPath(EA_PlayerInputArguments.EInputType.LeftArmActivate, "<Keyboard>/q");
            playerInput.SetActionPath(EA_PlayerInputArguments.EInputType.RightArmActivate, "<Keyboard>/e");
        }

    }


}
