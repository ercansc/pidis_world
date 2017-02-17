using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopItemDataContainer", menuName = "Buildings/Data Container")]
public class ShopItemDataContainer : ScriptableObject
{
    [SerializeField]
    private IngameBuilding p_ingameBuilding;

    public IngameBuilding IngameBuilding
    {
        get { return p_ingameBuilding; }
    }

    [SerializeField]
    private BuildingVisual p_buildingVisual;

    public BuildingVisual BuildingVisual
    {
        get { return p_buildingVisual; }
    }

    [SerializeField] private List<ShopItemData> m_liItemDatas;

    public List<ShopItemData> liItemDatas
    {
        get { return m_liItemDatas; }
    }

    public ShopItemData GetData(Building _eType)
    {
        return m_liItemDatas.FirstOrDefault(e => e.eType == _eType);
    }
}

[Serializable]
public class ShopItemData
{
    [SerializeField] private Building m_eType;

    public Building eType
    {
        get { return m_eType; }
        set { m_eType = value; }
    }

    [SerializeField] private int m_iCost;

    public int iCost
    {
        get { return m_iCost; }
    }

    [SerializeField] private bool m_bHasLevel = true;

    public bool bHasLevel
    {
        get { return m_bHasLevel; }
    }

    [SerializeField] private bool m_bHasWorker = false;

    public bool bHasWorker
    {
        get
        {
            return m_bHasWorker;
        }
    }

    [SerializeField] private int m_iWorkerCostPerLevel;

    public int iWorkerCostPerLevel
    {
        get
        {
            return m_iWorkerCostPerLevel;
        }
    }

    public int iWorkerCost
    {
        get
        {
            return m_iWorkerCostPerLevel * 1;
        }
    }

    [SerializeField] private Sprite m_sprite;

    public Sprite Sprite
    {
        get { return m_sprite; }
    }
}
