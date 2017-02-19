using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingRocket : IngameBuilding {

	void Start ()
    {
	    m_itemData = new ShopItemData();	
        m_itemData.eType = Building.Rocket;
        m_tile = transform.parent.GetComponent<GridTile>();
        GetComponentInChildren<SpriteRenderer>().sortingOrder = 10;
    }
}
