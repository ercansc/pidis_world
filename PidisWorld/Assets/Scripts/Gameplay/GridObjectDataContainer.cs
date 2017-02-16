using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "GridObjectDataContainer", menuName = "GridObjects/Data Container")]
public class GridObjectDataContainer : ScriptableObject
{
    [SerializeField] public GridObjectData GridObjectData;
}

[Serializable]
public class GridObjectData
{
    [SerializeField]
    private List<GameObject> _gridTypePrefabs;

    public List<GameObject> GridTypePrefabs
    {
        get { return _gridTypePrefabs; }
    }
}