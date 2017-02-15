using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Building
{
    Drill,
    Refinery,
    Rocket
}

public class ShopItem : MonoBehaviour
{
    [SerializeField] private Image m_image;
    [SerializeField] private ResourceCounter m_resourceCost;
    [SerializeField] private Building m_eType;

    private BuildingData m_data;
    private int m_iCost;

    private void Awake()
    {
        m_data = BuildingManager.Instance.GetBuildingData(m_eType);
        m_iCost = m_data.iCost;
        m_image.sprite = m_data.spriteShop;
    }

    private void Start()
    {
        m_resourceCost.SetValue(m_iCost);
    }

    public void StartBuildMode()
    {
        Controls.s_instance.EnterBuildMode(m_eType);
    }
}
