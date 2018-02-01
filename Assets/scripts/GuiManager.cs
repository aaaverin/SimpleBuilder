using UnityEngine;
using UnityEngine.UI;

public class GuiManager : MonoBehaviour, IManager
{
    public enum State
    {
        Closed,
        OpenBuildMenu,
        Hidden,
        OpenBuildingMenu,
        BuildingInfo
    }

    private State state = State.Closed;
    
    [SerializeField]
    private GameObject buildMenu;
    [SerializeField]
    private GameObject build;
    [SerializeField]
    private GameObject back;
    [SerializeField]
    private GameObject buildingMenu;
    [SerializeField]
    private GameObject infoPanel;

    [SerializeField] 
    private Text infoText;

    public void SetHiddenState()
    {
        state = State.Hidden;
        back.SetActive(false);
        buildMenu.SetActive(false);
        build.SetActive(false);
        buildingMenu.SetActive(false);
        infoPanel.SetActive(false);
    }

    public void SetClosedState()
    {
        state = State.Closed;
        back.SetActive(false);
        buildMenu.SetActive(false);
        buildingMenu.SetActive(false);
        infoPanel.SetActive(false);
        build.SetActive(true);
    }
    
    public void SetOpenBuildMenuState()
    {
        state = State.OpenBuildMenu;
        buildingMenu.SetActive(false);
        infoPanel.SetActive(false);
        back.SetActive(true);
        buildMenu.SetActive(true);
        build.SetActive(true);
    }

    public void SelectBuilding(IFigure figure)
    {
        infoText.text = string.Format("{0}x{0}", figure.Size);
        SetOpenBuildingMenuState();
    }
    
    private void SetOpenBuildingMenuState()
    {
        state = State.OpenBuildingMenu;
        infoPanel.SetActive(false);
        buildMenu.SetActive(false);
        build.SetActive(true);
        buildingMenu.SetActive(true);
        back.SetActive(true);
    }

    public void SetBuildingInfoState()
    {
        state = State.BuildingInfo;
        buildMenu.SetActive(false);
        buildingMenu.SetActive(false);
        build.SetActive(true);
        infoPanel.SetActive(true);
        back.SetActive(true);
    }

    public void StartBuild(int size)
    {
        Retranslator.Send(GameEvents.Build, size);
    }
    
    public void BreakBuild()
    {
        SetClosedState();
    }

    public void SendDelete()
    {
        Retranslator.Send(GameEvents.Delete);
    }
    
    public void SendDeselect()
    {
        Retranslator.Send(GameEvents.Deselect);
    }
    
    public void ApplyDelete(object obj)
    {
        SetClosedState();
    }
}
