using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGrid : MonoBehaviour
{
	[SerializeField]
	private Material bisyMat;
	[SerializeField]
	private Material freeMat;
	[SerializeField]
	private Material neutralMat;
	
	[SerializeField]
	private BuildingCell cellPrefab;
	
	private void Start()
	{
		CreateGrid();
	}

	
	//refactor1. move to editor script
	//refactor2. grid to sprite
	private void CreateGrid()
	{
		int cellCount = GameConstants.ColumnCount * GameConstants.RowCount;
		int startBusy = (int)(cellCount * GameConstants.StartBusy);
		int busy = 0;
		Vector2 step = new Vector2(GameConstants.GridSize.x / GameConstants.RowCount, 
			GameConstants.GridSize.y / GameConstants.ColumnCount );
		Vector2 offset = new Vector2( -GameConstants.GridSize.x / 2 + GameConstants.CellSize.x / 2, 
			-GameConstants.GridSize.y / 2 + GameConstants.CellSize.y / 2);
		for (int index = 0; index < cellCount; index++)
		{
			var cell = Instantiate(cellPrefab);
			var z = index % GameConstants.RowCount * step.x + offset.x;
			var x = index / GameConstants.RowCount * step.y + offset.y;
			cell.transform.localPosition = new Vector3(x, GameConstants.Y, z);
			cell.transform.SetParent(this.transform, true);
			var remainBusy = startBusy - busy;
			var remainCells = cellCount - index;
			//set seed
			var random = UnityEngine.Random.value;
			float busyChance = 1.0f * remainBusy / remainCells;
			if (busyChance > random)
			{
				busy++;
				cell.Material = bisyMat;
				cell.Busy = true;
			}
		}
	}
}
