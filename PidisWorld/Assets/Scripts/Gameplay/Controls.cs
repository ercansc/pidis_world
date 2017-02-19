using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ControlState
{
    Idle,
    Build,
    Move,
    Destroy,
}

public enum MoveState
{
    Idle,
    Moving
}

public enum DestroyState
{
    Idle,
    Confirm
}

public class Controls : MonoBehaviour
{
    private ControlState m_eControlState;

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

    [SerializeField] private Text m_txtControlMode;

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

    [Header("Move Mode")]
    [SerializeField] private LayerMask m_moveMask;

    private MoveState m_eMoveState = MoveState.Idle;
    private IngameBuilding m_buildingCurrent;
    private GridTile m_tileBeginMove;

    [Header("Destroy Mode")] [SerializeField] private LayerMask m_destroyMask;
    [SerializeField] private ConfirmDestroyMenu m_destroyMenu;
    private DestroyState m_eDestroyState = DestroyState.Idle;

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
                HandleDestroyMode();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public static string TranslateState(ControlState _eState)
    {
        switch (_eState)
        {
            case ControlState.Idle:
                return "";
            case ControlState.Build:
                return "Bauen";
            case ControlState.Move:
                return "Bewegen";
            case ControlState.Destroy:
                return "Zerstören";
            default:
                throw new ArgumentOutOfRangeException("_eState", _eState, null);
        }
    }

    private void EnterState(ControlState _eState)
    {
        m_eControlState = _eState;
        m_txtControlMode.text = TranslateState(m_eControlState);
    }

