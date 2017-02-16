using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Index
{
    public int x;
    public int y;

    public Index(int index_x, int index_y)
    {
        x = index_x;
        y = index_y;
    }

    public Vector2 GetPosition()
    {
        return new Vector2(x, y);
    }
}

public class GridTile : MonoBehaviour
{
    public Index PositionIndex;

    public GameObject ContainedObject;

    public bool Blocked
    {
        get { return ContainedObject != null; }
    }

    private MeshRenderer _renderer;

    void Start()
    {
        _renderer = GetComponent<MeshRenderer>();

        if (transform.childCount > 0)
        {
            ContainedObject = transform.GetChild(0).gameObject;
        }
    }

    public Grid GetGrid()
    {
        return transform.parent.GetComponent<Grid>();
    }

    public void SetObject(GameObject obj)
    {
        ContainedObject = obj;
    }

    public void RemoveObject()
    {
        ContainedObject = null;
    }

    public void Highlight(Color color)
    {
        _renderer.material.color = color;
    }


}