using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class IngameBuildingPair
{
    public IngameBuilding BuildingA;
    public IngameBuilding BuildingB;

    public IngameBuildingPair(IngameBuilding buildingA, IngameBuilding buildingB)
    {
        BuildingA = buildingA;
        BuildingB = buildingB;
    }

    public bool HasPair(IngameBuilding a, IngameBuilding b)
    {
        if (BuildingA == a || BuildingA == b)
        {
            if (BuildingB == a || BuildingB == b)
            {
                return true;
            }
        }
        return false;
    }

    public bool HasRocket()
    {
        return BuildingA.m_itemData.eType == Building.Rocket || BuildingB.m_itemData.eType == Building.Rocket;
    }

}

public class IngameBuilding : MonoBehaviour
{
    private BuildingVisual m_visual;

    public BuildingVisual Visual
    {
        get { return m_visual; }
    }
    public ShopItemData m_itemData { get; protected set; }
    private AudioSource _audioSource;

    private int _generatedEnergy;
    public int GeneratedEnergy
    {
        get
        {
            if (_generatedEnergy == 0)
            {
                CalculateEnergyOfBuilding();
            }
            return _generatedEnergy;
        }
        set { _generatedEnergy = value; }
    }

    protected GridTile m_tile;

    public GridTile Tile
    {
        get { return m_tile; }
    }

    protected Tooltip m_tooltip;

    protected virtual void Start()
    {
        InitTooltip();
        foreach (IngameBuilding building in GetAdjacentBuildings())
        {
            PlayerResources.s_instance.AddMathSignBuildingPair(this, building);
        }
    }

    public void Initialize(ShopItemData _itemData)
    {
        m_itemData = _itemData;
        m_visual = Instantiate(ShopItemManager.Instance.buildingVisualPrefab, transform, false);
        m_visual.SetSprite(_itemData.Sprite);
        _audioSource = GetComponent<AudioSource>();
        _audioSource.PlayOneShot(m_itemData.PlacementSfx);
    }

    protected virtual void InitTooltip()
    {
        if (m_itemData.eType == Building.Generator)
        {
            m_tooltip = PlayerResources.s_instance.CreateTooltip(Tooltip.Type.Worker, transform, m_itemData.iWorkerCost);
        }
    }


    public void SetTile(GridTile _tile)
    {
        m_tile = _tile;
    }

    public List<IngameBuilding> GetAdjacentBuildings()
    {
        List<IngameBuilding> adjacentBuildings = new List<IngameBuilding>();
        Index positionIndex = m_tile.PositionIndex;


        foreach (GridTile adjacentTile in m_tile.GetAdjacentTiles())
        {
            //Abbruchbedingungen
            if (adjacentTile.ContainedObject == null) continue;
            if (adjacentTile.ContainedObject.GetComponent<IngameBuilding>() == null) continue;

            IngameBuilding building = adjacentTile.ContainedObject.GetComponent<IngameBuilding>();
            if (!adjacentBuildings.Contains(building))
            {
                adjacentBuildings.Add(building);
            }
        }

        return adjacentBuildings;
    }

    public int GetEnergyOfAllConnected()
    {
        List<IngameBuilding> buildingsInSystem = GetAdjacentBuildings();

        while (HasAdjBuildingsLeft(buildingsInSystem))
        {
            List<IngameBuilding> tempList = new List<IngameBuilding>();
            foreach (IngameBuilding building in buildingsInSystem)
            {
                foreach (IngameBuilding adjacentBuilding in building.GetAdjacentBuildings())
                {
                    if (!buildingsInSystem.Contains(adjacentBuilding)) tempList.Add(adjacentBuilding);
                }
            }
            buildingsInSystem.AddRange(tempList);
        }

        if (HasRefineryInSystem(buildingsInSystem))
        {
            return buildingsInSystem.Sum(b => b.GeneratedEnergy);
        }
        return 0;
    }

    private bool HasRefineryInSystem(List<IngameBuilding> systemList)
    {
        return systemList.Any(building => building.m_itemData.eType == Building.Refinery);
    }

    private bool HasAdjBuildingsLeft(List<IngameBuilding> excludeList)
    {
        foreach (IngameBuilding building in excludeList)
        {
            foreach (IngameBuilding adjacentBuilding in building.GetAdjacentBuildings())
            {
                if (!excludeList.Contains(adjacentBuilding)) return true;
            }
        }
        return false;
    }

    private void CalculateEnergyOfBuilding()
    {
        if (m_itemData.eType == Building.Refinery)
        {
            _generatedEnergy = m_tile.GetComponentInChildren<OilField>().OilValue;
        }
        else if (m_itemData.eType == Building.Generator)
        {
            _generatedEnergy = m_itemData.iWorkerCost;
        }
    }

    public void OnDestroyBuilding()
    {
        switch (m_itemData.eType)
        {
            case Building.Refinery:
                break;
            case Building.Generator:
                PlayerResources.s_instance.AddWorkers(m_itemData.iWorkerCost);
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

        if (m_tooltip != null)
        {
            Destroy(m_tooltip.gameObject);
        }
    }

    public void OnMoveBuilding()
    {
        if (m_tooltip != null)
        {
            m_tooltip.UpdatePosition();
        }
    }
}
