using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        if (Input.GetKey(KeyCode.Space)) //Can thrust while rotating
        {
            print("Space Pressed");
        }
        if (Input.GetKey(KeyCode.A))
        {
            print("Rotating Left");
        }
        else if (ProcessInput.GetKey(KeyCode.D))
        {
            ProcessInput("Rotating Right");
        }
    }
}