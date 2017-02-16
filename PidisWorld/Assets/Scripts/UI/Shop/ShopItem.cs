using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Building
{
    Refinery,
    Generator,
    Pipeline,
    Wire,
    Rocket
}

public class ShopItem : MonoBehaviour
{
    [SerializeField] private Image m_image;
    [SerializeField] private ResourceCounter m_resourceCost;
    [SerializeField] private GameObject m_goLevel;
    [SerializeField] private GameObject m_goWorker;
    [SerializeField] private Text m_txtWorkerCount;
    [SerializeField]
    [HideInInspector]
    private Building m_eType;

    private ShopItemData m_data;
    private int m_iCost;

    private void Awake()
    {
        Initialize(ShopItemManager.Instance.GetBuildingData(m_eType));
    }

    public void StartBuildMode()
    {
        Controls.s_instance.EnterBuildMode(m_eType);
    }

    public void Initialize(ShopItemData _data)
    {
        m_eType = _data.eType;
        m_data = _data;
        m_iCost = m_data.iCost;
        m_image.sprite = m_data.Sprite;
        m_resourceCost.SetValue(m_iCost);
        m_goLevel.SetActive(_data.bHasLevel);
        m_goWorker.SetActive(_data.bHasWorker);
        m_txtWorkerCount.text = _data.iWorkerCost.ToString();
    }
}
