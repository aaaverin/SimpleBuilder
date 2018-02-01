using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour, IManager
{
    private const int PlaneLayer = 9;
    private const int BuildingLayer = 10;
    
    private const int LMB = 0;
    
    [SerializeField] 
    private Material busyMat;
    [SerializeField] 
    private Material freeMat;
    [SerializeField] 
    private Material neutralMat;
    
    [SerializeField] 
    private Building buildingPrefab;
    
    
    private bool buildState = false;
    
    private Building newBuilding;
    
    private Building selectedBuilding;

    //size == index or error
    public void Build(int size)
    {
        if (newBuilding != null)
        {
            Destroy(newBuilding);
        }
        
        newBuilding = Instantiate(buildingPrefab);
        newBuilding.Size = size;
        newBuilding.transform.localScale = new Vector3(GameConstants.CellSize.x, 
                                            GameConstants.CellSize.y, 
                                            GameConstants.CellSize.y) * size;
        newBuilding.Material = busyMat;
        newBuilding.gameObject.layer = BuildingLayer;
    }
    
    public void BreakBuild()
    {
        if (newBuilding != null)
        {
            Destroy(newBuilding);
        }
    }

    public void BuildStateChanged(bool buildState)
    {
        this.buildState = buildState;
        newBuilding.Material = buildState ? freeMat : busyMat;
    }
    
    public void Delete()
    {
        Retranslator.Send(GameEvents.ApplyDelete, selectedBuilding);
        Destroy(selectedBuilding.gameObject);
        selectedBuilding = null;
    }

    public void Deselect()
    {
        selectedBuilding = null;
    }
    
    void Update()
    {
        if (newBuilding != null)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var layerMask = 1 << PlaneLayer;
            if (Physics.Raycast(ray, out hit, float.PositiveInfinity, layerMask))
            {
                Vector3 point = hit.point;
                int x = Mathf.RoundToInt(point.x / 1  * GameConstants.CellSize.y - GameConstants.CellSize.y / 2);
                int z = Mathf.RoundToInt(point.z / 1  * GameConstants.CellSize.x - GameConstants.CellSize.x / 2);
                
                Vector3 offset = new Vector3(
                    (GameConstants.CellSize.y * newBuilding.Size / 2 )  ,
                    GameConstants.CellSize.y * newBuilding.Size / 2,
                    (GameConstants.CellSize.x * newBuilding.Size / 2 ) );
                newBuilding.transform.localPosition = new Vector3(x, point.y, z) + offset;

                if (buildState && Input.GetMouseButtonDown(LMB))
                {
                    Retranslator.Send(GameEvents.ApplyBuild);
                    newBuilding.Material = neutralMat;
                    newBuilding.X = x + 50;
                    newBuilding.Z = z + 50;
                    newBuilding = null;
                    Retranslator.Send(GameEvents.BreakBuild);
                }
            }
        }
        else
        {
            if (selectedBuilding == null)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                var layerMask = 1 << BuildingLayer;
                if (Physics.Raycast(ray, out hit, float.PositiveInfinity, layerMask))
                {
                    if (Input.GetMouseButtonDown(LMB))
                    {
                        selectedBuilding = hit.transform.gameObject.GetComponent<Building>();
                        Retranslator.Send(GameEvents.SelectBuilding, selectedBuilding);
                    }
                }
            }
        }
    }
}
