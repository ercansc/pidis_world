using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    public string Name;
    public GridTile Tile;

    public GridObject(string name, GridTile tile)
    {
        Name = name;
        Tile = tile;
    }
}
