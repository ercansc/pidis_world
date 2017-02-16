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
    private GridTile[,] _thisGrid;
    [HideInInspector]
    public List<GridTile> _gridList;

    public GridTile this[int x, int y]
    {
        get { return _thisGrid[x, y]; }
        set { _thisGrid[x, y] = value; }

    }

    public GridTile this[Index i]
    {
        get { return _thisGrid[i.x, i.y]; }
        set { _thisGrid[i.x, i.y] = value; }
    }

    public bool HasGrid
    {
        get
        {
            if (_thisGrid == null) return false;
            return _thisGrid.Length > 0;
        }
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

        Vector2 nullPosition = Vector2.zero;
        _thisGrid = new GridTile[SizeX, SizeY];

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
                _thisGrid[x, y] = newTile;
                _gridList.Add(newTile);

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
        _gridList.Clear();
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
        _thisGrid = new GridTile[0,0];
    }

    private void GridListToArray()
    {
        if (_gridList.Count == (SizeX * SizeY))
        {
            int i = 0;
            _thisGrid = new GridTile[SizeX, SizeY];

            for (int y = 0; y < SizeY; y++)
            {
                for (int x = 0; x < SizeX; x++)
                {
                    _thisGrid[x, y] = _gridList[i];
                    i++;
                }
            }
        }
    }

    #endregion


}
