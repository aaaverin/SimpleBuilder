using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;

public class GuiManager : MonoBehaviour, IManager
{
    public enum State
    {
        Closed,
        Open,
        Build
    }

    private State state = State.Closed;
    
    [SerializeField]
    private GameObject menu;
    [SerializeField]
    private GameObject build;
    [SerializeField]
    private GameObject back;

    public void SetBuildState()
    {
        back.SetActive(false);
        menu.SetActive(false);
        build.SetActive(false);
    }

    public void SetClosedState()
    {
        back.SetActive(false);
        menu.SetActive(false);
        build.SetActive(true);
    }
    
    public void SetOpenState()
    {
        back.SetActive(true);
        menu.SetActive(true);
        build.SetActive(true);
    }

    public void ChangeState()
    {
        if (state == State.Closed)
        {
            SetOpenState();
        }
        else
        {
            SetClosedState();
        }
    }

    public void BreakBuild()
    {
        SetClosedState();
    }
}
