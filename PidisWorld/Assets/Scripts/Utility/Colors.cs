using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colors : ResourcableSingleton<Colors>
{
    [SerializeField] private Color m_colBuildingNormal = Color.white;
    [SerializeField] private Color m_colBuildingBlocked;
    [SerializeField] private Color m_colBuildingFree;
    [SerializeField] private Color m_colBuildingHighlighted;
    [SerializeField] private Color m_colBuildingDestroy;

    public Color BuildingBlocked
    {
        get { return m_colBuildingBlocked; }
    }

    public Color BuildingFree
    {
        get { return m_colBuildingFree; }
    }

    public Color BuildingHighlighted
    {
        get { return m_colBuildingHighlighted; }
    }

    public Color BuildingNormal
    {
        get { return m_colBuildingNormal; }
    }

    public Color BuildingDestroy
    {
        get { return m_colBuildingDestroy; }
    }
}
