using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_CreateTestPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        S_InputManager.Instance.AddPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
