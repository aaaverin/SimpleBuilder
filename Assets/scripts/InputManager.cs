using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour, IManager
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Retranslator.Send("BreakBuild");
        }
    }
}
