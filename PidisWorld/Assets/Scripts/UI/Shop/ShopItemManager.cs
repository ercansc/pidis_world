using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelpers;

public class ShopItemManager : ResourcableSingleton<ShopItemManager>
{
    [SerializeField] private ShopItemDataContainer m_dataContainer;

    public IngameBuilding ingameBuildingPrefab
    {
        get { return m_dataContainer.IngameBuilding; }
    }

    public BuildingVisual buildingVisualPrefab
    {
        get { return m_dataContainer.BuildingVisual; }
    }

    public ShopItemData GetBuildingData(Building _eType)
    {
        return m_dataContainer.GetData(_eType);
    }

    public List<ShopItemData> liGetAllItemDatas()
    {
        return m_dataContainer.liItemDatas;
    }
}
