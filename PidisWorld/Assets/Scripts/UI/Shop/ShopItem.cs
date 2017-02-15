using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private GameObject p_goBuildingPrefab;
    [SerializeField] private int m_iCost;
    [SerializeField] private ResourceCounter m_resourceCost;

    private void Awake()
    {
        m_resourceCost.SetValue(m_iCost);
    }
}
