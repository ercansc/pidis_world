using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameBuilding : MonoBehaviour
{
    private BuildingVisual m_visual;

    public BuildingVisual Visual
    {
        get { return m_visual; }
    }
    public ShopItemData m_itemData { get; private set; }

    public List<IngameBuilding> AdjacentBuildings;

    private GridTile m_tile;

    public GridTile Tile
    {
        get { return m_tile; }
    }

    public void Initialize(ShopItemData _itemData)
    {
        m_itemData = _itemData;
        m_visual = Instantiate(ShopItemManager.Instance.buildingVisualPrefab, transform, false);
        m_visual.SetSprite(_itemData.Sprite);
    }

    public void SetTile(GridTile _tile)
    {
        m_tile = _tile;
    }

    void Update()
    {
        switch (m_itemData.eType)
        {
            case Building.Refinery:
                RefineryUpdate();
                break;
            case Building.Generator:
                GeneratorUpdate();
                break;
            case Building.Pipeline:
                break;
            case Building.Wire:
                break;
            case Building.Rocket:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

    }


    private void RefineryUpdate()
    {
        
    }

    private void GeneratorUpdate()
    {
        foreach (IngameBuilding building in AdjacentBuildings)
        {
            if (building.m_itemData.eType == Building.Refinery)
            {
                
            }
        }
    }

}
