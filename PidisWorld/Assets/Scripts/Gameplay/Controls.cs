using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControlState
{
    Idle,
    Build,
    Move,
    Destroy,
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

    [SerializeField]
    private BuildingVisual m_placementVisual;

    public BuildingVisual PlacementVisual
    {
        get { return m_placementVisual; }
    }

    public bool bBuildMode
    {
        get { return m_eControlState == ControlState.Build; }
    }

    public bool bMoveMode
    {
        get { return m_eControlState == ControlState.Move; }
    }

    public bool bDestroyMode
    {
        get { return m_eControlState == ControlState.Destroy; }
    }

    private GridTile m_currentBuildTile;
    private ShopItemData m_dataCurrent;

    [Header("Move Mode")] [SerializeField] private LayerMask m_moveMask;

    private IngameBuilding m_buildingCurrent;

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
        switch (m_eControlState)
        {
            case ControlState.Idle:
                break;
            case ControlState.Build:
                HandleBuildMode();
                break;
            case ControlState.Move:
                HandleMoveMode();
                break;
            case ControlState.Destroy:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void EnterIdleMode()
    {
        m_placementVisual.gameObject.SetActive(false);
        m_eControlState = ControlState.Idle;
        m_dataCurrent = null;
        m_currentBuildTile = null;
        m_buildingCurrent = null;
    }

    #region Raycasting

    private RaycastHit2D DoRaycast2D(LayerMask _layerMask)
    {
        return Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero,
            Mathf.Infinity, _layerMask.value);
    }

    private bool bRaycastHit(LayerMask _layerMask, out RaycastHit2D hit)
    {
        hit = DoRaycast2D(_layerMask);
        return hit.collider != null;
    }

    #endregion

    #region Build Mode

    public void EnterBuildMode(Building _eBuilding)
    {
        m_dataCurrent = ShopItemManager.Instance.GetBuildingData(_eBuilding);
        m_eControlState = ControlState.Build;
        
        m_placementVisual.SetSprite(m_dataCurrent.Sprite);
        m_placementVisual.gameObject.SetActive(true);
    }

    private void ExitBuildMode()
    {
        EnterIdleMode();
    }

    private void HandleBuildMode()
    {
        RaycastHit2D hit;
        if(bRaycastHit(m_buildMask, out hit))
        {
            GridTile tile = hit.transform.gameObject.GetComponentInParent<GridTile>();
            if (tile != null)
            {
                if (tile != m_currentBuildTile)
                {
                    m_currentBuildTile = tile;
                    m_placementVisual.OnEnterTile(tile);
                }

                if (Input.GetMouseButtonDown(0))
                {
                    if (bTryPlaceBuilding(m_currentBuildTile))
                    {
                        ExitBuildMode();
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            ExitBuildMode();
        }
    }

    private bool bTryPlaceBuilding(GridTile _tile)
    {
        if (_tile.Blocked)
        {
            return false;
        }
        
        PlaceBuilding(_tile);
        return true;
    }

    private void PlaceBuilding(GridTile _tile)
    {
        IngameBuilding building = Instantiate(ShopItemManager.Instance.ingameBuildingPrefab, _tile.transform.position, Quaternion.identity);
        building.Initialize(m_dataCurrent);
        _tile.ContainedObject = building.gameObject;
        PlayerResources.s_instance.AddCredits(-m_dataCurrent.iCost);
        PlayerResources.s_instance.AddWorkers(-m_dataCurrent.iWorkerCost);
    }

    #endregion

    #region Move Mode

    public void EnterMoveMode()
    {
        EnterIdleMode();
        m_eControlState = ControlState.Move;
    }

    private void HandleMoveMode()
    {
        RaycastHit2D hit;
        
        if (bRaycastHit(m_moveMask, out hit))
        {
            IngameBuilding buildingHit = hit.transform.GetComponentInParent<IngameBuilding>();
            if (buildingHit != m_buildingCurrent)
            {
                DeHighlightCurrentBuilding();
                m_buildingCurrent = buildingHit;
                m_buildingCurrent.Visual.Highlight(Colors.Instance.BuildingHighlighted);
            }            
        }
        else if (m_buildingCurrent != null)
        {
            DeHighlightCurrentBuilding();
            m_buildingCurrent = null;
        }

        if (Input.GetMouseButtonDown(1))
        {
            CancelMoveMode();
        }
    }

    private void CancelMoveMode()
    {
        DeHighlightCurrentBuilding();
        
        EnterIdleMode();
    }

    private void DeHighlightCurrentBuilding()
    {
        if (m_buildingCurrent != null)
        {
            m_buildingCurrent.Visual.Highlight(Colors.Instance.BuildingNormal);
        }
    }

    #endregion
}
