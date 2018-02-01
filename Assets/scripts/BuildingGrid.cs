using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGrid : MonoBehaviour
{
	private const int CellLayer = 8;
	
	[SerializeField] private Material busyMat;
	[SerializeField] private Material neutralMat;

	[SerializeField] private BuildingCell cellPrefab;

	private int testSize;
	private bool buildMode = false;
	private bool buildState = false;
	private int currentCell = -1;

	private List<BuildingCell> cells = new List<BuildingCell>();

	#region Create

	private void Start()
	{
		CreateGrid();
	}

	//refactor1. move to editor script
	//refactor2. grid to sprite
	private void CreateGrid()
	{
		int cellCount = GameConstants.ColumnCount * GameConstants.RowCount;
		int startBusy = (int) (cellCount * GameConstants.StartBusy);
		int busy = 0;
		Vector2 step = new Vector2(GameConstants.GridSize.x / GameConstants.RowCount,
			GameConstants.GridSize.y / GameConstants.ColumnCount);
		Vector2 offset = new Vector2(-GameConstants.GridSize.x / 2 + GameConstants.CellSize.x / 2,
			-GameConstants.GridSize.y / 2 + GameConstants.CellSize.y / 2);
		for (int index = 0; index < cellCount; index++)
		{
			var cell = Instantiate(cellPrefab);
			var z = index % GameConstants.RowCount * step.x + offset.x;
			var x = index / GameConstants.RowCount * step.y + offset.y;
			cell.transform.localPosition = new Vector3(x, GameConstants.Y, z);
			cell.transform.SetParent(this.transform, true);
			cell.Index = index;
			cells.Add(cell);
			var remainBusy = startBusy - busy;
			var remainCells = cellCount - index;
			//set seed
			var random = UnityEngine.Random.value;
			float busyChance = 1.0f * remainBusy / remainCells;
			if (busyChance > random)
			{
				busy++;
				cell.Material = busyMat;
				cell.Busy = true;
			}
		}
	}

	#endregion

	public void Build(int size)
	{
		testSize = size;
		buildMode = true;
	}

	public void BreakBuild()
	{
		buildMode = false;
		buildState = false;
		currentCell = -1;
	}

	private void Update()
	{
		if (buildMode)
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			var layerMask = 1 << CellLayer;
			if (Physics.Raycast(ray, out hit, float.PositiveInfinity, layerMask))
			{
				BuildingCell cell = hit.transform.GetComponent<BuildingCell>();
				
				if (currentCell != cell.Index)
				{
					currentCell = cell.Index;
					if (buildState != CheckCellsIsFree(cell.Index))
					{
						ChangeBuildState();
					}
				}
			}
			else
			{
				currentCell = -1;
			}
		}
	}
	
	public void ApplyBuild()
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		var layerMask = 1 << CellLayer;
		if (Physics.Raycast(ray, out hit, float.PositiveInfinity, layerMask))
		{
			BuildingCell cell = hit.transform.GetComponent<BuildingCell>();

			for (int i = 0; i < testSize; i++)
			{
				for (int j = 0; j < testSize; j++)
				{
					int cellIndex = currentCell + i + j * GameConstants.RowCount;
					cells[cellIndex].Busy = true;
					cells[cellIndex].Material = busyMat;
				}
			}
		}
	}
	
	public void ApplyDelete(IFigure figure)
	{
		var current= figure.Z + figure.X * GameConstants.RowCount;
		for (int i = 0; i < figure.Size; i++)
		{
			for (int j = 0; j < figure.Size; j++)
			{
				int cellIndex = current + i + j * GameConstants.RowCount;
				cells[cellIndex].Busy = false;
				cells[cellIndex].Material = neutralMat;
			}
		}
	}

	public void ChangeBuildState()
	{
		buildState = !buildState;
		Retranslator.Send(GameEvents.BuildStateChanged, buildState);
	}

	private bool CheckCellsIsFree(int current)
	{
		int maxIndex = GameConstants.RowCount * GameConstants.ColumnCount;
		for (int i = 0; i < testSize; i++)
		{
			for (int j = 0; j < testSize; j++)
			{
				int cellIndex = current + i + j * GameConstants.RowCount;
				if (cellIndex >= maxIndex || cells[cellIndex].Busy)
					return false;
			}
		}
		return true;
	}

}
