using System;
using UnityEngine;

public class BuildingRocket : IngameBuilding
{
    [SerializeField]
    private int m_iGoalEnergy;

    private int m_iCurrentEnergy;
    private int _energyInSystem;

    private bool m_bFinished = false;

    [SerializeField] private AudioClip m_successClip;

    void Awake()
    {
        m_tile = transform.parent.GetComponent<GridTile>();
    }

    protected override void Start()
    {
        base.Start();
        m_itemData = new ShopItemData();
        m_itemData.eType = Building.Rocket;
        GetComponentInChildren<SpriteRenderer>().sortingOrder = 1;
        PlayerResources.s_instance.Rocket = this;
    }

    private void Update()
    {
        m_tooltip.UpdateValue(_energyInSystem);

        if (_energyInSystem == m_iGoalEnergy && !m_bFinished)
        {
            m_bFinished = true;
            AudioSource audio = gameObject.AddComponent<AudioSource>();
            audio.PlayOneShot(m_successClip);
            PlayerResources.s_instance.ShowFinishedWindow();
        }
    }

    public void UpdateEnergyInSystem()
    {
        _energyInSystem = GetEnergyOfAllConnected();
    }

    protected override void InitTooltip()
    {
        if (m_tooltip == null)
        {
            m_tooltip = PlayerResources.s_instance.CreateTooltip(Tooltip.Type.Energy, transform, 0, m_iGoalEnergy);
        }
    }
}