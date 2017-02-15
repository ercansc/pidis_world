using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTile : MonoBehaviour
{
    public Position Position;
    public GridObject ContainedObject;

    public bool IsOccupied()
    {
        return ContainedObject != null;
    }

}


public class Position
{
    public int x;
    public int y;

    public Position(int X, int Y)
    {
        x = X;
        y = Y;
    }
}