using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelpers;

public class BuildingManager : ResourcableSingleton<BuildingManager>
{
    [SerializeField] private BuildingDataContainer m_dataContainer;

    public BuildingData GetBuildingData(Building _eType)
    {
        return m_dataContainer.GetData(_eType);
    }
}
