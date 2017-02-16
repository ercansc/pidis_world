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

    [SerializeField]
    private BuildingPlacementVisual m_placementVisual;

    public BuildingPlacementVisual PlacementVisual
    {
        get { return m_placementVisual; }
    }

    public bool bBuildMode
    {
        get { return m_eControlState == ControlState.BuildMode; }
    }

    private GridTile m_currentBuildTile;
    private ShopItemData m_dataCurrent;

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
        m_dataCurrent = ShopItemManager.Instance.GetBuildingData(_eBuilding);
        m_eControlState = ControlState.BuildMode;
        
        m_placementVisual.SetSprite(m_dataCurrent.spriteShop);
        m_placementVisual.gameObject.SetActive(true);
    }

    private void ExitBuildMode()
    {
        m_placementVisual.gameObject.SetActive(false);
        m_eControlState = ControlState.Idle;
        m_dataCurrent = null;
        m_currentBuildTile = null;
    }

    private void HandleBuildMode()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero,
            Mathf.Infinity, m_buildMask.value);
        if (hit.collider != null)
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
