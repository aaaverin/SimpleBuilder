using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Retranslator : MonoBehaviour
{
    private static Retranslator instance;

    [SerializeField]
    private List<MonoBehaviour> managers;

    private void Awake()
    {
        instance = this;
    }

    public static void Send(string message)
    {
        if (instance == null)
            return;
        
        foreach (var manager in instance.managers)
        {
            manager.SendMessage(message);
        }
    }
}
