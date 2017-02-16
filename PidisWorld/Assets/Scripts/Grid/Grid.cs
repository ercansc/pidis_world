using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum GridObjectType
{
    Blocker,
    OilField,
    Rocket,
    Empty,
    Count //Needs to be last
}

public class Grid : MonoBehaviour
{
    public uint SizeX;
    public uint SizeY;
    public GridTile TilePrefab;
    public GridTile[,] ThisGrid { get; private set; }
    [HideInInspector]
    public List<GridTile> GridList;

    public GridTile this[int x, int y]
    {
        get { return ThisGrid[x, y]; }
        set { ThisGrid[x, y] = value; }

    }

    public GridTile this[Index i]
    {
        get { return ThisGrid[i.x, i.y]; }
        set { ThisGrid[i.x, i.y] = value; }
    }

    public bool HasGrid
    {
        get
        {
            if (ThisGrid == null) return false;
            return transform.childCount > 1;
        }
    }

    public void SetNewGrid(GridTile[,] newGrid)
    {
        ThisGrid = newGrid;
        InitNewGrid();
    }

    void Awake()
    {
        GridListToArray();
    }

    #region EditorScripts

    [ContextMenu("BuildTheGrid")]
    public void InitNewGrid()
    {
        RemoveGrid();

        ThisGrid = new GridTile[SizeX, SizeY];

        Vector2 currentPosition = new Vector2(0,0);
        Sprite sprite = TilePrefab.GetComponent<SpriteRenderer>().sprite;
        float spriteWidth = sprite.bounds.extents.x;
        float spriteHeight = sprite.bounds.extents.y;

        for (int y = 0; y < SizeY; y++)
        {
            for (int x = 0; x < SizeX; x++)
            {
                GridTile newTile = (GridTile)PrefabUtility.InstantiatePrefab(TilePrefab);
                newTile.transform.position = new Vector2(currentPosition.x, currentPosition.y);
                newTile.transform.SetParent(transform);
                newTile.PositionIndex = new Index(x, y);
                newTile.name = String.Format("GridTile_{0}_{1}", x, y);
                ThisGrid[x, y] = newTile;
                GridList.Add(newTile);

                currentPosition.x += spriteWidth;
                currentPosition.y -= spriteHeight;
            }
            currentPosition.x = (y+1) * spriteWidth;
            currentPosition.y = (y+1) * spriteHeight;
        }
    }

    [ContextMenu("RemoveTheGrid")]
    public void RemoveGrid()
    {
        GridList.Clear();
        List<GameObject> liChildren = new List<GameObject>();
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeInHierarchy)
            {
                liChildren.Add(child.gameObject);
            }
        }
        foreach (GameObject child in liChildren)
        {
            if (child.GetComponent<GridTile>())
            {
                DestroyImmediate(child);
            }
        }
        ThisGrid = new GridTile[0,0];
    }

    private void GridListToArray()
    {
        if (GridList.Count == (SizeX * SizeY))
        {
            int i = 0;
            ThisGrid = new GridTile[SizeX, SizeY];

            for (int y = 0; y < SizeY; y++)
            {
                for (int x = 0; x < SizeX; x++)
                {
                    ThisGrid[x, y] = GridList[i];
                    i++;
                }
            }
        }
    }

    #endregion


}
