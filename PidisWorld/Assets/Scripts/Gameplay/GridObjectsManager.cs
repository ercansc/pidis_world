using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObjectsManager : ResourcableSingleton<GridObjectsManager>
{

    [SerializeField]
    private GridObjectDataContainer m_dataContainer;

    public GridObjectData GetObjectData()
    {
        return m_dataContainer.GridObjectData;
    }
}
