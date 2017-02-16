using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopItemDataContainer", menuName = "Buildings/Data Container")]
public class ShopItemDataContainer : ScriptableObject
{
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
