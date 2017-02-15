using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControlState
{
    Idle,
    BuildMode,
}

public class Controls : MonoBehaviour
{
    private ControlState m_eControlState = ControlState.Idle;

    public ControlState eControlState
    {
        get
        {
            return m_eControlState;
        }
    }

    public static Controls s_instance;

    [Header("Build Mode")]

    [SerializeField]
    private LayerMask m_buildMask;

    [SerializeField] private BuildingPlacementVisual m_placementVisualDrill;

    public BuildingPlacementVisual PlacementVisualDrill
    {
        get
        {
            return m_placementVisualDrill;
        }
    }
    [SerializeField] private BuildingPlacementVisual m_placementVisualRefinery;

    private BuildingPlacementVisual m_placementVisualCurrent;

    public BuildingPlacementVisual PlacementVisualCurrent
    {
        get { return m_placementVisualCurrent; }
    }

    public BuildingPlacementVisual PlacementVisualRefinery
    {
        get { return m_placementVisualRefinery; }
    }

    public bool bBuildMode
    {
        get { return m_eControlState == ControlState.BuildMode; }
    }

    private GridTile m_currentBuildTile;
    private BuildingData m_dataCurrent;

    private void Awake()
    {
        if (s_instance != null)
        {
            Debug.LogErrorFormat("There is already an instance of type {0}. Destroying this object.", GetType().Name);
            Destroy(gameObject);
        }
        else
        {
            s_instance = this;
        }
    }

    private void Update()
    {
        if (bBuildMode)
        {
            HandleBuildMode();
        }
    }

    #region Build Mode

    public void EnterBuildMode(Building _eBuilding)
    {
        m_dataCurrent = BuildingManager.Instance.GetBuildingData(_eBuilding);
        m_eControlState = ControlState.BuildMode;
        if (m_placementVisualCurrent != null)
        {
            m_placementVisualCurrent.gameObject.SetActive(false);
        }

        switch (_eBuilding)
        {
            case Building.Drill:
                m_placementVisualCurrent = PlacementVisualDrill;
                break;
            case Building.Refinery:
                m_placementVisualCurrent = PlacementVisualRefinery;
                break;
            case Building.Rocket:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (m_placementVisualCurrent != null)
        {
            m_placementVisualCurrent.gameObject.SetActive(true);
        }
    }

    private void ExitBuildMode()
    {
        m_placementVisualCurrent.gameObject.SetActive(false);
        m_eControlState = ControlState.Idle;
        m_dataCurrent = null;
        m_currentBuildTile = null;
        m_placementVisualCurrent = null;
    }

    private void HandleBuildMode()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_buildMask.value))
        {
            GameObject goHit = hit.collider.gameObject;
            GridTile tile = goHit.GetComponentInParent<GridTile>();
            if (tile != null)
            {
                if (tile != m_currentBuildTile)
                {
                    m_currentBuildTile = tile;
                    m_placementVisualCurrent.OnEnterTile(tile);
                }

                if (Input.GetMouseButtonDown(0))
                {
                    if (bTryPlaceBuilding(m_currentBuildTile))
                    {
                        ExitBuildMode();
                    }
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    ExitBuildMode();
                }
            }
        }
    }

    private bool bTryPlaceBuilding(GridTile _tile)
    {
        if (_tile.Blocked)
        {
            return false;
        }
        
        CompleteBuildMode(_tile);
        return true;
    }

    private void CompleteBuildMode(GridTile _tile)
    {
        GameObject goBuilding = Instantiate(m_dataCurrent.goPrefab, _tile.transform.position, Quaternion.identity);
        _tile.ContainedObject = goBuilding;
        PlayerResources.s_instance.AddCredits(-m_dataCurrent.iCost);
    }

    #endregion
}
