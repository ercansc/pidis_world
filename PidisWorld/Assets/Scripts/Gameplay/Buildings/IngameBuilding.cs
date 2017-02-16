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

    private ShopItemData m_itemData;

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
}
