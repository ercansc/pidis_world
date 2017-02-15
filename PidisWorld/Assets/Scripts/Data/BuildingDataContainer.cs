using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingDataContainer", menuName = "Buildings/Data Container")]
public class BuildingDataContainer : ScriptableObject
{
    [SerializeField] private List<BuildingData> m_liBuildingDatas;

    public BuildingData GetData(Building _eType)
    {
        return m_liBuildingDatas.FirstOrDefault(e => e.eType == _eType);
    }
}

[Serializable]
public class BuildingData
{
    [SerializeField] private Building m_eType;

    public Building eType
    {
        get { return m_eType; }
    }

    [SerializeField] private int m_iCost;

    public int iCost
    {
        get { return m_iCost; }
    }

    [SerializeField] private Sprite m_spriteShop;

    public Sprite spriteShop
    {
        get { return m_spriteShop; }
    }

    [SerializeField] private GameObject p_goPrefab;

    public GameObject goPrefab
    {
        get { return p_goPrefab; }
    }
}