    private void EnterIdleMode()
    {
        DeHighlightCurrentBuilding();
        m_placementVisual.gameObject.SetActive(false);
        EnterState(ControlState.Idle);
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

    private GridTile HandleVisualPlacement(RaycastHit2D hit)
    {
        GridTile tile = null;
        if (hit.transform != null)
        { 
            tile = hit.transform.gameObject.GetComponentInParent<GridTile>();
            if (tile != null)
            {
                if (tile != m_currentBuildTile)
                {
                    Building eBuildingType = Building.Refinery;
                    if (m_dataCurrent != null)
                    {
                        eBuildingType = m_dataCurrent.eType;
                    }
                    else if (m_buildingCurrent != null)
                    {
                        eBuildingType = m_buildingCurrent.m_itemData.eType;
                    }
                    bool bCanBePlaced = bCanPlaceBuilding(eBuildingType, tile);
                    m_placementVisual.OnEnterTile(bCanBePlaced, tile);
                }
            }
            m_currentBuildTile = tile;
        }

        return tile;
    }

    #endregion

    #region Build Mode

    public void EnterBuildMode(Building _eBuilding)
    {
        m_dataCurrent = ShopItemManager.Instance.GetBuildingData(_eBuilding);
        EnterState(ControlState.Build);
        
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
            HandleVisualPlacement(hit);
            if (m_currentBuildTile != null)
            {
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

    private bool bCanPlaceBuilding(Building _eType, GridTile _targetTile)
    {
        switch (_eType)
        {
            case Building.Refinery:
                return _targetTile.bIsOilField();
            case Building.Generator:
                return !_targetTile.Blocked;
            case Building.Pipeline:
                return !_targetTile.Blocked;
            case Building.Wire:
                return !_targetTile.Blocked;
            case Building.Rocket:
                return !_targetTile.Blocked;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private bool bTryPlaceBuilding(GridTile _tile)
    {
        if (!bCanPlaceBuilding(m_dataCurrent.eType, _tile))
        {
            return false;
        }

        IngameBuilding building = CreateBuilding();
        PlaceBuilding(building, _tile);
        BuyBuilding();
        return true;
    }

    private IngameBuilding CreateBuilding()
    {
        IngameBuilding building = Instantiate(ShopItemManager.Instance.ingameBuildingPrefab);
        building.Initialize(m_dataCurrent);

        return building;
    }

    private void PlaceBuilding(IngameBuilding _building, GridTile _tile)
    {
        _building.transform.position = _tile.transform.position;
        _building.SetTile(_tile);
        _tile.ContainedObject = _building.gameObject;
    }

    private void BuyBuilding()
    {
        PlayerResources.s_instance.AddCredits(-m_dataCurrent.iCost);
        PlayerResources.s_instance.AddWorkers(-m_dataCurrent.iWorkerCost);
    }

    #endregion

    #region Move Mode

    public void EnterMoveMode()
    {
        EnterIdleMode();
        EnterState(ControlState.Move);
        m_eMoveState = MoveState.Idle;
    }

    private void HandleMoveMode()
    {
        switch (m_eMoveState)
        {
            case MoveState.Idle:
                HandleMoveIdle();
                break;
            case MoveState.Moving:
                HandleMoveMoving();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (Input.GetMouseButtonDown(1))
        {
            LeaveMoveMode();
        }
    }

    private void LeaveMoveMode()
    {
        DeHighlightCurrentBuilding();
        m_eMoveState = MoveState.Idle;
        EnterIdleMode();
    }

    private void DeHighlightCurrentBuilding()
    {
        if (m_buildingCurrent != null)
        {
            m_buildingCurrent.Visual.Highlight(Colors.Instance.BuildingNormal);
        }
    }

    #region Idle

    private void HandleMoveIdle()
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

            if (m_buildingCurrent != null)
            {
                if (Input.GetMouseButton(0))
                {
                    EnterMoveMoving();
                }
            }
        }
        else if (m_buildingCurrent != null)
        {
            DeHighlightCurrentBuilding();
            m_buildingCurrent = null;
        }
    }

    #endregion

    #region Moving

    private void MoveBuilding(IngameBuilding _building, GridTile _targetTile)
    {
        m_tileBeginMove.ContainedObject = null;
        PlaceBuilding(m_buildingCurrent, _targetTile);
    }

    private void EnterMoveMoving()
    {
        RaycastHit2D hit = DoRaycast2D(m_buildMask);
        GridTile tile = HandleVisualPlacement(hit);
        m_placementVisual.SetSprite(m_buildingCurrent.Visual.Sprite);
        m_tileBeginMove = tile;
        m_eMoveState = MoveState.Moving;
        m_placementVisual.gameObject.SetActive(true);
    }

    private void ConfirmMoving(GridTile _tile)
    {
        DeHighlightCurrentBuilding();
        if (bCanPlaceBuilding(m_buildingCurrent.m_itemData.eType, _tile))
        {
            MoveBuilding(m_buildingCurrent, _tile);
            EnterMoveMode();
        }
        else
        {
            EnterMoveMode();
        }
    }

    private void HandleMoveMoving()
    {
        RaycastHit2D hit = DoRaycast2D(m_buildMask);
        GridTile tile = HandleVisualPlacement(hit);
        if (Input.GetMouseButtonUp(0))
        {
            ConfirmMoving(tile);
        }
        if (Input.GetMouseButtonDown(1))
        {
            LeaveMoveMode();
        }
    }

    #endregion

    #endregion

    #region Destroy Mode

    public void EnterDestroyMode()
    {
        DeHighlightCurrentBuilding();
        EnterIdleMode();
        EnterState(ControlState.Destroy);
        EnterDestroyIdle();
    }

    private void HandleDestroyIdle()
    {
        RaycastHit2D hit;
        if (bRaycastHit(m_moveMask, out hit))
        {
            IngameBuilding buildingHit = hit.transform.GetComponentInParent<IngameBuilding>();
            if (buildingHit != m_buildingCurrent)
            {
                DeHighlightCurrentBuilding();
                m_buildingCurrent = buildingHit;
                m_buildingCurrent.Visual.Highlight(Colors.Instance.BuildingDestroy);
            }

            if (m_buildingCurrent != null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    EnterDestroyConfirm();
                }
            }
        }
        else
        {
            DeHighlightCurrentBuilding();
            m_buildingCurrent = null;
        }
        if (Input.GetMouseButtonDown(1))
        {
            EnterIdleMode();
        }
    }

    private void ConfirmDestroyBuilding()
    {
        m_buildingCurrent.Tile.ContainedObject = null;
        DeHighlightCurrentBuilding();
        Destroy(m_buildingCurrent.gameObject);
        m_buildingCurrent = null;
        EnterDestroyIdle();
    }

    private void CancelDestroyBuilding()
    {
        DeHighlightCurrentBuilding();
        m_buildingCurrent = null;
        m_destroyMenu.gameObject.SetActive(false);
        EnterDestroyMode();
    }

    private void EnterDestroyConfirm()
    {
        m_eDestroyState = DestroyState.Confirm;
        m_destroyMenu.gameObject.SetActive(true);
        m_destroyMenu.Initialize(m_buildingCurrent, ConfirmDestroyBuilding, CancelDestroyBuilding);
    }

    private void HandleDestroyConfirm()
    {
        
    }

    private void EnterDestroyIdle()
    {
        m_eDestroyState = DestroyState.Idle;
    }

    private void HandleDestroyMode()
    {
        switch (m_eDestroyState)
        {
            case DestroyState.Idle:
                HandleDestroyIdle();
                break;
            case DestroyState.Confirm:
                HandleDestroyConfirm();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    #endregion
}
