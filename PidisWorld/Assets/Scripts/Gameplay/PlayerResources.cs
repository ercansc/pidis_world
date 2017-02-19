using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class PlayerResources : MonoBehaviour
{
    public static PlayerResources s_instance;
    public static CultureInfo s_cultureInfo;

    [SerializeField] private int m_iStartCredits;
    [SerializeField] private int m_iStartWorkers;

    [SerializeField] private ResourceCounter m_resourceCredits;
    [SerializeField] private ResourceCounter m_resourceWorkers;

    [SerializeField] private Tooltip p_tooltipSimple;
    [SerializeField]
    private Tooltip p_tooltipGoal;
    [SerializeField] private Sprite m_spriteOil;
    [SerializeField]
    private Sprite m_spriteWorker;
    [SerializeField]
    private Sprite m_spriteEnergy;

    private int m_iCredits;
    private int m_iWorkers;

    private void Awake()
    {
        s_cultureInfo = CultureInfo.CreateSpecificCulture("de-DE"); ;
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

    private void Start()
    {
        InitializeResources();
    }

    private void InitializeResources()
    {
        m_iCredits = m_iStartCredits;
        m_resourceCredits.SetValue(m_iCredits);
        m_iWorkers = m_iStartWorkers;
        m_resourceWorkers.SetValue(m_iWorkers);
    }

    public void AddCredits(int _iValue)
    {
        m_iCredits += _iValue;
        m_resourceCredits.SetValue(m_iCredits);
    }

    public void AddWorkers(int _iValue)
    {
        m_iWorkers += _iValue;
        m_resourceWorkers.SetValue(m_iWorkers);
    }

    public Tooltip CreateTooltip(Tooltip.Type _eType, Transform _target, int _iValue)
    {
        Tooltip tooltipPrefab = null;
        Sprite sprite = null;
        switch (_eType)
        {
            case Tooltip.Type.Oil:
                tooltipPrefab = p_tooltipSimple;
                sprite = m_spriteOil;
                break;
            case Tooltip.Type.Worker:
                tooltipPrefab = p_tooltipSimple;
                sprite = m_spriteWorker;
                break;
            case Tooltip.Type.Energy:
                tooltipPrefab = p_tooltipGoal;
                sprite = m_spriteEnergy;
                break;
            default:
                throw new ArgumentOutOfRangeException("_eType", _eType, null);
        }

        Tooltip newTooltip = Instantiate(tooltipPrefab, transform, false);
        newTooltip.transform.SetAsFirstSibling();
        newTooltip.Initialize(_target, sprite, _iValue);

        return newTooltip;
    }

    public Tooltip CreateTooltip(Tooltip.Type _eType, Transform _target, int _iValue, int _iOptionalValue)
    {
        Tooltip newTooltip = CreateTooltip(_eType, _target, _iValue);
        newTooltip.UpdateOptionalValue(_iOptionalValue);

        return newTooltip;
    }
}
