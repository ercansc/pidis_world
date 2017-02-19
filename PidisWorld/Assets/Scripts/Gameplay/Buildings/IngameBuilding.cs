using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

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

    public void Initialize(ShopItemData _itemData)
    {
        m_itemData = _itemData;
        m_visual = Instantiate(ShopItemManager.Instance.buildingVisualPrefab, transform, false);
        m_visual.SetSprite(_itemData.Sprite);
        _audioSource = GetComponent<AudioSource>();
        _audioSource.PlayOneShot(m_itemData.PlacementSfx);
    }

    public void SetTile(GridTile _tile)
    {
        m_tile = _tile;
    }

    void Update()
    {
        if(m_itemData.eType == Building.Rocket)
            Debug.Log(String.Format("The rocket has {0} Energy", GetEnergyOfAllConnected()));
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
        if (m_tile.ContainedObject.GetComponent<OilField>() != null && m_itemData.eType == Building.Refinery)
        {
            _generatedEnergy = m_tile.ContainedObject.GetComponent<OilField>().OilValue;
        }
        else if (m_itemData.eType == Building.Generator)
        {
            _generatedEnergy = m_itemData.iWorkerCost;
        }
    }
}
