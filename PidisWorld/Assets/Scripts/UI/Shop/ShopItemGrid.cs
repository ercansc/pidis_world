using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemGrid : MonoBehaviour
{
    [SerializeField] private ShopItem p_shopItem;

    [ContextMenu("Repopulate Shop")]
    private void Repopulate()
    {
        ClearShop();
        PopulateShop();
    }

    private void PopulateShop()
    {
        List<ShopItemData> liDatas = ShopItemManager.Instance.liGetAllItemDatas();
        foreach (ShopItemData itemData in liDatas)
        {
            ShopItem item = Instantiate(p_shopItem, transform, false);
            item.Initialize(itemData);
        }
    }

    [ContextMenu("Clear Shop")]
    private void ClearShop()
    {
        List<Transform> liChildren = new List<Transform>();
        foreach (Transform childTransform in transform)
        {
            liChildren.Add(childTransform);
        }

        foreach (Transform child in liChildren)
        {
            DestroyImmediate(child.gameObject);
        }
    }
}
